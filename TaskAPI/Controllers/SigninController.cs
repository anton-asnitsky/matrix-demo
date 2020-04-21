using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskAPI.Services.Interfaces;
using TaskAPI.Services.Models.Inbound;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SigninController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public SigninController(ILoginService loginService) {
            _loginService = loginService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            return Ok(await _loginService.Login(request));
        }

        [AllowAnonymous]
        [HttpPost("recover-password")]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoverPasswordRequest request)
        {
            await _loginService.RecoverPassword(request);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> PasswordChange([FromBody] PasswordChangeRequest request)
        {
            await _loginService.PasswordChange(request);
            return Ok();
        }
    }
}