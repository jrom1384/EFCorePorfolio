using AutoMapper;
using EFCore.API.Models;
using EFCore.API.Options;
using EFCore.Common;
using EFCore.DTO;
using EFCore.ServiceLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EFCore.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ApiClientsController : ControllerBase
    {
        private readonly IApiClientService _service;
        private readonly IMapper _mapper;
        private readonly JwtSettings _jwtSettings;

        public ApiClientsController(
            IApiClientService service, 
            IMapper mapper, 
            IOptions<JwtSettings> jwtSettings)
        {
            _service = service;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult> Authenticate(ApiClientLoginRequest request)
        {
            var result = await _service.AuthenticateAsync(request.Username, request.Password);
            if (!result.IsSuccess)
            {
                return BadRequest(new { Message = "Username or password is incorrect" } );
            }

            var apiClient = _mapper.Map<ApiClient>(result.Value);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, apiClient.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return Ok(new 
            {
                apiClient.LastName,
                apiClient.FirstName,
                apiClient.Username,
                Token = tokenHandler.WriteToken(token)
            });
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<ApiClientResponse>> Register(ApiClientRegistrationRequest request)
        {
            var result = await _service.AddAsync(_mapper.Map<ApiClientDTO>(request), request.Password);
            if (!result.IsSuccess)
            {
                switch (result.ErrorType)
                {
                    case ErrorType.Defined:
                        return BadRequest();
                        
                    case ErrorType.UnhandledException:
                    default:
                        return StatusCode(StatusCodes.Status500InternalServerError);

                }
            }

            return Created(nameof(Register), _mapper.Map<ApiClientResponse>(result.Value));
        }

        // PUT: api/ApiClients/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiClientResponse>> PutApiClient(long id, ApiClientRequest request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            var result = await _service.UpdateAsync(_mapper.Map<ApiClientDTO>(request), request.Password);
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

            return Ok(_mapper.Map<ApiClientResponse>(result.Value));
        }

        // GET: api/ApiClients
        [HttpGet]
        public async Task<ActionResult<List<ApiClientResponse>>> GetApiClient()
        {
            var result = await _service.GetListAsync();
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return Ok(_mapper.Map<List<ApiClientResponse>>(result.Value));
        }

        // GET: api/ApiClients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiClientResponse>> GetApiClient(long id)
        {
            var result = await _service.FindByIdAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var response = _mapper.Map<ApiClientResponse>(result.Value);
            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        // DELETE: api/ApiClients/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiClientResponse>> DeleteApiClient(long id)
        {
            var result = await _service.DeleteByIdAsync(id);
            if (!result.IsSuccess)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var response = _mapper.Map<ApiClientResponse>(result.Value);
            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        private async Task<bool> ApiClientExists(long id)
        {
            return await _service.AnyAsync(e => e.Id == id);
        }
    }
}
