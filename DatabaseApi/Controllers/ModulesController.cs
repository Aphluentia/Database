using DatabaseApi.Models.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly TherapistServices _Service;

        public ModulesController(TherapistServices _service) =>
            _Service = _service;

        [HttpGet("{WebPlatformId}")]
        public async Task<ICollection<Module>> Get(string WebPlatformId)
        {
            var modules = await _Service.GetConnectedModulesAsync(WebPlatformId);
            List<Module> output = new List<Module>();
            modules.ForEach(c =>
            {

                output.Add(new Module
                {
                    Id = c.Id,
                    ModuleType = c.ModuleType,
                    Data = $"@{c.Data}",
                    Timestamp = c.Timestamp,
                    Checksum = c.Checksum
                }); ;
            });
            return output;
        }

        [HttpGet("{WebPlatformId}/{ModuleId}")]
        public async Task<Module?> Get(string WebPlatformId, string ModuleId)
        {
            var module = await _Service.GetConnectedModulesByIdAsync(WebPlatformId, ModuleId);
            if (module == null)
                return null;
            return new Module
            {
                Id = module.Id,
                ModuleType = module.ModuleType,
                Data = $"@{module.Data}",
                Timestamp = module.Timestamp,
                Checksum = module.Checksum
            };
        }

        // POST api/<ModulesController>
        [HttpPost]
        public async Task<StatusCodeResult> Post([FromBody] Module value)
        {
            try
            {
                JsonDocument.Parse(value.Data.ToString());
                await _Service.CreateAsync(value);
                return Ok();
            }
            catch (JsonException ex)
            {
                return BadRequest();
            }
            
        }

        // PUT api/<ModulesController>/5
        [HttpPut("{id}")]
        public async Task<StatusCodeResult> Put(string id, [FromBody] Module value)
        {
            try
            {
                JsonDocument.Parse(value.Data.ToString());
            }
            catch (JsonException ex)
            {
                return BadRequest();
            }
            await _Service.UpdateAsync(id, value);
            return NoContent();
        }

        // DELETE api/<ModulesController>/5
        [HttpDelete("{id}")]
        public async Task<StatusCodeResult> Delete(string id)
        {
            await _Service.RemoveByIdAsync(id);
            return NoContent();
        }

        [HttpDelete]
        public async Task<StatusCodeResult> Purge()
        {
            await _Service.RemoveAllAsync();
            return NoContent();
        }
    }
}
