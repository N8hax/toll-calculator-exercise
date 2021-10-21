using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TollCalculatorExercise.Services
{
    public static class ServiceRegistration
    {
        public static void AddServicesLayer(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }
    }
}
