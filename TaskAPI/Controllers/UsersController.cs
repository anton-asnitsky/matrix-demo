using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskAPI.Common.Exceptions;
using TaskAPI.Data.DataContexts;
using TaskAPI.Data.Models;
using TaskAPI.Services.Interfaces;
using TaskAPI.Services.Models.Inbound;
using TaskAPI.Services.Models.Outbound;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;
        private readonly IMapper _mapper;
        private readonly IRequestValidator _requestValidator;

        public UsersController(
            IUsersService usersService,
            IMapper mapper,
            IRequestValidator requestValidator
        ) {
            _usersService = usersService;
            _mapper = mapper;
            _requestValidator = requestValidator;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _usersService.GetUsers();

            return Ok(_mapper.Map<List<GetUserResponse>>(users));
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUser([FromRoute]Guid userId)
        {
            var user = await _usersService.GetUser(userId);

            return Ok(_mapper.Map<GetUserResponse>(user));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser([FromBody]CreateUserRequest request)
        {
            var result = _requestValidator.Validate(request);
            if (!result.IsValid) {
                {
                    throw new InvalidValueException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToArray());
                }
            }

            var user = _mapper.Map<User>(request);
            await _usersService.AddUser(user);

            return Ok(new CreateUserResponse() {
                UserId = user.UserId
            });
        }

        [HttpPut("{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromRoute]Guid userId, [FromBody]UpdateUserRequest request)
        {
            var result = _requestValidator.Validate(request);
            if (!result.IsValid)
            {
                {
                    throw new InvalidValueException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToArray());
                }
            }

            await _usersService.UpdateUser(userId, _mapper.Map<UpdateUser>(request));

            return Ok();
        }

        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser([FromRoute]Guid userId)
        {
            await _usersService.DeleteUser(userId);

            return Ok();
        }
    }
}
