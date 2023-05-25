using DatabaseApi.Models.Dtos.Entities;
using DatabaseApi.Models.Entities;
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
        private readonly ModulesServices _ModuleService;

        public UserController(UserServices _service, ModulesServices _moduleServices)
        {
            _Service = _service;
            _ModuleService = _moduleServices;
        }

        [HttpGet]
        public async Task<List<User>> Get() =>
            await _Service.GetUserAsync();

        [HttpGet("{email}")]
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
            
            newUser.WebPlatformId = Guid.NewGuid();
            if (newUser.ActiveScenariosIds == null)
                newUser.ActiveScenariosIds = new HashSet<string>();
            if (newUser.Modules == null)
                newUser.Modules = new HashSet<string>();
            if (await _Service.Contains(newUser.Email))
                return BadRequest();
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
        [HttpPost("{email}/Connection")]
        public async Task<IActionResult> CreateModuleConnection(string email, [FromBody] string ModuleId)
        {
            var user = await _Service.GetUserAsync(email);

            if (user is null)
            {
                return NotFound();
            }
            var module = await _ModuleService.GetModulesAsync(ModuleId);
            if (module is null)
            {
                return NotFound();
            }
            if (user.Modules.Any(c => c == ModuleId)) user.Modules.Remove(ModuleId);

            user.Modules.Add(ModuleId);
            await _Service.UpdateUserAsync(email, user);

            return NoContent();
        }
  
        [HttpGet("{email}/Connection")]
        public async Task<ActionResult<ICollection<string>>> GetModuleConnections(string email)
        {
            var user = await _Service.GetUserModulesAsync(email);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        [HttpDelete("{email}/Connection")]
        public async Task<IActionResult> CloseModuleConnection(string email, [FromQuery] string ModuleId)
        {
            var user = await _Service.GetUserAsync(email);
            if (user is null)
            {
                return NotFound();
            }
            var module = user.Modules.FirstOrDefault(c => c == ModuleId);
            if (module is null)
            {
                return NotFound();
            }
            user.Modules.Remove(ModuleId);

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


