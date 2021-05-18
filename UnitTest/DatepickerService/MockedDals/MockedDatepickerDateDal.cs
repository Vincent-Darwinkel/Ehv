using Datepicker_Service.Dal.Interfaces;
using Moq;
using UnitTest.DatepickerService.TestModels;

namespace UnitTest.DatepickerService.MockedDals
{
    public class MockedDatepickerDateDal
    {
        public readonly IDatepickerDateDal DatepickerDateDal;

        public MockedDatepickerDateDal()
        {
            var testDatepicker = new TestDatepickerDto().Datepicker;
            var datepickerDalDateMock = new Mock<IDatepickerDateDal>();
            datepickerDalDateMock.Setup(dpd => dpd.Find(testDatepicker.Uuid)).ReturnsAsync(testDatepicker.Dates);

            DatepickerDateDal = datepickerDalDateMock.Object;
        }
    }
}
