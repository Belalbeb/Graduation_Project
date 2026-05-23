using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Graduation_Project.Repositories;
using System.Runtime.CompilerServices;

namespace Graduation_Project.Services
{
    public class SubscriptionPlanService:ISubscriptionPlanService
    {
        private readonly ISubscriptionPlanRepository subscriptionRepository;

        public SubscriptionPlanService(ISubscriptionPlanRepository subscriptionRepository)
        {
            this.subscriptionRepository = subscriptionRepository;
        }
        public async Task<List<SubscriptionPlanResponseDto>> GetAllAsync()
        {
            return await subscriptionRepository.GetAllAsync();
        }
        public async Task AddAsync(AddSubscriptionPlanDto dto)
        {
            SubscriptionPlan subscriptionPlan = new SubscriptionPlan
            {
                Name = dto.Name,
                ShortDescription = dto.ShortDescription,
                MonthlyPrice = dto.MonthlyPrice,
                YearlyPrice = dto.YearlyPrice,
                MaxJobPostsPerMonth = dto.MaxJobPostsPerMonth,
                FeaturedJobPostsPerMonth = dto.FeaturedJobPostsPerMonth,
                HasAiToolsAccess = dto.HasAiToolsAccess,
                HasCandidateSearch = dto.HasCandidateSearch,
                HasPrioritySupport = dto.HasPrioritySupport,
                IsPublished = dto.IsPublished,




            };

            await subscriptionRepository.AddAsync(subscriptionPlan);


        }
        public async Task<bool> UpdateAsync(Guid id,UpdateSubscriptionPlanDto dto)
        {
            var plan = await subscriptionRepository.GetById(id);
            if (plan == null) return false;
            plan.Name = dto.Name;
            plan.ShortDescription = dto.ShortDescription;
            plan.FeaturedJobPostsPerMonth = dto.FeaturedJobPostsPerMonth;
            plan.MaxJobPostsPerMonth = dto.MaxJobPostsPerMonth;
            plan.IsPublished = dto.IsPublished;
            plan.HasCandidateSearch = dto.HasCandidateSearch;
            plan.HasAiToolsAccess = dto.HasAiToolsAccess;
            plan.HasPrioritySupport = dto.HasPrioritySupport;
            plan.MonthlyPrice = dto.MonthlyPrice;
            plan.YearlyPrice = dto.YearlyPrice;

           await subscriptionRepository.update(plan);
            return true;

        }
        public async Task<bool> DeleteAsync(Guid Id)
        {
            var plan = await subscriptionRepository.GetById(Id);
            if (plan == null) return false;
            await subscriptionRepository.Delete(plan);
            return true;


        }
        public async Task<SubscriptionPlanResponseDto> GetByIdAsync(Guid id)
        {
            
            
               var plan=await subscriptionRepository.GetById(id);
            return new SubscriptionPlanResponseDto
            {
                Id = plan.Id,
                Name = plan.Name,
                ShortDescription = plan.ShortDescription,
                MonthlyPrice = plan.MonthlyPrice,
                YearlyPrice = plan.YearlyPrice,
                MaxJobPostsPerMonth = plan.MaxJobPostsPerMonth,
                FeaturedJobPostsPerMonth = plan.FeaturedJobPostsPerMonth,
                HasAiToolsAccess = plan.HasAiToolsAccess,
                HasCandidateSearch = plan.HasCandidateSearch,
                HasPrioritySupport = plan.HasPrioritySupport,
                IsPublished = plan.IsPublished
            };
        }
    }
}
