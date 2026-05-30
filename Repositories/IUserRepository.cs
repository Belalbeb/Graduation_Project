namespace Graduation_Project.Repositories
{
    public interface IUserRepository
    {
        Task<int> GetUserCountAsync();
        Task<int> GetTotalUsersCountAsync();
    }
}
