using DatabaseApi.Models.Entities;

namespace DatabaseApi.Services
{
    public interface IModuleTemplatesService
    {
        public Task<List<ModuleTemplate>> FindAllAsync();
        public Task<ModuleTemplate?> FindByIdAsync(string ModuleType);
        public Task<bool> CreateAsync(ModuleTemplate newObject);
        public Task<bool> UpdateAsync(string ModuleType, ModuleTemplate updatedObject);
        public Task<bool> RemoveByIdAsync(string ModuleType);
        public Task RemoveAllAsync();
        public Task<bool> Exists(string ModuleType);
    }
}
