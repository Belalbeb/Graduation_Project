using Graduation_Project.Dtos;
using Graduation_Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Graduation_Project.Repositories
{
    public class CouponRepository : Icouponrepository
    {
        private readonly ApplicationDbContext _db;

        public CouponRepository(ApplicationDbContext db)
        {
            _db = db;
        }

 
        public async Task<Coupon> CreateAsync(Coupon coupon)
        {
            _db.Coupons.Add(coupon);
            await _db.SaveChangesAsync();
            return coupon;
        }

        
        public async Task<Coupon?> GetByIdAsync(Guid id)
        {
            return await _db.Coupons
                .Include(x => x.CouponSubscriptionPlans)
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
        }


        public async Task<Coupon?> GetByCodeAsync(string code)
        {
            return await _db.Coupons
                .Include(x => x.CouponSubscriptionPlans)
                .FirstOrDefaultAsync(x =>
                    x.DeletedAt == null &&
                    x.Code == code);
        }

       
        public async Task<bool> ExistsByCodeAsync(string code, Guid? excludeId = null)
        {
            return await _db.Coupons.AnyAsync(x =>
                x.DeletedAt == null &&
                x.Code == code &&
                (!excludeId.HasValue || x.Id != excludeId));
        }

        public async Task<List<Coupon>> GetAllAsync(QueryCouponDto query)
        {
            var q = _db.Coupons
                .Include(x => x.CouponSubscriptionPlans)
                 .ThenInclude(x=>x.SubscriptionPlan)
                .Where(x => x.DeletedAt == null)
                .AsQueryable();

        
            if (!string.IsNullOrEmpty(query.Search))
                q = q.Where(x => x.Code.Contains(query.Search));

            if (query.Percentage.HasValue)
                q = q.Where(x => x.Percentage == query.Percentage.Value);

            
            if (query.SubscriptionPlanId.HasValue)
            {
                q = q.Where(x =>
                 
                    x.CouponSubscriptionPlans.Any(p =>
                        p.SubscriptionPlanId == query.SubscriptionPlanId.Value));
            }

            return await q
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

      
        public async Task<Coupon?> GetValidCouponForPlanAsync(string code, Guid subscriptionPlanId)
        {
            var now = DateTime.UtcNow;

            return await _db.Coupons
                .Include(x => x.CouponSubscriptionPlans)
                .FirstOrDefaultAsync(x =>
                    x.DeletedAt == null &&
                    x.Code == code &&
                    x.IsActive &&
                    x.UsedCount < x.TotalUsageLimit &&
                    (x.ExpiresAt == null || x.ExpiresAt > now) &&
                    (
                        
                        x.CouponSubscriptionPlans.Any(p =>
                            p.SubscriptionPlanId == subscriptionPlanId)
                    ));
        }

      
        public async Task<Coupon> UpdateAsync(Coupon coupon)
        {
            coupon.UpdatedAt = DateTime.UtcNow;

            _db.Coupons.Update(coupon);
            await _db.SaveChangesAsync();

            return coupon;
        }

        public async Task SoftDeleteAsync(Guid id)
        {
            var coupon = await _db.Coupons.FirstOrDefaultAsync(x => x.Id == id);

            if (coupon != null)
            {
                coupon.DeletedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();
            }
        }

        public async Task RestoreAsync(Guid id)
        {
            var coupon = await _db.Coupons.FirstOrDefaultAsync(x => x.Id == id);

            if (coupon != null)
            {
                coupon.DeletedAt = null;
                await _db.SaveChangesAsync();
            }
        }

     
        public async Task IncrementUsageAsync(Guid id)
        {
            await _db.Coupons
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(c => c.UsedCount, c => c.UsedCount + 1));
        }

        public async Task DecrementUsageAsync(Guid id)
        {
            await _db.Coupons
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(c => c.UsedCount, c => c.UsedCount - 1));
        }

     
        public async Task HardDeleteAsync(Guid id)
        {
            var coupon = await _db.Coupons.FirstOrDefaultAsync(x => x.Id == id);

            if (coupon != null)
            {
                _db.Coupons.Remove(coupon);
                await _db.SaveChangesAsync();
            }
        }
    }
}