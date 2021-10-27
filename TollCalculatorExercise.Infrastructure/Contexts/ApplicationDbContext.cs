using System;
using System.Collections.Generic;
using TollCalculatorExercise.Domain.Enums;
using TollCalculatorExercise.Domain.Models;

namespace TollCalculatorExercise.Infrastructure.Contexts
{
    public class ApplicationDbContext
    {
        // static lists, In the real world example we will get the required data from the database
        public IEnumerable<DateTollFee> DateTollFeeList
        {
            get
            {
                return new List<DateTollFee>()
                {
                    new DateTollFee (new DateTime(2021,1,1), new DateTime(2021,1,1), new TollFee(0M)),
                    new DateTollFee (new DateTime(2021,3,28), new DateTime(2021,3,28), new TollFee(0M)),
                    new DateTollFee (new DateTime(2021,3,29), new DateTime(2021,3,29), new TollFee(0M)),
                    new DateTollFee (new DateTime(2021,4,1), new DateTime(2021,4,1), new TollFee(0M)),
                    new DateTollFee (new DateTime(2021,4,30), new DateTime(2021,4,30), new TollFee(0M)),
                    new DateTollFee (new DateTime(2021,5,1), new DateTime(2021,5,1), new TollFee(0M)),
                    new DateTollFee (new DateTime(2021,5,8), new DateTime(2021,5,8), new TollFee(0M)),
                    new DateTollFee (new DateTime(2021,5,9), new DateTime(2021,5,9), new TollFee(0M)),
                    new DateTollFee (new DateTime(2021,6,5), new DateTime(2021,6,5), new TollFee(0M)),
                    new DateTollFee (new DateTime(2021,6,6), new DateTime(2021,6,6), new TollFee(0M)),
                    new DateTollFee (new DateTime(2021,6,21), new DateTime(2021,6,21),new TollFee(0M)),
                    new DateTollFee (new DateTime(2021,7,1), new DateTime(2021,7,31), new TollFee(0M)),
                    new DateTollFee (new DateTime(2021,11,1), new DateTime(2021,11,1), new TollFee(0M)),
                    new DateTollFee (new DateTime(2021,12,24), new DateTime(2021,12,26), new TollFee(0M)),
                    new DateTollFee (new DateTime(2021,12,31), new DateTime(2021,12,31), new TollFee(0M)),
                };
            }
        }
        public IEnumerable<DayOfWeekTollFee> DayOfWeekTollFeeList
        {
            get
            {
                return new List<DayOfWeekTollFee>()
                {
                    new DayOfWeekTollFee (DayOfWeek.Saturday, new TollFee(0M)),
                    new DayOfWeekTollFee (DayOfWeek.Sunday, new TollFee(0M)),
                };
            }
        }
        public IEnumerable<TimeSpanTollFee> TimeSpanTollFeeList
        {
            get
            {
                return new List<TimeSpanTollFee>()
                {
                    new TimeSpanTollFee (new TimeSpan(6,0,0), new TimeSpan(6,30,0), new TollFee(8M)),
                    new TimeSpanTollFee (new TimeSpan(6,30,0), new TimeSpan(7,0,0), new TollFee(13M)),
                    new TimeSpanTollFee (new TimeSpan(7,0,0), new TimeSpan(8,0,0), new TollFee(18M)),
                    new TimeSpanTollFee (new TimeSpan(8,0,0), new TimeSpan(8,30,0), new TollFee(13M)),
                    new TimeSpanTollFee (new TimeSpan(8,30,0), new TimeSpan(9,0,0), new TollFee(8M)),
                    new TimeSpanTollFee (new TimeSpan(9,30,0), new TimeSpan(10,0,0),new TollFee(8M)),
                    new TimeSpanTollFee (new TimeSpan(10,30,0), new TimeSpan(11,0,0), new TollFee(8M)),
                    new TimeSpanTollFee (new TimeSpan(11,30,0), new TimeSpan(12,0,0), new TollFee(8M)),
                    new TimeSpanTollFee (new TimeSpan(12,30,0), new TimeSpan(13,0,0), new TollFee(8M)),
                    new TimeSpanTollFee (new TimeSpan(13,30,0), new TimeSpan(14,0,0), new TollFee(8M)),
                    new TimeSpanTollFee (new TimeSpan(14,30,0), new TimeSpan(15,0,0), new TollFee(8M)),
                    new TimeSpanTollFee (new TimeSpan(15,0,0) , new TimeSpan(15,30,0), new TollFee(13M)),
                    new TimeSpanTollFee (new TimeSpan(15,30,0), new TimeSpan(17,0,0), new TollFee(18M)),
                    new TimeSpanTollFee (new TimeSpan(17,0,0), new TimeSpan(18,0,0), new TollFee(13M)),
                    new TimeSpanTollFee (new TimeSpan(18,0,0), new TimeSpan(18,30,0), new TollFee(8M)),
                };
            }
        }
        public IEnumerable<VehicleTypeTollFee> VehicleTypeTollFeeList
        {
            get
            {
                return new List<VehicleTypeTollFee>()
                {
                    new VehicleTypeTollFee (VehicleTypeEnum.Motorbike, new TollFee(0M)),
                    new VehicleTypeTollFee (VehicleTypeEnum.Tractor, new TollFee(0M)),
                    new VehicleTypeTollFee (VehicleTypeEnum.Emergency, new TollFee(0M)),
                    new VehicleTypeTollFee (VehicleTypeEnum.Diplomat, new TollFee(0M)),
                    new VehicleTypeTollFee (VehicleTypeEnum.Foreign, new TollFee(0M)),
                    new VehicleTypeTollFee (VehicleTypeEnum.Military, new TollFee(0M)),
                };
            }
        }
    }
}
