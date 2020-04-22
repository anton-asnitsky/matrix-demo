using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskAPI.Common.Exceptions;
using TaskAPI.Common.Identity;
using TaskAPI.Data.Models;
using TaskAPI.Services.Interfaces;
using TaskAPI.Services.Models.Inbound;
using TaskAPI.Services.Models.Outbound;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    public class TasksController : Controller
    {
        private readonly ITasksService _tasksService;
        private readonly IMapper _mapper;
        private readonly IRequestValidator _requestValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TasksController(
            ITasksService tasksService,
            IMapper mapper,
            IRequestValidator requestValidator,
            IHttpContextAccessor httpContextAccessor
        ) {
            _mapper = mapper;
            _requestValidator = requestValidator;
            _tasksService = tasksService;
            _httpContextAccessor = httpContextAccessor;
        } 

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _tasksService.GetTasks();

            return Ok(_mapper.Map<List<GetTaskResponse>>(tasks));
        }

        /**
         * Since input data for this request strunctired 
         * and need to be validated
         * I decided to use POST method
         * despite usual CRUD usage
         **/
        [HttpPost("user-tasks")]
        [Authorize]
        public async Task<IActionResult> GetUserTasks([FromBody] GetUserTasksRequest request)
        {
            var result = _requestValidator.Validate(request);
            if (!result.IsValid)
            {
                throw new InvalidValueException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToArray());
            }

            var tasks = await _tasksService.GetTasksByEmail(request.Email);

            return Ok(_mapper.Map<List<GetTaskResponse>>(tasks));
        }

        [HttpGet("send-me-my-tasks")]
        [Authorize]
        public async Task<IActionResult> SendTasksAssignedToUser()
        {
            var userId = _httpContextAccessor.GetUserId();
            await _tasksService.GetTasksByUserId(userId);

            return Ok();
        }

        [HttpGet("{taskId}")]
        [Authorize]
        public async Task<IActionResult> GetTask([FromRoute]Guid taskId)
        {
            var task = await _tasksService.GetTask(taskId);

            return Ok(_mapper.Map<GetTaskResponse>(task));
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddTask([FromBody]CreateTaskRequest request)
        {
            var result = _requestValidator.Validate(request);
            if (!result.IsValid)
            {
                throw new InvalidValueException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToArray());
            }

            var task = _mapper.Map<UserTask>(request);
            var (Assigned, NotAssigned) = await _tasksService.CreateTask(task, request.AssignTo);

            return Ok(new CreateTaskResponse() { 
                TaskId = task.TaskId,
                UsersAssigned = Assigned,
                UsersNotAssigned = NotAssigned
            });
        }

        [HttpPut("{taskId}")]
        [Authorize]
        public async Task<IActionResult> UpdateTask([FromRoute]Guid taskId, [FromBody]UpdateTaskRequest request)
        {
            var result = _requestValidator.Validate(request);
            if (!result.IsValid)
            {
                throw new InvalidValueException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToArray());
            }

            await _tasksService.UpdateTask(taskId, _mapper.Map<UpdateTask>(request));
            return Ok();
        }

        [HttpPut("{taskId}/assign-users")]
        [Authorize]
        public async Task<IActionResult> AssignUsers([FromRoute]Guid taskId, [FromBody]AssignTaskRequest request)
        {
            var result = _requestValidator.Validate(request);
            if (!result.IsValid)
            {
                throw new InvalidValueException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToArray());
            }

            var (Assigned, NotAssigned) = await _tasksService.AssignUsers(taskId, request.UsersToAssign, request.UsersToUnssign);
            return Ok(new AssignTaskResponse()
            {
                UsersAssigned = Assigned,
                UsersNotAssigned = NotAssigned
            });
        }

        [HttpPut("{taskId}/complete")]
        [Authorize]
        public async Task<IActionResult> CompleteTask([FromRoute]Guid taskId)
        {

            await _tasksService.CompleteTask(taskId);
            return Ok();
        }

        [HttpDelete("{taskId}")]
        [Authorize]
        public async Task<IActionResult> DeleteTask([FromRoute]Guid taskId)
        {
            await _tasksService.DeleteTask(taskId);

            return Ok();
        }
    }
}
