using DatabaseApi.Models.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TherapistController : ControllerBase
    {


        private readonly ITherapistService _therapistService;
        private readonly IPatientService _patientService;
        public TherapistController(ITherapistService therapistService, IPatientService patientService)
        {
            _therapistService = therapistService;
            _patientService = patientService;
        }

        [HttpGet] //public Task<List<Therapist>> FindAllAsync();
        public async Task<ActionResult<List<Therapist>>> GetAllTherapists()
        {
            var therapists = await _therapistService.FindAllAsync();
            return Ok(therapists);
        }

        [HttpGet("{email}")] //public Task<Therapist?> FindByIdAsync(string Email);
        public async Task<ActionResult<Therapist>> GetTherapistById(string email)
        {
            var therapist = await _therapistService.FindByIdAsync(email);
            if (therapist == null)
            {
                return NotFound();
            }

            return Ok(therapist);
        }

        [HttpPost] //public Task<bool> CreateAsync(Therapist newObject);
        public async Task<ActionResult> CreateTherapist(Therapist newTherapist)
        {
            newTherapist.AcceptedPatients = new HashSet<string>();
            newTherapist.RequestedPatients = new HashSet<string>();
            var success = await _therapistService.CreateAsync(newTherapist);
            if (success)
                return Ok();
            return BadRequest();
        }
        [HttpDelete] //public Task RemoveAllAsync();
        public async Task<ActionResult> PurgeTherapists()
        {
            await _therapistService.RemoveAllAsync();
            return Ok();
        }

        [HttpPut("{email}")] //public Task<bool> UpdateAsync(string Email, Therapist updatedObject);
        public async Task<ActionResult> UpdateTherapist(string email, Therapist updatedTherapist)
        {
            var success = await _therapistService.UpdateAsync(email, updatedTherapist);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpDelete("{email}")] //public Task<bool> RemoveByIdAsync(string Email);
        public async Task<ActionResult> RemoveTherapist(string email)
        {
            var success = await _therapistService.RemoveByIdAsync(email);
            if (success)
                return Ok();
            return BadRequest();
        }

        
       

        [HttpPut("{email}/Patient/{PatientEmail}")] //public Task<ICollection<string>> GetModules(string Email);
        public async Task<ActionResult> AssignTherapist(string email, string PatientEmail)
        {
            var patient = await _patientService.FindByIdAsync(PatientEmail);
            if (patient == null) return NotFound();

            var therapist = await _therapistService.FindByIdAsync(email);
            if (therapist == null) return NotFound();


            if (patient.RequestedTherapists.Contains(email)) return BadRequest("Patient Already Requested");
            if (therapist.AcceptedPatients.Contains(PatientEmail))
                return BadRequest("Patient Already Accepted");


            if (therapist.RequestedPatients.Contains(PatientEmail))
            {
                therapist.RequestedPatients.Remove(PatientEmail);
                therapist.AcceptedPatients.Add(PatientEmail);
                var TherapistUpdatedSuccess = await _therapistService.UpdatePatientsAsync(email, therapist);
                if (!TherapistUpdatedSuccess)
                    return BadRequest("Failed to Update Therapist");
                patient.AcceptedTherapists.Add(email);
            }
            else
            {
                patient.RequestedTherapists.Add(email);
            }

            var success = await _patientService.UpdateTherapistsAsync(PatientEmail, patient);
            if (success)
                return Ok();

            return BadRequest();
        }
        [HttpDelete("{email}/Patient/{PatientEmail}")] //public Task<ICollection<string>> GetModules(string Email);
        public async Task<ActionResult> RemovePatients(string email, string PatientEmail)
        {
            var patient = await _patientService.FindByIdAsync(PatientEmail);
            if (patient == null) return NotFound();

            var therapist = await _therapistService.FindByIdAsync(email);
            if (therapist == null) return NotFound();


            therapist.RequestedPatients.Remove(patient.Email);
            therapist.AcceptedPatients.Remove(patient.Email);
            patient.AcceptedTherapists.Remove(email);
            patient.RequestedTherapists.Remove(email);


            var success = await _therapistService.UpdatePatientsAsync(email, therapist);
            if (!success) return BadRequest();

            success = await _patientService.UpdateTherapistsAsync(PatientEmail, patient);
            if (success)
                return Ok();
            return BadRequest();
        }


    }
}
