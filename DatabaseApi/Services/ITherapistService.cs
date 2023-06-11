using DatabaseApi.Models.Entities;

namespace DatabaseApi.Services
{
    public interface ITherapistService
    {
        public Task<List<Therapist>> FindAllAsync();
        public Task<Therapist?> FindByIdAsync(string Email);
        public Task<bool> CreateAsync(Therapist newObject);
        public Task<bool> UpdateAsync(string Email, Therapist updatedObject);
        public Task<bool> RemoveByIdAsync(string Email);
        public Task RemoveAllAsync();
        public Task<ICollection<string>> GetPatients(string Email);
        public Task<bool> RemovePatient(string Email, string PatientEmail);
        public Task<bool> AssignPatient(string Email, string PatientEmail);
        public Task<bool> Exists(string Email);
    }
}
