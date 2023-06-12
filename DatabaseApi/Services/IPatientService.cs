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
        public Task<bool> AssignTherapist(string Email, string TherapistEmail);
        public Task<bool> RemoveTherapist(string Email, string TherapistEmail);
        public Task<bool> Exists(string Email);

        public Task<ICollection<Module>> GetModules(string Email);
        public Task<bool> AddModule(string Email,Module module);
        public Task<bool> UpdateModule(string Email, string ModuleName, Module module);
        public Task<bool> RemoveModule(string Email, string ModuleName);
    }
}
