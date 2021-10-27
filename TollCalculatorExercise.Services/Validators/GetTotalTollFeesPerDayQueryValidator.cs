using FluentValidation;
using System;
using System.Linq;
using TollCalculatorExercise.Services.Features.TollFee.Queries;

namespace TollCalculatorExercise.Services.Validators
{
    public class GetTotalTollFeesPerDayQueryValidator : AbstractValidator<GetTotalTollFeesPerDayQuery>
    {
        public GetTotalTollFeesPerDayQueryValidator()
        {
            RuleFor(m => m.VehicleType)
                .NotEmpty()
                .IsInEnum().WithMessage("Vehicle Type is not valid");

            RuleFor(m => m.Dates)
                .NotEmpty()
                .Must(AreInSameDay).WithMessage("Dates should be in same day")
                .Must(AreBeforeCurrentMoment).WithMessage("Dates should not be in the future");
        }

        private bool AreInSameDay(DateTime[] dates)
        {
            if (dates == null || dates.Length == 0)
                return false;
            Array.Sort(dates);
            // Check if dates are in same day
            return dates[0].Date == dates[dates.Length - 1].Date;
        }

        private bool AreBeforeCurrentMoment(DateTime[] dates)
        {
            if (dates == null || dates.Length == 0)
                return false;
            Array.Sort(dates);
            // Check if dates are not in the future
            return !dates.Any(d => d.Date > DateTime.Now);
        }
    }
}
