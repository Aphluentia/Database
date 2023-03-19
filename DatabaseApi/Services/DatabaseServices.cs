using DatabaseApi.Models.Dtos.Entities;
using DatabaseApi.Models.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DatabaseApi.Services
{
    public class DatabaseServices
    {
        private readonly IMongoCollection<Scenario> _collection;

        public DatabaseServices(
            IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(
                databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);

            _collection = mongoDatabase.GetCollection<Scenario>(
                databaseSettings.Value.CollectionName);
        }

        public async Task<List<Scenario>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<Scenario?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Scenario newBook) =>
            await _collection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, Scenario updatedBook) =>
            await _collection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }
   
    
}


