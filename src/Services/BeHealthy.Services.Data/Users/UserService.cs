namespace BeHealthy.Services.Data.Users
{
    using System.Linq;
    using System.Threading.Tasks;

    using BeHealthy.Data.Common.Repositories;
    using BeHealthy.Data.Models;
    using BeHealthy.Services.Mapping;
    using Microsoft.EntityFrameworkCore;

    public class UserService : IUserService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UserService(IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.usersRepository = usersRepository;
        }

        public async Task<T> GetUserDetailsAsync<T>(string username)
            => await this.usersRepository
                .AllAsNoTracking()
                .Where(x => x.UserName == username)
                .To<T>()
                .FirstOrDefaultAsync();

        public bool UserExists(string username)
            => this.usersRepository
                .AllAsNoTracking()
                .Any(x => x.UserName == username);
    }
}
