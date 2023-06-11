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
            var existingTherapist = await _therapistService.FindByIdAsync(email);
            if (existingTherapist == null)
                return NotFound();

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

                
            var success = await _therapistService.UpdateAsync(email, existingTherapist);
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

        
        [HttpGet("{email}/Patients")]  //public Task<ICollection<string>> GetPatients(string Email);
        public async Task<ActionResult<List<string>>> GetPatients(string email)
        {
            var patients = await _therapistService.GetPatients(email);
            return Ok(patients);
        }


    }
}
