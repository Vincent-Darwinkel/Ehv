using Datepicker_Service.Dal.Interfaces;
using Datepicker_Service.Models;
using Datepicker_Service.Models.HelperFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Datepicker_Service.Logic
{
    public class DatepickerAvailabilityLogic
    {
        private readonly IDatepickerAvailabilityDal _datepickerAvailabilityDal;
        private readonly IDatepickerDateDal _datepickerDateDal;

        public DatepickerAvailabilityLogic(IDatepickerAvailabilityDal datepickerAvailabilityDal, IDatepickerDateDal datepickerDateDal)
        {
            _datepickerAvailabilityDal = datepickerAvailabilityDal;
            _datepickerDateDal = datepickerDateDal;
        }

        public async Task AddOrUpdateAsync(List<Guid> availableDateUuidCollection, Guid datepickerUuid, UserHelper requestingUser)
        {
            List<DatepickerDateDto> datePickerDates = await _datepickerDateDal.Find(datepickerUuid) ?? new List<DatepickerDateDto>();
            IEnumerable<Guid> availabilitiesToRemove = datePickerDates.Select(dpd => dpd.Uuid);

            var availabilitiesToAdd = new List<DatepickerAvailabilityDto>();
            availableDateUuidCollection.ForEach(dateUuid => availabilitiesToAdd.Add(new DatepickerAvailabilityDto
            {
                DateUuid = dateUuid,
                UserUuid = requestingUser.Uuid,
                Uuid = Guid.NewGuid()
            }));

            await _datepickerAvailabilityDal.Delete(availabilitiesToRemove, requestingUser.Uuid);
            await _datepickerAvailabilityDal.Add(availabilitiesToAdd);
        }
    }
}
