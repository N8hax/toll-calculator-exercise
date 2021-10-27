using System;

namespace TollCalculatorExercise.Domain.Models
{
    public class DayOfWeekTollFee : Entity
    {
        public DayOfWeekTollFee(DayOfWeek dayOfWeek, TollFee tollFee)
        {
            DayOfWeek = dayOfWeek;
            TollFee = tollFee;
        }

        public DayOfWeek DayOfWeek { get; private set; }
        public TollFee TollFee { get; private set; }
    }
}
