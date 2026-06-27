using Bogus.DataSets;
using Graduation_Project.Dtos;
using Graduation_Project.DTOs.Subscriptions;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.ComponentModel.Design;

namespace Graduation_Project.Repositories
{
    public class SubscriptionRepository:ISubscriptionRepository
    {
        private readonly ApplicationDbContext _context;

        public SubscriptionRepository(ApplicationDbContext context)
        {
            _context = context;
        }
   
        public async Task AddAsync(CompanySubscription subscription)
        {
            await _context.companySubscriptions.AddAsync(subscription);
            await _context.SaveChangesAsync();
        }
        public async Task Update(CompanySubscription companySubscription)
        {
           _context.companySubscriptions.Update(companySubscription);
            await _context.SaveChangesAsync();
        }
        public async Task<List<SubscriptionDetailsDto>> GetAllAsync()
        {
            return await _context.companySubscriptions
                .Include(x => x.Company)
                  .ThenInclude(x=>x.User)
                .Include(x => x.SubscriptionPlan)
                .Select(x => new SubscriptionDetailsDto
                {
                    Id = x.Id,
                    CompanyName = x.Company.Name,
                    LogoUrl=x.Company.LogoUrl,
                    Email=x.Company.User.Email,
                    PlanName = x.SubscriptionPlan.Name,
                    BillingCycle = x.BillingCycle.ToString(),
                    PaidAmount = x.PaidAmount,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsActive = x.IsActive
                })
                .ToListAsync();
        }
    
        public async Task<SubscriptionDashboardDto> GetDashboardAsync()
        {
            var currentMonth = DateTime.UtcNow.Month;
            var currentYear = DateTime.UtcNow.Year;

            var totalSubscribers = await _context.companySubscriptions
                .Select(x => x.CompanyId)
                .Distinct()
                .CountAsync();

            var newSubscriptions = await _context.companySubscriptions
                .CountAsync(x =>
                    x.CreatedAt.Month == currentMonth &&
                    x.CreatedAt.Year == currentYear);

            var activeSubscriptions = await _context.companySubscriptions
                .CountAsync(x =>
                    x.IsActive &&
                    x.EndDate > DateTime.UtcNow);

            var cancelledSubscriptions = await _context.companySubscriptions
               .CountAsync(x => !x.IsActive);
            var plans = await _context.subscriptionPlans.Include(x=>x.Subscriptions).Select(x => new SubscriptionPlanResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                ShortDescription = x.ShortDescription,
                MonthlyPrice = x.MonthlyPrice,
                YearlyPrice = x.YearlyPrice,
                MaxJobPostsPerMonth = x.MaxJobPostsPerMonth,
                FeaturedJobPostsPerMonth = x.FeaturedJobPostsPerMonth,
                HasAiToolsAccess = x.HasAiToolsAccess,
                HasCandidateSearch = x.HasCandidateSearch,
                HasPrioritySupport = x.HasPrioritySupport,
                NumberOfUser=x.Subscriptions.Count,
                CreatedAt = x.CreatedAt,
                IsPublished = x.IsPublished
            }).ToListAsync();

            return new SubscriptionDashboardDto
            {
                TotalSubscribers = totalSubscribers,
                NewSubscriptions = newSubscriptions,
                ActiveSubscriptions = activeSubscriptions,
                CancelledSubscriptions = cancelledSubscriptions,
                Plans=plans
                
            };
        }
        public async Task<CompanySubscription?> GetSubscriptionAsync(Guid Id)
        {
            return await _context.companySubscriptions
                .Include(cs => cs.Company)
                  .ThenInclude(cs=>cs.User)
                .Include(cs => cs.SubscriptionPlan)
                .FirstOrDefaultAsync(cs =>
                    cs.Id == Id &&
                    cs.IsActive);
        }

        public async Task<int> GetActiveJobsCountAsync(Guid companyId)
        {
            return await _context.JobPostings
                .CountAsync(j =>
                    j.CompanyID == companyId &&
                    j.IsActive&&j.Status==JobStatus.Approved);
        }

