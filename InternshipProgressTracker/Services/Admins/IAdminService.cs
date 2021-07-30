using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Entities.Enums;
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

        Task CreateAdminAsync(int userId);

        Task CreateMentorAsync(int userId, MentorRole role);
    }
}
