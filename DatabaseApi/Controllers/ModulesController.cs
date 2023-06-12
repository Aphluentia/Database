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
        private readonly IModuleRegistryService _MTService;
        public ModulesController(IModuleService _service, IModuleRegistryService _mtservice)
        {
            _Service = _service;
            _MTService = _mtservice;
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
        public async Task<StatusCodeResult> CreateModule([FromBody] Module value)
        {
            try
            {
                JsonDocument.Parse(value.Data.ToString());
            }
            catch (JsonException ex)
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(value.ModuleTemplate.ModuleName) || string.IsNullOrEmpty(value.ModuleTemplate.VersionId)) return BadRequest();

            var moduleTemplate = await _MTService.FindByIdAsync(value.ModuleTemplate.ModuleName);
            if (moduleTemplate == null) return NotFound();

            var moduleVersion = moduleTemplate.Versions.Where(c=>c.VersionId == value.ModuleTemplate.VersionId).FirstOrDefault();
            if (moduleVersion == null) return NotFound();

            value.ModuleTemplate = CustomModuleTemplate.FromModuleTemplate(moduleTemplate, moduleVersion);

            var success = await _Service.CreateAsync(value);
            if (success)
                return Ok();
            return BadRequest();

        }

        [HttpPut("{id}")] //public Task<bool> UpdateAsync(string ModuleId, Module updatedObject);
        public async Task<StatusCodeResult> Put(string id, [FromBody] Module updatedModule)
        {
            try
            {
                JsonDocument.Parse(updatedModule.Data.ToString());
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
            var existingModule = await _Service.FindByIdAsync(id);
            if (existingModule == null) return NotFound();

            if (!string.IsNullOrEmpty(updatedModule.Data)) existingModule.Data = $"@{updatedModule.Data}";

            existingModule.Timestamp = updatedModule.Timestamp;
            existingModule.Checksum = updatedModule.Checksum;

            if (updatedModule.ModuleTemplate.VersionId != existingModule.ModuleTemplate.VersionId)
            {

                var moduleTemplate = await _MTService.FindByIdAsync(existingModule.ModuleTemplate.ModuleName);
                if (moduleTemplate == null) return NotFound();

                var moduleVersion = moduleTemplate.Versions.Where(c => c.VersionId == updatedModule.ModuleTemplate.VersionId).FirstOrDefault();
                if (moduleVersion == null) return NotFound();

                existingModule.ModuleTemplate = CustomModuleTemplate.FromModuleTemplate(moduleTemplate, moduleVersion);
            }
            else
            {
                if (!string.IsNullOrEmpty(updatedModule.ModuleTemplate.HtmlCard)) existingModule.ModuleTemplate.HtmlCard = updatedModule.ModuleTemplate.HtmlCard;
                if (!string.IsNullOrEmpty(updatedModule.ModuleTemplate.HtmlDashboard)) existingModule.ModuleTemplate.HtmlDashboard = updatedModule.ModuleTemplate.HtmlDashboard;
            }
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
