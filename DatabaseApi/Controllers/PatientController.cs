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

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Patient>>> GetAllPatients()
        {
            var patients = await _patientService.FindAllAsync();
            return Ok(patients);
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<Patient>> GetPatientByEmail(string email)
        {
            var patient = await _patientService.FindByIdAsync(email);
            if (patient == null)
                return NotFound();

            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePatient([FromBody] Patient newPatient)
        {
            await _patientService.CreateAsync(newPatient);
            return CreatedAtAction(nameof(GetPatientByEmail), new { email = newPatient.Email }, newPatient);
        }
        [HttpDelete]
        public async Task<ActionResult> PurgePatients()
        {
            await _patientService.RemoveAllAsync();
            return Ok();
        }

        [HttpPut("{email}")]
        public async Task<ActionResult> UpdatePatient(string email, [FromBody] Patient updatedPatient)
        {
            await _patientService.UpdateAsync(email, updatedPatient);
            return NoContent();
        }

        [HttpDelete("{email}")]
        public async Task<ActionResult> DeletePatient(string email)
        {
            await _patientService.RemoveByIdAsync(email);
            return NoContent();
        }

        [HttpGet("{email}/Modules")]
        public async Task<ActionResult<ICollection<Module>>> GetPatientModules(string email)
        {
            var modules = await _patientService.GetModules(email);
            return Ok(modules);
        }

        [HttpGet("{email}/Modules/{moduleId}")]
        public async Task<ActionResult<Module>> GetPatientModuleById(string email, string moduleId)
        {
            var module = await _patientService.GetModulesById(email, moduleId);
            if (module == null)
                return NotFound();

            return Ok(module);
        }

        [HttpPost("{email}/Modules")]
        public async Task<ActionResult> AddModuleToPatient(string email, [FromBody] Module module)
        {
            await _patientService.AddModule(email, module);
            return NoContent();
        }

        [HttpPut("{email}/Modules/{moduleId}")]
        public async Task<ActionResult> UpdatePatientModule(string email, string moduleId, [FromBody] Module updatedModule)
        {
            await _patientService.UpdateModule(email, moduleId, updatedModule);
            return NoContent();
        }

        [HttpDelete("{email}/Modules/{moduleId}")]
        public async Task<ActionResult> RemovePatientModule(string email, string moduleId)
        {
            await _patientService.RemoveModule(email, moduleId);
            return NoContent();
        }

        [HttpPost("{email}/Therapist")]
        public async Task<ActionResult> AssignTherapistToPatient(string email, [FromBody] string therapistEmail)
        {
            await _patientService.AssignTherapist(email, therapistEmail);
            return NoContent();
        }

        [HttpDelete("{email}/Therapist/{therapistEmail}")]
        public async Task<ActionResult> RemoveTherapistFromPatient(string email, string therapistEmail)
        {
            await _patientService.RemoveTherapist(email, therapistEmail);
            return NoContent();
        }
    }
}
