using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskAPI.Common.Exceptions;
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

        public TasksController(
            ITasksService tasksService,
            IMapper mapper,
            IRequestValidator requestValidator
        ) {
            _mapper = mapper;
            _requestValidator = requestValidator;
            _tasksService = tasksService;
        } 

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _tasksService.GetTasks();

            return Ok(_mapper.Map<GetTaskResponse>(tasks));
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

            return Ok(_mapper.Map<GetTaskResponse>(tasks));
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetTask(Guid taskId)
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

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTask(Guid taskId, [FromBody]UpdateTaskRequest request)
        {
            var result = _requestValidator.Validate(request);
            if (!result.IsValid)
            {
                throw new InvalidValueException(result.Errors.Select(e => e.ErrorMessage).Distinct().ToArray());
            }

            await _tasksService.UpdateTask(taskId, _mapper.Map<UpdateTask>(request));
            return Ok();
        }

        [HttpPut("{id}/assign-users")]
        [Authorize]
        public async Task<IActionResult> AssignUsers(Guid taskId, [FromBody]AssignTaskRequest request)
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

        [HttpPut("{id}/complete")]
        [Authorize]
        public async Task<IActionResult> CompleteTask(Guid taskId)
        {

            await _tasksService.CompleteTask(taskId);
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteTask(Guid taskId)
        {
            await _tasksService.DeleteTask(taskId);

            return Ok();
        }
    }
}
