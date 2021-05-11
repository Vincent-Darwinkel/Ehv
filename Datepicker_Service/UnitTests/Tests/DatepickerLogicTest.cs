using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Datepicker_Service.CustomExceptions;
using Datepicker_Service.Logic;
using Datepicker_Service.Models;
using Datepicker_Service.UnitTests.MockedLogic;
using Datepicker_Service.UnitTests.TestModels;
using Datepicker_Service.UnitTests.TestModels.FromFrontend;
using NUnit.Framework;

namespace Datepicker_Service.UnitTests.Tests
{
    [TestFixture]
    public class DatepickerLogicTest
    {
        private readonly DatepickerLogic _datepickerLogic;

        public DatepickerLogicTest()
        {
            _datepickerLogic = new MockedDatepickerLogic().DatepickerLogic;
        }

        [Test]
        public void AddUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _datepickerLogic.Add(new DatepickerDto(), new TestUser().User));
        }

        [Test]
        public async Task FindTest()
        {
            var testDatepicker = new TestDatepickerDto().Datepicker;
            DatepickerDto result = await _datepickerLogic.Find(testDatepicker.Uuid);
            Assert.NotNull(result);
        }

        [Test]
        public void FindUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _datepickerLogic.Find(Guid.Empty));
        }

        [Test]
        public void UpdateUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _datepickerLogic.Update(new DatepickerDto()));
        }

        [Test]
        public void UpdateTest()
        {
            Assert.DoesNotThrowAsync(() => _datepickerLogic.Update(new TestDatepickerDto().DatepickerNoUsers));
        }

        [Test]
        public void DeleteTest()
        {
            var datepicker = new TestDatepickerDto().DatepickerNoUsers;
            Assert.DoesNotThrowAsync(() => _datepickerLogic.Delete(datepicker.Uuid, new TestUser().User.Uuid));
        }

        [Test]
        public void DeleteKeyNotFoundExceptionTest()
        {
            Assert.ThrowsAsync<KeyNotFoundException>(() => _datepickerLogic.Delete(Guid.Parse("af128fe3-d828-4b44-9411-bdf27235f34d"), Guid.Empty));
        }

        [Test]
        public void DeleteUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _datepickerLogic.Delete(Guid.Empty, Guid.Empty));
        }
    }
}
