using System;

namespace TollCalculatorExercise.Services.Interfaces.Repositories
{
    public interface IDateTollFeeRepository
    {
        /// <summary>
        /// Check if the date is fee-free.
        /// </summary>
        /// <param name="desiredDate">The date when a vehicle has passed the toll.</param>
        /// <returns>true for free dates otherwise false.</returns>
        bool IsTollFree(DateTime desiredDate);
    }
}
