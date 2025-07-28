using InnoClinic.Offices.Application.Interfaces.Repositories;
using InnoClinic.Offices.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace InnoClinic.Offices.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoConnectionString = configuration.GetConnectionString("MongoDb");
            var mongoClient = new MongoClient(mongoConnectionString);
            var mongoDatabase = mongoClient.GetDatabase("InnoClinicOffices");

            services.AddSingleton<IMongoDatabase>(mongoDatabase);
            services.AddScoped<IOfficeRepository, MongoOfficeRepository>();

            return services;
        }
    }
}
