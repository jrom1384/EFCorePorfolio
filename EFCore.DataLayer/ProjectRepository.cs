using EFCore.DataLayer.EFClasses;

namespace EFCore.DataLayer
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDBContext context) : base(context)
        {

        }
    }
}
