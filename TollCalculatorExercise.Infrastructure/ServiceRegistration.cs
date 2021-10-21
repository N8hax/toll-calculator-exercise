using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TollCalculatorExercise.Infrastructure.Repositories;
using TollCalculatorExercise.Services.Interfaces.Repositories;

namespace TollCalculatorExercise.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureLayer(this IServiceCollection services)
        {
            #region Repositories
            services.AddTransient<ITollFeeRepository, TollFeeRepository>();
            #endregion
        }
    }
}
