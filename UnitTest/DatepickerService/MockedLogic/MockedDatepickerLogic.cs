using Datepicker_Service.Logic;
using Datepicker_Service.Models.HelperFiles;
using Datepicker_Service.RabbitMq.Publishers;
using Datepicker_Service.RabbitMq.Rpc;
using Moq;
using RabbitMQ.Client;
using UnitTest.DatepickerService.MockedDals;
using UnitTest.DatepickerService.TestModels;

namespace UnitTest.DatepickerService.MockedLogic
{
    public class MockedDatepickerLogic
    {
        public readonly DatepickerLogic DatepickerLogic;

        public MockedDatepickerLogic()
        {
            var mockedDatepickerDal = new MockedDatepickerDal().DatepickerDal;
            var mockedDatepickerDateDal = new MockedDatepickerDateDal().DatepickerDateDal;
            var mockedPublisher = new Mock<IPublisher>();
            var mockedRpcClient = new Mock<IRpcClient>();

            var testDatepicker = new TestDatepickerDto();
            mockedRpcClient.Setup(rpc => rpc.Call<bool>(testDatepicker.DatepickerDuplicatedEventName.Title, RabbitMqQueues.ExistsEventQueue)).Returns(true);
            var mockedChannel = new Mock<IModel>().Object;
            var autoMapperConfig = AutoMapperConfig.Config.CreateMapper();
            var datepickerLogic = new DatepickerLogic(mockedDatepickerDal, mockedChannel, mockedPublisher.Object,
                mockedRpcClient.Object, autoMapperConfig, mockedDatepickerDateDal);

            DatepickerLogic = datepickerLogic;
        }
    }
}
