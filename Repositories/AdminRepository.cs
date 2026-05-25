using Graduation_Project.Models;

namespace Graduation_Project.Repositories
{
    public class AdminRepository:IAdminRepository
    {
        private readonly ApplicationDbContext dbContext;

        public AdminRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
       
    }
}
