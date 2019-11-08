using EFCore.DataLayer.EFClasses;

namespace EFCore.DataLayer
{
    public class AssignmentRepository : GenericRepository<Assignment>, IAssignmentRepository
    {
        public AssignmentRepository(ApplicationDBContext context) : base(context)
        {
        }
    }
}
