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
                .FirstOrDefaultAsync(x => x.Id == id && x.DeletedAt == null);
        }

        public async Task<bool> ExistsByCodeAsync(string code, Guid? excludeId = null)
        {
            return await _db.Coupons.AnyAsync(x =>
                x.DeletedAt == null &&
                x.Code == code &&
                (!excludeId.HasValue || x.Id != excludeId));
        }

        public async Task<PaginatedResult<Coupon>> GetAllAsync(QueryCouponDto query)
        {
            var q = _db.Coupons.Where(x => x.DeletedAt == null);

            if (!string.IsNullOrEmpty(query.Search))
                q = q.Where(x => x.Code.Contains(query.Search.ToUpper()));

            if (query.Percentage.HasValue)
                q = q.Where(x => x.Percentage == query.Percentage.Value);

            if (query.Plan.HasValue)
                q = q.Where(x => x.ApplicablePlans == null ||
                                 x.ApplicablePlans.Contains(query.Plan.Value));

            if (query.Status.HasValue)
                q = ApplyStatusFilter(q, query.Status.Value);

            var total = await q.CountAsync();

            var data = await q
                .OrderByDescending(x => x.CreatedAt)
                .Skip((query.Page - 1) * query.Limit)
                .Take(query.Limit)
                .ToListAsync();

            return new PaginatedResult<Coupon>
            {
                Data = data,
                Total = total,
                Page = query.Page,
                Limit = query.Limit,
                TotalPages = (int)Math.Ceiling((double)total / query.Limit)
            };
        }

        public async Task<Coupon?> GetValidCouponForPlanAsync(string code, Coupon.ApplicablePlan plan)
        {
            var now = DateTime.UtcNow;

            return await _db.Coupons.FirstOrDefaultAsync(x =>
                x.DeletedAt == null &&
                x.Code == code &&
                x.IsActive &&
                x.UsedCount < x.TotalUsageLimit &&
                (x.ExpiresAt == null || x.ExpiresAt > now) &&
                (x.ApplicablePlans == null || x.ApplicablePlans.Contains(plan)));
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

        public async Task IncrementUsageAsync(Guid id)
        {
            await _db.Coupons
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x =>
                    x.SetProperty(c => c.UsedCount, c => c.UsedCount + 1));
        }

        private static IQueryable<Coupon> ApplyStatusFilter(IQueryable<Coupon> q, Coupon.CouponStatus status)
        {
            var now = DateTime.UtcNow;

            return status switch
            {
                Coupon.CouponStatus.Active => q.Where(x =>
                    x.IsActive &&
                    x.UsedCount < x.TotalUsageLimit &&
                    (x.ExpiresAt == null || x.ExpiresAt > now)),

                Coupon.CouponStatus.Inactive => q.Where(x => !x.IsActive),

                Coupon.CouponStatus.Expired => q.Where(x =>
                    x.ExpiresAt != null && x.ExpiresAt <= now),

                _ => q
            };
        }

        public Task<Coupon?> GetByCodeAsync(string code)
        {
            throw new NotImplementedException();
        }

        public Task DecrementUsageAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task RestoreAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task HardDeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}