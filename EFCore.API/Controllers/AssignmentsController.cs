using AutoMapper;
using EFCore.API.Models;
using EFCore.Common;
using EFCore.DTO;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFCore.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly IAssignmentService _service;
        private readonly IMapper _mapper;

        public AssignmentsController(IAssignmentService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/Assignments
        [HttpGet]
        public async Task<ActionResult<List<AssignmentResponse>>> GetAssignment()
        {
            var result = await _service.GetListAsync();
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(_mapper.Map<List<AssignmentResponse>>(result.Value));
        }

        // GET: api/Assignments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AssignmentResponse>> GetAssignment(long id)
        {
            var result = await _service.FindByIdAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var response = _mapper.Map<AssignmentResponse>(result.Value);
            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        // POST: api/Assignments
        [HttpPost]
        public async Task<ActionResult<AssignmentResponse>> PostAssignment(NewAssignmentRequest request)
        {
            var result = await _service.AddAsync(_mapper.Map<AssignmentDTO>(request));
            if (!result.IsSuccess)
            {
                if (result.ErrorType == ErrorType.Defined)
                {
                    if (result.ErrorMessage.Contains(ErrorMessages.RecordAlreadyExists))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            var response = _mapper.Map<AssignmentResponse>(result.Value);
            return CreatedAtAction(nameof(GetAssignment), new { id = response.Id }, response);
        }
        
        // PUT: api/Assignments/5
        [HttpPut("{id}")]
        public async Task<ActionResult<AssignmentResponse>> PutAssignment(long id, AssignmentRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            var result = await _service.UpdateAsync(_mapper.Map<AssignmentDTO>(request));
            if (!result.IsSuccess)
            {
                if (result.ErrorType == ErrorType.Defined)
                {
                    if (result.ErrorMessage.Contains(ErrorMessages.RecordAlreadyExists))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                //else if (result.ErrorType == ErrorType.DBUpdateConcurrency && !await DepartmentExists(id))
                //{
                //    return NotFound();
                //}
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            return Ok(_mapper.Map<AssignmentResponse>(result.Value));
        }

        // DELETE: api/Assignments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<AssignmentResponse>> DeleteAssignment(long id)
        {
            var result = await _service.DeleteByIdAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var response = _mapper.Map<AssignmentResponse>(result.Value);
            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        // POST: api/Assignments/Range
        [HttpPost("Range")]
        public async Task<ActionResult<List<AssignmentResponse>>> PostAssignment(List<NewAssignmentRequest> request)
        {
            var result = await _service.AddRangeAsync(_mapper.Map<List<AssignmentDTO>>(request));
            if (!result.IsSuccess)
            {
                if (result.ErrorType == ErrorType.Defined)
                {
                    return BadRequest();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            var response = _mapper.Map<List<AssignmentResponse>>(result.Value);
            return CreatedAtAction(nameof(GetAssignment), response);
        }

        [HttpPut("Range")]
        public async Task<ActionResult<List<AssignmentResponse>>> PutAssignment(List<AssignmentRequest> request)
        {
            var result = await _service.UpdateRangeAsync(_mapper.Map<List<AssignmentDTO>>(request));
            if (!result.IsSuccess)
            {
                if (result.ErrorType == ErrorType.Defined)
                {
                    if (result.ErrorMessage.Contains(ErrorMessages.RecordAlreadyExists))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            return Ok(_mapper.Map<List<AssignmentResponse>>(result.Value));
        }

        [HttpDelete("Range")]
        public async Task<ActionResult<List<AssignmentResponse>>> DeleteAssignment(List<AssignmentRequest> request)
        {
            var result = await _service.DeleteRangeAsync(_mapper.Map<List<AssignmentDTO>>(request));
            if (!result.IsSuccess)
            {
                if (result.ErrorType == ErrorType.Defined)
                {
                    if (result.ErrorMessage.Contains(ErrorMessages.RecordAlreadyExists))
                    {
                        return StatusCode(StatusCodes.Status403Forbidden);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            return Ok(_mapper.Map<List<AssignmentResponse>>(result.Value));
        }

        private async Task<bool> AssignmentExists(long id)
        {
            var result =  await _service.AnyAsync(e => e.Id == id);
            if (!result.IsSuccess)
            {
                return false;
            }

            return result.Value;
        }
    }
}
