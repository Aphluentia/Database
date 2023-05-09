using DatabaseApi.Models.Dtos.Entities;
using DatabaseApi.Models.Entities;
using DatabaseApi.Models.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DatabaseApi.Services
{
    public class ModulesServices
    {

            private readonly IMongoCollection<Module> _modules;

            public ModulesServices(
                IOptions<DatabaseSettings> databaseSettings)
            {
                var mongoClient = new MongoClient(
                    databaseSettings.Value.ConnectionString);

                var mongoDatabase = mongoClient.GetDatabase(
                    databaseSettings.Value.DatabaseName);


                _modules = mongoDatabase.GetCollection<Module>(
                    databaseSettings.Value.ModulesCollectionName);
            }

            public async Task<List<Module>> GetModulesAsync() =>
                await _modules.Find(_ => true).ToListAsync();

            public async Task<Module?> GetModulesAsync(string id) =>
                await _modules.Find(x => x.Id == id).FirstOrDefaultAsync();

            public async Task CreateModulesAsync(Module newModule) =>
                await _modules.InsertOneAsync(newModule);

            public async Task UpdateModulesAsync(string id, Module updatedModule) =>
                await _modules.ReplaceOneAsync(x => x.Id == id, updatedModule);

            public async Task RemoveModulesAsync(string id) =>
                await _modules.DeleteOneAsync(x => x.Id == id);
            public async Task PurgeModulesAsync() =>
                await _modules.DeleteManyAsync(x => true);


        
    }
}
