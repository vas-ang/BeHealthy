namespace BeHealthy.Services.Data.Users
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data.Common.Repositories;
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task<T> GetUserDetailsAsync<T>(string username)
            => await this.usersRepository
                .AllAsNoTracking()
                .Where(x => x.UserName == username)
                .To<T>()
                .FirstOrDefaultAsync();

        public async Task<bool> UserExistsAsync(string username)
            => await this.usersRepository
                .AllAsNoTracking()
                .AnyAsync(x => x.UserName == username);
    }
}
