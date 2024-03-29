﻿using DatabaseApi.Models.Entities;

namespace DatabaseApi.Services
{
    public interface IApplicationService
    {
        public Task<List<Application>> FindAllAsync();
        public Task<Application?> FindByIdAsync(string ModuleType);
        public Task<bool> CreateAsync(Application newObject);
        public Task<ICollection<ModuleVersion>> GetVersions(string ModuleName);
        public Task<bool> CreateVersion(string ModuleName, ModuleVersion newObject);
        public Task<bool> UpdateModuleVersion(string ModuleName, string ModuleVersion, ModuleVersion newObject);
        public Task<bool> DeleteModuleVersion(string ModuleName, string ModuleVersion);
        public Task<bool> UpdateAsync(string ModuleType, Application updatedObject);
        public Task<bool> RemoveByIdAsync(string ModuleType);
        public Task RemoveAllAsync();
    }
}
