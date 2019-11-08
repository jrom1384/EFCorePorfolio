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
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _repository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IMapper _mapper;

        public AssignmentService(
            IAssignmentRepository repository, 
            IEmployeeRepository employeeRepository,
            IRoleRepository roleRepository,
            IProjectRepository projectRepository,
            IAssignmentRepository assignmentRepository,
            IMapper mapper)
        {
            _repository = repository;
            _employeeRepository = employeeRepository;
            _roleRepository = roleRepository;
            _projectRepository = projectRepository;
            _mapper = mapper;
        }

        public async Task<Result<AssignmentDTO>> AddAsync(AssignmentDTO dto)
        {
            try
            {
                dto = StringHelper.Trim(dto);

                //sanity check
                if (dto.Project_Id == 0 || dto.Employee_Id == 0 || dto.Role_Id == 0)
                {
                    return new Result<AssignmentDTO>(ErrorMessages.InvalidParameterValues);
                }

                var assignment = _mapper.Map<Assignment>(dto);

                //Verify if project exists.
                var project = await _projectRepository.FirstOrDefaultAsync(p => p.ProjectID == assignment.ProjectID);
                if (project == null)
                {
                    return new Result<AssignmentDTO>("Project does not exists.");
                }

                //Verify if role exists.
                var role = await _roleRepository.FirstOrDefaultAsync(r => r.RoleID == assignment.RoleID);
                if (role == null)
                {
                    return new Result<AssignmentDTO>("Role does not exists.");
                }

                //Verify if employee exists.
                var employee = await _employeeRepository.GetTable()
                    .Include(e => e.Department)
                    .FirstOrDefaultAsync(e => e.EmployeeID == assignment.EmployeeID);

                if (employee == null)
                {
                    return new Result<AssignmentDTO>("Employee does not exists.");
                }

                Expression<Func<Assignment, bool>> predicate =
                    (a) => a.ProjectID == assignment.ProjectID
                    && a.EmployeeID == assignment.EmployeeID;

                //unique constraint check
                if (await _repository.AnyAsync(predicate))
                {
                    return new Result<AssignmentDTO>($"{ employee.LastName }, { employee.FirstName} { ErrorMessages.RecordAlreadyExists }");
                }

                assignment = await _repository.AddAsync(assignment);
                project.MemberCount = await _repository.GetTable().CountAsync(a => a.ProjectID == project.ProjectID);

                await _projectRepository.UpdateAsync(project);

                return new Result<AssignmentDTO>(_mapper.Map<AssignmentDTO>(assignment));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<AssignmentDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<bool>> AnyAsync(Expression<Func<AssignmentDTO, bool>> predicate)
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

        public async Task<Result<AssignmentDTO>> DeleteAsync(AssignmentDTO dto)
        {
            try
            {
                var assignment = await _repository.GetTable()
                   .Include(a => a.Project)
                   .Include(a => a.Role)
                   .Include(a => a.Employee)
                   .ThenInclude(a => a.Department)
                   .FirstOrDefaultAsync(a => a.AssignmentID == dto.Id);

                if (assignment != null)
                {
                    assignment = await _repository.DeleteAsync(_mapper.Map<Assignment>(dto));

                    var project = await _projectRepository.FindByIdAsync(dto.Project_Id);
                    if (project != null)
                    {
                        project.MemberCount = await _repository.GetTable().CountAsync(a => a.ProjectID == project.ProjectID);
                        await _projectRepository.UpdateAsync(project);
                    }
                }

                return new Result<AssignmentDTO>(_mapper.Map<AssignmentDTO>(assignment));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<AssignmentDTO>(ErrorMessages.DBDeleteConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<AssignmentDTO>(ErrorMessages.DBDeleteRecordWasInUse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<AssignmentDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<AssignmentDTO>> DeleteByIdAsync(long id)
        {
            try
            {
                var assignment = await _repository.GetTable()
                    .Include(a => a.Project)
                    .Include(a => a.Role)
                    .Include(a => a.Employee)
                    .ThenInclude(a => a.Department)
                    .FirstOrDefaultAsync(a => a.AssignmentID == id);

                if (assignment != null)
                {
                    assignment = await _repository.DeleteByIdAsync(id);

                    var project = await _projectRepository.FindByIdAsync(assignment.ProjectID);
                    if (project != null)
                    {
                        project.MemberCount = await _repository.GetTable().CountAsync(a => a.ProjectID == project.ProjectID);
                        await _projectRepository.UpdateAsync(project);
                    }
                }

                return new Result<AssignmentDTO>(_mapper.Map<AssignmentDTO>(assignment));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<AssignmentDTO>(ErrorMessages.DBDeleteConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<AssignmentDTO>(ErrorMessages.DBDeleteRecordWasInUse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<AssignmentDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<AssignmentDTO>> FindByIdAsync(long id)
        {
            try
            {
                var assignment = await _repository.GetTable()
                    .Include(a => a.Project)
                    .Include(a => a.Role)
                    .Include(a => a.Employee)
                    .ThenInclude(a => a.Department)
                    .FirstOrDefaultAsync(a => a.AssignmentID == id);

                return new Result<AssignmentDTO>(_mapper.Map<AssignmentDTO>(assignment));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<AssignmentDTO>(ex.Message, ex.HResult);
            }
        }

        private IQueryable<AssignmentDTO> GetTable()
        {
            return _repository.GetTable()
                .Select(a => _mapper.Map<AssignmentDTO>(a));
        }

        public async Task<Result<AssignmentDTO>> FirstOrDefaultAsync(Expression<Func<AssignmentDTO, bool>> predicate)
        {
            try
            {
                var assignment = await this.GetTable()
                    .FirstOrDefaultAsync(predicate);
                return new Result<AssignmentDTO>(assignment);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<AssignmentDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<List<AssignmentDTO>>> GetListAsync()
        {
            try
            {
                var assignments = await _repository.GetTable()
                    .Include(a => a.Project)
                    .Include(a => a.Role)
                    .Include(a => a.Employee)
                    .ThenInclude(e => e.Department)
                    .Select(a => _mapper.Map<AssignmentDTO>(a))
                    .OrderBy(a => a.LastName)
                    .ThenBy(a => a.FirstName)
                    .ToListAsync();

                return new Result<List<AssignmentDTO>>(assignments);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<List<AssignmentDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<AssignmentDTO>> SingleOrDefaultAsync(Expression<Func<AssignmentDTO, bool>> predicate)
        {
            try
            {
                var assignment = await _repository.GetTable()
                    .Include(a => a.Project)
                    .Include(a => a.Role)
                    .Include(a => a.Employee)
                    .ThenInclude(e => e.Department)
                    .Select(a => _mapper.Map<AssignmentDTO>(a))
                    .SingleOrDefaultAsync(predicate);

                return new Result<AssignmentDTO>(assignment);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<AssignmentDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<AssignmentDTO>> UpdateAsync(AssignmentDTO dto)
        {
            try
            {
                dto = StringHelper.Trim(dto);

                //sanity check
                if (dto.Project_Id == 0 || dto.Employee_Id == 0 || dto.Role_Id == 0)
                {
                    return new Result<AssignmentDTO>(ErrorMessages.InvalidParameterValues);
                }

                var assignment = _mapper.Map<Assignment>(dto);

                //Verify if project exists.
                var project = await _projectRepository.FirstOrDefaultAsync(p => p.ProjectID == assignment.ProjectID);
                if (project == null)
                {
                    return new Result<AssignmentDTO>("Project does not exists.");
                }

                //Verify if role exists.
                var role = await _roleRepository.FirstOrDefaultAsync(r => r.RoleID == assignment.RoleID);
                if (role == null)
                {
                    return new Result<AssignmentDTO>("Role does not exists.");
                }

                //Verify if employee exists.
                var employee = await _employeeRepository.GetTable()
                    .Include(e => e.Department)
                    .FirstOrDefaultAsync(e => e.EmployeeID == assignment.EmployeeID);

                if (employee == null)
                {
                    return new Result<AssignmentDTO>("Employee does not exists.");
                }

                //unique constraint check
                var uniqueCheck = await _repository.GetTable().AsNoTracking().FirstOrDefaultAsync(a => a.AssignmentID == assignment.AssignmentID);
                if (uniqueCheck != null && uniqueCheck.EmployeeID != assignment.EmployeeID)
                {
                    Expression<Func<Assignment, bool>> predicate =
                           (a) => a.ProjectID == assignment.ProjectID
                           && a.EmployeeID == assignment.EmployeeID;

                    if (await _repository.AnyAsync(predicate))
                    {
                        return new Result<AssignmentDTO>($"{ employee.LastName }, { employee.FirstName} { ErrorMessages.RecordAlreadyExists }");
                    }
                }

                assignment = await _repository.UpdateAsync(_mapper.Map<Assignment>(dto));

                return new Result<AssignmentDTO>(_mapper.Map<AssignmentDTO>(assignment));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<AssignmentDTO>(ErrorMessages.DBUpdateConcurrency);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<AssignmentDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<IEnumerable<AssignmentDTO>>> AddRangeAsync(IEnumerable<AssignmentDTO> dtos)
        {
            try
            {
                var assignments = await _repository.AddRangeAsync(_mapper.Map<IEnumerable<Assignment>>(dtos));

                foreach (var assignment in assignments.GroupBy(a => a.ProjectID))
                {
                    var project = await _projectRepository.FindByIdAsync(assignment.Key);
                    if (project != null)
                    {
                        project.MemberCount = await _repository.GetTable().CountAsync(a => a.ProjectID == project.ProjectID);

                        await _projectRepository.UpdateAsync(project);
                    }
                }

                return new Result<IEnumerable<AssignmentDTO>>(_mapper.Map<IEnumerable<AssignmentDTO>>(assignments));
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<AssignmentDTO>>(ErrorMessages.DBUpdateException);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<AssignmentDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<IEnumerable<AssignmentDTO>>> DeleteRangeAsync(IEnumerable<AssignmentDTO> dtos)
        {
            try
            {
                var assignments = await _repository.DeleteRangeAsync(_mapper.Map<IEnumerable<Assignment>>(dtos));

                foreach (var assignment in assignments.GroupBy(a => a.ProjectID))
                {
                    var project = await _projectRepository.FindByIdAsync(assignment.Key);
                    if (project != null)
                    {
                        project.MemberCount = await _repository.GetTable().CountAsync(a => a.ProjectID == project.ProjectID);

                        await _projectRepository.UpdateAsync(project);
                    }
                }

                return new Result<IEnumerable<AssignmentDTO>>(_mapper.Map<IEnumerable<AssignmentDTO>>(assignments));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<AssignmentDTO>>(ErrorMessages.DBDeleteConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<AssignmentDTO>>(ErrorMessages.DBDeleteRecordWasInUse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<AssignmentDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<IEnumerable<AssignmentDTO>>> UpdateRangeAsync(IEnumerable<AssignmentDTO> dtos)
        {
            try
            {
                var assignments = await _repository.UpdateRangeAsync(_mapper.Map<IEnumerable<Assignment>>(dtos));
                return new Result<IEnumerable<AssignmentDTO>>(_mapper.Map<IEnumerable<AssignmentDTO>>(assignments));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<AssignmentDTO>>(ErrorMessages.DBUpdateConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<AssignmentDTO>>(ErrorMessages.DBUpdateException);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<AssignmentDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<AssignmentPaginatedListDTO>> CreatePageListAsync(AssignmentPageFilterDTO pageFilter)
        {
            var data = _repository.GetTable()
                .Include(a => a.Project)
                .Include(a => a.Role)
                .Include(a => a.Employee)
                .ThenInclude(e => e.Department)
                .Select(a => a);

            if (!string.IsNullOrEmpty(pageFilter.SearchString))
            {
                data = data.Where(a => (a.Employee.LastName.ToLower().Contains(pageFilter.SearchString.ToLower())
                        || a.Employee.FirstName.ToLower().Contains(pageFilter.SearchString.ToLower())));
            }

            if (pageFilter.Department_Id > 0)
            {
                data = data.Where(a => a.Employee.Department.DepartmentID == pageFilter.Department_Id);
            }

            if (pageFilter.Role_Id > 0)
            {
                data = data.Where(a => a.Role.RoleID == pageFilter.Role_Id);
            }

            if (pageFilter.Project_Id > 0)
            {
                data = data.Where(a => a.Project.ProjectID == pageFilter.Project_Id);
            }

            pageFilter.SortField = pageFilter.SortField ?? string.Empty;
            switch (pageFilter.SortField.ToLower())
            {
                case "id":
                    if (pageFilter.SortOrder == SortOrder.Descending)
                    {
                        data = data.OrderByDescending(a => a.AssignmentID);
                    }
                    else
                    {
                        data = data.OrderBy(a => a.AssignmentID);
                    }
                    break;

                case "project":
                    if (pageFilter.SortOrder == SortOrder.Descending)
                    {
                        data = data.OrderByDescending(a => a.Project.ProjectName);
                    }
                    else
                    {
                        data = data.OrderBy(a => a.Project.ProjectName);
                    }
                    break;

                case "role":
                    if (pageFilter.SortOrder == SortOrder.Descending)
                    {
                        data = data.OrderByDescending(a => a.Role.RoleName);
                    }
                    else
                    {
                        data = data.OrderBy(a => a.Role.RoleName);
                    }
                    break;

                case "name":
                default:
                    if (pageFilter.SortOrder == SortOrder.Descending)
                    {
                        data = data.OrderByDescending(a => a.Employee.LastName).ThenBy(a => a.Employee.FirstName);
                    }
                    else
                    {
                        data = data.OrderBy(a => a.Employee.LastName).ThenBy(a => a.Employee.FirstName);
                    }
                    break;
            }

            try
            {
                var matchCount = await data.CountAsync();
                var list = await data.Skip((pageFilter.CurrentPageIndex - 1) * pageFilter.PageSize).Take(pageFilter.PageSize)
                    .Select(i => _mapper.Map<AssignmentDTO>(i))
                    .ToListAsync();

                return new Result<AssignmentPaginatedListDTO>(new AssignmentPaginatedListDTO(list, matchCount));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<AssignmentPaginatedListDTO>(ex.Message, ex.HResult);
            }
        }

    }
}
