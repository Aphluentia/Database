using DatabaseApi.Configurations;
using DatabaseApi.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DatabaseApi.Services
{
    public class TherapistServices: ITherapistService
    {

        private readonly IMongoCollection<Therapist> _therapists;
        public TherapistServices(IOptions<MongoConfigSection> databaseSettings) 
        {
            var mongoDatabase = new MongoClient(
                   databaseSettings.Value.ConnectionString).GetDatabase(databaseSettings.Value.DatabaseName);

            _therapists = mongoDatabase.GetCollection<Therapist>(
                databaseSettings.Value.TherapistCollectionName);
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

        public async Task<ICollection<string>> GetPatients(string Email)
        {
            var therapist = await FindByIdAsync(Email);
            if (therapist == null) return new List<string>();
            return therapist.Patients;
        }
        public async Task RemovePatient(string Email, string PatientEmail)
        {
            var therapist = await FindByIdAsync(Email);
            if (therapist == null) return;
            therapist.Patients.Remove(PatientEmail);
            await _therapists.ReplaceOneAsync(x => x.Email == Email, therapist);
        }
        public async Task AssignPatient(string Email, string PatientEmail)
        {
            var therapist = await FindByIdAsync(Email);
            if (therapist == null) return;
            therapist.Patients.Add(PatientEmail);
            await _therapists.ReplaceOneAsync(x => x.Email == Email, therapist);
        }



    }
}
