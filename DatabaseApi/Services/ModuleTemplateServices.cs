using DatabaseApi.Configurations;
using DatabaseApi.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DatabaseApi.Services
{
    public class ModuleTemplateServices: IModuleTemplatesService
    {
        private readonly IMongoCollection<ModuleTemplate> _moduleTemplates;
        public ModuleTemplateServices(IOptions<MongoConfigSection> databaseSettings)
        {
            var mongoDatabase = new MongoClient(
                   databaseSettings.Value.ConnectionString).GetDatabase(databaseSettings.Value.DatabaseName);

            _moduleTemplates = mongoDatabase.GetCollection<ModuleTemplate>(
                databaseSettings.Value.ModuleTemplatesCollectionName);
        }

        public async Task CreateAsync(ModuleTemplate newObject)
        { 
            await _moduleTemplates.InsertOneAsync(newObject);
        }

        public async Task<List<ModuleTemplate>> FindAllAsync()
        {
            return await _moduleTemplates.Find(_ => true).ToListAsync();
        }

        public async Task<ModuleTemplate?> FindByIdAsync(string ModuleType)
        {
            return await _moduleTemplates.Find(c => c.ModuleType == ModuleType).FirstOrDefaultAsync();
        }

        public async Task RemoveAllAsync()
        {
            await _moduleTemplates.DeleteManyAsync(x => true);
        }

        public async Task RemoveByIdAsync(string ModuleType)
        {
            await _moduleTemplates.DeleteOneAsync(x => x.ModuleType == ModuleType);
        }

        public async Task UpdateAsync(string ModuleType, ModuleTemplate updatedObject)
        {
            await _moduleTemplates.ReplaceOneAsync(x => x.ModuleType == ModuleType, updatedObject);
        }
    }
}
