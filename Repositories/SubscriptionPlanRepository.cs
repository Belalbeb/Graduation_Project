using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class SubscriptionPlanRepository:ISubscriptionPlanRepository
    {
        private readonly ApplicationDbContext dbContext;

        public SubscriptionPlanRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<List<SubscriptionPlanResponseDto>> GetAllAsync()
        {
            return await dbContext.subscriptionPlans.Select(x => new SubscriptionPlanResponseDto { Id = x.Id, Name = x.Name,
                ShortDescription = x.ShortDescription,
                MonthlyPrice = x.MonthlyPrice,
                YearlyPrice = x.YearlyPrice,
                MaxJobPostsPerMonth = x.MaxJobPostsPerMonth,
                FeaturedJobPostsPerMonth = x.FeaturedJobPostsPerMonth,
                HasAiToolsAccess = x.HasAiToolsAccess,
                HasCandidateSearch = x.HasCandidateSearch,
                HasPrioritySupport = x.HasPrioritySupport,
                IsPublished = x.IsPublished,
                CreatedAt=x.CreatedAt

            }).ToListAsync();
        }
   
        public async Task AddAsync(SubscriptionPlan subscription)
        {
            await dbContext.subscriptionPlans.AddAsync(subscription);
            await dbContext.SaveChangesAsync();


        }
        public async Task<SubscriptionPlan> GetById(Guid Id)
        {
            return await dbContext.subscriptionPlans.FirstOrDefaultAsync(x => x.Id == Id);
        }
        public async Task update(SubscriptionPlan plan)
        {
            dbContext.subscriptionPlans.Update(plan);
           await dbContext.SaveChangesAsync();
        }
        public async Task Delete(SubscriptionPlan plan)
        {
            dbContext.subscriptionPlans.Remove(plan);
           await dbContext.SaveChangesAsync();
        }
    }
}
