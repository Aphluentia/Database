using DatabaseApi.Models.Dtos.Entities;
using DatabaseApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DatabaseApi.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserServices _Service;

        public UserController(UserServices _service) =>
            _Service = _service;

        [HttpGet]
        public async Task<List<User>> Get() =>
            await _Service.GetUserAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> FindByEmail(string email)
        {
            var user = await _Service.GetUserAsync(email);

            if (user is null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User newUser)
        {
            newUser.WebPlatformId = new Guid();
            newUser.ActiveScenariosIds = new HashSet<string>();
            if (newUser.PermissionLevel == null)
            {
                newUser.PermissionLevel = Models.Dtos.Enums.PermissionLevel.Client;
            }
            await _Service.CreateUserAsync(newUser);

            return CreatedAtAction(nameof(AddUser), new { id = newUser.Email }, newUser);
        }

        [HttpPut("{email}")]
        public async Task<IActionResult> Update(string email, [FromBody] User updatedUser)
        {
            var user = await _Service.GetUserAsync(email);

            if (user is null)
            {
                return NotFound();
            }

            if (updatedUser.Name!=null) user.Name = updatedUser.Name;
            if (updatedUser.Password!=null) user.Password = updatedUser.Password;
            if (updatedUser.PermissionLevel!=null) user.PermissionLevel = updatedUser.PermissionLevel;

            await _Service.UpdateUserAsync(email, user);

            return NoContent();
        }
        [HttpPost("{email}/Scenario")]
        public async Task<IActionResult> AddScenario(string email, [FromBody] string ScenarioId)
        {
            var user = await _Service.GetUserAsync(email);

            if (user is null)
            {
                return NotFound();
            }

            user.ActiveScenariosIds.Add(ScenarioId);

            await _Service.UpdateUserAsync(email, user);

            return NoContent();
        }
        [HttpDelete("{email}/Scenario")]
        public async Task<IActionResult> DeleteScenario(string email, [FromBody] string ScenarioId)
        {
            var user = await _Service.GetUserAsync(email);

            if (user is null)
            {
                return NotFound();
            }

            user.ActiveScenariosIds.Remove(ScenarioId);

            await _Service.UpdateUserAsync(email, user);

            return NoContent();
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> Delete(string email)
        {
            var user = await _Service.GetUserAsync(email);

            if (user is null)
            {
                return NotFound();
            }

            await _Service.RemoveUserAsync(email);

            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> Purge()
        {
            
            await _Service.PurgeUsersAsync();

            return NoContent();
        }
    }

}


