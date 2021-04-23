using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Datepicker_Service.Logic;
using Datepicker_Service.RabbitMq.Publishers;
using Datepicker_Service.UnitTests.MockedDals;
using Moq;

namespace Datepicker_Service.UnitTests.MockedLogic
{
    public class MockedDatepickerLogic
    {
        public readonly DatepickerLogic DatepickerLogic;

        public MockedDatepickerLogic()
        {
            var mockedDatepickerDal = new MockedDatepickerDal().DatepickerDal;
            var mockedPublisher = new Mock<IPublisher>();
            var datepickerLogic = new DatepickerLogic(mockedDatepickerDal, null, mockedPublisher.Object);

            DatepickerLogic = datepickerLogic;
        }
    }
}
