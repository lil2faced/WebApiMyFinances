using Microsoft.AspNetCore.Mvc;
using WebApiMyFinances.Core.Interfaces;
using WebApiMyFinances.WebApi.DTO;

namespace WebApiMyFinances.WebApi.Controllers
{
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserApiService _userApiService;
        public UserApiController(IUserApiService userApiService)
        {
            _userApiService = userApiService;
        }

        [HttpPost]
        [Route("/Login")]
        public async Task<ActionResult<string>> Login([FromBody] DTOUserApiLogin user)
        {
            var cts = new CancellationTokenSource();
            var jwt = await _userApiService.Login(user, cts.Token);
            Response.Cookies.Append("auth-token", jwt, new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.None, 
                Expires = DateTime.UtcNow.AddHours(12)
            });
            return Ok(jwt);
        }
        [HttpPost]
        [Route("/Register")]
        public async Task<ActionResult> Register([FromBody] DTOUserAPIRegister user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var cts = new CancellationTokenSource();
            await _userApiService.Register(user, cts.Token);
            return Ok();
        }
    }
}
