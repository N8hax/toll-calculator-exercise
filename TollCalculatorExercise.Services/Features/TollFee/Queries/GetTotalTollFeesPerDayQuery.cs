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
        private readonly ITollFeeRepository _tollFeeRepository;
        private readonly ApplicationSettings _config;
        public GetTotalTollFeesPerDayQueryHandler(ITollFeeRepository tollFeeRepository, IOptions<ApplicationSettings> config)
        {
            _tollFeeRepository = tollFeeRepository;
            _config = config.Value;
        }
        public async Task<Response<decimal>> Handle(GetTotalTollFeesPerDayQuery request, CancellationToken cancellationToken= default(CancellationToken))
        {
            var totalFees = await GetTotalTollFeesPerDayAsync(request.VehicleType, request.Dates);
            return new Response<decimal>(totalFees);
        }

        private Task<decimal> GetTotalTollFeesPerDayAsync(VehicleTypeEnum vehicleType, DateTime[] dates)
        {
            // Validating null or empty dates parameter
            if (dates == null || dates.Length == 0)
            {
                throw new ArgumentException("dates parameter cannot be null or empty.");
            }
            // Sorting the dates
            Array.Sort(dates);
            // Validating dates parameter to be in same day
            if (dates[0].Date != dates[dates.Length - 1].Date)
            {
                throw new ArgumentException("dates should be in the same day.");
            }
            // Check if the vehicle type or the date is fee-free
            if (_tollFeeRepository.IsTollFreeAsync(vehicleType) || _tollFeeRepository.IsTollFreeAsync(dates[0]))
            {
                return Task.FromResult(0M);
            }
            
            decimal totalFee = 0M;
            var currentIntervalStartDate = dates[0];
            decimal currentIntervalMaxFee = 0M;
            // calculating the total fee for all pass times
            foreach (DateTime date in dates)
            {
                var currentDateMaxFee = _tollFeeRepository.GetMaxFeeByTimeSpanAsync(date.TimeOfDay);
                if (date.Subtract(currentIntervalStartDate).TotalSeconds > _config.CHARGE_INTERVAL_IN_SECONDS)
                {
                    totalFee += currentIntervalMaxFee;
                    currentIntervalMaxFee = currentDateMaxFee;
                }
                else
                {
                    currentIntervalMaxFee = Math.Max(currentIntervalMaxFee, currentDateMaxFee);
                }
            }
            totalFee += currentIntervalMaxFee;
            // Comparing total calculated fee with the configured maximum fees per day to get the minimum of both
            totalFee = Math.Min(_config.MAX_FEES_PER_DAY, totalFee);
            return Task.FromResult(totalFee);

        }
    }
}
