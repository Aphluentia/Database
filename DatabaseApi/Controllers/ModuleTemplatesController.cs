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

        [HttpGet]
        public async Task<ActionResult<List<ModuleTemplate>>> GetAllModuleTemplates()
        {
            var moduleTemplates = await _moduleTemplatesService.FindAllAsync();
            return Ok(moduleTemplates);
        }

        [HttpGet("{moduleType}")]
        public async Task<ActionResult<ModuleTemplate>> GetModuleTemplateById(string moduleType)
        {
            var moduleTemplate = await _moduleTemplatesService.FindByIdAsync(moduleType);
            if (moduleTemplate == null)
            {
                return NotFound();
            }

            return Ok(moduleTemplate);
        }

        [HttpPost]
        public async Task<ActionResult> CreateModuleTemplate(ModuleTemplate newModuleTemplate)
        {
            await _moduleTemplatesService.CreateAsync(newModuleTemplate);
            return Ok();
        }
    
        [HttpPut("{moduleType}")]
        public async Task<ActionResult> UpdateModuleTemplate(string moduleType, ModuleTemplate updatedModuleTemplate)
        {
            await _moduleTemplatesService.UpdateAsync(moduleType, updatedModuleTemplate);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveAllModuleTemplates()
        {
            await _moduleTemplatesService.RemoveAllAsync();
            return Ok();
        }

        [HttpDelete("{moduleType}")]
        public async Task<ActionResult> RemoveModuleTemplate(string moduleType)
        {
            await _moduleTemplatesService.RemoveByIdAsync(moduleType);
            return Ok();
        }
    }
}
