using Datepicker_Service.Dal.Interfaces;
using System;
using System.Threading.Tasks;

namespace Datepicker_Service.Logic
{
    public class DatepickerDateLogic
    {
        private readonly IDatepickerDateDal _datepickerDateDal;

        public DatepickerDateLogic(IDatepickerDateDal datepickerDateDal)
        {
            _datepickerDateDal = datepickerDateDal;
        }

        public async Task DeleteUserFromDate(Guid userUuid)
        {
            await _datepickerDateDal.Delete(userUuid);
        }
    }
}