using DatabaseApi.Models.Entities;

namespace DatabaseApi.Services
{
    public interface IModuleTemplates
    {
        public Task<List<ModuleTemplate>> FindAllAsync();
        public Task<ModuleTemplate?> FindByIdAsync(string ModuleType);
        public Task CreateAsync(ModuleTemplate newObject);
        public Task UpdateAsync(string ModuleType, ModuleTemplate updatedObject);
        public Task RemoveByIdAsync(string ModuleType);
        public Task RemoveAllAsync();
    }
}
