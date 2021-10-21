using System;

namespace TollCalculatorExercise.Domain.Models
{
    public class TimeSpanTollFee : TollFeeBase
    {
        public TimeSpan StartTimeIncluded { get; set; }
        public TimeSpan EndTimeNotIncluded { get; set; }
    }
}
