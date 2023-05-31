using DatabaseApi.Configurations;
using DatabaseApi.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


namespace DatabaseApi.Services
{
    public class ModulesServices: IDatabaseProvider<Module>
    {

        private readonly IMongoCollection<Module> _modules;

        public ModulesServices(
            IOptions<MongoConfigSection> databaseSettings)
        {
            var mongoClient = new MongoClient(
                databaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                databaseSettings.Value.DatabaseName);


            _modules = mongoDatabase.GetCollection<Module>(
                databaseSettings.Value.ModulesCollectionName);
        }

        public async Task<List<Module>> FindAllAsync() =>
            await _modules.Find(_ => true).ToListAsync();

        public async Task<Module?> FindByIdAsync(string id) =>
            await _modules.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Module newModule) =>
            await _modules.InsertOneAsync(newModule);

        public async Task UpdateAsync(string id, Module updatedModule) =>
            await _modules.ReplaceOneAsync(x => x.Id == id, updatedModule);

        public async Task RemoveByIdAsync(string id) =>
            await _modules.DeleteOneAsync(x => x.Id == id);
        public async Task RemoveAllAsync() =>
            await _modules.DeleteManyAsync(x => true);

      

    }
}
