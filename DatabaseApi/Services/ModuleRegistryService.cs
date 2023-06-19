using DatabaseApi.Configurations;
using DatabaseApi.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DatabaseApi.Services
{
    public class ModuleRegistryService: IModuleRegistryService
    {
        private readonly IMongoCollection<Application> _moduleTemplates;
        public ModuleRegistryService(IOptions<MongoConfigSection> databaseSettings)
        {
            var mongoDatabase = new MongoClient(
                   databaseSettings.Value.ConnectionString).GetDatabase(databaseSettings.Value.DatabaseName);

            _moduleTemplates = mongoDatabase.GetCollection<Application>(
                databaseSettings.Value.ModuleTemplatesCollectionName);
        }

        public async Task<bool> CreateAsync(Application newObject)
        {
            if (await _moduleTemplates.Find(c => c.ModuleName == newObject.ModuleName).FirstOrDefaultAsync() != null) return false;
            await _moduleTemplates.InsertOneAsync(newObject);
            return true;
        }

        public async Task<List<Application>> FindAllAsync()
        {
            return await _moduleTemplates.Find(_ => true).ToListAsync();
        }

        public async Task<Application?> FindByIdAsync(string ModuleName)
        {
            return await _moduleTemplates.Find(c => c.ModuleName == ModuleName).FirstOrDefaultAsync();
        }

        public async Task RemoveAllAsync()
        {
            await _moduleTemplates.DeleteManyAsync(x => true);
        }

        public async Task<bool> RemoveByIdAsync(string ModuleName)
        {
            if (await _moduleTemplates.Find(c => c.ModuleName == ModuleName).FirstOrDefaultAsync() == null) return false;
            await _moduleTemplates.DeleteOneAsync(x => x.ModuleName == ModuleName);
            return true;
        }

        public async Task<bool> UpdateAsync(string ModuleName, Application updatedObject)
        {
            if (await _moduleTemplates.Find(c => c.ModuleName == ModuleName).FirstOrDefaultAsync() == null) return false;
            if (!(await _moduleTemplates.ReplaceOneAsync(x => x.ModuleName == ModuleName, updatedObject)).IsAcknowledged) return false;
            return true;
        }
        public async Task<bool> CreateVersion(string ModuleName, ModuleVersion newObject)
        {
            var moduleRegistry = await _moduleTemplates.Find(c => c.ModuleName == ModuleName).FirstOrDefaultAsync();
            if (moduleRegistry == null) return false;
            if (moduleRegistry.Versions.Any(c => c.VersionId == newObject.VersionId)) return false;
            moduleRegistry.Versions.Add(newObject);
            if (!(await _moduleTemplates.ReplaceOneAsync(x => x.ModuleName == ModuleName, moduleRegistry)).IsAcknowledged) return false;
            return true;

        }

        public async Task<bool> DeleteModuleVersion(string ModuleName, string ModuleVersion)
        {
            var moduleRegistry = await _moduleTemplates.Find(c => c.ModuleName == ModuleName).FirstOrDefaultAsync();
            if (moduleRegistry == null) return false;
            var mVersion = moduleRegistry.Versions.Where(c => c.VersionId == ModuleVersion).FirstOrDefault();
            if (mVersion == null) return false;
            moduleRegistry.Versions.Remove(mVersion);
            if (!(await _moduleTemplates.ReplaceOneAsync(x => x.ModuleName == ModuleName, moduleRegistry)).IsAcknowledged) return false;
            return true;
        }

        public async Task<bool> UpdateModuleVersion(string ModuleName, string ModuleVersion, ModuleVersion newObject)
        {
            newObject.VersionId = ModuleVersion;
            if (!(await DeleteModuleVersion(ModuleName, ModuleVersion))) return false;
            if (!await CreateVersion(ModuleName, newObject)) return false;
            return true;
        }

        public async Task<ICollection<ModuleVersion>> GetVersions(string ModuleName)
        {
            var moduleRegistry = await _moduleTemplates.Find(c => c.ModuleName == ModuleName).FirstOrDefaultAsync();
            if (moduleRegistry == null) return new List<ModuleVersion>();
            return moduleRegistry.Versions;
        }
    }
}
