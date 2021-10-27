using System;

namespace TollCalculatorExercise.Domain.Models
{
    public class DateTollFee : Entity
    {
        public DateTollFee(DateTime startDateIncluded, DateTime endDateIncluded, TollFee tollFee)
        {
            StartDateIncluded = startDateIncluded;
            EndDateIncluded = endDateIncluded;
            TollFee = tollFee;
        }

        public DateTime StartDateIncluded { get; private set; }
        public DateTime EndDateIncluded { get; private set; }
        public TollFee TollFee { get; private set; }

    }
}
