using DatabaseApi.Models.Entities;

namespace DatabaseApi.Services
{
    public interface IPatientService
    {
        public Task<List<Patient>> FindAllAsync();
        public Task<Patient?> FindByIdAsync(string Email);
        public Task<bool> CreateAsync(Patient newObject);
        public Task<bool> UpdateAsync(string Email, Patient updatedObject);
        public Task<bool> RemoveByIdAsync(string Email);
        public Task RemoveAllAsync();
        public Task<ICollection<Module>> GetModules(string Email);
        public Task<bool> RemoveModule(string Email, string ModuleName);
        public Task<bool> UpdateTherapistsAsync(string Email, Patient _patient);
        public Task<bool> AddModule(string Email, Module module);
        public Task<bool> UpdateModule(string Email, string ModuleId, Module module);
    }
}
