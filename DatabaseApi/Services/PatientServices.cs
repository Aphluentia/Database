using DatabaseApi.Configurations;
using DatabaseApi.Models.Entities;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DatabaseApi.Services
{
    public class PatientServices: IPatientService
    {
        private readonly IMongoCollection<Patient> _patients;
        public PatientServices(IOptions<MongoConfigSection> databaseSettings)
        {
            var mongoDatabase = new MongoClient(
                   databaseSettings.Value.ConnectionString).GetDatabase(databaseSettings.Value.DatabaseName);

            _patients = mongoDatabase.GetCollection<Patient>(
                databaseSettings.Value.PatientsCollectionName);
        }

        public async Task<List<Patient>> FindAllAsync()
        {
            return await _patients.Find(_ => true).ToListAsync();
        }

        public async Task<Patient?> FindByIdAsync(string Email)
        {
            return await _patients.Find(x => x.Email == Email).FirstOrDefaultAsync();
        }

        public async Task<bool> CreateAsync(Patient newObject)
        {
            if (await _patients.Find(c => c.Email == newObject.Email || c.WebPlatform.WebPlatformId == newObject.WebPlatform.WebPlatformId).FirstOrDefaultAsync() != null)
                return false;
            await _patients.InsertOneAsync(newObject);
            return true;
        }

        public async Task<bool> UpdateAsync(string Email, Patient updatedObject)
        {
            if (await _patients.Find(c => c.Email == Email).FirstOrDefaultAsync() == null)
                return false;
            await _patients.ReplaceOneAsync(x => x.Email == Email, updatedObject);
            return true;
        }

        public async Task<bool> RemoveByIdAsync(string Email)
        {
            if (await _patients.Find(c => c.Email == Email).FirstOrDefaultAsync() == null)
                return false;
            await _patients.DeleteOneAsync(x => x.Email == Email);
            return true;
        }

        public async Task RemoveAllAsync()
        {
            await _patients.DeleteManyAsync(x => true);
        }
       

        public async Task<bool> AssignTherapist(string Email, string TherapistEmail)
        {
            var patient =  await _patients.Find(c => c.Email == Email).FirstOrDefaultAsync();
            if (patient == null || !string.IsNullOrEmpty(patient.AssignedTherapist)) return false;
            patient.AssignedTherapist = TherapistEmail;
            if (!(await _patients.ReplaceOneAsync(x => x.Email == Email, patient)).IsAcknowledged)
                return false;
            return true;
        }

        public async Task<bool> RemoveTherapist(string Email, string TherapistEmail)
        {
            var patient = await _patients.Find(c => c.Email == Email).FirstOrDefaultAsync();
            if (patient == null || patient.AssignedTherapist != TherapistEmail) return false;
            patient.AssignedTherapist = "";
            if (!(await _patients.ReplaceOneAsync(x => x.Email == Email, patient)).IsAcknowledged)
                return false;
            return true;
        }

        public async Task<bool> Exists(string Email) 
        {
            var patient = await _patients.Find(c => c.Email == Email).FirstOrDefaultAsync();
            if (patient == null) return false;
            return true;
        }

        public async Task<ICollection<Module>> GetModules(string Email)
        {
            if (await _patients.Find(c => c.Email == Email).FirstOrDefaultAsync() == null)
                return null;
            return (await _patients.Find(c => c.Email == Email).FirstOrDefaultAsync()).WebPlatform.Modules;
        }

        public async Task<bool> AddModule(string Email, Module module)
        {
            var patient = await _patients.Find(c => c.Email == Email).FirstOrDefaultAsync();
            if (patient == null)
                return false;
            if (patient.WebPlatform.Modules.Any(c => c.Id == module.Id)) return false;
            patient.WebPlatform.Modules.Add(module);
            if (!(await _patients.ReplaceOneAsync(c => c.Email == Email, patient)).IsAcknowledged) return false;
            return true;
        }

        public async Task<bool> UpdateModule(string Email,string ModuleId, Module module)
        {
            var patient = await _patients.Find(c => c.Email == Email).FirstOrDefaultAsync();
            if (patient == null)
                return false;
            var m = patient.WebPlatform.Modules.Where(c => c.Id == ModuleId).FirstOrDefault();
            if (m == null) return false;
            patient.WebPlatform.Modules.Remove(m);
            
            patient.WebPlatform.Modules.Add(module);
            if (!(await _patients.ReplaceOneAsync(c => c.Email == Email, patient)).IsAcknowledged) return false;
            return true;
        }

        

        public async Task<bool> RemoveModule(string Email, string ModuleId)
        {
            var patient = await _patients.Find(c => c.Email == Email).FirstOrDefaultAsync();
            if (patient == null)
                return false;
            var m = patient.WebPlatform.Modules.Where(c => c.Id == ModuleId).FirstOrDefault();
            if (m == null) return false;
            patient.WebPlatform.Modules.Remove(m);
            if (!(await _patients.ReplaceOneAsync(c => c.Email == Email, patient)).IsAcknowledged) return false;
            return true;
        }
    }
}
