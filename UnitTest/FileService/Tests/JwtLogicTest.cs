using File_Service.CustomExceptions;
using File_Service.Enums;
using File_Service.Logic;
using NUnit.Framework;
using System;

namespace UnitTest.FileService.Tests
{
    [TestFixture]
    public class JwtLogicTest
    {
        private readonly JwtLogic _jwtLogic;

        public JwtLogicTest()
        {
            _jwtLogic = new JwtLogic();
        }

        [Test]
        public void GetClaimsTest()
        {
            string jwt =
                "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJVdWlkIjoiZTA1OGY1NDgtM2IxOC00MTg3LWI0YzItM2QxMDEyMmY4ODdjIiwiVXNlcm5hbWUiOiJUZXN0IiwiQWNjb3VudFJvbGUiOiJVc2VyIiwiYXVkIjoiaHR0cDovL3Rlc3QuZXhhbXBsZS5jb20iLCJleHAiOjE2MTg4NjQ3MzQsImlzcyI6IkF1dGgiLCJpYXQiOjE2MTg4NjM4MzQsIm5iZiI6MTYxODg2MzgzNH0.rxkUiPdZD7fk_ar2erfTLHhwjQK1CDI9kSvgPhTifOgeq0s8M2Glbp5vIa5jIsblmaI0SL1GeWD07j8dYB3bMA";
            Assert.IsTrue(_jwtLogic.GetClaim<Guid>(jwt, JwtClaim.Uuid) != Guid.Empty);
            Assert.IsTrue(_jwtLogic.GetClaim<AccountRole>(jwt, JwtClaim.AccountRole) != AccountRole.Undefined);
        }

        [Test]
        public void GetClaimsUnprocessableExceptionTest()
        {
            Assert.Throws<UnprocessableException>(() => _jwtLogic.GetClaim<Guid>(null, JwtClaim.Uuid));
        }

        [Test]
        public void GetClaimsReturnsDefaultTest()
        {
            string jwt =
                "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJVdWlkIjoiZTA1OGY1NDgtM2IxOC00MTg3LWI0YzItM2QxMDEyMmY4ODdjIiwiVXNlcm5hbWUiOiJUZXN0IiwiQWNjb3VudFJvbGUiOiJVc2VyIiwiYXVkIjoiaHR0cDovL3Rlc3QuZXhhbXBsZS5jb20iLCJleHAiOjE2MTg4NjQ3MzQsImlzcyI6IkF1dGgiLCJpYXQiOjE2MTg4NjM4MzQsIm5iZiI6MTYxODg2MzgzNH0.rxkUiPdZD7fk_ar2erfTLHhwjQK1CDI9kSvgPhTifOgeq0s8M2Glbp5vIa5jIsblmaI0SL1GeWD07j8dYB3bMA";
            string result = _jwtLogic.GetClaim<string>(jwt, JwtClaim.Uuid);
            Assert.IsTrue(result == null);
        }
    }
}
