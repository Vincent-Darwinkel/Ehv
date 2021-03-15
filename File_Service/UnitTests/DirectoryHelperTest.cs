using System;
using System.Collections.Generic;
using File_Service.HelperFiles;
using NUnit.Framework;

namespace File_Service.UnitTests
{
    [TestFixture]
    public class DirectoryHelperTest
    {
        [Test]
        public void PathIsValidTest()
        {
            var pathsToTest = new List<string>();
            pathsToTest.AddRange(FilePaths.AllowedImagePaths);
            pathsToTest.AddRange(FilePaths.AllowedVideoPaths);

            foreach (var path in pathsToTest)
            {
                Assert.IsTrue(DirectoryHelper.PathIsValid(path));
            }
        }
    }
}
