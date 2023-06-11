using DatabaseApi.Configurations;
using DatabaseApi.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ThirdParty.BouncyCastle.Utilities.IO.Pem;

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

        public async Task<bool> CreateAsync(Therapist newObject)
        {
            if (await _therapists.Find(c => c.Email == newObject.Email).FirstOrDefaultAsync() != null)
                return false;
            await _therapists.InsertOneAsync(newObject);
            return true;
        }

        public async Task<bool> UpdateAsync(string Email, Therapist updatedObject)
        {
            if (await _therapists.Find(c => c.Email == Email).FirstOrDefaultAsync() == null)
                return false;
            if (!(await _therapists.ReplaceOneAsync(x => x.Email == Email, updatedObject)).IsAcknowledged)
                return false;
            return true;
        }

        public async Task<bool> RemoveByIdAsync(string Email)
        {
            if (await _therapists.Find(c => c.Email == Email).FirstOrDefaultAsync() == null)
                return false;
            await _therapists.DeleteOneAsync(x => x.Email == Email);
            return true;
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
        public async Task<bool> RemovePatient(string Email, string PatientEmail)
        {
            var therapist = await _therapists.Find(c => c.Email == Email).FirstOrDefaultAsync();
            if (therapist == null) return false;
            therapist.Patients.Remove(PatientEmail);
            if(!(await _therapists.ReplaceOneAsync(x => x.Email == Email, therapist)).IsAcknowledged) return false;
            return true;
        }
        public async Task<bool> AssignPatient(string Email, string PatientEmail)
        {
            var therapist = await _therapists.Find(c => c.Email == Email).FirstOrDefaultAsync();
            if (therapist == null) return false;
            therapist.Patients.Add(PatientEmail);
            if (!(await _therapists.ReplaceOneAsync(x => x.Email == Email, therapist)).IsAcknowledged) return false;
            return true;
        }
        public async Task<bool> Exists(string Email)
        {
            var therapists = await _therapists.Find(c => c.Email == Email).FirstOrDefaultAsync();
            if (therapists == null) return false;
            return true;
        }



    }
}
