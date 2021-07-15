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
            //_fileLogic = new FileLogic(fileHelper);
        }

        //todo add test
    }
}