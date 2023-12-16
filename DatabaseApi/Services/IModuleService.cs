using DatabaseApi.Models.Entities;

namespace DatabaseApi.Services
{
    public interface IModuleService
    {
        public Task<List<Module>> FindAllAsync();
        public Task<Module?> FindByIdAsync(string ModuleId);
        public Task<bool> CreateAsync(Module newObject);
        public Task<bool> UpdateAsync(string ModuleId, Module updatedObject);
        public Task<bool> RemoveByIdAsync(string ModuleId);
        public Task RemoveAllAsync();
    }
}
