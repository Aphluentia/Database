using DatabaseApi.Models.Entities;

namespace DatabaseApi.Services
{
    public interface ITherapistService
    {
        public Task<List<Therapist>> FindAllAsync();

        public Task<Therapist?> FindByIdAsync(string Email);

        public Task CreateAsync(Therapist newObject);

        public Task UpdateAsync(string Email, Therapist updatedObject);
        public Task RemoveByIdAsync(string Email);
        public Task RemoveAllAsync();
        public Task<List<Patient>> GetTherapistPatients(string Email);
        public Task RemoveTherapistPatient(string Email, string PatientEmail);
        public Task AddTherapistPatient(string Email, string PatientEmail);
    }
}
