using System.Linq;
using TollCalculatorExercise.Domain.Enums;
using TollCalculatorExercise.Infrastructure.Contexts;
using TollCalculatorExercise.Services.Interfaces.Repositories;

namespace TollCalculatorExercise.Infrastructure.Repositories
{
    public class VehicleTypeTollFeeRepository: IVehicleTypeTollFeeRepository
    {
        private ApplicationDbContext _dbContext;
        public VehicleTypeTollFeeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool IsTollFree(VehicleTypeEnum vehicleType)
        {
            // Check if the Vehicle type is fee-free
            return _dbContext.VehicleTypeTollFeeList.Any(v => v.TollFee.Amount == 0 && v.VehicleType == vehicleType);
        }
    }
}
