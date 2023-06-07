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
        public TherapistController(ITherapistService therapistService)
        {
            _therapistService = therapistService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Therapist>>> GetAllTherapists()
        {
            var therapists = await _therapistService.FindAllAsync();
            return Ok(therapists);
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<Therapist>> GetTherapistById(string email)
        {
            var therapist = await _therapistService.FindByIdAsync(email);
            if (therapist == null)
            {
                return NotFound();
            }

            return Ok(therapist);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTherapist(Therapist newTherapist)
        {
            await _therapistService.CreateAsync(newTherapist);
            return Ok();
        }
        [HttpDelete]
        public async Task<ActionResult> PurgeTherapists()
        {
            await _therapistService.RemoveAllAsync();
            return Ok();
        }

        [HttpPut("{email}")]
        public async Task<ActionResult> UpdateTherapist(string email, Therapist updatedTherapist)
        {
            await _therapistService.UpdateAsync(email, updatedTherapist);
            return Ok();
        }

        [HttpDelete("{email}")]
        public async Task<ActionResult> RemoveTherapist(string email)
        {
            await _therapistService.RemoveByIdAsync(email);
            return Ok();
        }

        [HttpGet("{email}/Patients")]
        public async Task<ActionResult<List<string>>> GetTherapistPatients(string email)
        {
            var patients = await _therapistService.GetPatients(email);
            return Ok(patients);
        }

        [HttpDelete("{email}/Patients/{patientEmail}")]
        public async Task<ActionResult> RemoveTherapistPatient(string email, string patientEmail)
        {
            await _therapistService.RemovePatient(email, patientEmail);
            return Ok();
        }

        [HttpPost("{email}/Patients/{patientEmail}")]
        public async Task<ActionResult> AssignTherapistPatient(string email, string patientEmail)
        {
            await _therapistService.AssignPatient(email, patientEmail);
            return Ok();
        }
    }
}
