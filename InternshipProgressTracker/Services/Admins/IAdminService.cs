using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Entities.Enums;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Admins
{
    /// <summary>
    /// Interface for user service
    /// </summary>
    public interface IAdminService
    {
        Task<IReadOnlyCollection<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);

        Task CreateAdminAsync(int userId, CancellationToken cancellationToken = default);

        Task CreateMentorAsync(int userId, MentorRole role, CancellationToken cancellationToken = default);
    }
}
