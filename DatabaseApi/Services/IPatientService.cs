using DatabaseApi.Models.Entities;

namespace DatabaseApi.Services
{
    public interface IPatientService
    {
        public Task<List<Patient>> FindAllAsync();
        public Task<Patient?> FindByIdAsync(string Email);
        public Task CreateAsync(Patient newObject);
        public Task UpdateAsync(string Email, Patient updatedObject);
        public Task RemoveByIdAsync(string Email);
        public Task RemoveAllAsync();
        public Task<ICollection<Module>> GetModules(string Email);
        public Task<Module> GetModulesById(string Email, string ModuleId);
        public Task AddModule(string Email, Module Module);
        public Task UpdateModule(string Email, string ModuleId, Module updatedModule);
        public Task RemoveModule(string Email, string ModuleId);
        public Task AssignTherapist(string Email, string TherapistEmail);
        public Task RemoveTherapist(string Email, string TherapistEmail);
    }
}
