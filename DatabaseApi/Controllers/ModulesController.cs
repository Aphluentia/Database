using DatabaseApi.Models.Dto;
using DatabaseApi.Models.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using System;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DatabaseApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly ModulesServices _Service;

        public ModulesController(ModulesServices _service) =>
            _Service = _service;
        // GET: api/<ModulesController>
        [HttpGet]
        public async Task<ICollection<ModulesOutputDto>> Get()
        {
            var modules = await _Service.GetModulesAsync();
            List<ModulesOutputDto> output = new List<ModulesOutputDto>();
            modules.ForEach(c =>
            {

                output.Add(new ModulesOutputDto
                {
                    Id = c.Id,
                    ModuleType = c.ModuleType,
                    Data = JsonDocument.Parse(c.Data)
                }); ;
            });
            return output;
        }
        
        [HttpGet("{id}")]
        public async Task<ModulesOutputDto?> Get(string id)
        {
            var module = await _Service.GetModulesAsync(id);
            return new ModulesOutputDto
            {
                Id = module.Id,
                ModuleType = module.ModuleType,
                Data = JsonDocument.Parse(module.Data)
            };
        }

        // POST api/<ModulesController>
        [HttpPost]
        public async Task<StatusCodeResult> Post([FromBody] Module value)
        {
            try
            {
                JsonDocument.Parse(value.Data.ToString());
                await _Service.CreateModulesAsync(value);
                return NoContent();
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
            await _Service.UpdateModulesAsync(id, value);
            return NoContent();
        }

        // DELETE api/<ModulesController>/5
        [HttpDelete("{id}")]
        public async Task<StatusCodeResult> Delete(string id)
        {
            await _Service.RemoveModulesAsync(id);
            return NoContent();
        }

        [HttpDelete]
        public async Task<StatusCodeResult> Purge()
        {
            await _Service.PurgeModulesAsync();
            return NoContent();
        }
    }
}
