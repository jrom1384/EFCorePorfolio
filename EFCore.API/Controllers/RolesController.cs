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
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _service;
        private readonly IMapper _mapper;

        public RolesController(IRoleService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<List<RoleResponse>>> GetRole()
        {
            var result = await _service.GetListAsync();
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(_mapper.Map<List<RoleResponse>>(result.Value));
        }

        // GET: api/Roles/Search
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
                PageResult = _mapper.Map<List<RoleResponse>>(result.Value)
            });
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RoleResponse>> GetRole(long id)
        {
            var result = await _service.FindByIdAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var response = _mapper.Map<RoleResponse>(result.Value);
            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<ActionResult<RoleResponse>> PutRole(long id, RoleRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            var result = await _service.UpdateAsync(_mapper.Map<RoleDTO>(request));
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
                //else if (result.ErrorType == ErrorType.DBUpdateConcurrency && !await RoleExists(id))
                //{
                //    return NotFound();
                //}
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
            }

            return Ok(_mapper.Map<RoleResponse>(result.Value));
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult<RoleResponse>> PostRole(NewRoleRequest request)
        {
            var result = await _service.AddAsync(_mapper.Map<RoleDTO>(request));
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

            var response = _mapper.Map<RoleResponse>(result.Value);
            return CreatedAtAction(nameof(GetRole), new { id = response.Id }, response);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RoleResponse>> DeleteRole(long id)
        {
            var result = await _service.DeleteByIdAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var response = _mapper.Map<RoleResponse>(result.Value);
            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        private async Task<bool> RoleExists(long id)
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
