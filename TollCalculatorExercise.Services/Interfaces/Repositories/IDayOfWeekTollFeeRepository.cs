using System;

namespace TollCalculatorExercise.Services.Interfaces.Repositories
{
    public interface IDayOfWeekTollFeeRepository
    {
        /// <summary>
        /// Check if the weekday is fee-free.
        /// </summary>
        /// <param name="desiredDate">The date when a vehicle has passed the toll.</param>
        /// <returns>true for free weekdays otherwise false.</returns>
        bool IsTollFree(DateTime desiredDate);
    }
}
