using DatabaseApi.Models.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssociationsController : ControllerBase
    {
        private readonly IPatientService _patientService;
        private readonly ITherapistService _therapistService;

        public AssociationsController(IPatientService patientService, ITherapistService therapistService, IModuleService moduleService, IModuleRegistryService moduleRegistryService)
        {
            _patientService = patientService;
            _therapistService = therapistService;
        }


        [HttpGet("/api/AssignTherapist/{Patient}/{Therapist}")]
        public async Task<ActionResult> AssignTherapistPatient(string Therapist, string Patient)
        {
            if (await _patientService.Exists(Patient) && await _therapistService.Exists(Therapist)){
                var result = await _patientService.AssignTherapist(Patient, Therapist);
                if (!result) return NotFound($"Couldn't assign therapist to patient {Patient}");
                result = await _therapistService.AssignPatient(Therapist, Patient);
                if (!result) return NotFound($"Couldn't assign patient to therapist {Therapist}");
                return Ok();
            }
            return BadRequest();
        }
        [HttpDelete("/api/RevokeTherapist/{Patient}/{Therapist}")]
        public async Task<ActionResult> RemoveTherapistPatient(string Therapist, string Patient)
        {
            if (await _patientService.Exists(Patient) && await _therapistService.Exists(Therapist))
            {
                var result = await _patientService.RemoveTherapist(Patient, Therapist);
                if (!result) return NotFound($"Couldn't revoke therapist from patient {Patient}");
                result = await _therapistService.RemovePatient(Therapist, Patient);
                if (!result) return NotFound($"Couldn't revoke patient from therapist {Therapist}");
                return Ok();
            }
            return BadRequest();
        }


       


    }
}
