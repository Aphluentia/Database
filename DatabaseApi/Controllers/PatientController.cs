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
        private readonly IApplicationService _moduleRegistryService;
        public PatientController(IPatientService patientService, ITherapistService therapistService, IModuleService moduleService, IApplicationService moduleRegistryService)
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

        [HttpGet("{email}/Modules/{ModuleId}")] //public Task<ICollection<string>> GetModules(string Email);
        public async Task<ActionResult<ICollection<Module>>> GetPatientModuleById(string email, string ModuleId)
        {
            var modules = await _patientService.FindModuleById(email, ModuleId);
            return Ok(modules);
        }

        [HttpPost("{email}/Modules/{ModuleId}")] //public Task<ICollection<string>> GetModules(string Email);
        public async Task<ActionResult> AddModuleFromRegistry(string email, string ModuleId)
        {
            var patient = await _patientService.FindByIdAsync(email);
            if (patient == null) return NotFound("Patient Not Found");

            var newModule = await _moduleService.FindByIdAsync(ModuleId);
            if (newModule == null) return NotFound("Module Not Found");

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
          
            if (patient.Modules.Any(c => c.Id == newModule.Id)) return BadRequest("Module Already Exists");
             
            var success = await _patientService.AddModule(email, newModule);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpPut("{email}/Modules/{ModuleId}")]
        public async Task<IActionResult> UpdatePatientModule(string email, string ModuleId, [FromBody] Module updatedModule)
        {
            var patient = await _patientService.FindByIdAsync(email);
            if (patient == null)
                return NotFound();
            var existingModule = patient.Modules.Where(c => c.Id == ModuleId).FirstOrDefault();
            if (existingModule == null) return NotFound();

            if (updatedModule.ModuleData.DataStructure != null)
            {
                existingModule.ModuleData.DataStructure = updatedModule.ModuleData.DataStructure;
            }
            if (!string.IsNullOrEmpty(updatedModule.ModuleData.HtmlCard))
            {
                existingModule.ModuleData.HtmlCard = updatedModule.ModuleData.HtmlCard;
            }
            if (!string.IsNullOrEmpty(updatedModule.ModuleData.HtmlDashboard))
            {
                existingModule.ModuleData.HtmlDashboard = updatedModule.ModuleData.HtmlDashboard;
            }
            existingModule.ModuleData.Timestamp = updatedModule.ModuleData.Timestamp;
            existingModule.ModuleData.Checksum = updatedModule.ModuleData.Checksum;

            var success = await _patientService.UpdateModule(email, ModuleId, existingModule);
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

            if (therapist.RequestedPatients.Contains(email)) return BadRequest("Therapist Already Requested");
            if (patient.AcceptedTherapists.Contains(TherapistEmail))
                return BadRequest("Therapist Already Accepted");
            if (patient.RequestedTherapists.Contains(TherapistEmail))
            {
                patient.RequestedTherapists.Remove(TherapistEmail);
                patient.AcceptedTherapists.Add(TherapistEmail);
                var PatientUpdatedSuccess = await _patientService.UpdateTherapistsAsync(email, patient);
                if (!PatientUpdatedSuccess)
                    return BadRequest("Failed to Update Patient");
                therapist.AcceptedPatients.Add(email);
            }
            else
            {
                therapist.RequestedPatients.Add(email);
            }

            var success = await _therapistService.UpdatePatientsAsync(TherapistEmail, therapist);
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

            therapist.RequestedPatients.Remove(patient.Email);
            therapist.AcceptedPatients.Remove(patient.Email);
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
