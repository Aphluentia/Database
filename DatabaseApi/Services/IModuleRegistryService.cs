using DatabaseApi.Models.Entities;

namespace DatabaseApi.Services
{
    public interface IModuleRegistryService
    {
        public Task<List<ModuleRegistry>> FindAllAsync();
        public Task<ModuleRegistry?> FindByIdAsync(string ModuleType);
        public Task<bool> CreateAsync(ModuleRegistry newObject);
        public Task<ICollection<ModuleVersion>> GetVersions(string ModuleName);
        public Task<bool> CreateVersion(string ModuleName, ModuleVersion newObject);
        public Task<bool> UpdateModuleVersion(string ModuleName, string ModuleVersion, ModuleVersion newObject);
        public Task<bool> DeleteModuleVersion(string ModuleName, string ModuleVersion);
        public Task<bool> UpdateAsync(string ModuleType, ModuleRegistry updatedObject);
        public Task<bool> RemoveByIdAsync(string ModuleType);
        public Task RemoveAllAsync();
        public Task<bool> Exists(string ModuleType);
    }
}
