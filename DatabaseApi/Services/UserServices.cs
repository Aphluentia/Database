using DatabaseApi.Models.Dtos.Entities;
using DatabaseApi.Models.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DatabaseApi.Services
{
    public class UserServices
    {

        private readonly IMongoCollection<User> _users;
        public UserServices(IOptions<DatabaseSettings> databaseSettings) 
        {
            var mongoDatabase = new MongoClient(
                   databaseSettings.Value.ConnectionString).GetDatabase(databaseSettings.Value.DatabaseName);
   
            _users = mongoDatabase.GetCollection<User>(
                databaseSettings.Value.UserCollectionName);
        }

        public async Task<List<User>> GetUserAsync() =>
           await _users.Find(_ => true).ToListAsync();

        
        public async Task<User?> GetUserAsync(string email) =>
            await _users.Find(x => x.Email == email).FirstOrDefaultAsync();

        public async Task<ICollection<string>?> GetUserModulesAsync(string email) =>
            (await _users.Find(x => x.Email == email).FirstOrDefaultAsync())?.Modules;

        public async Task<bool?> ExistsWebPlatformId(string WebPlatformId) =>
            await _users.Find(x => x.WebPlatformId == new Guid(WebPlatformId)).FirstOrDefaultAsync() != null;

        public async Task CreateUserAsync(User newUser) =>
            await _users.InsertOneAsync(newUser);

        public async Task UpdateUserAsync(string email, User updatedUser) =>
            await _users.ReplaceOneAsync(x => x.Email == email, updatedUser);

        public async Task RemoveUserAsync(string email) =>
            await _users.DeleteOneAsync(x => x.Email == email);

        public async Task PurgeUsersAsync() =>
            await _users.DeleteManyAsync(x => true);
        public async Task<bool> Contains(string Email) =>
          await _users.Find(x => x.Email == Email).FirstOrDefaultAsync() != null;
    }
}
