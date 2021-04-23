using Datepicker_Service.Dal.Interfaces;
using Datepicker_Service.UnitTests.TestModels;
using Moq;

namespace Datepicker_Service.UnitTests.MockedDals
{
    public class MockedDatepickerDal
    {
        public readonly IDatepickerDal DatepickerDal;

        public MockedDatepickerDal()
        {
            var testDatepicker = new TestDatepickerDto().Datepicker;
            var datepickerDalMock = new Mock<IDatepickerDal>();
            datepickerDalMock.Setup(dpd => dpd.Find(testDatepicker.Uuid)).ReturnsAsync(testDatepicker);
            datepickerDalMock.Setup(dpd => dpd.Exists(testDatepicker.Title)).ReturnsAsync(true);

            DatepickerDal = datepickerDalMock.Object;
        }
    }
}