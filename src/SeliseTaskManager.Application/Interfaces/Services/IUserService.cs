using SeliseTaskManager.Domain.User;

namespace SeliseTaskManager.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserEntity> GetUserAsync(string username, string password);
    }
}
