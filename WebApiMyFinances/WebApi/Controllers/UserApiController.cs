﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiMyFinances.Core.Services;
using WebApiMyFinances.WebApi.DTO;

namespace WebApiMyFinances.WebApi.Controllers
{
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly UserApiService _userApiService;
        public UserApiController(UserApiService userApiService)
        {
            _userApiService = userApiService;
        }

        [HttpPost]
        [Route("/Login")]
        public async Task<ActionResult<string>> Login([FromBody] DTOUserApiLogin user)
        {
            var cts = new CancellationTokenSource();
            var jwt = await _userApiService.Login(user, cts.Token);
            Response.Cookies.Append("auth-token", jwt);
            return Ok();
        }
        [HttpPost]
        [Route("/Register")]
        public async Task<ActionResult> Register([FromBody] DTOUserAPIRegister user)
        {
            var cts = new CancellationTokenSource();
            await _userApiService.Register(user, cts.Token);
            return Ok();
        }
    }
}
