using TollCalculatorExercise.Domain.Enums;

namespace TollCalculatorExercise.Domain.Models
{
    public class VehicleTypeTollFee : Entity
    {
        public VehicleTypeTollFee(VehicleTypeEnum vehicleType, TollFee tollFee)
        {
            VehicleType = vehicleType;
            TollFee = tollFee;
        }

        public VehicleTypeEnum VehicleType { get; private set; }
        public TollFee TollFee { get; private set; }
    }
}
