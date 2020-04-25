namespace BeHealthy.Services.Data.Users
{
    using System.Threading.Tasks;

    public interface IUsersService
    {
        public Task<T> GetUserDetailsAsync<T>(string username);

        public Task<bool> UserExistsAsync(string username);
    }
}
