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
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IMapper _mapper;

        public ProjectService(
            IProjectRepository repository,
            IAssignmentRepository assignmentRepository,
            IMapper mapper) 
        {
            _repository = repository;
            _assignmentRepository = assignmentRepository;
            _mapper = mapper;
        }

        public async Task<Result<ProjectDTO>> AddAsync(ProjectDTO dto)
        {
            try
            {
                dto = StringHelper.Trim(dto);

                //sanity check
                if (string.IsNullOrEmpty(dto.Project))
                {
                    return new Result<ProjectDTO>(ErrorMessages.InvalidParameterValues);
                }

                var project = _mapper.Map<Project>(dto);

                //unique constraint check
                if (await _repository.AnyAsync(p => p.ProjectName.Equals(project.ProjectName)))
                {
                    return new Result<ProjectDTO>($"{ project.ProjectName } { ErrorMessages.RecordAlreadyExists }");
                }

                project = await _repository.AddAsync(project);
                return new Result<ProjectDTO>(_mapper.Map<ProjectDTO>(project));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ProjectDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<bool>> AnyAsync(Expression<Func<ProjectDTO, bool>> predicate)
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

        public async Task<Result<ProjectDTO>> DeleteAsync(ProjectDTO dto)
        {
            try
            {
                var project = await _repository.DeleteAsync(_mapper.Map<Project>(dto));
                return new Result<ProjectDTO>(_mapper.Map<ProjectDTO>(project));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ProjectDTO>(ErrorMessages.DBDeleteConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ProjectDTO>(ErrorMessages.DBDeleteRecordWasInUse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ProjectDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<ProjectDTO>> DeleteByIdAsync(long id)
        {
            try
            {
                var project = await _repository.DeleteByIdAsync(id);
                return new Result<ProjectDTO>(_mapper.Map<ProjectDTO>(project));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ProjectDTO>(ErrorMessages.DBDeleteConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ProjectDTO>(ErrorMessages.DBDeleteRecordWasInUse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ProjectDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<ProjectDTO>> FindByIdAsync(long id)
        {
            try
            {
                var project = await _repository.FindByIdAsync(id);
                return new Result<ProjectDTO>(_mapper.Map<ProjectDTO>(project));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ProjectDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<ProjectDTO>> FirstOrDefaultAsync(Expression<Func<ProjectDTO, bool>> predicate)
        {
            try
            {
                var project = await this.GetTable().FirstOrDefaultAsync(predicate);
                return new Result<ProjectDTO>(project);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ProjectDTO>(ex.Message, ex.HResult);
            }
        }

        private IQueryable<ProjectDTO> GetTable()
        {
            return _repository.GetTable()
                    .Select(p => _mapper.Map<ProjectDTO>(p));
        }

        public async Task<Result<ProjectDTO>> UpdateAsync(ProjectDTO dto)
        {
            try
            {
                dto = StringHelper.Trim(dto);

                //sanity check
                if (string.IsNullOrEmpty(dto.Project))
                {
                    return new Result<ProjectDTO>(ErrorMessages.InvalidParameterValues);
                }

                var project = _mapper.Map<Project>(dto);

                //unique constraint check
                if (await _repository.AnyAsync(p => p.ProjectName.Equals(project.ProjectName) && p.ProjectID != project.ProjectID))
                {
                    return new Result<ProjectDTO>($"{ project.ProjectName } { ErrorMessages.RecordAlreadyExists }");
                }

                project = await _repository.UpdateAsync(project);
                return new Result<ProjectDTO>(_mapper.Map<ProjectDTO>(project));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ProjectDTO>(ErrorMessages.DBUpdateConcurrency);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ProjectDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<PaginatedListDTO<ProjectDTO>>> CreatePageListAsync(PageFilterDTO pageFilter)
        {
            var data = _repository.GetTable()
                .Select(p => p);

            if (!string.IsNullOrEmpty(pageFilter.SearchString))
            {
                data = data.Where(p => p.ProjectName.ToLower().Contains(pageFilter.SearchString.ToLower()));
            }

            pageFilter.SortField = pageFilter.SortField ?? string.Empty;
            switch (pageFilter.SortField.ToLower())
            {
                case "id":
                    if (pageFilter.SortOrder == SortOrder.Descending)
                    {
                        data = data.OrderByDescending(p => p.ProjectID);
                    }
                    else
                    {
                        data = data.OrderBy(p => p.ProjectID);
                    }
                    break;
                case "project":
                default:
                    if (pageFilter.SortOrder == SortOrder.Descending)
                    {
                        data = data.OrderByDescending(p => p.ProjectName);
                    }
                    else
                    {
                        data = data.OrderBy(p => p.ProjectName);
                    }
                    break;
            }

            try
            {
                var matchCount = await data.CountAsync();
                var list = await data.Skip((pageFilter.CurrentPageIndex - 1) * pageFilter.PageSize).Take(pageFilter.PageSize)
                    .Select(i => _mapper.Map<ProjectDTO>(i))
                    .ToListAsync();

                return new Result<PaginatedListDTO<ProjectDTO>>(new PaginatedListDTO<ProjectDTO>(list, matchCount));
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<PaginatedListDTO<ProjectDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<ProjectDTO>> SingleOrDefaultAsync(Expression<Func<ProjectDTO, bool>> predicate)
        {
            try
            {
                var project = await this.GetTable().SingleOrDefaultAsync(predicate);
                return new Result<ProjectDTO>(project);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<ProjectDTO>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<List<ProjectDTO>>> GetListAsync()
        {
            try
            {                
                var projects = await _repository.GetTable()
                    .OrderBy(p => p.ProjectName)
                    .Select(p => _mapper.Map<ProjectDTO>(p))
                    .ToListAsync();

                return new Result<List<ProjectDTO>>(projects);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<List<ProjectDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<IEnumerable<ProjectDTO>>> AddRangeAsync(IEnumerable<ProjectDTO> dtos)
        {
            try
            {
                var projects = await _repository.AddRangeAsync(_mapper.Map<IEnumerable<Project>>(dtos));
                return new Result<IEnumerable<ProjectDTO>>(_mapper.Map<IEnumerable<ProjectDTO>>(projects));
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<ProjectDTO>>(ErrorMessages.DBUpdateException);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<ProjectDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<IEnumerable<ProjectDTO>>> DeleteRangeAsync(IEnumerable<ProjectDTO> dtos)
        {
            try
            {
                var projects = await _repository.DeleteRangeAsync(_mapper.Map<IEnumerable<Project>>(dtos));
                return new Result<IEnumerable<ProjectDTO>>(_mapper.Map<IEnumerable<ProjectDTO>>(projects));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<ProjectDTO>>(ErrorMessages.DBDeleteConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<ProjectDTO>>(ErrorMessages.DBDeleteRecordWasInUse);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<ProjectDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<IEnumerable<ProjectDTO>>> UpdateRangeAsync(IEnumerable<ProjectDTO> dtos)
        {
            try
            {
                var projects = await _repository.UpdateRangeAsync(_mapper.Map<IEnumerable<Project>>(dtos));
                return new Result<IEnumerable<ProjectDTO>>(_mapper.Map<IEnumerable<ProjectDTO>>(projects));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<ProjectDTO>>(ErrorMessages.DBUpdateConcurrency);
            }
            catch (DbUpdateException ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<ProjectDTO>>(ErrorMessages.DBUpdateException);
            }
            catch (Exception ex)
            {
                Log.Error(ex, string.Empty);
                return new Result<IEnumerable<ProjectDTO>>(ex.Message, ex.HResult);
            }
        }

        public async Task<Result<AssignmentPaginatedListDTO>> CreatePageListAsync(AssignmentPageFilterDTO pageFilter)
        {
            var data = _assignmentRepository.GetTable()
                .Include(a => a.Project)
                .Include(a => a.Role)
                .Include(a => a.Employee)
                .Where(a => a.ProjectID == pageFilter.Project_Id)
                .Select(a => a);

            if (!string.IsNullOrEmpty(pageFilter.SearchString))
            {
                data = data.Where(a => (a.Employee.LastName.ToLower().Contains(pageFilter.SearchString.ToLower())
                        || a.Employee.FirstName.ToLower().Contains(pageFilter.SearchString.ToLower())));
            }

            if (pageFilter.Role_Id > 0)
            {
                data = data.Where(a => a.Role.RoleID == pageFilter.Role_Id);
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
