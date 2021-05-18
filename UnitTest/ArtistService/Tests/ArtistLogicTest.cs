using System;
using System.Collections.Generic;
using Favorite_Artist_Service.CustomExceptions;
using Favorite_Artist_Service.Logic;
using NUnit.Framework;
using UnitTest.ArtistService.MockedLogics;
using UnitTest.ArtistService.TestModels;

namespace UnitTest.ArtistService.Tests
{
    [TestFixture]
    public class ArtistLogicTest
    {
        private readonly FavoriteArtistLogic _artistLogic;

        public ArtistLogicTest()
        {
            _artistLogic = new MockedArtistLogic().ArtistLogic;
        }

        [Test]
        public void AddUnprocessableExceptionTest()
        {
            var artist = new TestArtistDto().Empty;
            Assert.ThrowsAsync<UnprocessableException>(() => _artistLogic.Add(artist));
        }

        [Test]
        public void AddTest()
        {
            var artist = new TestArtistDto().Artist;
            Assert.DoesNotThrowAsync(() => _artistLogic.Add(artist));
        }

        [Test]
        public void AllTest()
        {
            Assert.DoesNotThrowAsync(() => _artistLogic.All());
        }

        [Test]
        public void AllRabbitMqTest()
        {
            Assert.DoesNotThrowAsync(() => _artistLogic.AllRabbitMq());
        }

        [Test]
        public void UpdateTest()
        {
            var artist = new TestArtistDto().Artist;
            Assert.DoesNotThrowAsync(() => _artistLogic.Update(artist));
        }

        [Test]
        public void UpdateUnprocessableExceptionTest()
        {
            var artist = new TestArtistDto().Empty;
            Assert.ThrowsAsync<UnprocessableException>(() => _artistLogic.Update(artist));
        }

        [Test]
        public void DeleteTest()
        {
            var uuidCollection = new List<Guid> { new TestArtistDto().Artist.Uuid };
            Assert.DoesNotThrowAsync(() => _artistLogic.Delete(uuidCollection));
        }

        [Test]
        public void DeleteUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _artistLogic.Delete(new List<Guid>()));
        }
    }
}