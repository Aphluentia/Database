using DatabaseApi.Models.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IModuleRegistryService _moduleTemplatesService;
        public ApplicationController(IModuleRegistryService moduleTemplatesService)
        {
            _moduleTemplatesService = moduleTemplatesService;
        }

        [HttpGet] //public Task<List<ModuleTemplate>> FindAllAsync();
        public async Task<ActionResult<List<Application>>> GetAllModuleTemplates()
        {
            var moduleTemplates = await _moduleTemplatesService.FindAllAsync();
            return Ok(moduleTemplates);
        }
         
        [HttpGet("{appId}")] //public Task<ModuleTemplate?> FindByIdAsync(string ModuleType);
        public async Task<ActionResult<Application>> GetModuleTemplateById(string appId)
        {
            var moduleTemplate = await _moduleTemplatesService.FindByIdAsync(appId);
            if (moduleTemplate == null)
            {
                return NotFound();
            }
            return Ok(moduleTemplate);
        }

        [HttpPost] //public Task<bool> CreateAsync(ModuleTemplate newObject);
        public async Task<IActionResult> CreateModuleTemplate(Application newModuleTemplate)
        {
            var success = await _moduleTemplatesService.CreateAsync(newModuleTemplate);
            if (success)
                return Ok();
            return BadRequest();
        }
   

        [HttpPost("{appId}/Version")] //public Task<bool> UpdateAsync(string ModuleType, ModuleTemplate updatedObject);
        public async Task<ActionResult> AddModuleVersion(string appId, ModuleVersion updatedVersion)
        {
            var existingModuleTemplate = await _moduleTemplatesService.FindByIdAsync(appId);
            if (existingModuleTemplate == null) return NotFound();
            var version = existingModuleTemplate.Versions.Where(c=>c.VersionId == updatedVersion.VersionId).FirstOrDefault();
            if (version != null) return BadRequest();
            existingModuleTemplate.Versions.Add(updatedVersion);
            
            var success = await _moduleTemplatesService.UpdateAsync(appId, existingModuleTemplate);
            if (success)
                return Ok();
            return BadRequest();
        }
        [HttpPut("{appId}/Version/{ModuleVersion}")] //public Task<bool> UpdateAsync(string ModuleType, ModuleTemplate updatedObject);
        public async Task<ActionResult> UpdateModuleVersion(string appId,string ModuleVersion, ModuleVersion updatedVersion)
        {
            var existingModuleTemplate = await _moduleTemplatesService.FindByIdAsync(appId);
            if (existingModuleTemplate == null) return NotFound();
            var existingVersion = existingModuleTemplate.Versions.Where(c => (c.VersionId == ModuleVersion)).FirstOrDefault();
            if (existingVersion == null) return NotFound();

            existingModuleTemplate.Versions = existingModuleTemplate.Versions.Where(c => (c.VersionId != updatedVersion.VersionId)).ToList();
      
            if (!string.IsNullOrEmpty(updatedVersion.HtmlCard)) existingVersion.HtmlCard = updatedVersion.HtmlCard;
            if (!string.IsNullOrEmpty(updatedVersion.HtmlDashboard)) existingVersion.HtmlDashboard = updatedVersion.HtmlDashboard;

            existingModuleTemplate.Versions.Add(existingVersion);
            var success = await _moduleTemplatesService.UpdateAsync(appId, existingModuleTemplate);
            if (success)
                return Ok();
            return BadRequest();
        }
        [HttpDelete("{appId}/Version/{ModuleVersion}")] //public Task<bool> UpdateAsync(string ModuleType, ModuleTemplate updatedObject);
        public async Task<ActionResult> DeleteModuleVersion(string appId, string ModuleVersion)
        {
            var existingModuleTemplate = await _moduleTemplatesService.FindByIdAsync(appId);
            if (existingModuleTemplate == null) return NotFound();
            var existingVersion = existingModuleTemplate.Versions.Where(c => (c.VersionId == ModuleVersion)).FirstOrDefault();
            if (existingVersion == null) return NotFound();

            existingModuleTemplate.Versions = existingModuleTemplate.Versions.Where(c => (c.VersionId != ModuleVersion)).ToList();

            var success = await _moduleTemplatesService.UpdateAsync(appId, existingModuleTemplate);
            if (success)
                return Ok();
            return BadRequest();
        }


        [HttpDelete] //public Task RemoveAllAsync();
        public async Task<ActionResult> RemoveAllModuleTemplates()
        {
            await _moduleTemplatesService.RemoveAllAsync();
            return Ok();
        }

        [HttpDelete("{appId}")] //public Task<bool> RemoveByIdAsync(string ModuleType);
        public async Task<ActionResult> RemoveModuleTemplate(string appId)
        {
            var success = await _moduleTemplatesService.RemoveByIdAsync(appId);
            if (success)
                return Ok();
            return BadRequest();
        }
    }
}
