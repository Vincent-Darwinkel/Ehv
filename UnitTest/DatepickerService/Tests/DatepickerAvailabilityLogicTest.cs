using Datepicker_Service.Logic;
using NUnit.Framework;
using System.Linq;
using UnitTest.DatepickerService.MockedLogic;
using UnitTest.DatepickerService.TestModels;
using UnitTest.DatepickerService.TestModels.FromFrontend;

namespace UnitTest.DatepickerService.Tests
{
    [TestFixture]
    public class DatepickerAvailabilityLogicTest
    {
        private readonly DatepickerAvailabilityLogic _datepickerAvailabilityLogicTest;

        public DatepickerAvailabilityLogicTest()
        {
            _datepickerAvailabilityLogicTest = new MockedDatepickerAvailabilityLogic().DatepickerAvailabilityLogic;
        }

        [Test]
        public void AddOrUpdateAsyncTest()
        {
            var testUser = new TestUser().User;
            var testDatepicker = new TestDatepickerDto().Datepicker;
            var availableDates = testDatepicker.Dates
                .Select(d => d.Uuid)
                .ToList();

            Assert.DoesNotThrowAsync(() => _datepickerAvailabilityLogicTest.AddOrUpdateAsync(availableDates, testDatepicker.Uuid, testUser));
        }
    }
}