        public async Task<int> GetFeaturedJobsCountAsync(Guid companyId)
        {
            return await _context.JobPostings
                .CountAsync(j =>
                    j.CompanyID == companyId &&
                    j.IsFeatured);
        }
        public async Task<CompanySubscription> GetActiveSubscription(Guid companyId)
        {
            var activeSubscription = await _context.companySubscriptions.Include(x=>x.SubscriptionPlan)
             .FirstOrDefaultAsync(x =>
             x.CompanyId == companyId &&
             x.IsActive);
            return activeSubscription;
        }
        public async Task<SubscriptionFullDetailsDto?> GetFullDetailsBySubscriptionIdAsync(Guid subscriptionId)
        {
            // ── 1. Load current subscription + plan + company ───────────────────────
            var current = await _context.companySubscriptions
                .Include(s => s.SubscriptionPlan)
                .Include(s => s.Company)
                   .ThenInclude(s=>s.User)
                .FirstOrDefaultAsync(s => s.Id == subscriptionId);

            if (current is null) return null;

            var plan = current.SubscriptionPlan;
            var company = current.Company;
            var now = DateTime.UtcNow;

            // ── 2. Subscription history (same company) ──────────────────────────────
            var history = await _context.companySubscriptions
                .Where(s => s.CompanyId == company.CompanyID)
                .Include(s => s.SubscriptionPlan)
                .OrderByDescending(s => s.StartDate)
                .Select(s => new
                {
                    s.Id,
                    PlanName = s.SubscriptionPlan.Name,
                    s.PaidAmount,
                    BillingDate = s.StartDate,
                    s.IsActive,
                    s.EndDate,
                })
                .ToListAsync();

            // ── 3. Usage counters (BASED ON COMPANY, NOT SUBSCRIPTION) ──────────────
           

            DateTime cycleStart;
            DateTime cycleEnd;

            if (current.BillingCycle == BillingCycle.Monthly)
            {
                var monthsPassed =
                    ((now.Year - current.StartDate.Year) * 12) +
                    now.Month - current.StartDate.Month;

                cycleStart = current.StartDate.AddMonths(monthsPassed);

                if (cycleStart > now)
                    cycleStart = cycleStart.AddMonths(-1);

                cycleEnd = cycleStart.AddMonths(1);
            }
            else // Yearly
            {
                var yearsPassed = now.Year - current.StartDate.Year;

                cycleStart = current.StartDate.AddYears(yearsPassed);

                if (cycleStart > now)
                    cycleStart = cycleStart.AddYears(-1);

                cycleEnd = cycleStart.AddYears(1);
            }
            var activeJobsUsed = await _context.JobPostings
             .CountAsync(j =>
            j.CompanyID == company.CompanyID &&
            j.IsActive &&
            
            j.PostedDate >= cycleStart &&
            j.PostedDate < cycleEnd);

            var featuredJobsUsed = await _context.JobPostings
                .CountAsync(j =>
                    j.CompanyID == company.CompanyID &&
                    j.IsFeatured &&
                    j.IsActive &&
                    
                    j.PostedDate >= cycleStart &&
                    j.PostedDate < cycleEnd);

            // ── 4. Build DTO ────────────────────────────────────────────────────────
            return new SubscriptionFullDetailsDto
            {
                // ── Company Info ─────────────────────────────────────────────────────
                Company = new CompanyInfoDto
                {
                    CompanyId = company.CompanyID,
                    CompanyName = company.Name,
                    CompanyEmail = company.User.Email,
                    Industry = company.Industry,
                    Location = company.Location,
                    JoinedAt = company.User.CreatedAt,
                    CompanyLogoUrl = company.LogoUrl,
                },

                // ── Current Subscription ─────────────────────────────────────────────
                CurrentSubscription = new CurrentSubscriptionDto
                {
                    SubscriptionId = current.Id,
                    PlanName = plan.Name,
                    PlanDescription = plan.ShortDescription,
                    Price = current.PaidAmount,
                    BillingCycle = current.BillingCycle.ToString(),
                    StartDate = current.StartDate,
                    EndDate = current.EndDate,
                    DaysLeft = Math.Max(0, (current.EndDate.Value - now).Days),
                    IsActive = current.IsActive,
                    Status = ResolveStatus(!current.IsActive, current.StartDate, current.EndDate, now),
                },

                // ── Usage ────────────────────────────────────────────────────────────
                PlanUsage = new PlanUsageDto
                {
                    ActiveJobs = new UsageItemDto
                    {
                        Used = activeJobsUsed,
                        Limit = plan.MaxJobPostsPerMonth,
                    },
                    FeaturedPosts = new UsageItemDto
                    {
                        Used = featuredJobsUsed,
                        Limit = plan.FeaturedJobPostsPerMonth,
                    },
                    SubscriptionProgress = new UsageItemDto
                    {
                        Used = Math.Max(0, (now - current.StartDate).Days),
                        Limit = Math.Max(1, (current.EndDate.Value - current.StartDate).Days),
                    },
                },

                // ── Features ─────────────────────────────────────────────────────────
                AllowedFeatures = new PlanFeaturesDto
                {
                    ActiveJobPostsLimit = plan.MaxJobPostsPerMonth,
                    FeaturedJobsLimit = plan.FeaturedJobPostsPerMonth,
                    HasAiToolsAccess = plan.HasAiToolsAccess,
                    HasPrioritySupport = plan.HasPrioritySupport,
                    HasCandidateSearch = plan.HasCandidateSearch,
                },

                // ── History ──────────────────────────────────────────────────────────
                SubscriptionHistory = new SubscriptionHistoryDto
                {
                    TotalSubscriptions = history.Count,
                    Records = history.Select((s, index) => new SubscriptionRecordDto
                    {
                        
                        Id = s.Id,
                        PlanName = s.PlanName,
                        Price = s.PaidAmount,
                        BillingDate = s.BillingDate,
                        Status = ResolveStatus(!s.IsActive, default, s.EndDate, now),
                    }).ToList(),
                },
            };
        }

