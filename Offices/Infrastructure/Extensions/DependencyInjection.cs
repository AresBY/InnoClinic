using InnoClinic.Offices.Application.Interfaces.Repositories;
using InnoClinic.Offices.Infrastructure.Persistence.Repositories;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MongoDB.Driver;

namespace InnoClinic.Offices.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoConnectionString = configuration["Mongo:ConnectionString"];
            var mongoDatabaseName = configuration["Mongo:DatabaseName"];

            var mongoClient = new MongoClient(mongoConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDatabaseName);

            services.AddSingleton<IMongoDatabase>(mongoDatabase);
            services.AddScoped<IOfficeRepository, MongoOfficeRepository>();

            return services;
        }
    }
}
