using InnoClinic.Offices.Application.Interfaces.Repositories;
using InnoClinic.Offices.Domain.Entities;

using MongoDB.Driver;

namespace InnoClinic.Offices.Infrastructure.Persistence.Repositories
{
    public class MongoOfficeRepository : IOfficeRepository
    {
        private readonly IMongoCollection<Office> _officeCollection;

        public IQueryable<Office> Query()
        {
            return _officeCollection.AsQueryable();
        }

        public MongoOfficeRepository(IMongoDatabase database)
        {
            _officeCollection = database.GetCollection<Office>("offices");
        }

        public async Task DeleteAsync(string id, CancellationToken cancellationToken)
        {
            var filter = Builders<Office>.Filter.Eq(o => o.Id, id);
            await _officeCollection.DeleteOneAsync(filter, cancellationToken);
        }

        public async Task<List<Office>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _officeCollection.Find(_ => true).ToListAsync(cancellationToken);
        }

        public async Task InsertAsync(Office office, CancellationToken cancellationToken)
        {
            await _officeCollection.InsertOneAsync(office, cancellationToken: cancellationToken);
        }

        public async Task UpdateAsync(Office office, CancellationToken cancellationToken)
        {
            var filter = Builders<Office>.Filter.Eq(o => o.Id, office.Id);
            await _officeCollection.ReplaceOneAsync(filter, office, cancellationToken: cancellationToken);
        }

        public async Task<Office?> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var filter = Builders<Office>.Filter.Eq(o => o.Id, id);
            return await _officeCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
