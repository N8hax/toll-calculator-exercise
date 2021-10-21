using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollCalculatorExercise.Domain.Enums;
using TollCalculatorExercise.Domain.Models;
using TollCalculatorExercise.Domain.Settings;
using TollCalculatorExercise.Services.Exceptions;
using TollCalculatorExercise.Services.Interfaces.Repositories;

namespace TollCalculatorExercise.Infrastructure.Repositories
{
    public class TollFeeRepository : ITollFeeRepository
    {
        

        // static lists, In the real world example we will get the required data from the database
        private readonly IEnumerable<DateTollFee> _dateTollFeeList;
        private readonly IEnumerable<DayOfWeekTollFee> _dayOfWeekTollFeeList;
        private readonly IEnumerable<TimeSpanTollFee> _timeSpanTollFeeList;
        private readonly IEnumerable<VehicleTypeTollFee> _vehicleTypeTollFee;
        public TollFeeRepository()
        {
            // static list, In the real world example we will get the required data from the database
            _dateTollFeeList = new List<DateTollFee>()
            {
                new DateTollFee {StartDateIncluded = new DateTime(2013,1,1), EndDateIncluded = new DateTime(2013,1,1), Amount=0M},
                new DateTollFee {StartDateIncluded = new DateTime(2013,3,28), EndDateIncluded = new DateTime(2013,3,28), Amount=0M},
                new DateTollFee {StartDateIncluded = new DateTime(2013,3,29), EndDateIncluded = new DateTime(2013,3,29), Amount=0M},
                new DateTollFee {StartDateIncluded = new DateTime(2013,4,1), EndDateIncluded = new DateTime(2013,4,1), Amount=0M},
                new DateTollFee {StartDateIncluded = new DateTime(2013,4,30), EndDateIncluded = new DateTime(2013,4,30), Amount=0M},
                new DateTollFee {StartDateIncluded = new DateTime(2013,5,1), EndDateIncluded = new DateTime(2013,5,1), Amount=0M},
                new DateTollFee {StartDateIncluded = new DateTime(2013,5,8), EndDateIncluded = new DateTime(2013,5,8), Amount=0M},
                new DateTollFee {StartDateIncluded = new DateTime(2013,5,9), EndDateIncluded = new DateTime(2013,5,9), Amount=0M},
                new DateTollFee {StartDateIncluded = new DateTime(2013,6,5), EndDateIncluded = new DateTime(2013,6,5), Amount=0M},
                new DateTollFee {StartDateIncluded = new DateTime(2013,6,6), EndDateIncluded = new DateTime(2013,6,6), Amount=0M},
                new DateTollFee {StartDateIncluded = new DateTime(2013,6,21), EndDateIncluded = new DateTime(2013,6,21), Amount=0M},
                new DateTollFee {StartDateIncluded = new DateTime(2013,7,1), EndDateIncluded = new DateTime(2013,7,31), Amount=0M},
                new DateTollFee {StartDateIncluded = new DateTime(2013,11,1), EndDateIncluded = new DateTime(2013,11,1), Amount=0M},
                new DateTollFee {StartDateIncluded = new DateTime(2013,12,24), EndDateIncluded = new DateTime(2013,12,26), Amount=0M},
                new DateTollFee {StartDateIncluded = new DateTime(2013,12,31), EndDateIncluded = new DateTime(2013,12,31), Amount=0M},
            };

            // static list, In the real world example we will get the required data from the database
            _dayOfWeekTollFeeList = new List<DayOfWeekTollFee>()
            {
                new DayOfWeekTollFee { DayOfWeek = DayOfWeek.Saturday, Amount=0M},
                new DayOfWeekTollFee { DayOfWeek = DayOfWeek.Sunday, Amount=0M},
            };

            // static list, In the real world example we will get the required data from the database
            _timeSpanTollFeeList = new List<TimeSpanTollFee>()
            {
                new TimeSpanTollFee {StartTimeIncluded=new TimeSpan(6,0,0) , EndTimeNotIncluded =new TimeSpan(6,30,0), Amount= 8M},
                new TimeSpanTollFee {StartTimeIncluded=new TimeSpan(6,30,0) , EndTimeNotIncluded =new TimeSpan(7,0,0), Amount= 13M },
                new TimeSpanTollFee {StartTimeIncluded=new TimeSpan(7,0,0) , EndTimeNotIncluded =new TimeSpan(8,0,0), Amount= 18M },
                new TimeSpanTollFee {StartTimeIncluded=new TimeSpan(8,0,0) , EndTimeNotIncluded =new TimeSpan(8,30,0), Amount= 13M },
                new TimeSpanTollFee {StartTimeIncluded=new TimeSpan(8,30,0) , EndTimeNotIncluded =new TimeSpan(9,0,0), Amount= 8M},
                new TimeSpanTollFee {StartTimeIncluded=new TimeSpan(9,30,0) , EndTimeNotIncluded =new TimeSpan(10,0,0), Amount= 8M},
                new TimeSpanTollFee {StartTimeIncluded=new TimeSpan(10,30,0) , EndTimeNotIncluded =new TimeSpan(11,0,0), Amount= 8M},
                new TimeSpanTollFee {StartTimeIncluded=new TimeSpan(11,30,0) , EndTimeNotIncluded =new TimeSpan(12,0,0), Amount= 8M},
                new TimeSpanTollFee {StartTimeIncluded=new TimeSpan(12,30,0) , EndTimeNotIncluded =new TimeSpan(13,0,0), Amount= 8M},
                new TimeSpanTollFee {StartTimeIncluded=new TimeSpan(13,30,0) , EndTimeNotIncluded =new TimeSpan(14,0,0), Amount= 8M},
                new TimeSpanTollFee {StartTimeIncluded=new TimeSpan(14,30,0) , EndTimeNotIncluded =new TimeSpan(15,0,0), Amount= 8M },
                new TimeSpanTollFee {StartTimeIncluded=new TimeSpan(15,0,0) , EndTimeNotIncluded =new TimeSpan(15,30,0), Amount= 13M},
                new TimeSpanTollFee {StartTimeIncluded=new TimeSpan(15,0,0) , EndTimeNotIncluded =new TimeSpan(16,0,0), Amount= 18M},
                new TimeSpanTollFee {StartTimeIncluded=new TimeSpan(16,0,0) , EndTimeNotIncluded =new TimeSpan(17,0,0), Amount= 18M},
                new TimeSpanTollFee {StartTimeIncluded=new TimeSpan(17,0,0) , EndTimeNotIncluded =new TimeSpan(18,0,0), Amount= 13M },
                new TimeSpanTollFee {StartTimeIncluded=new TimeSpan(18,0,0) , EndTimeNotIncluded =new TimeSpan(18,30,0), Amount= 8M },
            };

            // static list, In the real world example we will get the required data from the database
            _vehicleTypeTollFee = new List<VehicleTypeTollFee>()
            {
                new VehicleTypeTollFee {VehicleType = VehicleTypeEnum.Motorbike, Amount=0M},
                new VehicleTypeTollFee {VehicleType = VehicleTypeEnum.Tractor, Amount=0M},
                new VehicleTypeTollFee {VehicleType = VehicleTypeEnum.Emergency, Amount=0M},
                new VehicleTypeTollFee {VehicleType = VehicleTypeEnum.Diplomat, Amount=0M},
                new VehicleTypeTollFee {VehicleType = VehicleTypeEnum.Foreign, Amount=0M},
                new VehicleTypeTollFee {VehicleType = VehicleTypeEnum.Military, Amount=0M},
            };
        }   
        public decimal GetMaxFeeByTimeSpanAsync(TimeSpan passTime)
        {
            var maxTollFee = _timeSpanTollFeeList
                .Where(p => passTime >= p.StartTimeIncluded &&
                            passTime < p.EndTimeNotIncluded)
                .OrderByDescending(x => x.Amount)
                .FirstOrDefault();

            if (maxTollFee == null)
                return 0M;

            return maxTollFee.Amount;
        }

        public bool IsTollFreeAsync(VehicleTypeEnum vehicleType)
        {
            return _vehicleTypeTollFee.Any(v => v.Amount == 0 && v.VehicleType == vehicleType);
        }
        public bool IsTollFreeAsync(DateTime desiredDate)
        {
            return
                _dayOfWeekTollFeeList.Any(d => d.Amount == 0 && d.DayOfWeek == desiredDate.DayOfWeek) ||
                _dateTollFeeList.Any(d => d.Amount == 0 && desiredDate.Date >= d.StartDateIncluded.Date && desiredDate.Date <= d.EndDateIncluded.Date);
        }
    }
}
