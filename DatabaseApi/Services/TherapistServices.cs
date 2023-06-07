using DatabaseApi.Configurations;
using DatabaseApi.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DatabaseApi.Services
{
    public class TherapistServices: ITherapistService
    {

        private readonly IMongoCollection<Therapist> _therapists;
        private readonly IMongoCollection<Patient> _patients;
        public TherapistServices(IOptions<MongoConfigSection> databaseSettings) 
        {
            var mongoDatabase = new MongoClient(
                   databaseSettings.Value.ConnectionString).GetDatabase(databaseSettings.Value.DatabaseName);

            _therapists = mongoDatabase.GetCollection<Therapist>(
                databaseSettings.Value.TherapistCollectionName);
            _patients = mongoDatabase.GetCollection<Patient>(
                databaseSettings.Value.PatientsCollectionName);
        }

        public async Task<List<Therapist>> FindAllAsync()
        {
            return await _therapists.Find(_ => true).ToListAsync();
        }

        public async Task<Therapist?> FindByIdAsync(string Email)
        {
            return await _therapists.Find(x => x.Email == Email).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Therapist newObject)
        {
            await _therapists.InsertOneAsync(newObject);
        }

        public async Task UpdateAsync(string Email, Therapist updatedObject)
        {
            await _therapists.ReplaceOneAsync(x => x.Email == Email, updatedObject);
        }

        public async Task RemoveByIdAsync(string Email)
        {
            await _therapists.DeleteOneAsync(x => x.Email == Email);
        }

        public async Task RemoveAllAsync()
        {
            await _therapists.DeleteManyAsync(x => true);
        }

        public async Task<List<Patient>> GetTherapistPatients(string Email)
        {
            var therapist = await FindByIdAsync(Email);
            if (therapist == null) return new List<Patient>();
            var patients = new List<Patient>();
            foreach(var patientEmail in therapist.Patients)
            {
                var pat = await _patients.Find(x => x.Email == patientEmail).FirstOrDefaultAsync();
                if (pat != null)
                    patients.Add(pat);
            }
            return patients;
        }
        public async Task RemoveTherapistPatient(string Email, string PatientEmail)
        {
            var therapist = await FindByIdAsync(Email);
            if (therapist == null) return;
            var patient = await _patients.Find(x => x.Email == PatientEmail).FirstOrDefaultAsync();
            if (patient == null) return;
            therapist.Patients.Remove(PatientEmail);
            patient.AssignedTherapist = "";
            await _therapists.ReplaceOneAsync(x => x.Email == Email, therapist);
            await _patients.ReplaceOneAsync(x => x.Email == PatientEmail, patient);
        }
        public async Task AddTherapistPatient(string Email, string PatientEmail)
        {
            var therapist = await FindByIdAsync(Email);
            if (therapist == null) return;
            var patient = await _patients.Find(x => x.Email == PatientEmail).FirstOrDefaultAsync();
            if (patient == null) return;
            therapist.Patients.Add(PatientEmail);
            patient.AssignedTherapist = Email;
            await _therapists.ReplaceOneAsync(x => x.Email == Email, therapist);
            await _patients.ReplaceOneAsync(x => x.Email == PatientEmail, patient);
        }



    }
}
