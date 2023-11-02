using DatabaseApi.Models.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _ApplicationsService;
        public ApplicationController(IApplicationService ModuleDatasService)
        {
            _ApplicationsService = ModuleDatasService;
        }

        [HttpGet] //public Task<List<ModuleData>> FindAllAsync();
        public async Task<ActionResult<List<Application>>> GetAllModuleDatas()
        {
            var ModuleDatas = await _ApplicationsService.FindAllAsync();
            return Ok(ModuleDatas);
        }
         
        [HttpGet("{appId}")] //public Task<ModuleData?> FindByIdAsync(string ModuleType);
        public async Task<ActionResult<Application>> GetModuleDataById(string appId)
        {
            var ModuleData = await _ApplicationsService.FindByIdAsync(appId);
            if (ModuleData == null)
            {
                return NotFound();
            }
            return Ok(ModuleData);
        }

        [HttpPost] //public Task<bool> CreateAsync(ModuleData newObject);
        public async Task<IActionResult> CreateModuleData(Application newModuleData)
        {
            var success = await _ApplicationsService.CreateAsync(newModuleData);
            if (success)
                return Ok();
            return BadRequest();
        }
   

        [HttpPost("{applicationName}/Version")] //public Task<bool> UpdateAsync(string ModuleType, ModuleData updatedObject);
        public async Task<ActionResult> AddModuleVersion(string applicationName, ModuleVersion updatedVersion)
        {
            var existingModuleData = await _ApplicationsService.FindByIdAsync(applicationName);
            if (existingModuleData == null) return NotFound();

            var version = existingModuleData.Versions.Where(c=>c.VersionId == updatedVersion.VersionId).FirstOrDefault();
            if (version != null) return BadRequest();
            existingModuleData.Versions.Add(updatedVersion);
            
            var success = await _ApplicationsService.UpdateAsync(applicationName, existingModuleData);
            if (success)
                return Ok();
            return BadRequest();
        }
     
        [HttpDelete("{appId}/Version/{ModuleVersion}")] //public Task<bool> UpdateAsync(string ModuleType, ModuleData updatedObject);
        public async Task<ActionResult> DeleteModuleVersion(string appId, string ModuleVersion)
        {
            var existingModuleData = await _ApplicationsService.FindByIdAsync(appId);
            if (existingModuleData == null) return NotFound();
            var existingVersion = existingModuleData.Versions.Where(c => (c.VersionId == ModuleVersion)).FirstOrDefault();
            if (existingVersion == null) return NotFound();

            existingModuleData.Versions = existingModuleData.Versions.Where(c => (c.VersionId != ModuleVersion)).ToList();

            var success = await _ApplicationsService.UpdateAsync(appId, existingModuleData);
            if (success)
                return Ok();
            return BadRequest();
        }


        [HttpDelete] //public Task RemoveAllAsync();
        public async Task<ActionResult> RemoveAllModuleDatas()
        {
            await _ApplicationsService.RemoveAllAsync();
            return Ok();
        }

        [HttpDelete("{appId}")] //public Task<bool> RemoveByIdAsync(string ModuleType);
        public async Task<ActionResult> RemoveModuleData(string appId)
        {
            var success = await _ApplicationsService.RemoveByIdAsync(appId);
            if (success)
                return Ok();
            return BadRequest();
        }
    }
}