        // ── Private helpers ───────────────────────────────────────────────────────────

        private static string ResolveStatus(bool isActive, DateTime startDate, DateTime? endDate, DateTime now)
        {
            if (isActive) return "Active";
            if (endDate < now) return "Expired";
           
            return "Cancelled";
        }


        public async Task<CompanySubscriptionPageDto?> GetSubscriptionPageAsync(Guid companyId)
        {
            var now = DateTime.UtcNow;


            var current = await _context.companySubscriptions
                .Include(s => s.SubscriptionPlan)
                .Include(s=>s.Company)
                .Where(s => s.CompanyId == companyId && s.IsActive)
                .OrderByDescending(s => s.StartDate)
                .FirstOrDefaultAsync();

            if (current is null) return null;

            var plan = current.SubscriptionPlan;
            var company = current.Company;


            DateTime cycleStart;
            DateTime cycleEnd;

            if (current.BillingCycle == BillingCycle.Monthly)
            {
                var monthsPassed =
                    ((now.Year - current.StartDate.Year) * 12) +
                    now.Month - current.StartDate.Month;

                cycleStart = current.StartDate.AddMonths(monthsPassed);

                if (cycleStart > now)
                    cycleStart = cycleStart.AddMonths(-1);

                cycleEnd = cycleStart.AddMonths(1);
            }
            else // Yearly
            {
                var yearsPassed = now.Year - current.StartDate.Year;

                cycleStart = current.StartDate.AddYears(yearsPassed);

                if (cycleStart > now)
                    cycleStart = cycleStart.AddYears(-1);

                cycleEnd = cycleStart.AddYears(1);
            }
            var activeJobsUsed = await _context.JobPostings
             .CountAsync(j =>
            j.CompanyID == company.CompanyID &&
            j.IsActive &&
            
            j.PostedDate >= cycleStart &&
            j.PostedDate < cycleEnd);

            var featuredJobsUsed = await _context.JobPostings
                .CountAsync(j =>
                    j.CompanyID == company.CompanyID &&
                    j.IsFeatured &&
                    j.IsActive &&
                   
                    j.PostedDate >= cycleStart &&
                    j.PostedDate < cycleEnd);

            var allPlans = await _context.subscriptionPlans
               
                .OrderBy(p => p.MonthlyPrice)
                .Select(p => new AvailablePlanDto
                {
                    PlanId = p.Id,
                    Name = p.Name,
                    Description = p.ShortDescription,
                    MonthlyPrice = p.MonthlyPrice,
                    YearlyPrice = p.YearlyPrice,
                    MaxActiveJobs = p.MaxJobPostsPerMonth,
                    MaxFeaturedJobs = p.FeaturedJobPostsPerMonth,
                    HasAiToolsAccess = p.HasAiToolsAccess,
                    HasStandardSupport = p.HasPrioritySupport,
                    HasCandidateSearch = p.HasCandidateSearch,
                    IsCurrentPlan = p.Id == plan.Id,
                })
                .ToListAsync();

            // ── 4. Billing history (all subscriptions for this company) ───────────────
            var billingRecords = await _context.companySubscriptions
                .Where(s => s.CompanyId == companyId)
                .OrderByDescending(s => s.StartDate)
                .Select(s => new BillingRecordDto
                {
                    SubscriptionId = s.Id,
                    PlanName = s.SubscriptionPlan.Name,
                    Amount = s.PaidAmount,
                    PurchaseDate = s.StartDate,
                    EndDate = s.EndDate,
                    Status = !s.IsActive? "Cancelled"
                                   : s.EndDate < now ? "Expired"
                                   : s.StartDate > now ? "Pending"
                                   : "Active",
                })
                .ToListAsync();

            // ── 5. Assemble ───────────────────────────────────────────────────────────
            return new CompanySubscriptionPageDto
            {
                CurrentPlan = new CurrentPlanDetailsDto
                {
                    SubscriptionId = current.Id,
                    PlanName = plan.Name,
                    BillingCycle = current.BillingCycle.ToString(),
                    RenewalDate = current.EndDate,
                    Status = !current.IsActive? "Cancelled"
                                   : current.EndDate < now ? "Expired"
                                   : "Active",

                    ActiveJobs = new UsageItem
                    {
                        Used = activeJobsUsed,
                        Limit = plan.MaxJobPostsPerMonth,
                    },
                    FeaturedPosts = new UsageItem
                    {
                        Used = featuredJobsUsed,
                        Limit = plan.FeaturedJobPostsPerMonth,
                    },
                    SubscriptionProgress = new UsageItem
                    {
                        Used = Math.Max(0, (now - current.StartDate).Days),
                        Limit = current.EndDate.HasValue
                     ? Math.Max(1, (current.EndDate.Value - current.StartDate).Days)
                     : 0
                    },
                },

                AvailablePlans = allPlans,

                BillingHistory = new BillingHistoryDto
                {
                    TotalTransactions = billingRecords.Count,
                    Records = billingRecords,
                },
            };
        }
    }
}



