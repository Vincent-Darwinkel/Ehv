using System;
using System.Threading.Tasks;
using Datepicker_Service.Dal.Interfaces;
using Datepicker_Service.Models;

namespace Datepicker_Service.Dal
{
    public class DatepickerDal : IDatepickerDal
    {
        public async Task Add(DatepickerDto datepicker)
        {
            throw new NotImplementedException();
        }

        public async Task<DatepickerDto> Find(Guid uuid)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Exists(string title)
        {
            throw new NotImplementedException();
        }

        public async Task Update(DatepickerDto datepicker)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(Guid uuid)
        {
            throw new NotImplementedException();
        }
    }
}
