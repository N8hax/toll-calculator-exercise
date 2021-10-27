using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TollCalculatorExercise.Infrastructure.Contexts;
using TollCalculatorExercise.Services.Interfaces.Repositories;

namespace TollCalculatorExercise.Infrastructure.Repositories
{
    public class TimeSpanTollFeeRepository : ITimeSpanTollFeeRepository
    {
        private ApplicationDbContext _dbContext;
        public TimeSpanTollFeeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public decimal GetFeeByTimeSpan(TimeSpan passTime)
        {
            var maxTollFee = _dbContext.TimeSpanTollFeeList
                .Where(p => passTime >= p.StartTimeIncluded &&
                            passTime < p.EndTimeNotIncluded)
                .FirstOrDefault();

            if (maxTollFee == null)
                return 0M;

            return maxTollFee.TollFee.Amount;
        }
    }
}
