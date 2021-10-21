using System;
using System.Collections.Generic;
using System.Text;

namespace TollCalculatorExercise.Domain.Settings
{
    public class ApplicationSettings
    {
        public decimal MAX_FEES_PER_DAY { get; set; }
        public int CHARGE_INTERVAL_IN_SECONDS { get; set; }
    }
}
