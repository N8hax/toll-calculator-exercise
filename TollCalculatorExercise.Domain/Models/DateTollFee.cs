using System;

namespace TollCalculatorExercise.Domain.Models
{
    public class DateTollFee : TollFeeBase
    {
        public DateTime StartDateIncluded { get; set; }
        public DateTime EndDateIncluded { get; set; }
    }
}
