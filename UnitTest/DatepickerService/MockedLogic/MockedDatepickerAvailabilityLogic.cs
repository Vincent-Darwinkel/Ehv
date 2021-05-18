using Datepicker_Service.Dal.Interfaces;
using Datepicker_Service.Logic;
using Moq;
using UnitTest.DatepickerService.MockedDals;

namespace UnitTest.DatepickerService.MockedLogic
{
    public class MockedDatepickerAvailabilityLogic
    {
        public readonly DatepickerAvailabilityLogic DatepickerAvailabilityLogic;

        public MockedDatepickerAvailabilityLogic()
        {
            var mockedDatepickerAvailabilityDal = new Mock<IDatepickerAvailabilityDal>();
            var mockedDatepickerDateDal = new MockedDatepickerDateDal();
            var datepickerAvailabilityLogic = new DatepickerAvailabilityLogic(mockedDatepickerAvailabilityDal.Object, mockedDatepickerDateDal.DatepickerDateDal);
            DatepickerAvailabilityLogic = datepickerAvailabilityLogic;
        }
    }
}