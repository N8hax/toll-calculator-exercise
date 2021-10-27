using TollCalculatorExercise.Domain.Enums;

namespace TollCalculatorExercise.Services.Interfaces.Repositories
{
    public interface IVehicleTypeTollFeeRepository
    {
        /// <summary>
        /// Check if the vehicle type is fee-free.
        /// </summary>
        /// <param name="vehicleType">The vehicle type which has passed the toll.</param>
        /// <returns>true for free vehicle type otherwise false.</returns>
        bool IsTollFree(VehicleTypeEnum vehicleType);
    }
}
