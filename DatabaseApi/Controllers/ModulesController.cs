using DatabaseApi.Models.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleService _Service;
        private readonly IModuleRegistryService _ApplicationService;
        public ModulesController(IModuleService _service, IModuleRegistryService _mtservice)
        {
            _Service = _service;
            _ApplicationService = _mtservice;
        }

        [HttpGet] //public Task<List<Module>> FindAllAsync();
        public async Task<ActionResult<ICollection<Module>>> Get()
        {
            return Ok(await _Service.FindAllAsync());
        }

        [HttpGet("{id}")] //public Task<Module?> FindByIdAsync(string ModuleId);
        public async Task<ActionResult<Module?>> Get(string id)
        {
            var module = await _Service.FindByIdAsync(id);
            if (module == null) return NotFound();
            return Ok(module);
        }

        [HttpPost]                       //public Task<bool> CreateAsync(Module newObject);
        public async Task<IActionResult> CreateModule([FromBody] Module value)
        {
            try
            {
                JsonDocument.Parse(value.Data.ToString());
            }
            catch (JsonException ex)
            {
                return BadRequest(ex);
            }
            
            var moduleTemplate = await _ApplicationService.FindByIdAsync(value.ModuleTemplate.ModuleName);
            if (moduleTemplate == null) return NotFound();

            var moduleVersion = moduleTemplate.Versions.Where(c => c.VersionId == value.ModuleTemplate.VersionId).FirstOrDefault();
            if (moduleVersion == null) return NotFound();

            value.ModuleTemplate = CustomModuleTemplate.FromModuleTemplate(moduleTemplate, moduleVersion);

            var success = await _Service.CreateAsync(value);
            if (success)
                return Ok();
            return BadRequest();

        }

        [HttpPut("{id}")] //public Task<bool> UpdateAsync(string ModuleId, Module updatedObject);
        public async Task<IActionResult> Put(string id, [FromBody] Module updatedModule)
        {
            try
            {
                JsonDocument.Parse(updatedModule.Data.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            var existingModule = await _Service.FindByIdAsync(id);
            if (existingModule == null) return NotFound();

            if (updatedModule.Data != null)
            {
                existingModule.Data = updatedModule.Data;
                existingModule.Timestamp = updatedModule.Timestamp;
                existingModule.Checksum = updatedModule.Checksum;
            }
            
            var success = await _Service.UpdateAsync(id, existingModule);
            if (success)
                return Ok();
            return BadRequest();
        }
        [HttpPut("{id}/Version/{AppVersion}")] //public Task<bool> UpdateAsync(string ModuleId, Module updatedObject);
        public async Task<StatusCodeResult> SetModuleVersion(string id, string AppVersion)
        {
           
            var existingModule = await _Service.FindByIdAsync(id);
            if (existingModule == null) return NotFound();

            var existingModuleTemplate = await _ApplicationService.FindByIdAsync(existingModule.ModuleTemplate.ModuleName);
            if (existingModuleTemplate == null) return NotFound();

            var mNewVersion = existingModuleTemplate.Versions.Where(c=>c.VersionId == AppVersion).FirstOrDefault();
            if (mNewVersion == null) return NotFound();
            existingModule.ModuleTemplate = CustomModuleTemplate.FromModuleTemplate(existingModuleTemplate, mNewVersion);
            
            var success = await _Service.UpdateAsync(id, existingModule);
            if (success)
                return Ok();
            return BadRequest();
        }
        [HttpPut("{id}/Version")] //public Task<bool> UpdateAsync(string ModuleId, Module updatedObject);
        public async Task<StatusCodeResult> UpdateModuleTemplate(string id, [FromBody] ModuleVersion updatedVersion)
        {

            var existingModule = await _Service.FindByIdAsync(id);
            if (existingModule == null) return NotFound();

            if (!string.IsNullOrEmpty(updatedVersion.HtmlCard))  existingModule.ModuleTemplate.HtmlCard = updatedVersion.HtmlCard;
            if (!string.IsNullOrEmpty(updatedVersion.HtmlDashboard))  existingModule.ModuleTemplate.HtmlDashboard = updatedVersion.HtmlDashboard;

            var success = await _Service.UpdateAsync(id, existingModule);
            if (success)
                return Ok();
            return BadRequest();
        }


        [HttpDelete("{id}")] //public Task<bool> RemoveByIdAsync(string ModuleId);
        public async Task<StatusCodeResult> Delete(string id)
        {
            var success = await _Service.RemoveByIdAsync(id);
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpDelete] //public Task RemoveAllAsync();
        public async Task<StatusCodeResult> Purge()
        {
            await _Service.RemoveAllAsync();
            return NoContent();
        }
    }
}
