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
        private readonly IModuleService _moduleService;

        public AssociationsController(IPatientService patientService, ITherapistService therapistService, IModuleService moduleService)
        {
            _patientService = patientService;
            _therapistService = therapistService;
            _moduleService = moduleService;
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


        //public Task<bool> AssignModule(string Email, string ModuleId);
        //public Task<bool> RevokeModule(string Email, string ModuleId);
        //public Task<bool> AssignModule(string Email, string ModuleId);
        //public Task<bool> RevokeModule(string Email, string ModuleId);
        [HttpGet("/api/AssignModule/{Patient}/{ModuleId}")]
        public async Task<ActionResult> AssignModule(string Patient, string ModuleId)
        {
            if (await _patientService.Exists(Patient) && await _moduleService.Exists(ModuleId))
            {
                var result = await _patientService.AssignModule(Patient, ModuleId);
                if (!result) return NotFound($"Couldn't assign module to patient {Patient}");
                result = await _moduleService.AssignModule(Patient, ModuleId);
                if (!result) return NotFound($"Couldn't assign patient to module {ModuleId} ");
                return Ok();
            }
            return BadRequest();
        }
        [HttpDelete("/api/RevokeModule/{Patient}/{ModuleId}")]
        public async Task<ActionResult> RevokeModule(string Patient, string ModuleId)
        {
            if (await _patientService.Exists(Patient) && await _moduleService.Exists(ModuleId))
            {
                var result = await _patientService.RevokeModule(Patient, ModuleId);
                if (!result) return NotFound($"Couldn't revoke module from patient {Patient}");
                await _moduleService.RevokeModule(Patient, ModuleId);
                if (!result) return NotFound($"Couldn't revoke patient from module {ModuleId} ");
                return Ok();
            }
            return BadRequest();
        }




    }
}
