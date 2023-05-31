using DatabaseApi.Configurations;
using DatabaseApi.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DatabaseApi.Services
{
    public class UserServices: IDatabaseProvider<User>
    {

        private readonly IMongoCollection<User> _users;
        public UserServices(IOptions<MongoConfigSection> databaseSettings) 
        {
            var mongoDatabase = new MongoClient(
                   databaseSettings.Value.ConnectionString).GetDatabase(databaseSettings.Value.DatabaseName);
   
            _users = mongoDatabase.GetCollection<User>(
                databaseSettings.Value.UserCollectionName);
        }

        public async Task<List<User>> FindAllAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        public async Task<User?> FindByIdAsync(string id)
        {
            return await _users.Find(x => x.Email == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(User newObject)
        {
            await _users.InsertOneAsync(newObject);
        }

        public async Task UpdateAsync(string id, User updatedObject)
        {
            await _users.ReplaceOneAsync(x => x.Email == id, updatedObject);
        }

        public async Task RemoveByIdAsync(string id)
        {
            await _users.DeleteOneAsync(x => x.Email == id);
        }

        public async Task RemoveAllAsync()
        {
            await _users.DeleteManyAsync(x => true);
        }

        public async Task AddModuleConnectionAsync(ModuleConnection connection)
        {
            var user = (await _users.Find(x => x.WebPlatformId.ToString() == connection.Email).FirstOrDefaultAsync());
            if (user!= null)
            {
                user.Modules.Add(connection.ModuleId);
                await this.UpdateAsync(user.Email, user);
            }
        }
        public async Task RemoveModuleConnectionAsync(ModuleConnection connection)
        {
            var user = (await _users.Find(x => x.WebPlatformId.ToString() == connection.Email).FirstOrDefaultAsync());
            if (user != null)
            {
                user.Modules.Remove(connection.ModuleId);
                await this.UpdateAsync(user.Email, user);
            }
        }
        public async Task<ICollection<string>?> RetrieveModulesAsync(string email) =>
           (await _users.Find(x => x.Email == email).FirstOrDefaultAsync())?.Modules;

        public async Task<bool?> ExistsWebPlatformId(string WebPlatformId) =>
            await _users.Find(x => x.WebPlatformId == new Guid(WebPlatformId)).FirstOrDefaultAsync() != null;

        public async Task<bool> Contains(string Email) =>
          await _users.Find(x => x.Email == Email).FirstOrDefaultAsync() != null;

    }
}
