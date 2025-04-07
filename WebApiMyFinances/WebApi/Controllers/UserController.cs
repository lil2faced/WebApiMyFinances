using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiMyFinances.Core.Interfaces;
using WebApiMyFinances.WebApi.DTO;

namespace WebApiMyFinances.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<DTOUserGet>> GetUserById(string email)
        {
            var cts = new CancellationTokenSource();
            DTOUserGet user = await _userService.GetUserByEmail(email, cts.Token);
            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTOUserGet>>> GetAllUsers()
        {
            var cts = new CancellationTokenSource();
            var users = await _userService.GetAllUsers(cts.Token);
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] DTOUserSet user)
        {
            var cts = new CancellationTokenSource();
            await _userService.SetUser(user, cts.Token);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> EditUser([FromBody] DTOUserEdit user, string email)
        {
            var cts = new CancellationTokenSource();
            await _userService.EditUser(email, user, cts.Token);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser([FromBody] string email)
        {
            var cts = new CancellationTokenSource();
            await _userService.DeleteUser(email, cts.Token);
            return Ok();
        }
    }
}
