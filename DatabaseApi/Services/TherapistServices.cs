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

        public async Task<bool> UpdateAsync(string Email, Therapist updatedTherapist)
        {
            var existingTherapist = await _therapists.Find(c => c.Email == Email).FirstOrDefaultAsync();
            if (existingTherapist == null)
                return false;
            if (!string.IsNullOrEmpty(updatedTherapist.FirstName))
                existingTherapist.FirstName = updatedTherapist.FirstName;

            if (!string.IsNullOrEmpty(updatedTherapist.LastName))
                existingTherapist.LastName = updatedTherapist.LastName;

            if (!string.IsNullOrEmpty(updatedTherapist.Password))
                existingTherapist.Password = updatedTherapist.Password;

            if (updatedTherapist.Age != 0)
                existingTherapist.Age = updatedTherapist.Age;

            if (!string.IsNullOrEmpty(updatedTherapist.Credentials))
                existingTherapist.Credentials = updatedTherapist.Credentials;

            if (!string.IsNullOrEmpty(updatedTherapist.Description))
                existingTherapist.Description = updatedTherapist.Description;

            if (!string.IsNullOrEmpty(updatedTherapist.ProfilePicture))
                existingTherapist.ProfilePicture = updatedTherapist.ProfilePicture;
            if (!(await _therapists.ReplaceOneAsync(x => x.Email == Email, existingTherapist)).IsAcknowledged)
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
        public async Task<bool> UpdatePatientsAsync(string Email, Therapist _therapist)
        {
            var therapist = await FindByIdAsync(Email);
            if (therapist == null) return false;
            therapist.PatientsAccepted = _therapist.PatientsAccepted;
            therapist.PatientRequests = _therapist.PatientRequests;
            await _therapists.ReplaceOneAsync(x=>x.Email == Email, therapist);
            return true;
        }
        
      



    }
}
