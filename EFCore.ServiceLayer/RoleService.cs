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
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public RoleService(IRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<RoleDTO>> AddAsync(RoleDTO dto)
        {
            try
            {
                dto = StringHelper.Trim(dto);

                //sanity check
                if (string.IsNullOrEmpty(dto.Role))
                {
                    return new Result<RoleDTO>(ErrorMessages.InvalidParameterValues);
                }

                var role = _mapper.Map<Role>(dto);

                //unique constraint check
                if (await _repository.AnyAsync(r => r.RoleName.Equals(role.RoleName)))
                {
                    return new Result<RoleDTO>($"{ role.RoleName } { ErrorMessages.RecordAlreadyExists }");
                }

                role = await _repository.AddAsync(role);
                return new Result<RoleDTO>(_mapper.Map<RoleDTO>(role));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<RoleDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<bool>> AnyAsync(Expression<Func<RoleDTO, bool>> predicate)
        {
            try
            {
                return new Result<bool>(await this.GetTable().AnyAsync(predicate));
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<bool>(ErrorMessages.DBUpdateException);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<bool>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<RoleDTO>> DeleteAsync(RoleDTO dto)
        {
            try
            {
                var role = await _repository.DeleteAsync(_mapper.Map<Role>(dto));
                return new Result<RoleDTO>(_mapper.Map<RoleDTO>(role));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<RoleDTO>(ErrorMessages.DBDeleteConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<RoleDTO>(ErrorMessages.DBDeleteRecordWasInUse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<RoleDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<RoleDTO>> DeleteByIdAsync(long id)
        {
            try
            {
                var role = await _repository.DeleteByIdAsync(id);
                return new Result<RoleDTO>(_mapper.Map<RoleDTO>(role));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<RoleDTO>(ErrorMessages.DBDeleteConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<RoleDTO>(ErrorMessages.DBDeleteRecordWasInUse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<RoleDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<RoleDTO>> FindByIdAsync(long id)
        {
            try
            {
                var role = await _repository.FindByIdAsync(id);
                return new Result<RoleDTO>(_mapper.Map<RoleDTO>(role));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<RoleDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<RoleDTO>> FirstOrDefaultAsync(Expression<Func<RoleDTO, bool>> predicate)
        {
            try
            {
                var role = await this.GetTable().FirstOrDefaultAsync(predicate);
                return new Result<RoleDTO>(role);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<RoleDTO>(ex.Message, ex.HResult);
            }
        }

        private IQueryable<RoleDTO> GetTable()
        {
            return _repository.GetTable()
                .Select(r => _mapper.Map<RoleDTO>(r));
        }

        public async Task<Result<RoleDTO>> UpdateAsync(RoleDTO dto)
        {
            try
            {
                dto = StringHelper.Trim(dto);

                //sanity check
                if (string.IsNullOrEmpty(dto.Role))
                {
                    return new Result<RoleDTO>(ErrorMessages.InvalidParameterValues);
                }

                var role = _mapper.Map<Role>(dto);

                //unique constraint check
                if (await _repository.AnyAsync(r => r.RoleName.Equals(role.RoleName) && r.RoleID != role.RoleID))
                {
                    return new Result<RoleDTO>($"{ role.RoleName } { ErrorMessages.RecordAlreadyExists }");
                }

                role = await _repository.UpdateAsync(role);
                return new Result<RoleDTO>(_mapper.Map<RoleDTO>(role));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<RoleDTO>(ErrorMessages.DBUpdateConcurrency);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<RoleDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<PaginatedListDTO<RoleDTO>>> CreatePageListAsync(PageFilterDTO pageFilter)
        {
            var data = _repository.GetTable()
                .Select(r => r);

            if (!string.IsNullOrEmpty(pageFilter.SearchString))
            {
                data = data.Where(p => p.RoleName.ToLower().Contains(pageFilter.SearchString.ToLower()));
            }

            pageFilter.SortField = pageFilter.SortField ?? string.Empty;
            switch (pageFilter.SortField.ToLower())
            {
                case "id":
                    if (pageFilter.SortOrder == SortOrder.Descending)
                    {
                        data = data.OrderByDescending(p => p.RoleID);
                    }
                    else
                    {
                        data = data.OrderBy(p => p.RoleID);
                    }
                    break;
                case "role":
                default:
                    if (pageFilter.SortOrder == SortOrder.Descending)
                    {
                        data = data.OrderByDescending(p => p.RoleName);
                    }
                    else
                    {
                        data = data.OrderBy(p => p.RoleName);
                    }
                    break;
            }

            try
            {
                var matchCount = await data.CountAsync();
                var list = await data.Skip((pageFilter.CurrentPageIndex - 1) * pageFilter.PageSize).Take(pageFilter.PageSize)
                    .Select(i => _mapper.Map<RoleDTO>(i))
                    .ToListAsync();

                return new Result<PaginatedListDTO<RoleDTO>>(new PaginatedListDTO<RoleDTO>(list, matchCount));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<PaginatedListDTO<RoleDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<RoleDTO>> SingleOrDefaultAsync(Expression<Func<RoleDTO, bool>> predicate)
        {
            try
            {
                var role = await this.GetTable().SingleOrDefaultAsync(predicate);
                return new Result<RoleDTO>(role);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<RoleDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<List<RoleDTO>>> GetListAsync()
        {
            try
            {
                var roles = await _repository.GetTable()
                    .OrderBy(r => r.RoleName)
                    .Select(r => _mapper.Map<RoleDTO>(r))
                    .ToListAsync();

                return new Result<List<RoleDTO>>(roles);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<List<RoleDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<IEnumerable<RoleDTO>>> AddRangeAsync(IEnumerable<RoleDTO> dtos)
        {
            try
            {
                var roles = await _repository.AddRangeAsync(_mapper.Map<IEnumerable<Role>>(dtos));
                return new Result<IEnumerable<RoleDTO>>(_mapper.Map<IEnumerable<RoleDTO>>(roles));
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<RoleDTO>>(ErrorMessages.DBUpdateException);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<RoleDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<IEnumerable<RoleDTO>>> DeleteRangeAsync(IEnumerable<RoleDTO> dtos)
        {
            try
            {
                var roles = await _repository.DeleteRangeAsync(_mapper.Map<IEnumerable<Role>>(dtos));
                return new Result<IEnumerable<RoleDTO>>(_mapper.Map<IEnumerable<RoleDTO>>(roles));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<RoleDTO>>(ErrorMessages.DBDeleteConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<RoleDTO>>(ErrorMessages.DBDeleteRecordWasInUse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<RoleDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<IEnumerable<RoleDTO>>> UpdateRangeAsync(IEnumerable<RoleDTO> dtos)
        {
            try
            {
                var roles = await _repository.UpdateRangeAsync(_mapper.Map<IEnumerable<Role>>(dtos));
                return new Result<IEnumerable<RoleDTO>>(_mapper.Map<IEnumerable<RoleDTO>>(roles));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<RoleDTO>>(ErrorMessages.DBUpdateConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<RoleDTO>>(ErrorMessages.DBUpdateException);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<RoleDTO>>(ex.Message, ex.HResult);
            }
        }
    }
}
