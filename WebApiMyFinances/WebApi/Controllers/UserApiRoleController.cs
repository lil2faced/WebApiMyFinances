using Microsoft.AspNetCore.Mvc;
using WebApiMyFinances.Core.Interfaces;
using WebApiMyFinances.WebApi.DTO;

namespace WebApiMyFinances.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserApiRoleController : ControllerBase
    {
        private readonly IUserApiRoleService _userApiRoleService;
        public UserApiRoleController(IUserApiRoleService apiRoleService)
        {
            _userApiRoleService = apiRoleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DTOUserApiRole>>> GetAllRoles()
        {
            var cts = new CancellationTokenSource();
            var res = await _userApiRoleService.GetAllRoles(cts.Token);

            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(DTOUserApiRole role)
        {
            var cts = new CancellationTokenSource();
            await _userApiRoleService.SetRole(role, cts.Token);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> EditRole(string roleToEdit, DTOUserApiRole role)
        {
            var cts = new CancellationTokenSource();
            await _userApiRoleService.EditRole(roleToEdit, role, cts.Token);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRole(string role)
        {
            var cts = new CancellationTokenSource();
            await _userApiRoleService.DeleteRole(role, cts.Token);

            return Ok();
        }
    }
}
