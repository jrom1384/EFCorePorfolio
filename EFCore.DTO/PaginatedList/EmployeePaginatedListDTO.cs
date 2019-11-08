using System.Collections.Generic;

namespace EFCore.DTO
{
    public class EmployeePaginatedListDTO : PaginatedListDTO<EmployeeDTO>
    {
        public EmployeePaginatedListDTO(List<EmployeeDTO> list, int matchCount)
            : base(list, matchCount)
        {

        }
    }
}
