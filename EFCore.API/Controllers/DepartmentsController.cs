using AutoMapper;
using EFCore.API.Models;
using EFCore.Common;
using EFCore.DTO;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EFCore.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentService _service;
        private readonly IMapper _mapper;

        public DepartmentsController(IDepartmentService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        //// GET: api/Departments
        [HttpGet]
        public async Task<ActionResult<List<DepartmentResponse>>> GetDepartment()
        {
            var result = await _service.GetListAsync();
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(_mapper.Map<List<DepartmentResponse>>(result.Value));
        }

        // GET: api/Departments/Search
        [HttpGet("Search")]
        public async Task<ActionResult> Search(SearchRequest request)
        {
            var result = await _service.CreatePageListAsync(_mapper.Map<PageFilterDTO>(request));
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(new
            {
                result.Value.MatchCount,
                PageResult = _mapper.Map<List<DepartmentResponse>>(result.Value)
            });
        }

        // GET: api/Departments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentResponse>> GetDepartment(long id)
        {
            var result = await _service.FindByIdAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var response = _mapper.Map<DepartmentResponse>(result.Value);
            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        // PUT: api/Departments/5
        [HttpPut("{id}")]
        public async Task<ActionResult<DepartmentResponse>> PutDepartment(long id, DepartmentRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            var result = await _service.UpdateAsync(_mapper.Map<DepartmentDTO>(request));
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

            return Ok(_mapper.Map<DepartmentResponse>(result.Value));
        }

        // POST: api/Departments
        [HttpPost]
        public async Task<ActionResult<DepartmentResponse>> PostDepartment(NewDepartmentRequest request)
        {
            var result = await _service.AddAsync(_mapper.Map<DepartmentDTO>(request));
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

            var response = _mapper.Map<DepartmentResponse>(result.Value);
            return CreatedAtAction(nameof(GetDepartment), new { id = response.Id }, response);
        }

        // DELETE: api/Departments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DepartmentResponse>> DeleteDepartment(long id)
        {
            var result = await _service.DeleteByIdAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var response = _mapper.Map<DepartmentResponse>(result.Value);
            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        private async Task<bool> DepartmentExists(long id)
        {
            var result = await _service.AnyAsync(e => e.Id == id);
            if (!result.IsSuccess)
            {
                return false;
            }

            return result.Value;
        }
    }
}
