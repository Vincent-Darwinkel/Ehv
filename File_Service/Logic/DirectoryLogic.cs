using File_Service.Dal.Interfaces;
using File_Service.Models;
using File_Service.Models.HelperFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace File_Service.Logic
{
    public class DirectoryLogic
    {
        private readonly IDirectoryDal _directoryDal;

        public DirectoryLogic(IDirectoryDal directoryDal)
        {
            _directoryDal = directoryDal;
        }

        /// <summary>
        /// Creates a directory and saves it to the database
        /// </summary>
        /// <param name="userSpecifiedPath"></param>
        /// <param name="requestingUserUuid"></param>
        public async Task CreateDirectory(string userSpecifiedPath, Guid requestingUserUuid)
        {
            int index = userSpecifiedPath.LastIndexOf("/", StringComparison.Ordinal);
            string directoryName = userSpecifiedPath.Substring(index, userSpecifiedPath.Length);
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
    }
}