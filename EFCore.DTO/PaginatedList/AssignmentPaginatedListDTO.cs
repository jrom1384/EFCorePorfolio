using System.Collections.Generic;

namespace EFCore.DTO
{
    public class AssignmentPaginatedListDTO : PaginatedListDTO<AssignmentDTO>
    {
        public AssignmentPaginatedListDTO(List<AssignmentDTO> list, int matchCount) 
            : base(list, matchCount)
        {

        }
    }
}
