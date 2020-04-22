namespace BeHealthy.Services.Data.Users
{
    using System.Threading.Tasks;

    public interface IUserService
    {
        public Task<T> GetUserDetailsAsync<T>(string username);

        public Task<bool> UserExistsAsync(string username);
    }
}
