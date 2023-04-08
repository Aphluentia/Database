using DatabaseApi.Models.Dtos.Entities;
using DatabaseApi.Models.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DatabaseApi.Services
{
    public class DatabaseServices
    {
        private readonly IMongoCollection<Scenario> _scenarios;
        private readonly IMongoCollection<User> _users;

        public DatabaseServices(
            IOptions<DatabaseSettings> databaseSettings)
        {
            var mongoClient = new MongoClient(
                databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);


            _scenarios = mongoDatabase.GetCollection<Scenario>(
                databaseSettings.Value.ScenarioCollectionName);
            _users = mongoDatabase.GetCollection<User>(
                databaseSettings.Value.UserCollectionName);
        }

        public async Task<List<Scenario>> GetScenarioAsync() =>
            await _scenarios.Find(_ => true).ToListAsync();

        public async Task<Scenario?> GetScenarioAsync(string id) =>
            await _scenarios.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateScenarioAsync(Scenario newBook) =>
            await _scenarios.InsertOneAsync(newBook);

        public async Task UpdateScenarioAsync(string id, Scenario updatedBook) =>
            await _scenarios.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveScenarioAsync(string id) =>
            await _scenarios.DeleteOneAsync(x => x.Id == id);
        public async Task PurgeScenarioAsync() =>
            await _scenarios.DeleteManyAsync(x => true);

        public async Task<List<User>> GetUserAsync() =>
            await _users.Find(_ => true).ToListAsync();

        public async Task<User?> GetUserAsync(string id) =>
            await _users.Find(x => x.Email == id).FirstOrDefaultAsync();

        public async Task CreateUserAsync(User newBook) =>
            await _users.InsertOneAsync(newBook);

        public async Task UpdateUserAsync(string id, User updatedBook) =>
            await _users.ReplaceOneAsync(x => x.Email == id, updatedBook);

        public async Task RemoveUserAsync(string id) =>
            await _users.DeleteOneAsync(x => x.Email == id);

        public async Task PurgeUsersAsync() =>
            await _users.DeleteManyAsync(x=> true);
    }
   
    
}


