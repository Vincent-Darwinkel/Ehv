using File_Service.CustomExceptions;
using File_Service.Logic;
using File_Service.Models.HelperFiles;
using NUnit.Framework;
using System;
using System.IO;

namespace UnitTest.FileService.Tests
{
    [TestFixture]
    public class TestFileLogic
    {
        private readonly FileLogic _fileLogic;

        public TestFileLogic()
        {
            var fileHelper = new FileHelper(null);
            _fileLogic = new FileLogic(fileHelper);
        }

        [Test]
        public void FindFileNotFoundExceptionTest()
        {
            Assert.ThrowsAsync<FileNotFoundException>(() => _fileLogic.Find(Guid.Empty));
        }

        [Test]
        public void SaveFileUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _fileLogic.SaveFile(null, null, Guid.Empty));
            Assert.ThrowsAsync<UnprocessableException>(() => _fileLogic.SaveFile(null, "test", Guid.Empty));
        }

        [Test]
        public void RemoveTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _fileLogic.Delete(Guid.Empty, null));
        }
    }
}