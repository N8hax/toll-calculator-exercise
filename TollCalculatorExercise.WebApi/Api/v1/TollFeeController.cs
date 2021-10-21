using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TollCalculatorExercise.Domain.Enums;
using TollCalculatorExercise.Services.Features.TollFee.Queries;

namespace TollCalculatorExercise.WebApi.Api
{
    public class TollFeeController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> CalculateFeesAsync([FromQuery] VehicleTypeEnum vehicleType, [FromQuery] DateTime[] dates)
        {
            return Ok(await Mediator.Send(new GetTotalTollFeesPerDayQuery() { VehicleType = vehicleType, Dates= dates }));
        }
    }
}
