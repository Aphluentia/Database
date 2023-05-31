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
            await _Service.FindAllAsync();

        [HttpGet("{email}")]
        public async Task<ActionResult<User>> FindByEmail(string email)
        {
            var user = await _Service.FindByIdAsync(email);

            if (user is null)
            {
                return NotFound();
            }

            return user;
        }


        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User newUser)
        {
            if (await _Service.Contains(newUser.Email))
                return BadRequest();
            await _Service.CreateAsync(newUser);

            return CreatedAtAction(nameof(AddUser), new { id = newUser.Email }, newUser);
        }

        [HttpPut("{email}")]
        public async Task<IActionResult> Update(string email, [FromBody] User updatedUser)
        {
            var user = await _Service.FindByIdAsync(email);

            if (user is null)
            {
                return NotFound();
            }

            if (!string.IsNullOrEmpty(updatedUser.Name)) user.Name = updatedUser.Name;
            if (!string.IsNullOrEmpty(updatedUser.Password)) user.Password = updatedUser.Password;
            if (updatedUser.PermissionLevel>user.PermissionLevel) user.PermissionLevel = updatedUser.PermissionLevel;

            await _Service.UpdateAsync(email, user);

            return NoContent();
        }
        [HttpPost("{email}/Scenario")]
        public async Task<IActionResult> AddScenario(string email, [FromBody] string ScenarioId)
        {
            var user = await _Service.FindByIdAsync(email);

            if (user is null)
            {
                return NotFound();
            }

            user.ActiveScenarios.Add(ScenarioId);
            
            await _Service.UpdateAsync(email, user);

            return NoContent();
        }
        [HttpDelete("{email}/Scenario")]
        public async Task<IActionResult> DeleteScenario(string email, [FromBody] string ScenarioId)
        {
            var user = await _Service.FindByIdAsync(email);

            if (user is null)
            {
                return NotFound();
            }

            user.ActiveScenarios.Remove(ScenarioId);

            await _Service.UpdateAsync(email, user);

            return NoContent();
        }
        [HttpPost("{email}/Connection")]
        public async Task<IActionResult> CreateModuleConnection(string _Email, [FromBody] string _ModuleId)
        {
            var user = await _Service.FindByIdAsync(_Email);

            if (user is null)
            {
                return NotFound();
            }
            var module = await _ModuleService.FindByIdAsync(_ModuleId);
            if (module is null)
            {
                return NotFound();
            }
           
            await _Service.AddModuleConnectionAsync(new ModuleConnection { ModuleId = _ModuleId, Email = _Email });

            return NoContent();
        }
  
        [HttpGet("{email}/Connection")]
        public async Task<ActionResult<ICollection<string>>> GetModuleConnections(string email)
        {
            var user = await _Service.FindByIdAsync(email);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user.Modules);
        }
        [HttpDelete("{email}/Connection")]
        public async Task<IActionResult> CloseModuleConnection(string _email, [FromQuery] string _moduleId)
        {
            var user = await _Service.FindByIdAsync(_email);
            if (user is null)
            {
                return NotFound();
            }
            var module = user.Modules.FirstOrDefault(c => c == _moduleId);
            if (module is null)
            {
                return NotFound();
            }
            await _Service.RemoveModuleConnectionAsync(new ModuleConnection { Email = _email, ModuleId = _moduleId });

            return NoContent();
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> Delete(string email)
        {
            var user = await _Service.FindByIdAsync(email);

            if (user is null)
            {
                return NotFound();
            }

            await _Service.RemoveByIdAsync(email);

            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> Purge()
        {
            
            await _Service.RemoveAllAsync();

            return NoContent();
        }
    }

}


