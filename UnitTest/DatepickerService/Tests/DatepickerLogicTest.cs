using Datepicker_Service.CustomExceptions;
using Datepicker_Service.Logic;
using Datepicker_Service.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using UnitTest.DatepickerService.MockedLogic;
using UnitTest.DatepickerService.TestModels;
using UnitTest.DatepickerService.TestModels.FromFrontend;

namespace UnitTest.DatepickerService.Tests
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
        public void AddDuplicateNameException()
        {
            var testDatepicker = new TestDatepickerDto().DatepickerDuplicatedEventName;
            var testUser = new TestUser().User;
            Assert.ThrowsAsync<DuplicateNameException>(() => _datepickerLogic.Add(testDatepicker, testUser));
        }

        [Test]
        public void AddExpiredDatepickerUnprocessableException()
        {
            var testDatepicker = new TestDatepickerDto().DatepickerNotExisting;
            var testUser = new TestUser().User;
            Assert.DoesNotThrowAsync(() => _datepickerLogic.Add(testDatepicker, testUser));
        }

        [Test]
        public void AddDatepickerNoDatesUnprocessableException()
        {
            var testDatepicker = new TestDatepickerDto().DatepickerNoDates;
            var testUser = new TestUser().User;
            Assert.ThrowsAsync<UnprocessableException>(() => _datepickerLogic.Add(testDatepicker, testUser));
        }

        [Test]
        public void ConvertDatepickerUnprocessableExceptionTest()
        {
            var testDatepickerConversion = new TestDatepickerConversion().DatePickerConversionNoDates;
            var testUser = new TestUser().User;
            Assert.ThrowsAsync<UnprocessableException>(() => _datepickerLogic.ConvertDatepicker(testDatepickerConversion, testUser));
        }

        [Test]
        public void ConvertDatepickerNoNullAllowedExceptionTest()
        {
            var testDatepickerConversion = new TestDatepickerConversion().DatePickerConversionNotExist;
            var testUser = new TestUser().User;
            Assert.ThrowsAsync<UnprocessableException>(() => _datepickerLogic.ConvertDatepicker(testDatepickerConversion, testUser));
        }

        [Test]
        public void ConvertDatepickerUnauthorizedAccessExceptionTest()
        {
            var testDatepickerConversion = new TestDatepickerConversion().DatePickerConversion;
            var testUser = new TestUser().SiteAdmin;
            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _datepickerLogic.ConvertDatepicker(testDatepickerConversion, testUser));
        }

        [Test]
        public void ConvertDatepickerDatesNotInDatepickerUnprocessableExceptionTest()
        {
            var testDatepickerConversion = new TestDatepickerConversion().DatePickerConversionInvalidDates;
            var testUser = new TestUser().User;
            Assert.ThrowsAsync<UnprocessableException>(() => _datepickerLogic.ConvertDatepicker(testDatepickerConversion, testUser));
        }

        [Test]
        public void ConvertDatepickerTest()
        {
            var testDatepickerConversion = new TestDatepickerConversion().DatePickerConversion;
            var testUser = new TestUser().User;
            Assert.DoesNotThrowAsync(() => _datepickerLogic.ConvertDatepicker(testDatepickerConversion, testUser));
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
        public void FindKeyNotFoundExceptionTest()
        {
            var testDatepicker = new TestDatepickerDto().DatepickerNotExisting;
            Assert.ThrowsAsync<KeyNotFoundException>(() => _datepickerLogic.Find(testDatepicker.Uuid));
        }

        [Test]
        public void AllTest()
        {
            Assert.DoesNotThrowAsync(() => _datepickerLogic.All());
        }

        [Test]
        public void UpdateUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _datepickerLogic.Update(new DatepickerDto()));
        }

        [Test]
        public void UpdateDuplicatedNameExceptionTest()
        {
            var datepicker = new TestDatepickerDto().Datepicker;
            datepicker.Title = new TestDatepickerDto().Datepicker2.Title;
            Assert.ThrowsAsync<DuplicateNameException>(() => _datepickerLogic.Update(datepicker));
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
            var testDatepicker = new TestDatepickerDto().DatepickerNotExisting;
            Assert.ThrowsAsync<KeyNotFoundException>(() => _datepickerLogic.Delete(testDatepicker.Uuid, Guid.Empty));
        }

        [Test]
        public void DeleteUnauthorizedAccessExceptionTest()
        {
            var testDatepicker = new TestDatepickerDto().Datepicker;
            var testUser = new TestUser().SiteAdmin;
            Assert.ThrowsAsync<UnauthorizedAccessException>(() => _datepickerLogic.Delete(testDatepicker.Uuid, testUser.Uuid));
        }

        [Test]
        public void DeleteUnprocessableExceptionTest()
        {
            Assert.ThrowsAsync<UnprocessableException>(() => _datepickerLogic.Delete(Guid.Empty, Guid.Empty));
        }
    }
}
