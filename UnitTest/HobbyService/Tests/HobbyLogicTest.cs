using Hobby_Service.CustomExceptions;
using Hobby_Service.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnitTest.HobbyService.MockedLogics;
using UnitTest.HobbyService.TestModels;

namespace UnitTest.HobbyService.Tests
{
    [TestFixture]
    public class HobbyLogicTest
    {
        private readonly HobbyLogic _hobbyLogic;

        public HobbyLogicTest()
        {
            _hobbyLogic = new MockedHobbyLogic().HobbyLogic;
        }

        [Test]
        public void AddUnprocessableExceptionTest()
        {
            var hobby = new TestHobbyDto().Empty;
            Assert.ThrowsAsync<UnprocessableException>(() => _hobbyLogic.Add(hobby));
        }

        [Test]
        public void AddTest()
        {
            var hobby = new TestHobbyDto().Hobby;
            Assert.DoesNotThrowAsync(() => _hobbyLogic.Add(hobby));
        }

        [Test]
        public void AllTest()
        {
            Assert.DoesNotThrowAsync(() => _hobbyLogic.All());
        }

        [Test]
        public void AllRabbitMqTest()
        {
            Assert.DoesNotThrowAsync(() => _hobbyLogic.AllRabbitMq());
        }

        [Test]
        public void UpdateTest()
        {
            var hobby = new TestHobbyDto().Hobby;
            Assert.DoesNotThrowAsync(() => _hobbyLogic.Update(hobby));
        }

        [Test]
        public void UpdateUnprocessableExceptionTest()
        {
            var hobby = new TestHobbyDto().Empty;
            Assert.ThrowsAsync<UnprocessableException>(() => _hobbyLogic.Update(hobby));
        }

        [Test]
        public void DeleteTest()
        {
            var uuidCollection = new List<Guid> { new TestHobbyDto().Hobby.Uuid };
            Assert.DoesNotThrowAsync(() => _hobbyLogic.Delete(uuidCollection));
        }

        [Test]
        public void DeleteUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _hobbyLogic.Delete(new List<Guid>()));
        }
    }
}