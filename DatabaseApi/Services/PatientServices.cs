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

        public async Task CreateAsync(Patient newObject)
        {
            await _patients.InsertOneAsync(newObject);
        }

        public async Task UpdateAsync(string Email, Patient updatedObject)
        {
            await _patients.ReplaceOneAsync(x => x.Email == Email, updatedObject);
        }

        public async Task RemoveByIdAsync(string Email)
        {
            await _patients.DeleteOneAsync(x => x.Email == Email);
        }

        public async Task RemoveAllAsync()
        {
            await _patients.DeleteManyAsync(x => true);
        }
        public async Task<ICollection<Module>> GetModules(string Email)
        {
            var patient = await FindByIdAsync(Email);
            if (patient == null) return new List<Module>();
            return patient.WebPlatform.Modules;
        }

        public async Task<Module?> GetModulesById(string Email, string ModuleId)
        {
            var patient = await FindByIdAsync(Email);
            if (patient == null) return null;
            var module = patient.WebPlatform.Modules.FirstOrDefault(c => c.Id == ModuleId);
            return module;
        }
        public async Task AddModule(string Email, Module Module)
        {
            var patient = await FindByIdAsync(Email);
            if (patient == null) return;
            patient.WebPlatform.Modules.Add(Module);
            await _patients.ReplaceOneAsync(x => x.Email == Email, patient);
        }

        public async Task UpdateModule(string Email, string ModuleId, Module updatedModule)
        {
            var patient = await FindByIdAsync(Email);
            if (patient == null) return;

            var module = patient.WebPlatform.Modules.FirstOrDefault(c=>c.Id == ModuleId);
            if (module == null) return;
            patient.WebPlatform.Modules.Remove(module);
            if (!string.IsNullOrEmpty(updatedModule.Data)) module.Data = updatedModule.Data;
            if (!string.IsNullOrEmpty(updatedModule.Checksum)) module.Checksum = updatedModule.Checksum;
            if (updatedModule.Timestamp!=null) module.Timestamp = updatedModule.Timestamp;
            patient.WebPlatform.Modules.Add(module);
            await _patients.ReplaceOneAsync(x => x.Email == Email, patient);
        }

        public async Task RemoveModule(string Email, string ModuleId)
        {
            var patient = await FindByIdAsync(Email);
            if (patient == null) return;

            var module = patient.WebPlatform.Modules.FirstOrDefault(c => c.Id == ModuleId);
            if (module == null) return;
            patient.WebPlatform.Modules.Remove(module);
            
            await _patients.ReplaceOneAsync(x => x.Email == Email, patient);
        }

        public async Task AssignTherapist(string Email, string TherapistEmail)
        {
            var patient = await FindByIdAsync(Email);
            if (patient == null) return;
            patient.AssignedTherapist = TherapistEmail;
            await _patients.ReplaceOneAsync(x => x.Email == Email, patient);
        }

        public async Task RemoveTherapist(string Email, string TherapistEmail)
        {
            var patient = await FindByIdAsync(Email);
            if (patient == null) return;
            if (patient.AssignedTherapist != TherapistEmail) return;
            patient.AssignedTherapist = "";
            await _patients.ReplaceOneAsync(x => x.Email == Email, patient);
        }

        
    }
}
