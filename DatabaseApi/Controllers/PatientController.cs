using DatabaseApi.Models;
using DatabaseApi.Models.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
            newPatient.AcceptedTherapists = new HashSet<string>();
            newPatient.RequestedTherapists = new HashSet<string>();
            newPatient.WebPlatform.Modules = new HashSet<Module>();
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
            var success = await _patientService.UpdateAsync(email, updatedPatient);
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

       
        [HttpPost("{email}/Modules/{ModuleId}")] //public Task<ICollection<string>> GetModules(string Email);
        public async Task<ActionResult> AddModuleFromRegistry(string email, string ModuleId)
        {
            var patient = await _patientService.FindByIdAsync(email);
            if (patient == null) return NotFound();

            var newModule = await _moduleService.FindByIdAsync(ModuleId);
            if (newModule == null) return NotFound();

            await _moduleService.RemoveByIdAsync(newModule.Id);
             
            var success = await _patientService.AddModule(email, newModule);
            if (success)
                return Ok();
            return BadRequest();
        }   
        
        [HttpPost("{email}/Modules")] //public Task<ICollection<string>> GetModules(string Email);
        public async Task<ActionResult> AddNewModule(string email, [FromBody] Module newModule)
        {
            var patient = await _patientService.FindByIdAsync(email);
            if (patient == null) return NotFound();

            var moduleTemplate = await _moduleRegistryService.FindByIdAsync(newModule.ModuleTemplate.ModuleName);
            if (moduleTemplate == null) return NotFound();

            var moduleVersion = moduleTemplate.Versions.Where(c => c.VersionId == newModule.ModuleTemplate.VersionId).FirstOrDefault();
            if (moduleVersion == null) return NotFound();

            var cModuleTemplate = CustomModuleTemplate.FromModuleTemplate(moduleTemplate, moduleVersion);
            newModule.ModuleTemplate = cModuleTemplate;
            try
            {
                JsonDocument.Parse(newModule.Data.ToString());
            }
            catch (JsonException ex)
            {
                return BadRequest(ex);
            }
            if (patient.WebPlatform.Modules.Any(c => c.Id == newModule.Id)) return BadRequest();
             
            var success = await _patientService.AddModule(email, newModule);
            if (success)
                return Ok();
            return BadRequest();
        }
        [HttpPut("{email}/Modules/{ModuleId}")]
        public async Task<IActionResult> UpdatePatientModuleVersion(string email, string ModuleId, string? Version, [FromBody] Module updatedModule)
        {
            var patient = await _patientService.FindByIdAsync(email);
            if (patient == null)
                return NotFound();
            var module = patient.WebPlatform.Modules.Where(c => c.Id == ModuleId).FirstOrDefault();
            if (module == null) return NotFound();
            if (!string.IsNullOrEmpty(Version))
            {
                var moduleTemplate = await _moduleRegistryService.FindByIdAsync(module.ModuleTemplate.ModuleName);
                if (moduleTemplate == null) return NotFound();

                var moduleVersion = moduleTemplate.Versions.Where(c => c.VersionId == Version).FirstOrDefault();
                if (moduleVersion == null) return NotFound();

                var newCustomModuleTemplate = CustomModuleTemplate.FromModuleTemplate(moduleTemplate, moduleVersion);

                module.ModuleTemplate = newCustomModuleTemplate;
            }
            if (!string.IsNullOrEmpty(updatedModule.Data))
            {
                try
                {
                    JsonDocument.Parse(updatedModule.Data.ToString());
                    module.Data = updatedModule.Data;
                    module.Checksum = updatedModule.Checksum;
                    module.Timestamp = updatedModule.Timestamp;
                }
                catch (Exception ex)
                {
                    return BadRequest(ex);
                }
             

            }
            if (!string.IsNullOrEmpty(updatedModule.ModuleTemplate.HtmlCard)) module.ModuleTemplate.HtmlCard = updatedModule.ModuleTemplate.HtmlCard;
            if (!string.IsNullOrEmpty(updatedModule.ModuleTemplate.HtmlDashboard)) module.ModuleTemplate.HtmlDashboard = updatedModule.ModuleTemplate.HtmlDashboard;
            module.ModuleTemplate.Timestamp = DateTime.UtcNow;
            var success = await _patientService.UpdateModule(email, ModuleId, module);
            if (success)
                return Ok();
            return BadRequest();
        }
      
        [HttpDelete("{email}/Modules/{ModuleId}")] //public Task<ICollection<string>> GetModules(string Email);
        public async Task<ActionResult> RemoveModuleFromPatient(string email, string ModuleId)
        {
            var patient = await _patientService.FindByIdAsync(email);
            if (patient == null) return NotFound();

            
            var success = await _patientService.RemoveModule(email, ModuleId);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpPut("{email}/Therapist/{TherapistEmail}")] //public Task<ICollection<string>> GetModules(string Email);
        public async Task<ActionResult> RequestTherapist(string email, string TherapistEmail)
        {
            var patient = await _patientService.FindByIdAsync(email);
            if (patient == null) return NotFound();

            var therapist = await _therapistService.FindByIdAsync(TherapistEmail);
            if (therapist == null) return NotFound();
            if (patient.AcceptedTherapists.Contains(TherapistEmail))
                return BadRequest();

            therapist.PatientRequests.Add(patient.Email);
            patient.RequestedTherapists.Add(TherapistEmail);

            var success = await _therapistService.UpdatePatientsAsync(TherapistEmail, therapist);
            if (!success) return BadRequest();

            success = await _patientService.UpdateTherapistsAsync(email, patient);
            if (success)
                return Ok();
            return BadRequest();
        }
        [HttpDelete("{email}/Therapist/{TherapistEmail}")] //public Task<ICollection<string>> GetModules(string Email);
        public async Task<ActionResult> RemoveTherapist(string email, string TherapistEmail)
        {
            var patient = await _patientService.FindByIdAsync(email);
            if (patient == null) return NotFound();

            var therapist = await _therapistService.FindByIdAsync(TherapistEmail);
            if (therapist == null) return NotFound();

            therapist.PatientRequests.Remove(patient.Email);
            therapist.PatientsAccepted.Remove(patient.Email);
            patient.AcceptedTherapists.Remove(TherapistEmail);
            patient.RequestedTherapists.Remove(TherapistEmail);

            var success = await _therapistService.UpdatePatientsAsync(TherapistEmail, therapist);
            if (!success) return BadRequest();

            success = await _patientService.UpdateTherapistsAsync(email, patient);
            if (success)
                return Ok();
            return BadRequest();
        }


    }
}
