using DatabaseApi.Models.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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
        public async Task<ICollection<Module>> Get()
        {
            return await _Service.GetModulesAsync();
        }
        
        [HttpGet("{id}")]
        public async Task<Module?> Get(string id)
        {
            return await _Service.GetModulesAsync(id);
        }

        // POST api/<ModulesController>
        [HttpPost]
        public async Task<StatusCodeResult> Post([FromBody] Module value)
        {
            try
            {
                JsonDocument.Parse(value.Data);
            }
            catch (JsonException ex)
            {
                return BadRequest();
            }
            await _Service.CreateModulesAsync(value);
            return NoContent();
        }

        // PUT api/<ModulesController>/5
        [HttpPut("{id}")]
        public async Task<StatusCodeResult> Put(string id, [FromBody] Module value)
        {
            try
            {
                JsonDocument.Parse(value.Data);
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
    }
}
