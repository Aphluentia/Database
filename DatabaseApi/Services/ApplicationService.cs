using DatabaseApi.Configurations;
using DatabaseApi.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DatabaseApi.Services
{
    public class ApplicationService: IApplicationService
    {
        private readonly IMongoCollection<Application> _Applications;
        public ApplicationService(IOptions<MongoConfigSection> databaseSettings)
        {
            var mongoDatabase = new MongoClient(
                   databaseSettings.Value.ConnectionString).GetDatabase(databaseSettings.Value.DatabaseName);

            _Applications = mongoDatabase.GetCollection<Application>(
                databaseSettings.Value.ApplicationsCollectionName);
        }

        public async Task<bool> CreateAsync(Application newObject)
        {
            if (await _Applications.Find(c => c.ApplicationName == newObject.ApplicationName).FirstOrDefaultAsync() != null) return false;
            await _Applications.InsertOneAsync(newObject);
            return true;
        }

        public async Task<List<Application>> FindAllAsync()
        {
            return await _Applications.Find(_ => true).ToListAsync();
        }

        public async Task<Application?> FindByIdAsync(string ModuleName)
        {
            return await _Applications.Find(c => c.ApplicationName == ModuleName).FirstOrDefaultAsync();
        }

        public async Task RemoveAllAsync()
        {
            await _Applications.DeleteManyAsync(x => true);
        }

        public async Task<bool> RemoveByIdAsync(string ModuleName)
        {
            if (await _Applications.Find(c => c.ApplicationName == ModuleName).FirstOrDefaultAsync() == null) return false;
            await _Applications.DeleteOneAsync(x => x.ApplicationName == ModuleName);
            return true;
        }

        public async Task<bool> UpdateAsync(string ModuleName, Application updatedObject)
        {
            if (await _Applications.Find(c => c.ApplicationName == ModuleName).FirstOrDefaultAsync() == null) return false;
            if (!(await _Applications.ReplaceOneAsync(x => x.ApplicationName == ModuleName, updatedObject)).IsAcknowledged) return false;
            return true;
        }
        public async Task<bool> CreateVersion(string ModuleName, ModuleVersion newObject)
        {
            var moduleRegistry = await _Applications.Find(c => c.ApplicationName == ModuleName).FirstOrDefaultAsync();
            if (moduleRegistry == null) return false;
            if (moduleRegistry.Versions.Any(c => c.VersionId == newObject.VersionId)) return false;
            moduleRegistry.Versions.Add(newObject);
            if (!(await _Applications.ReplaceOneAsync(x => x.ApplicationName == ModuleName, moduleRegistry)).IsAcknowledged) return false;
            return true;

        }

        public async Task<bool> DeleteModuleVersion(string ModuleName, string ModuleVersion)
        {
            var moduleRegistry = await _Applications.Find(c => c.ApplicationName == ModuleName).FirstOrDefaultAsync();
            if (moduleRegistry == null) return false;
            var mVersion = moduleRegistry.Versions.Where(c => c.VersionId == ModuleVersion).FirstOrDefault();
            if (mVersion == null) return false;
            moduleRegistry.Versions.Remove(mVersion);
            if (!(await _Applications.ReplaceOneAsync(x => x.ApplicationName == ModuleName, moduleRegistry)).IsAcknowledged) return false;
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
            var moduleRegistry = await _Applications.Find(c => c.ApplicationName == ModuleName).FirstOrDefaultAsync();
            if (moduleRegistry == null) return new List<ModuleVersion>();
            return moduleRegistry.Versions;
        }
    }
}
