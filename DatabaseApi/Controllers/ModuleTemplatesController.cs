using DatabaseApi.Models.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleTemplatesController : ControllerBase
    {
        private readonly IModuleTemplatesService _moduleTemplatesService;
        public ModuleTemplatesController(IModuleTemplatesService moduleTemplatesService)
        {
            _moduleTemplatesService = moduleTemplatesService;
        }

        [HttpGet] //public Task<List<ModuleTemplate>> FindAllAsync();
        public async Task<ActionResult<List<ModuleTemplate>>> GetAllModuleTemplates()
        {
            var moduleTemplates = await _moduleTemplatesService.FindAllAsync();
            return Ok(moduleTemplates);
        }
         
        [HttpGet("{moduleType}")] //public Task<ModuleTemplate?> FindByIdAsync(string ModuleType);
        public async Task<ActionResult<ModuleTemplate>> GetModuleTemplateById(string moduleType)
        {
            var moduleTemplate = await _moduleTemplatesService.FindByIdAsync(moduleType);
            if (moduleTemplate == null)
            {
                return NotFound();
            }
            return Ok(moduleTemplate);
        }

        [HttpPost] //public Task<bool> CreateAsync(ModuleTemplate newObject);
        public async Task<ActionResult> CreateModuleTemplate(ModuleTemplate newModuleTemplate)
        {
            var success = await _moduleTemplatesService.CreateAsync(newModuleTemplate);
            if (success)
                return Ok();
            return BadRequest();
        }
    
        [HttpPut("{moduleType}")] //public Task<bool> UpdateAsync(string ModuleType, ModuleTemplate updatedObject);
        public async Task<ActionResult> UpdateModuleTemplate(string moduleType, ModuleTemplate updatedModuleTemplate)
        {
            var existingModuleTemplate = await _moduleTemplatesService.FindByIdAsync(moduleType);
            if (existingModuleTemplate == null) return NotFound();

            if (!string.IsNullOrEmpty(updatedModuleTemplate.HtmlCard)) existingModuleTemplate.HtmlCard = updatedModuleTemplate.HtmlCard;
            if (!string.IsNullOrEmpty(updatedModuleTemplate.HtmlDashboard)) existingModuleTemplate.HtmlDashboard = updatedModuleTemplate.HtmlDashboard;
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
