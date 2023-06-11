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
        public Task<ICollection<string>> GetModules(string Email);
        public Task<bool> AssignModule(string Email, string ModuleId);
        public Task<bool> RevokeModule(string Email, string ModuleId);
        public Task<bool> AssignTherapist(string Email, string TherapistEmail);
        public Task<bool> RemoveTherapist(string Email, string TherapistEmail);
        public Task<bool> Exists(string Email);
    }
}
