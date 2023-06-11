using DatabaseApi.Configurations;
using DatabaseApi.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ThirdParty.BouncyCastle.Utilities.IO.Pem;

namespace DatabaseApi.Services
{
    public class ModuleService: IModuleService
    {
        private readonly IMongoCollection<Module> _modules;
        private readonly IMongoCollection<ModuleTemplate> _moduleTemplates;
        public ModuleService(IOptions<MongoConfigSection> databaseSettings)
        {
            var mongoDatabase = new MongoClient(
                   databaseSettings.Value.ConnectionString).GetDatabase(databaseSettings.Value.DatabaseName);

            _modules = mongoDatabase.GetCollection<Module>(
                databaseSettings.Value.ModulesCollectionName);

            _moduleTemplates = mongoDatabase.GetCollection<ModuleTemplate>(
                databaseSettings.Value.ModuleTemplatesCollectionName);
        }
        public async Task<bool> CreateAsync(Module newObject)
        {
            if (await _modules.Find(c => c.Id == newObject.Id).FirstOrDefaultAsync() != null) return false;
            await _modules.InsertOneAsync(newObject);
            return true;
        }

        public async Task<List<Module>> FindAllAsync()
        {
            return await _modules.Find(_ => true).ToListAsync();
        }

        public async Task<Module?> FindByIdAsync(string moduleId)
        {
            return await _modules.Find(c => c.Id == moduleId).FirstOrDefaultAsync();
            
        }

        public async Task RemoveAllAsync()
        {
            await _modules.DeleteManyAsync(x => true);
        }

        public async Task<bool> RemoveByIdAsync(string moduleId)
        {
            if (await _modules.Find(c => c.Id == moduleId).FirstOrDefaultAsync() == null) return false;
            await _modules.DeleteOneAsync(x => x.Id == moduleId);
            return true;
        }

        public async Task<bool> UpdateAsync(string moduleId, Module updatedModule)
        {
            var existingModule = await _modules.Find(c => c.Id == moduleId).FirstOrDefaultAsync();
            if (existingModule == null) return false;
            if (!(await _modules.ReplaceOneAsync(x => x.Id == moduleId, updatedModule)).IsAcknowledged) return false;

            return true;
        }
        public async Task<bool> AssignModule(string Email, string ModuleId)
        {
            var module = await _modules.Find(c => c.Id == ModuleId).FirstOrDefaultAsync();
            if (module == null || module.IsAssigned) return false;
            module.IsAssigned = true;
            var result = await _modules.ReplaceOneAsync(x => x.Id == ModuleId, module);
            if (!result.IsAcknowledged) return false;
            return true;
        }
        public async Task<bool> RevokeModule(string Email, string ModuleId)
        {
            var module = await _modules.Find(c => c.Id == ModuleId).FirstOrDefaultAsync();
            if (module == null) return false;
            module.IsAssigned = false;
            var result = await _modules.ReplaceOneAsync(x => x.Id == ModuleId, module);
            if (!result.IsAcknowledged) return false;
            return true;
        }
        public async Task<bool> Exists(string ModuleId)
        {
            var module = await _modules.Find(c => c.Id == ModuleId).FirstOrDefaultAsync();
            if (module == null) return false;
            return true;
        }

    }
}
