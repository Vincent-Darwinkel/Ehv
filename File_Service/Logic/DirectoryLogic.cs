using File_Service.Dal.Interfaces;
using File_Service.Models;
using File_Service.Models.HelperFiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using File_Service.CustomExceptions;
using File_Service.Enums;

namespace File_Service.Logic
{
    public class DirectoryLogic
    {
        private readonly IDirectoryDal _directoryDal;
        private readonly IFileDal _fileDal;

        public DirectoryLogic(IDirectoryDal directoryDal, IFileDal fileDal)
        {
            _directoryDal = directoryDal;
            _fileDal = fileDal;
        }

        /// <summary>
        /// Creates a directory and saves it to the database
        /// </summary>
        /// <param name="userSpecifiedPath"></param>
        /// <param name="requestingUserUuid"></param>
        public async Task CreateDirectory(string userSpecifiedPath, Guid requestingUserUuid)
        {
            if (!ValidFilePaths.FilePathIsValid(userSpecifiedPath))
            {
                throw new UnprocessableException();
            }
            if (await _directoryDal.Exists(userSpecifiedPath))
            {
                throw new DuplicateNameException();
            }

            int index = userSpecifiedPath.LastIndexOf("/", StringComparison.Ordinal) + 1;
            string directoryName = userSpecifiedPath.Substring(index, userSpecifiedPath.Length - index);
            string fullPath = $"{Environment.CurrentDirectory}{userSpecifiedPath}";
            if (DirectoryHelper.GetFilesInDirectory(fullPath).Any())
            {
                throw new UnprocessableException();
            }

            Directory.CreateDirectory(fullPath);
            await _directoryDal.Add(new DirectoryDto
            {
                Uuid = Guid.NewGuid(),
                Name = directoryName,
                OwnerUuid = requestingUserUuid,
                Path = userSpecifiedPath
            });
        }

        public async Task RenameDirectory(Guid uuid, UserHelper requestingUser, string name)
        {
            DirectoryDto directory = await _directoryDal.Find(uuid);
            if (requestingUser.Uuid != directory.OwnerUuid)
            {
                throw new UnauthorizedAccessException();
            }

            int index = directory.Path.LastIndexOf("/", StringComparison.CurrentCulture);
            string newPath = directory.Path.Substring(0, index + 1) + name;
            string oldPath = directory.Path;
            string newFullPath = Environment.CurrentDirectory + newPath;
            string oldFullPath = Environment.CurrentDirectory + directory.Path;
            if (Directory.Exists(newFullPath))
            {
                throw new DuplicateNameException();
            }

            Directory.Move(oldFullPath, newFullPath);

            directory.Path = newPath;
            directory.Name = name;
            await _directoryDal.Update(directory);
            List<FileDto> filesToUpdate = await _fileDal.FindInDirectory(directory.Uuid);
            filesToUpdate.ForEach(file =>
                    file.FullPath = file.FullPath.Replace(oldPath, newPath));
            await _fileDal.Update(filesToUpdate);
        }

        public async Task<DirectoryDto> Find(string path)
        {
            return await _directoryDal.Find(path);
        }

        public async Task<List<DirectoryDto>> GetFoldersInDirectory(string path)
        {
            if (!ValidFilePaths.FilePathIsValid(path))
            {
                throw new UnprocessableException();
            }

            List<DirectoryDto> directories = await _directoryDal.FindAll(path);
            if (!directories.Any())
            {
                return new List<DirectoryDto>();
            }

            return directories;
        }

        public async Task<List<FileDto>> GetFileNamesInDirectory(string path)
        {
            if (!ValidFilePaths.FilePathIsValid(path))
            {
                throw new UnprocessableException();
            }

            DirectoryDto directory = await _directoryDal.Find(path);
            if (directory == null)
            {
                return new List<FileDto>();
            }

            return await _fileDal.FindInDirectory(directory.Uuid);
        }

        public async Task Delete(Guid uuid, UserHelper requestingUser)
        {
            DirectoryDto directory = await _directoryDal.Find(uuid);
            if (directory.OwnerUuid != requestingUser.Uuid && requestingUser.AccountRole == AccountRole.User)
            {
                throw new UnauthorizedAccessException();
            }

            await _fileDal.Delete(directory.Uuid);
            string fullPath = Environment.CurrentDirectory + directory.Path;
            DirectoryHelper.DeleteDirectory(fullPath);
            await _directoryDal.Delete(directory);
        }
    }
}