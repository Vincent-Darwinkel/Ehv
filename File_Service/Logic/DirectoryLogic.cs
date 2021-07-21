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
            Directory.CreateDirectory(fullPath);
            await _directoryDal.Add(new DirectoryDto
            {
                Uuid = Guid.NewGuid(),
                Name = directoryName,
                OwnerUuid = requestingUserUuid,
                Path = userSpecifiedPath
            });
        }

        public async Task<DirectoryDto> Find(string path)
        {
            return await _directoryDal.Find(path);
        }

        public async Task<List<Guid>> GetFileNamesInDirectory(string path)
        {
            if (!ValidFilePaths.FilePathIsValid(path))
            {
                throw new UnprocessableException();
            }

            DirectoryDto directory = await _directoryDal.Find(path);
            if (directory == null)
            {
                throw new KeyNotFoundException();
            }

            List<FileDto> files = await _fileDal.FindInDirectory(directory.Uuid);
            return files.Select(file => file.Uuid)
                .ToList();
        }

        public async Task Delete(string path, UserHelper requestingUser)
        {
            DirectoryDto directory = await _directoryDal.Find(path);
            if (directory.OwnerUuid != requestingUser.Uuid && requestingUser.AccountRole == AccountRole.User)
            {
                throw new UnauthorizedAccessException();
            }

            string fullPath = Environment.CurrentDirectory + path;
            DirectoryHelper.DeleteDirectory(fullPath);
            await _directoryDal.Delete(directory);
        }
    }
}