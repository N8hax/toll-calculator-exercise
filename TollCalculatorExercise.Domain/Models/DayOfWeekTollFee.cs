using System;

namespace TollCalculatorExercise.Domain.Models
{
    public class DayOfWeekTollFee : TollFeeBase
    {
        public DayOfWeek DayOfWeek { get; set; }
    }
}
