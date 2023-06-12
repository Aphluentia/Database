using DatabaseApi.Models.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly ITherapistService _therapistService;
        private readonly IModuleService _moduleService;
        private readonly IModuleRegistryService _moduleRegistryService;
        public PatientController(IPatientService patientService, ITherapistService therapistService, IModuleService moduleService, IModuleRegistryService moduleRegistryService)
        {
            _patientService = patientService;
            _therapistService = therapistService;
            _moduleService = moduleService;
            _moduleRegistryService = moduleRegistryService;
        }

        [HttpGet] //public Task<List<Patient>> FindAllAsync();
        public async Task<ActionResult<List<Patient>>> GetAllPatients()
        {
            var patients = await _patientService.FindAllAsync();
            return Ok(patients);
        }

        [HttpGet("{email}")] //public Task<Patient?> FindByIdAsync(string Email);
        public async Task<ActionResult<Patient>> GetPatientByEmail(string email)
        {
            var patient = await _patientService.FindByIdAsync(email);
            if (patient == null)
                return NotFound();

            return Ok(patient);
        }

        [HttpPost] //public Task<bool> CreateAsync(Patient newObject);
        public async Task<ActionResult> CreatePatient([FromBody] Patient newPatient)
        {
            if (!string.IsNullOrEmpty(newPatient.AssignedTherapist) && !(await _therapistService.Exists(newPatient.AssignedTherapist)))
                return NotFound();
            var modules = new List<Module>();
            foreach(Module value in newPatient.WebPlatform.Modules)
            {
                if (string.IsNullOrEmpty(value.ModuleTemplate.ModuleName) || string.IsNullOrEmpty(value.ModuleTemplate.VersionId)) return BadRequest();

                var moduleTemplate = await _moduleRegistryService.FindByIdAsync(value.ModuleTemplate.ModuleName);
                if (moduleTemplate == null) return NotFound();

                var moduleVersion = moduleTemplate.Versions.Where(c => c.VersionId == value.ModuleTemplate.VersionId).FirstOrDefault();
                if (moduleVersion == null) return NotFound();

                value.ModuleTemplate = CustomModuleTemplate.FromModuleTemplate(moduleTemplate, moduleVersion);
                modules.Add(value);
            }

            var success = await _patientService.CreateAsync(newPatient);
            if (success)
                return Ok();
            return BadRequest();
        }
        [HttpDelete] //public Task RemoveAllAsync();
        public async Task<ActionResult> PurgePatients()
        {
            await _patientService.RemoveAllAsync();
            return Ok();
        }

        [HttpPut("{email}")] //public Task<bool> UpdateAsync(string Email, Patient updatedObject);
        public async Task<ActionResult> UpdatePatient(string email, [FromBody] Patient updatedPatient)
        {
            var existingPatient = await _patientService.FindByIdAsync(email);
            if (existingPatient == null)
                return NotFound();

            if (!string.IsNullOrEmpty(updatedPatient.FirstName))
                existingPatient.FirstName = updatedPatient.FirstName;

            if (!string.IsNullOrEmpty(updatedPatient.LastName))
                existingPatient.LastName = updatedPatient.LastName;

            if (!string.IsNullOrEmpty(updatedPatient.PhoneNumber))
                existingPatient.PhoneNumber = updatedPatient.PhoneNumber;

            if (!string.IsNullOrEmpty(updatedPatient.CountryCode))
                existingPatient.CountryCode = updatedPatient.CountryCode;

            if (updatedPatient.Age != 0)
                existingPatient.Age = updatedPatient.Age;

            if (!string.IsNullOrEmpty(updatedPatient.ConditionName))
                existingPatient.ConditionName = updatedPatient.ConditionName;

            if (updatedPatient.ConditionAcquisitionDate != DateTime.MinValue)
                existingPatient.ConditionAcquisitionDate = updatedPatient.ConditionAcquisitionDate;

            if (!string.IsNullOrEmpty(updatedPatient.ProfilePicture))
                existingPatient.ProfilePicture = updatedPatient.ProfilePicture;

            if (!string.IsNullOrEmpty(updatedPatient.AssignedTherapist))
            {
                if (!(await _therapistService.Exists(updatedPatient.AssignedTherapist))) return NotFound();
                existingPatient.AssignedTherapist = updatedPatient.AssignedTherapist;
            }

            var modules = new List<Module>();
            foreach (Module value in updatedPatient.WebPlatform.Modules)
            {
                if (await _moduleService.Exists(value.Id)) await _moduleService.RemoveByIdAsync(value.Id);
                if (string.IsNullOrEmpty(value.ModuleTemplate.ModuleName) || string.IsNullOrEmpty(value.ModuleTemplate.VersionId)) return BadRequest();

                var moduleTemplate = await _moduleRegistryService.FindByIdAsync(value.ModuleTemplate.ModuleName);
                if (moduleTemplate == null) return NotFound();

                var moduleVersion = moduleTemplate.Versions.Where(c => c.VersionId == value.ModuleTemplate.VersionId).FirstOrDefault();
                if (moduleVersion == null) return NotFound();

                var existingModule = existingPatient.WebPlatform.Modules.Where(c => c.Id == value.Id).FirstOrDefault();
                if (existingModule != null){

                    if (!string.IsNullOrEmpty(value.Data)) existingModule.Data = $"@{value.Data}";

                    existingModule.Timestamp = value.Timestamp;
                    existingModule.Checksum = value.Checksum;

                    if (value.ModuleTemplate.VersionId != existingModule.ModuleTemplate.VersionId)
                    {
                        existingModule.ModuleTemplate = CustomModuleTemplate.FromModuleTemplate(moduleTemplate, moduleVersion);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(value.ModuleTemplate.HtmlCard)) existingModule.ModuleTemplate.HtmlCard = value.ModuleTemplate.HtmlCard;
                        if (!string.IsNullOrEmpty(value.ModuleTemplate.HtmlDashboard)) existingModule.ModuleTemplate.HtmlDashboard = value.ModuleTemplate.HtmlDashboard;
                    }
                }
                else
                {
                    existingModule = value;
                }
                
                modules.Add(existingModule);
            }
            existingPatient.WebPlatform.Modules = modules;

            var success = await _patientService.UpdateAsync(email, existingPatient);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpDelete("{email}")] //public Task<bool> RemoveByIdAsync(string Email);
        public async Task<ActionResult> DeletePatient(string email)
        {
            var success = await _patientService.RemoveByIdAsync(email);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpGet("{email}/Modules")] //public Task<ICollection<string>> GetModules(string Email);
        public async Task<ActionResult<ICollection<Module>>> GetPatientModules(string email)
        {
            var modules = await _patientService.GetModules(email);
            return Ok(modules);
        }
       

    }
}
