using TollCalculatorExercise.Domain.Enums;

namespace TollCalculatorExercise.Domain.Models
{
    public class VehicleTypeTollFee : TollFeeBase
    {
        public VehicleTypeEnum VehicleType { get; set; }
    }
}
