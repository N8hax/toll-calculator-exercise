using System;

namespace TollCalculatorExercise.Services.Interfaces.Repositories
{
    public interface ITimeSpanTollFeeRepository
    {
        /// <summary>
        /// Get the fee for a pass time.
        /// </summary>
        /// <param name="passTime">The time when a vehicle has passed the toll.</param>
        /// <returns>fee. If no fees configured it returns zero.</returns>
        decimal GetFeeByTimeSpan(TimeSpan passTime);
    }
}
