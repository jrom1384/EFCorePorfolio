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
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _repository;
        private readonly IMapper _mapper;

        public DepartmentService(DataLayer.IDepartmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<DepartmentDTO>> AddAsync(DepartmentDTO dto)
        {
            try
            {
                dto = StringHelper.Trim(dto);

                //sanity check
                if (string.IsNullOrEmpty(dto.Department))
                {
                    return new Result<DepartmentDTO>(ErrorMessages.InvalidParameterValues);
                }

                var department = _mapper.Map<Department>(dto);

                //unique constraint check
                if (await _repository.AnyAsync(d => d.DepartmentName.Equals(department.DepartmentName)))
                {
                    return new Result<DepartmentDTO>($"{ department.DepartmentName} { ErrorMessages.RecordAlreadyExists }");
                }

                department = await _repository.AddAsync(department);
                return new Result<DepartmentDTO>(_mapper.Map<DepartmentDTO>(department));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<DepartmentDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<bool>> AnyAsync(Expression<Func<DepartmentDTO, bool>> predicate)
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

        public async Task<Result<DepartmentDTO>> DeleteAsync(DepartmentDTO dto)
        {
            try
            {
                var department = await _repository.DeleteAsync(_mapper.Map<Department>(dto));
                return new Result<DepartmentDTO>(_mapper.Map<DepartmentDTO>(department));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<DepartmentDTO>(ErrorMessages.DBDeleteConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<DepartmentDTO>(ErrorMessages.DBDeleteRecordWasInUse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<DepartmentDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<DepartmentDTO>> DeleteByIdAsync(long id)
        {
            try
            {
                var department = await _repository.DeleteByIdAsync(id);
                return new Result<DepartmentDTO>(_mapper.Map<DepartmentDTO>(department));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<DepartmentDTO>(ErrorMessages.DBDeleteConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<DepartmentDTO>(ErrorMessages.DBDeleteRecordWasInUse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<DepartmentDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<DepartmentDTO>> FindByIdAsync(long id)
        {
            try
            {
                var department = await _repository.FindByIdAsync(id);
                return new Result<DepartmentDTO>(_mapper.Map<DepartmentDTO>(department));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<DepartmentDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<DepartmentDTO>> FirstOrDefaultAsync(Expression<Func<DepartmentDTO, bool>> predicate)
        {
            try
            {
                var department = await this.GetTable()
                    .FirstOrDefaultAsync(predicate);

                return new Result<DepartmentDTO>(department);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<DepartmentDTO>(ex.Message, ex.HResult);
            }
        }

        private IQueryable<DepartmentDTO> GetTable()
        {
            return _repository.GetTable()
                    .Select(d => _mapper.Map<DepartmentDTO>(d));
        }

        public async Task<Result<DepartmentDTO>> UpdateAsync(DepartmentDTO dto)
        {
            try
            {
                dto = StringHelper.Trim(dto);

                //sanity check
                if (string.IsNullOrEmpty(dto.Department))
                {
                    return new Result<DepartmentDTO>(ErrorMessages.InvalidParameterValues);
                }

                var department = _mapper.Map<Department>(dto);

                //unique constraint check
                if (await _repository.AnyAsync(d => d.DepartmentName.Equals(department.DepartmentName) && d.DepartmentID != department.DepartmentID))
                {
                    return new Result<DepartmentDTO>($"{ department.DepartmentName} { ErrorMessages.RecordAlreadyExists }");
                }

                department = await _repository.UpdateAsync(department);
                return new Result<DepartmentDTO>(_mapper.Map<DepartmentDTO>(department));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<DepartmentDTO>(ErrorMessages.DBUpdateConcurrency);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<DepartmentDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<PaginatedListDTO<DepartmentDTO>>> CreatePageListAsync(PageFilterDTO pageFilter)
        {
            var data = _repository.GetTable()
                .Select(d => d);

            if (!string.IsNullOrEmpty(pageFilter.SearchString))
            {
                data = data.Where(d => d.DepartmentName.ToLower().Contains(pageFilter.SearchString.ToLower()));
            }

            pageFilter.SortField = pageFilter.SortField ?? string.Empty;
            switch (pageFilter.SortField.ToLower())
            {
                case "id":
                    if (pageFilter.SortOrder == SortOrder.Descending)
                    {
                        data = data.OrderByDescending(d => d.DepartmentID);
                    }
                    else
                    {
                        data = data.OrderBy(d => d.DepartmentID);
                    }
                    break;

                case "department":
                default:
                    if (pageFilter.SortOrder == SortOrder.Descending)
                    {
                        data = data.OrderByDescending(d => d.DepartmentName);
                    }
                    else
                    {
                        data = data.OrderBy(d => d.DepartmentName);
                    }
                    break;
            }

            try
            {
                var matchCount = await data.CountAsync();
                var list = await data.Skip((pageFilter.CurrentPageIndex - 1) * pageFilter.PageSize).Take(pageFilter.PageSize)
                    .Select(i => _mapper.Map<DepartmentDTO>(i))
                    .ToListAsync();

                return new Result<PaginatedListDTO<DepartmentDTO>>(new PaginatedListDTO<DepartmentDTO>(list, matchCount));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<PaginatedListDTO<DepartmentDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<DepartmentDTO>> SingleOrDefaultAsync(Expression<Func<DepartmentDTO, bool>> predicate)
        {
            try
            {
                var department = await this.GetTable().SingleOrDefaultAsync(predicate);
                return new Result<DepartmentDTO>(department);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message);
                return new Result<DepartmentDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<List<DepartmentDTO>>> GetListAsync()
        {
            try
            {
                var departments = await _repository.GetTable()
                        .OrderBy(d => d.DepartmentName)
                        .Select(d => _mapper.Map<DepartmentDTO>(d))
                        .ToListAsync();

                return new Result<List<DepartmentDTO>>(departments);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<List<DepartmentDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<IEnumerable<DepartmentDTO>>> AddRangeAsync(IEnumerable<DepartmentDTO> dtos)
        {
            try
            {
                var departments = await _repository.AddRangeAsync(_mapper.Map<IEnumerable<Department>>(dtos));
                return new Result<IEnumerable<DepartmentDTO>>(_mapper.Map<IEnumerable<DepartmentDTO>>(departments));
            }
            catch(DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<DepartmentDTO>>(ErrorMessages.DBUpdateException);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<DepartmentDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<IEnumerable<DepartmentDTO>>> DeleteRangeAsync(IEnumerable<DepartmentDTO> dtos)
        {
            try
            {
                var departments = await _repository.DeleteRangeAsync(_mapper.Map<IEnumerable<Department>>(dtos));
                return new Result<IEnumerable<DepartmentDTO>>(_mapper.Map<IEnumerable<DepartmentDTO>>(departments));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<DepartmentDTO>>(ErrorMessages.DBDeleteConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<DepartmentDTO>>(ErrorMessages.DBDeleteRecordWasInUse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<DepartmentDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<IEnumerable<DepartmentDTO>>> UpdateRangeAsync(IEnumerable<DepartmentDTO> dtos)
        {
            try
            {
                var departments = await _repository.UpdateRangeAsync(_mapper.Map<IEnumerable<Department>>(dtos));
                return new Result<IEnumerable<DepartmentDTO>>(_mapper.Map<IEnumerable<DepartmentDTO>>(departments));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<DepartmentDTO>>(ErrorMessages.DBUpdateConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<DepartmentDTO>>(ErrorMessages.DBUpdateException);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<DepartmentDTO>>(ex.Message, ex.HResult);
            }
        }

    }
}
