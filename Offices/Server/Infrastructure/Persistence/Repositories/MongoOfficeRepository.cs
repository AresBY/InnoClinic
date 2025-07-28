using InnoClinic.Offices.Domain.Entities;
using InnoClinic.Offices.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace InnoClinic.Offices.Infrastructure.Persistence.Repositories
{
    public class MongoOfficeRepository : IOfficeRepository
    {
        private readonly IMongoCollection<Office> _officeCollection;

        public MongoOfficeRepository(IMongoDatabase database)
        {
            _officeCollection = database.GetCollection<Office>("offices");
        }

        public async Task InsertAsync(Office office, CancellationToken cancellationToken)
        {
            await _officeCollection.InsertOneAsync(office, cancellationToken: cancellationToken);
        }
    }
}
