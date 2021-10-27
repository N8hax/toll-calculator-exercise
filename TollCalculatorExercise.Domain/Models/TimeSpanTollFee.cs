using System;

namespace TollCalculatorExercise.Domain.Models
{
    public class TimeSpanTollFee : Entity
    {
        public TimeSpanTollFee(TimeSpan startTimeIncluded, TimeSpan endTimeNotIncluded, TollFee tollFee)
        {
            StartTimeIncluded = startTimeIncluded;
            EndTimeNotIncluded = endTimeNotIncluded;
            TollFee = tollFee;
        }

        public TimeSpan StartTimeIncluded { get; private set; }
        public TimeSpan EndTimeNotIncluded { get; private set; }
        public TollFee TollFee { get; private set; }
    }
}
