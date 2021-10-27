using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using TollCalculatorExercise.Domain.Enums;
using TollCalculatorExercise.Domain.Settings;
using TollCalculatorExercise.Services.Interfaces.Repositories;
using TollCalculatorExercise.Services.Wrappers;

namespace TollCalculatorExercise.Services.Features.TollFee.Queries
{
    /// <summary>
    /// Total fees per day for a vehicle type query.
    /// </summary>
    public class GetTotalTollFeesPerDayQuery : IRequest<Response<decimal>>
    {
        /// <summary>
        /// Gets or sets the vehicle type. 
        /// </summary>
        public VehicleTypeEnum VehicleType { get; set; }

        /// <summary>
        /// Gets or sets the pass dates. It should be in the same day. 
        /// </summary>
        public DateTime[] Dates { get; set; }
    }

    /// <summary>
    /// Handler for getting the total fees per day for a vehicle type query.
    /// </summary>
    public class GetTotalTollFeesPerDayQueryHandler : IRequestHandler<GetTotalTollFeesPerDayQuery, Response<decimal>>
    {
        private readonly IDateTollFeeRepository _dateTollFeeRepository;
        private readonly IDayOfWeekTollFeeRepository _dayOfWeekTollFeeRepository;
        private readonly ITimeSpanTollFeeRepository _timeSpanTollFeeRepository;
        private readonly IVehicleTypeTollFeeRepository _vehicleTypeTollFeeRepository;
        
        private readonly ApplicationSettings _config;
        public GetTotalTollFeesPerDayQueryHandler(IDateTollFeeRepository dateTollFeeRepository, IDayOfWeekTollFeeRepository dayOfWeekTollFeeRepository, 
            ITimeSpanTollFeeRepository timeSpanTollFeeRepository, IVehicleTypeTollFeeRepository vehicleTypeTollFeeRepository, 
            IOptions<ApplicationSettings> config)
        {
            _dateTollFeeRepository = dateTollFeeRepository;
            _dayOfWeekTollFeeRepository = dayOfWeekTollFeeRepository;
            _timeSpanTollFeeRepository = timeSpanTollFeeRepository;
            _vehicleTypeTollFeeRepository = vehicleTypeTollFeeRepository;
            _config = config.Value;
        }
        public async Task<Response<decimal>> Handle(GetTotalTollFeesPerDayQuery request, CancellationToken cancellationToken= default(CancellationToken))
        {
            var totalFees = await GetTotalTollFeesPerDayAsync(request.VehicleType, request.Dates);
            return new Response<decimal>(totalFees);
        }

        private Task<decimal> GetTotalTollFeesPerDayAsync(VehicleTypeEnum vehicleType, DateTime[] dates)
        {
            // Sorting the dates
            Array.Sort(dates);

            // Check if the vehicle type or the date is fee-free
            if (_vehicleTypeTollFeeRepository.IsTollFree(vehicleType) 
                || _dateTollFeeRepository.IsTollFree(dates[0]) 
                || _dayOfWeekTollFeeRepository.IsTollFree(dates[0]))
            {
                return Task.FromResult(0M);
            }

            // Set initial values
            decimal totalFee = 0M;
            var currentIntervalStartDate = dates[0];
            decimal currentIntervalMaxFee = 0M;
            
            // Calculating the total fee for all pass times
            foreach (DateTime date in dates)
            {
                // Get the fee of current pass time
                var currentDateFee = _timeSpanTollFeeRepository.GetFeeByTimeSpan(date.TimeOfDay);
                
                // Check if the current pass time does not belong to the current interval
                if (date.Subtract(currentIntervalStartDate).TotalSeconds > _config.CHARGE_INTERVAL_IN_SECONDS)
                {
                    // Add current interval maximum fee to the total fee
                    totalFee += currentIntervalMaxFee;
                    // Moving to the next interval
                    currentIntervalStartDate = currentIntervalStartDate.AddSeconds(_config.CHARGE_INTERVAL_IN_SECONDS);
                    // Set the new interval maximum fee
                    currentIntervalMaxFee = currentDateFee;
                }

                // Current pass time is inside the current interval
                else
                {
                    // compare the current date fee with the current interval maximum fee and take the maximum of both
                    currentIntervalMaxFee = Math.Max(currentIntervalMaxFee, currentDateFee);
                }
            }

            // Add last interval maximun fee the total fee
            totalFee += currentIntervalMaxFee;

            // Comparing total calculated fee with the configured maximum fees per day to get the minimum of both
            totalFee = Math.Min(_config.MAX_FEES_PER_DAY, totalFee);

            return Task.FromResult(totalFee);

        }
    }
}
