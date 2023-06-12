using DatabaseApi.Models.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleRegistryController : ControllerBase
    {
        private readonly IModuleRegistryService _moduleTemplatesService;
        public ModuleRegistryController(IModuleRegistryService moduleTemplatesService)
        {
            _moduleTemplatesService = moduleTemplatesService;
        }

        [HttpGet] //public Task<List<ModuleTemplate>> FindAllAsync();
        public async Task<ActionResult<List<ModuleRegistry>>> GetAllModuleTemplates()
        {
            var moduleTemplates = await _moduleTemplatesService.FindAllAsync();
            return Ok(moduleTemplates);
        }
         
        [HttpGet("{moduleType}")] //public Task<ModuleTemplate?> FindByIdAsync(string ModuleType);
        public async Task<ActionResult<ModuleRegistry>> GetModuleTemplateById(string moduleType)
        {
            var moduleTemplate = await _moduleTemplatesService.FindByIdAsync(moduleType);
            if (moduleTemplate == null)
            {
                return NotFound();
            }
            return Ok(moduleTemplate);
        }

        [HttpPost] //public Task<bool> CreateAsync(ModuleTemplate newObject);
        public async Task<ActionResult> CreateModuleTemplate(ModuleRegistry newModuleTemplate)
        {
            var success = await _moduleTemplatesService.CreateAsync(newModuleTemplate);
            if (success)
                return Ok();
            return BadRequest();
        }
    
        [HttpPut("{moduleType}")] //public Task<bool> UpdateAsync(string ModuleType, ModuleTemplate updatedObject);
        public async Task<ActionResult> UpdateModuleVersion(string moduleType, ModuleVersion updatedVersion)
        {
            var existingModuleTemplate = await _moduleTemplatesService.FindByIdAsync(moduleType);
            if (existingModuleTemplate == null) return NotFound();
            switch(existingModuleTemplate.Versions.Any(c => (c.VersionId == updatedVersion.VersionId)))
            {
                case true:
                    var Versions = existingModuleTemplate.Versions.Where(c => (c.VersionId != updatedVersion.VersionId)).ToList();
                    var IdentifiedVersion = existingModuleTemplate.Versions.Where(c => (c.VersionId == updatedVersion.VersionId)).FirstOrDefault();
                    if (!string.IsNullOrEmpty(updatedVersion.HtmlCard)) IdentifiedVersion.HtmlCard = updatedVersion.HtmlCard;
                    if (!string.IsNullOrEmpty(updatedVersion.HtmlDashboard)) IdentifiedVersion.HtmlDashboard = updatedVersion.HtmlDashboard;
                    Versions.Add(IdentifiedVersion);
                    existingModuleTemplate.Versions = Versions;
                    break;
                case false:
                    existingModuleTemplate.Versions.Add(updatedVersion);
                    break;
                default:
                    return BadRequest();
            }
            
            var success = await _moduleTemplatesService.UpdateAsync(moduleType, existingModuleTemplate);
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

        [HttpDelete("{moduleType}")] //public Task<bool> RemoveByIdAsync(string ModuleType);
        public async Task<ActionResult> RemoveModuleTemplate(string moduleType)
        {
            var success = await _moduleTemplatesService.RemoveByIdAsync(moduleType);
            if (success)
                return Ok();
            return BadRequest();
        }
    }
}
