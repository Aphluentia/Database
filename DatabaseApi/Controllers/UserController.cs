using DatabaseApi.Models.Dtos.Entities;
using DatabaseApi.Models.Dtos.Enums;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly DatabaseServices _Service;

        public UserController(DatabaseServices _service) =>
            _Service = _service;

        [HttpGet]
        public async Task<List<User>> Get() =>
            await _Service.GetUserAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var book = await _Service.GetUserAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            return book;
        }

        [HttpPost]
        public async Task<IActionResult> Post(User newBook)
        {
            await _Service.CreateUserAsync(newBook);

            return CreatedAtAction(nameof(Get), new { id = newBook.Email }, newBook);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, User updatedUser)
        {
            var book = await _Service.GetUserAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            updatedUser.Email = book.Email;

            await _Service.UpdateUserAsync(id, updatedUser);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _Service.GetUserAsync(id);

            if (book is null)
            {
                return NotFound();
            }

            await _Service.RemoveUserAsync(id);

            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> Purge()
        {
            
            await _Service.PurgeUsersAsync();

            return NoContent();
        }

        [HttpPut("{userId}/Scenarios/{ScenarioId}")]
        public async Task<IActionResult> AddScenarioToUser(string userId, string ScenarioId)
        {
            var User = await _Service.GetUserAsync(userId);

            if (User is null)
            {
                return NotFound();
            }

            User.ActiveScenariosIds.Add(ScenarioId);

            await _Service.UpdateUserAsync(userId, User);

            return NoContent();
        }

        [HttpDelete("{userId}/Scenarios/{ScenarioId}")]
        public async Task<IActionResult> RemoveScenarioFromUser(string userId, string ScenarioId)
        {
            var User = await _Service.GetUserAsync(userId);

            if (User is null)
            {
                return NotFound();
            }

            User.ActiveScenariosIds.Remove(ScenarioId);

            await _Service.UpdateUserAsync(userId, User);

            return NoContent();
        }

        [HttpPut("{userId}/Connection")]
        public async Task<IActionResult> AddConnection(string userId, Connection newConnection)
        {
            var User = await _Service.GetUserAsync(userId);

            if (User is null)
            {
                return NotFound();
            }
            // If trying to add a different module when a connection of AppType already exists
            if (User.Connections.Any(x=> x.ApplicationType == newConnection.ApplicationType))
            {
                return BadRequest("Connection Already Exists");
            }
            User.Connections.Add(newConnection);

            await _Service.UpdateUserAsync(userId, User);

            return NoContent();
        }

        [HttpDelete("{userId}/Connection/{ConnectionId}")]
        public async Task<IActionResult> RemoveConnection(string userId, ApplicationType appType)
        {
            var User = await _Service.GetUserAsync(userId);

            if (User is null)
            {
                return NotFound();
            }
            if (User.Connections.Any(x=> x.ApplicationType == appType))
                User.Connections.Remove(User.Connections.First(x => x.ApplicationType == appType));

            await _Service.UpdateUserAsync(userId, User);

            return NoContent();
        }
    }

}


