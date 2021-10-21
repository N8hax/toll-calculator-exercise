using TollCalculatorExercise.Domain.Enums;

namespace TollCalculatorExercise.Domain.Models
{
    public class Vehicle
    {
        public VehicleTypeEnum VehicleType { get; }
        public Vehicle(VehicleTypeEnum vehicleType)
        {
            VehicleType = vehicleType;
        }
    }
}
