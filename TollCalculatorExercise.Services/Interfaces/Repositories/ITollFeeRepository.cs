using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TollCalculatorExercise.Domain.Enums;

namespace TollCalculatorExercise.Services.Interfaces.Repositories
{
    public interface ITollFeeRepository
    {
        /// <summary>
        /// Get the maximum possible fee for a pass time.
        /// </summary>
        /// <param name="passTime">The time when a vehicle has passed the toll.</param>
        /// <returns>Maximun fee. If no fees configured it returns zero.</returns>
        decimal GetMaxFeeByTimeSpanAsync(TimeSpan passTime);

        /// <summary>
        /// Check if the vehicle type is fee-free.
        /// </summary>
        /// <param name="vehicleType">The vehicle type which has passed the toll.</param>
        /// <returns>true for free vehicle type otherwise false.</returns>
        bool IsTollFreeAsync(VehicleTypeEnum vehicleType);

        /// <summary>
        /// Check if the date or the weekday is fee-free.
        /// </summary>
        /// <param name="desiredDate">The date when a vehicle has passed the toll.</param>
        /// <returns>true for free dates otherwise false.</returns>
        bool IsTollFreeAsync(DateTime desiredDate);

    }
}
