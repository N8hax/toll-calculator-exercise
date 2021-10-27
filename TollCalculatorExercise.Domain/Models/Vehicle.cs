using TollCalculatorExercise.Domain.Enums;

namespace TollCalculatorExercise.Domain.Models
{
    public class Vehicle
    {
        public Vehicle(VehicleTypeEnum vehicleType)
        {
            VehicleType = vehicleType;
        }
        public VehicleTypeEnum VehicleType { get; private set; }
    }
}
