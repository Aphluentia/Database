using DatabaseApi.Models.Dtos.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class ScenariosController : ControllerBase
    {

        private readonly DatabaseServices _Service;

        public ScenariosController(DatabaseServices _service) =>
            _Service = _service;

        [HttpGet]
        public async Task<List<Scenario>> Get() =>
            await _Service.GetScenarioAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Scenario>> Get(string id)
        {
            var book = await _Service.GetScenarioAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Scenario newBook)
        {
            await _Service.CreateScenarioAsync(newBook);

            return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Scenario updatedBook)
        {
            var book = await _Service.GetScenarioAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            updatedBook.Id = book.Id;

            await _Service.UpdateScenarioAsync(id, updatedBook);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _Service.GetScenarioAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            await _Service.RemoveScenarioAsync(id);

            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> Purge()
        {

            await _Service.PurgeScenarioAsync();

            return NoContent();
        }
    }

}


