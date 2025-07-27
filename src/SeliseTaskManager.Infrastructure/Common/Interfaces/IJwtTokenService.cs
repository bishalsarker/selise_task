using SeliseTaskManager.Domain.User;

namespace SeliseTaskManager.Infrastructure.Common.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(Guid userId, string email, UserRoles role);
    }
}
