using DatabaseApi.Models.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly IModuleService _Service;
        private readonly IApplicationService _ApplicationService;
        public ModulesController(IModuleService _service, IApplicationService _mtservice)
        {
            _Service = _service;
            _ApplicationService = _mtservice;
        }

        [HttpGet]
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
       
            var ModuleData = await _ApplicationService.FindByIdAsync(value.ModuleData.ApplicationName);
            if (ModuleData == null) return NotFound();
            var success = await _Service.CreateAsync(value);
            if (success)
                return Ok();
            return BadRequest();

        }
        // Update Module
        [HttpPut("{id}")] //public Task<bool> UpdateAsync(string ModuleId, Module updatedObject);
        public async Task<IActionResult> UpdateModule(string id, [FromBody] Module updatedModule)
        {
         
            var existingModule = await _Service.FindByIdAsync(id);
            if (existingModule == null) return NotFound();
          
            var success = await _Service.UpdateAsync(id, updatedModule);
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
