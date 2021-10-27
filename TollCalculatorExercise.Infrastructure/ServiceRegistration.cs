using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TollCalculatorExercise.Infrastructure.Contexts;
using TollCalculatorExercise.Infrastructure.Repositories;
using TollCalculatorExercise.Services.Interfaces.Repositories;

namespace TollCalculatorExercise.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureLayer(this IServiceCollection services)
        {
            services.AddTransient<ApplicationDbContext>();

            #region Repositories
            services.AddTransient<IDateTollFeeRepository, DateTollFeeRepository>();
            services.AddTransient<IDayOfWeekTollFeeRepository, DayOfWeekTollFeeRepository>();
            services.AddTransient<ITimeSpanTollFeeRepository, TimeSpanTollFeeRepository>();
            services.AddTransient<IVehicleTypeTollFeeRepository, VehicleTypeTollFeeRepository>();
            #endregion
        }
    }
}
