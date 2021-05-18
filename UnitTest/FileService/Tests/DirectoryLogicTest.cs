using System;
using System.IO;
using File_Service.CustomExceptions;
using File_Service.Logic;
using File_Service.Models.FromFrontend;
using File_Service.Models.HelperFiles;
using NUnit.Framework;

namespace UnitTest.FileService.Tests
{
    [TestFixture]
    public class DirectoryLogicTest
    {
        private readonly DirectoryLogic _directoryLogic;

        public DirectoryLogicTest()
        {
            _directoryLogic = new DirectoryLogic();
        }

        [Test]
        public void GetItemsUnprocessableExceptionTest()
        {
            Assert.Throws<UnprocessableException>(() => _directoryLogic.GetItems(null));
        }

        [Test]
        public void GetDirectoryInfoUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _directoryLogic.GetDirectoryInfo(null, Guid.Empty));
        }

        [Test]
        public void GetDirectoryInfoFileNotFoundExceptionTest()
        {
            Assert.ThrowsAsync<FileNotFoundException>(() => _directoryLogic.GetDirectoryInfo("/public/gallery/23jfwoifwejifff", Guid.Empty));
        }

        [Test]
        public void CreateFolderUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _directoryLogic.CreateFolder(new FolderUpload(), Guid.Empty));
        }

        [Test]
        public void RemoveDirectoryUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _directoryLogic.Delete(null, new UserHelper()));
        }
    }
}