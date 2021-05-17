using Datepicker_Service.Models.FromFrontend;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest.DatepickerService.TestModels.FromFrontend
{
    public class TestDatepickerConversion
    {
        public readonly DatePickerConversion DatePickerConversion = new DatePickerConversion
        {
            DatepickerUuid = new TestDatepickerDto().Datepicker.Uuid,
            SelectedDates = new TestDatepickerDto().Datepicker.Dates
                .Select(dpd => dpd.Uuid)
                .ToList(),
            EventSteps = new List<EventStep>
            {
                new EventStep
                {
                    StepNr = 0,
                    Text = "Test"
                }
            }
        };
        public readonly DatePickerConversion DatePickerConversionNoDates = new DatePickerConversion
        {
            DatepickerUuid = new TestDatepickerDto().Datepicker.Uuid,
            SelectedDates = new List<Guid>(),
            EventSteps = new List<EventStep>
            {
                new EventStep
                {
                    StepNr = 0,
                    Text = "Test"
                }
            }
        };
        public readonly DatePickerConversion DatePickerConversionNotExist = new DatePickerConversion
        {
            DatepickerUuid = Guid.Parse("602f3c57-d206-400d-99d2-0174822dc70e"),
            SelectedDates = new List<Guid>(),
            EventSteps = new List<EventStep>
            {
                new EventStep
                {
                    StepNr = 0,
                    Text = "Test"
                }
            }
        };
        public readonly DatePickerConversion DatePickerConversionInvalidDates = new DatePickerConversion
        {
            DatepickerUuid = new TestDatepickerDto().Datepicker.Uuid,
            SelectedDates = new List<Guid> { Guid.Parse("56225bd3-c71b-4f2f-843b-97c0e7229998") },
            EventSteps = new List<EventStep>
            {
                new EventStep
                {
                    StepNr = 0,
                    Text = "Test"
                }
            }
        };
    }
}