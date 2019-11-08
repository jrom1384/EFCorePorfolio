using AutoMapper;
using EFCore.Common;
using EFCore.DataLayer;
using EFCore.DataLayer.EFClasses;
using EFCore.DTO;
using EFCore.Utilities;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Exceptions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EFCore.ServiceLayer
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeService(
            IEmployeeRepository repository, 
            IDepartmentRepository departmentRepository,
            IMapper mapper)
        {
            _repository = repository;
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<Result<EmployeeDTO>> AddAsync(EmployeeDTO dto)
        {
            try
            {
                dto = StringHelper.Trim(dto);

                //sanity check
                if (string.IsNullOrEmpty(dto.FirstName) 
                    || string.IsNullOrEmpty(dto.LastName) 
                    || dto.Department_Id == 0)
                {
                    return new Result<EmployeeDTO>(ErrorMessages.InvalidParameterValues);
                }

                var employee = _mapper.Map<Employee>(dto);
                
                //Verify if department exists.
                var department = await _departmentRepository.FirstOrDefaultAsync(d => d.DepartmentID == employee.DepartmentID);
                if (department == null)
                {
                    return new Result<EmployeeDTO>("Department does not exists.");
                }

                Expression<Func<Employee, bool>> predicate = 
                    (e) => e.FirstName.Equals(employee.FirstName)
                          && e.LastName.Equals(employee.LastName)
                          && e.DateOfBirth == employee.DateOfBirth;

                //unique constraint check
                if (await _repository.AnyAsync(predicate))
                {
                    return new Result<EmployeeDTO>($"{ employee.LastName }, { employee.FirstName} { ErrorMessages.RecordAlreadyExists }");
                }

                employee = await _repository.AddAsync(employee);
                return new Result<EmployeeDTO>(_mapper.Map<EmployeeDTO>(employee));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<EmployeeDTO>(ex.Message, ex.HResult);
            }
        }

      
        public async Task<Result<bool>> AnyAsync(Expression<Func<EmployeeDTO, bool>> predicate)
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

        public async Task<Result<EmployeeDTO>> DeleteAsync(EmployeeDTO dto)
        {
            try
            {
                var employee = await _repository.DeleteAsync(_mapper.Map<Employee>(dto));
                return new Result<EmployeeDTO>(_mapper.Map<EmployeeDTO>(employee));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<EmployeeDTO>(ErrorMessages.DBDeleteConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<EmployeeDTO>(ErrorMessages.DBDeleteRecordWasInUse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<EmployeeDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<EmployeeDTO>> DeleteByIdAsync(long id)
        {
            try
            {
                var employee = await _repository.GetTable()
                    .Include(e => e.Department)
                    .FirstOrDefaultAsync(e => e.EmployeeID == id);

                if (employee != null)
                {
                    employee = await _repository.DeleteAsync(employee);
                }

                return new Result<EmployeeDTO>(_mapper.Map<EmployeeDTO>(employee));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<EmployeeDTO>(ErrorMessages.DBDeleteConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<EmployeeDTO>(ErrorMessages.DBDeleteRecordWasInUse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<EmployeeDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<EmployeeDTO>> FindByIdAsync(long id)
        {
            try
            {
                var employee = await _repository.FindByIdAsync(id);
                return new Result<EmployeeDTO>(_mapper.Map<EmployeeDTO>(employee));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<EmployeeDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<EmployeeDTO>> FirstOrDefaultAsync(Expression<Func<EmployeeDTO, bool>> predicate)
        {
            try
            {
                var employee = await this.GetTable().FirstOrDefaultAsync(predicate);
                return new Result<EmployeeDTO>(employee);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<EmployeeDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<EmployeeDTO>> FirstOrDefaultIncludeDepartmentAsync(Expression<Func<EmployeeDTO, bool>> predicate)
        {
            try
            {
                var employee = await _repository.GetTable()
                        .Include(e => e.Department)
                        .Select(e => _mapper.Map<EmployeeDTO>(e))
                        .FirstOrDefaultAsync(predicate);

                return new Result<EmployeeDTO>(employee);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<EmployeeDTO>(ex.Message, ex.HResult);
            }
        }

        private IQueryable<EmployeeDTO> GetTable()
        {
            return _repository.GetTable()
                    .Select(e => _mapper.Map<EmployeeDTO>(e));
        }

        public async Task<Result<EmployeeDTO>> UpdateAsync(EmployeeDTO dto)
        {
            try
            {
                dto = StringHelper.Trim(dto);

                //sanity check
                if (string.IsNullOrEmpty(dto.FirstName) 
                    || string.IsNullOrEmpty(dto.LastName) 
                    || dto.Department_Id == 0)
                {
                    return new Result<EmployeeDTO>(ErrorMessages.InvalidParameterValues);
                }

                var employee = _mapper.Map<Employee>(dto);

                //Verify if department exists.
                var department = await _departmentRepository.FirstOrDefaultAsync(d => d.DepartmentID == employee.DepartmentID);
                if (department == null)
                {
                    return new Result<EmployeeDTO>("Department does not exists.");
                }

                Expression<Func<Employee, bool>> predicate =
                    (e) => e.FirstName.Equals(employee.FirstName)
                          && e.LastName.Equals(employee.LastName)
                          && e.DateOfBirth == employee.DateOfBirth
                          && e.EmployeeID != employee.EmployeeID;

                //unique constraint check
                if (await _repository.AnyAsync(predicate))
                {
                    return new Result<EmployeeDTO>($"{ employee.LastName }, { employee.FirstName} { ErrorMessages.RecordAlreadyExists }");
                }

                employee = await _repository.UpdateAsync(employee);
                return new Result<EmployeeDTO>(_mapper.Map<EmployeeDTO>(employee));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<EmployeeDTO>(ErrorMessages.DBUpdateConcurrency);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<EmployeeDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<EmployeeDTO>> SingleOrDefaultAsync(Expression<Func<EmployeeDTO, bool>> predicate)
        {
            try
            {
                var employee = await this.GetTable().SingleOrDefaultAsync(predicate);
                return new Result<EmployeeDTO>(employee);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<EmployeeDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<List<EmployeeDTO>>> GetListAsync()
        {
            try
            {
                var employees = await _repository.GetTable()
                    .Include(e => e.Department)
                    .Select(e => _mapper.Map<EmployeeDTO>(e))
                    .OrderBy(e => e.LastName)
                    .ThenBy(e => e.FirstName)
                    .ToListAsync();

                return new Result<List<EmployeeDTO>>(employees);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<List<EmployeeDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<List<EmployeeDTO>>> GetListAsync(long id)
        {
            try
            {
                var employees = await _repository.GetTable()
                    .Include(e => e.Department)
                    .Where(e => e.DepartmentID == id)
                    .Select(e => _mapper.Map<EmployeeDTO>(e))
                    .OrderBy(e => e.LastName)
                    .ThenBy(e => e.FirstName)
                    .ToListAsync();

                return new Result<List<EmployeeDTO>>(employees);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<List<EmployeeDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<EmployeePaginatedListDTO>> CreatePageListAsync(EmployeePageFilterDTO pageFilter)
        {
            var data = _repository.GetTable()
                .Include(e => e.Department)
                .Select(e => e);

            if (!string.IsNullOrEmpty(pageFilter.SearchString))
            {
                data = data.Where(e => (e.LastName.ToLower().Contains(pageFilter.SearchString.ToLower())
                        || e.FirstName.ToLower().Contains(pageFilter.SearchString.ToLower())));
            }

            if (pageFilter.Gender.HasValue)
            {
                data = data.Where(e => e.Gender == pageFilter.Gender.Value);
            }

            if (pageFilter.IsActive.HasValue)
            {
                data = data.Where(e => e.IsActive == pageFilter.IsActive.Value);
            }

            if (pageFilter.Department_Id > 0)
            {
                data = data.Where(e => e.DepartmentID == pageFilter.Department_Id);
            }

            pageFilter.SortField = pageFilter.SortField ?? string.Empty;
            switch (pageFilter.SortField.ToLower())
            {
                case "id":
                    if (pageFilter.SortOrder == SortOrder.Descending)
                    {
                        data = data.OrderByDescending(e => e.EmployeeID);
                    }
                    else
                    {
                        data = data.OrderBy(e => e.EmployeeID);
                    }
                    break;
                case "dateofbirth":
                    if (pageFilter.SortOrder == SortOrder.Descending)
                    {
                        data = data.OrderByDescending(e => e.DateOfBirth);
                    }
                    else
                    {
                        data = data.OrderBy(e => e.DateOfBirth);
                    }
                    break;
                case "isactive":
                    if (pageFilter.SortOrder == SortOrder.Descending)
                    {
                        data = data.OrderByDescending(e => e.IsActive);
                    }
                    else
                    {
                        data = data.OrderBy(e => e.IsActive);
                    }
                    break;
                case "gender":
                    if (pageFilter.SortOrder == SortOrder.Descending)
                    {
                        data = data.OrderByDescending(e => e.Gender);
                    }
                    else
                    {
                        data = data.OrderBy(e => e.Gender);
                    }
                    break;
                case "department":
                    if (pageFilter.SortOrder == SortOrder.Descending)
                    {
                        data = data.OrderByDescending(e => e.Department.DepartmentName);
                    }
                    else
                    {
                        data = data.OrderBy(e => e.Department.DepartmentName);
                    }
                    break;
                case "name":
                default:
                    if (pageFilter.SortOrder == SortOrder.Descending)
                    {
                        data = data.OrderByDescending(e => e.LastName).ThenBy(e => e.FirstName);
                    }
                    else
                    {
                        data = data.OrderBy(e => e.LastName).ThenBy(e => e.FirstName);
                    }
                    break;
            }

            try
            {
                var matchCount = await data.CountAsync();
                var list = await data.Skip((pageFilter.CurrentPageIndex - 1) * pageFilter.PageSize).Take(pageFilter.PageSize)
                    .Select(i => _mapper.Map<EmployeeDTO>(i))
                    .ToListAsync();

                return new Result<EmployeePaginatedListDTO>(new EmployeePaginatedListDTO(list, matchCount));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<EmployeePaginatedListDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<IEnumerable<EmployeeDTO>>> AddRangeAsync(IEnumerable<EmployeeDTO> dtos)
        {
            try
            {
                var employees = await _repository.AddRangeAsync(_mapper.Map<IEnumerable<Employee>>(dtos));
                return new Result<IEnumerable<EmployeeDTO>>(_mapper.Map<IEnumerable<EmployeeDTO>>(employees));
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<EmployeeDTO>>(ErrorMessages.DBUpdateException);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<EmployeeDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<IEnumerable<EmployeeDTO>>> DeleteRangeAsync(IEnumerable<EmployeeDTO> dtos)
        {
            try
            {
                var employees = await _repository.DeleteRangeAsync(_mapper.Map<IEnumerable<Employee>>(dtos));
                return new Result<IEnumerable<EmployeeDTO>>(_mapper.Map<IEnumerable<EmployeeDTO>>(employees));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<EmployeeDTO>>(ErrorMessages.DBDeleteConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<EmployeeDTO>>(ErrorMessages.DBDeleteRecordWasInUse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<EmployeeDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<IEnumerable<EmployeeDTO>>> UpdateRangeAsync(IEnumerable<EmployeeDTO> dtos)
        {
            try
            {
                var employees = await _repository.UpdateRangeAsync(_mapper.Map<IEnumerable<Employee>>(dtos));
                return new Result<IEnumerable<EmployeeDTO>>(_mapper.Map<IEnumerable<EmployeeDTO>>(employees));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<EmployeeDTO>>(ErrorMessages.DBUpdateConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<EmployeeDTO>>(ErrorMessages.DBUpdateException);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<EmployeeDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<EmployeeDTO>> PatchEmployeeAsync(long id, JsonPatchDocument<EmployeeDTO> patchDoc)
        {
            try
            {
                var employee = await _repository.FindByIdAsync(id);
                if (employee == null)
                {
                    return new Result<EmployeeDTO>("Employee not found");
                }

                var employeeDTO = _mapper.Map<EmployeeDTO>(employee);
                patchDoc.ApplyTo(employeeDTO);

                employeeDTO = StringHelper.Trim(employeeDTO);

                //sanity check
                if (string.IsNullOrEmpty(employeeDTO.FirstName)
                    || string.IsNullOrEmpty(employeeDTO.LastName)
                    || employeeDTO.Id <= 0
                    || employeeDTO.Department_Id <= 0
                    || (int)employeeDTO.Gender < 0
                    || (int)employeeDTO.Gender > 1
                    || employeeDTO.FirstName.Length > 50
                    || employeeDTO.LastName.Length > 50)
                {
                    return new Result<EmployeeDTO>(ErrorMessages.InvalidParameterValues);
                }

                //Verify if department exists.
                var department = await _departmentRepository.FirstOrDefaultAsync(d => d.DepartmentID == employeeDTO.Department_Id);
                if (department == null)
                {
                    return new Result<EmployeeDTO>("Department does not exists.");
                }

                //Check changes
                if (!employee.FirstName.Equals(employeeDTO.FirstName)
                    || !employee.LastName.Equals(employeeDTO.LastName)
                    || employee.DateOfBirth != employeeDTO.DateOfBirth)
                {
                    Expression<Func<Employee, bool>> predicate =
                        (e) => e.FirstName.Equals(employeeDTO.FirstName)
                              && e.LastName.Equals(employeeDTO.LastName)
                              && e.DateOfBirth == employeeDTO.DateOfBirth;

                    //unique constraint check
                    if (await _repository.AnyAsync(predicate))
                    {
                        return new Result<EmployeeDTO>($"{ employee.LastName }, { employee.FirstName} { ErrorMessages.RecordAlreadyExists }");
                    }
                }

                _mapper.Map(employeeDTO, employee);
                employee = await _repository.UpdateAsync(employee);
                return new Result<EmployeeDTO>(_mapper.Map<EmployeeDTO>(employee));
            }
            catch (JsonPatchException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<EmployeeDTO>(ErrorMessages.JsonPatchException);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<EmployeeDTO>(ex.Message, ex.HResult);
            }
        }
    }
}
