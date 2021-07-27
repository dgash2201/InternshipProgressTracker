using InternshipProgressTracker.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Admins
{
    /// <summary>
    /// Interface for user service
    /// </summary>
    public interface IAdminService
    {
        Task<IReadOnlyCollection<User>> GetAllUsersAsync();

        Task SetUserRoleAsync(int userId, string role);
    }
}
