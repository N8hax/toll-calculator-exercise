using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TollCalculatorExercise.Infrastructure.Contexts;
using TollCalculatorExercise.Services.Interfaces.Repositories;

namespace TollCalculatorExercise.Infrastructure.Repositories
{
    public class DateTollFeeRepository : IDateTollFeeRepository
    {
        private ApplicationDbContext _dbContext;
        public DateTollFeeRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool IsTollFree(DateTime desiredDate)
        {
            return
                //Check if the date is a Holiday 
                _dbContext.DateTollFeeList.Any(d => d.TollFee.Amount == 0 && desiredDate.Date >= d.StartDateIncluded.Date && desiredDate.Date <= d.EndDateIncluded.Date);
        }
    }
}
