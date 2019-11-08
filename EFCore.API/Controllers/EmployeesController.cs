using AutoMapper;
using EFCore.API.Models;
using EFCore.Common;
using EFCore.DTO;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _service;
        private readonly IMapper _mapper;

        public EmployeesController(IEmployeeService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<List<EmployeeResponse>>> GetEmployee()
        {
            var result = await _service.GetListAsync();
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(_mapper.Map<List<EmployeeResponse>>(result.Value));
        }

        // GET: api/Employees/Search
        [HttpGet("Search")]
        public async Task<ActionResult> Search(EmployeeSearchRequest request)
        {
            var result = await _service.CreatePageListAsync(_mapper.Map<EmployeePageFilterDTO>(request));
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(new
            {
                result.Value.MatchCount,
                PageResult = _mapper.Map<List<EmployeeResponse>>(result.Value)
            });
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeResponse>> GetEmployee(long id)
        {
            //var result = await _service.FindByIdAsync(id);
            var result = await _service.FirstOrDefaultIncludeDepartmentAsync(e => e.Id == id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var response = _mapper.Map<EmployeeResponse>(result.Value);
            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        // PUT: api/Employees/5
        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeResponse>> PutEmployee(long id, EmployeeRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            DateTime parsedDateOfBirth;
            if (DateTime.TryParse(request.DateOfBirth, out parsedDateOfBirth) == false)
            {
                return BadRequest();
            }

            var result = await _service.UpdateAsync(_mapper.Map<EmployeeDTO>(request));
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
                //else if (result.ErrorType == ErrorType.DBUpdateConcurrency && !await EmployeeExists(id))
                //{
                //    return NotFound();
                //}
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            return Ok(_mapper.Map<EmployeeResponse>(result.Value));
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<ActionResult<EmployeeResponse>> PostEmployee(NewEmployeeRequest request)
        {
            DateTime parsedDateOfBirth;
            if (DateTime.TryParse(request.DateOfBirth, out parsedDateOfBirth) == false)
            {
                return BadRequest();
            }

            var result = await _service.AddAsync(_mapper.Map<EmployeeDTO>(request));
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

            var response = _mapper.Map<EmployeeResponse>(result.Value);
            return CreatedAtAction(nameof(GetEmployee), new { id = response.Id }, response);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<EmployeeResponse>> DeleteEmployee(long id)
        {
            var result = await _service.DeleteByIdAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var response = _mapper.Map<EmployeeResponse>(result.Value);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        private async Task<bool> EmployeeExists(long id)
        {
            var result = await _service.AnyAsync(e => e.Id == id);
            if (!result.IsSuccess)
            {
                return false;
            }

            return result.Value;
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> PatchEmployee(long id, JsonPatchDocument<EmployeeDTO> patchDoc)
        {
            if (patchDoc == null || patchDoc.Operations.Any(o => o.OperationType == OperationType.Invalid))
            {
                return BadRequest();
            }

            var result = await _service.PatchEmployeeAsync(id, patchDoc);
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

            return Ok(result.Value);
        }
    }
}
