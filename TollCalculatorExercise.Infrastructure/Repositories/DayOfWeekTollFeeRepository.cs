using System;
using System.Linq;
using TollCalculatorExercise.Infrastructure.Contexts;
using TollCalculatorExercise.Services.Interfaces.Repositories;

namespace TollCalculatorExercise.Infrastructure.Repositories
{
    public class DayOfWeekTollFeeRepository : IDayOfWeekTollFeeRepository
    {
        private ApplicationDbContext _dbContext;
        public DayOfWeekTollFeeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool IsTollFree(DateTime desiredDate)
        {
            return
                //Check if the date is a weekend 
                _dbContext.DayOfWeekTollFeeList.Any(d => d.TollFee.Amount == 0 && d.DayOfWeek == desiredDate.DayOfWeek);
        }
    }
}
