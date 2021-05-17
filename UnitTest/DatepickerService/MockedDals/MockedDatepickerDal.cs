using Datepicker_Service.Dal.Interfaces;
using Moq;
using UnitTest.DatepickerService.TestModels;

namespace UnitTest.DatepickerService.MockedDals
{
    public class MockedDatepickerDal
    {
        public readonly IDatepickerDal DatepickerDal;

        public MockedDatepickerDal()
        {
            var testDatepicker = new TestDatepickerDto();
            var datepickerDalMock = new Mock<IDatepickerDal>();
            datepickerDalMock.Setup(dpd => dpd.Find(testDatepicker.Datepicker.Uuid)).ReturnsAsync(testDatepicker.Datepicker);
            datepickerDalMock.Setup(dpd => dpd.Exists(testDatepicker.Datepicker.Title)).ReturnsAsync(true);
            datepickerDalMock.Setup(dpd => dpd.Exists(testDatepicker.Datepicker2.Title)).ReturnsAsync(true);
            datepickerDalMock.Setup(dpd => dpd.Exists(testDatepicker.DatepickerDuplicatedEventName.Title)).ReturnsAsync(true);
            datepickerDalMock.Setup(dpd => dpd.Find(testDatepicker.DatepickerDuplicatedEventName.Uuid))
                .ReturnsAsync(testDatepicker.DatepickerDuplicatedEventName);
            datepickerDalMock.Setup(dpd => dpd.Find(testDatepicker.Datepicker2.Uuid))
                .ReturnsAsync(testDatepicker.Datepicker2);

            DatepickerDal = datepickerDalMock.Object;
        }
    }
}