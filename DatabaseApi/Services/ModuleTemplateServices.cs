using DatabaseApi.Configurations;
using DatabaseApi.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Reflection;
using ThirdParty.BouncyCastle.Utilities.IO.Pem;

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

        public async Task<bool> CreateAsync(ModuleTemplate newObject)
        {
            if (await _moduleTemplates.Find(c => c.ModuleType == newObject.ModuleType).FirstOrDefaultAsync() != null) return false;
            await _moduleTemplates.InsertOneAsync(newObject);
            return true;
        }

        public async Task<bool> Exists(string ModuleType)
        {
            if (await _moduleTemplates.Find(c => c.ModuleType == ModuleType).FirstOrDefaultAsync() == null)
                return false;
            return true;
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

        public async Task<bool> RemoveByIdAsync(string ModuleType)
        {
            if (await _moduleTemplates.Find(c => c.ModuleType == ModuleType).FirstOrDefaultAsync() == null) return false;
            await _moduleTemplates.DeleteOneAsync(x => x.ModuleType == ModuleType);
            return true;
        }

        public async Task<bool> UpdateAsync(string ModuleType, ModuleTemplate updatedObject)
        {
            if (await _moduleTemplates.Find(c => c.ModuleType == ModuleType).FirstOrDefaultAsync() == null) return false;
            if (!(await _moduleTemplates.ReplaceOneAsync(x => x.ModuleType == ModuleType, updatedObject)).IsAcknowledged) return false;
            return true;
        }
    }
}
