using AutoMapper;
using EFCore.Common;
using EFCore.DataLayer;
using EFCore.DataLayer.EFClasses;
using EFCore.DTO;
using EFCore.Utilities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFCore.ServiceLayer
{
    public class ApiClientService : IApiClientService
    {
        private readonly IApiClientRepository _repository;
        private readonly IMapper _mapper;

        public ApiClientService(IApiClientRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> AnyAsync(Expression<Func<ApiClientDTO, bool>> predicate)
        {
            return await this.GetTable().AnyAsync(predicate);
        }

        public async Task<Result<ApiClientDTO>> AddAsync(ApiClientDTO dto, string password)
        {
            dto = StringHelper.Trim(dto);

            //sanity check
            if (string.IsNullOrEmpty(dto.LastName) || string.IsNullOrEmpty(dto.FirstName) || string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(password))
            {
                //Invalid parameter values.
                return new Result<ApiClientDTO>(string.Empty);
            }

            password = password.Trim();
            if (string.IsNullOrEmpty(password))
            {
                //Password is required
                return new Result<ApiClientDTO>(string.Empty);
            }

            try
            {
                //verify if username already taken.
                if (await _repository.AnyAsync(c => c.Username == dto.Username))
                {
                    //$"Username \"{ dto.Username }\" is already taken"
                    return new Result<ApiClientDTO>(string.Empty);
                }

                byte[] passwordHash;
                byte[] passwordSalt;
                HashHelper.Create(password, out passwordHash, out passwordSalt);
                
                var apiClient = await _repository.AddAsync(new ApiClient
                {
                    LastName = dto.LastName,
                    FirstName = dto.FirstName,
                    Username = dto.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                });

                return new Result<ApiClientDTO>(_mapper.Map<ApiClientDTO>(apiClient));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ApiClientDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<ApiClientDTO>> UpdateAsync(ApiClientDTO dto, string password = null)
        {
            dto = StringHelper.Trim(dto);

            //sanity check
            if (string.IsNullOrEmpty(dto.LastName) || string.IsNullOrEmpty(dto.FirstName) || string.IsNullOrEmpty(dto.Username) || string.IsNullOrEmpty(password))
            {
                //Invalid parameter values.
                return new Result<ApiClientDTO>(string.Empty);
            }

            password = password.Trim();
            if (string.IsNullOrEmpty(password))
            {
                //Password is required
                return new Result<ApiClientDTO>(string.Empty);
            }

            try
            {
                var apiClient =  await _repository.FindByIdAsync(dto.Id);
                if (apiClient == null)
                {
                    //User not found
                    return new Result<ApiClientDTO>(string.Empty);
                }
                else if (!apiClient.Username.Equals(dto.Username))
                {
                    //Verify if new username value is already taken.
                    if (await _repository.AnyAsync(c => c.Username.Equals(dto.Username)))
                    {
                        //$"Username \"{ dto.Username }\" is already taken"
                        return new Result<ApiClientDTO>(string.Empty);
                    }
                }

                //Update user properties
                apiClient.FirstName = dto.FirstName;
                apiClient.LastName = dto.LastName;
                apiClient.Username = dto.Username;

                byte[] passwordHash;
                byte[] passwordSalt;
                HashHelper.Create(password, out passwordHash, out passwordSalt);

                apiClient.PasswordHash = passwordHash;
                apiClient.PasswordSalt = passwordSalt;
          
                apiClient = await _repository.UpdateAsync(apiClient);

                return new Result<ApiClientDTO>(_mapper.Map<ApiClientDTO>(apiClient));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ApiClientDTO>(ErrorMessages.DBUpdateConcurrency);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ApiClientDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<ApiClientDTO>> AuthenticateAsync(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                //Invalid parameter values
                return new Result<ApiClientDTO>(string.Empty);
            }

            username = username.Trim();
            password = password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                //Username and password can not be empty.
                return new Result<ApiClientDTO>(string.Empty);
            }

            try
            {
                //Check if user exists
                var apiClient = await _repository.SingleOrDefaultAsync(c => c.Username.Equals(username));
                if (apiClient == null)
                {
                    return new Result<ApiClientDTO>(string.Empty);
                }

                //Check if password is correct
                if (!VerifyPasswordHash(password, apiClient.PasswordHash, apiClient.PasswordSalt))
                {
                    return new Result<ApiClientDTO>(string.Empty);
                }

                //Authentication successful
                return new Result<ApiClientDTO>(_mapper.Map<ApiClientDTO>(apiClient));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ApiClientDTO>(ex.Message, ex.HResult);
            }
        }

        private HashResult CreatePasswordHashWithHashResult(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            return HashHelper.Create(password, out passwordHash, out passwordSalt);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            HashResult result = HashHelper.Create(password, out passwordHash, out passwordSalt);
            switch (result)
            {
                case HashResult.Error_NullHash:
                    throw new ArgumentNullException("password");

                case HashResult.Error_EmptyOrWhiteSpaceHash:
                    throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

                case HashResult.Failed:
                case HashResult.Success:
                    return;

                case HashResult.Error_HashLengthNot64:
                case HashResult.Error_SaltLengthNot128:
                case HashResult.None:
                default:
                    throw new NotSupportedException("Implementation for Hash Result type not supported.");
            }
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            HashResult result = HashHelper.Verify(password, storedHash, storedSalt);
            switch (result)
            {
                case HashResult.Success:
                    return true;
                 
                case HashResult.Error_HashLengthNot64:
                case HashResult.Error_NullHash:
                case HashResult.Error_EmptyOrWhiteSpaceHash:
                case HashResult.Error_SaltLengthNot128:
                case HashResult.Failed:
                case HashResult.None:
                default:
                    return false;
                    //Commented not a good idea to throw exception and let clients have an idea about security features.
                    //throw new ArgumentNullException("password");
                    //throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
                    //throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
                    //throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordSalt"); 
                    //throw new NotSupportedException("Implementation for Hash Result type not supported.");
            }
        }

        public async Task<Result<ApiClientDTO>> DeleteAsync(ApiClientDTO dto)
        {
            try
            {
                var apiClient = await _repository.DeleteAsync(_mapper.Map<ApiClient>(dto));
                return new Result<ApiClientDTO>(_mapper.Map<ApiClientDTO>(apiClient));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ApiClientDTO>(ErrorMessages.DBDeleteConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ApiClientDTO>(ErrorMessages.DBDeleteRecordWasInUse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ApiClientDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<ApiClientDTO>> DeleteByIdAsync(long id)
        {
            try
            {
                var apiClient = await _repository.DeleteByIdAsync(id);
                return new Result<ApiClientDTO>(_mapper.Map<ApiClientDTO>(apiClient));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ApiClientDTO>(ErrorMessages.DBDeleteConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ApiClientDTO>(ErrorMessages.DBDeleteRecordWasInUse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ApiClientDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<ApiClientDTO>> FindByIdAsync(long id)
        {
            try
            {
                var apiClient = await _repository.FindByIdAsync(id);
                return new Result<ApiClientDTO>(_mapper.Map<ApiClientDTO>(apiClient));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ApiClientDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<ApiClientDTO>> FirstOrDefaultAsync(Expression<Func<ApiClientDTO, bool>> predicate)
        {
            try
            {
                var apiClient = await this.GetTable().FirstOrDefaultAsync(predicate);
                return new Result<ApiClientDTO>(_mapper.Map<ApiClientDTO>(apiClient));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ApiClientDTO>(ex.Message, ex.HResult);
            }
        }

        private IQueryable<ApiClientDTO> GetTable()
        {
            return _repository.GetTable().Select(r => _mapper.Map<ApiClientDTO>(r));
        }

        public async Task<Result<ApiClientDTO>> SingleOrDefaultAsync(Expression<Func<ApiClientDTO, bool>> predicate)
        {
            try
            {
                var apiClient = await this.GetTable().SingleOrDefaultAsync(predicate);
                return new Result<ApiClientDTO>(apiClient);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ApiClientDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<List<ApiClientDTO>>> GetListAsync()
        {
            try
            {
                return new Result<List<ApiClientDTO>>(await this.GetTable().OrderBy(c => c.Username).ToListAsync());
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<List<ApiClientDTO>>(ex.Message, ex.HResult);
            }
        }
    }
}
