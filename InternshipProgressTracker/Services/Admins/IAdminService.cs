using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Entities.Enums;
using InternshipProgressTracker.Models.Users;
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
        Task<IReadOnlyCollection<UserResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);

        Task<UserResponseDto> CreateAdminAsync(int userId, CancellationToken cancellationToken = default);

        Task<UserResponseDto> CreateMentorAsync(int userId, MentorRole role, CancellationToken cancellationToken = default);
    }
}
