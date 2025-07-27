using SeliseTaskManager.Application.Interfaces.Common;
using SeliseTaskManager.Application.Interfaces.Models;
using SeliseTaskManager.Application.Interfaces.Services;
using SeliseTaskManager.Domain.User;

namespace SeliseTaskManager.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<UserEntity> _repository;

        public UserService(IRepository<UserEntity> repository)
        {
            _repository = repository;
        }

        public async Task<UserEntity> GetUserAsync(string username, string password)
        {
            var existingUser =
                (await _repository
                .Query(c => c.Email == username && c.Password == password, 
                new PaginationFilter()
                {
                    PageNumber = 1, 
                    PageSize = 1
                })).Item1.FirstOrDefault();

            return existingUser ?? default!;
        }
    }
}
