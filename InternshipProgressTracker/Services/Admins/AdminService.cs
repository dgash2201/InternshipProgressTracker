using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Entities.Enums;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Services.Mentors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Admins
{
    /// <summary>
    /// Service for admins
    /// </summary>
    public class AdminService : IAdminService
    {
        private readonly InternshipProgressTrackerDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IMentorService _mentorService;

        public AdminService(InternshipProgressTrackerDbContext dbContext, 
            UserManager<User> userManager, IMentorService mentorService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mentorService = mentorService;
        }

        /// <summary>
        /// Gets list of all users
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<User>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            var users = await _dbContext
                .Users
                .ToListAsync(cancellationToken);

            return users.AsReadOnly();
        }

        /// <summary>
        /// Adds admin role to user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task CreateAdminAsync(int userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new NotFoundException("User with this id was not found");
            }

            var previousRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, previousRoles);
            await _userManager.AddToRoleAsync(user, "Admin");

            if (user.Mentor == null)
            {
                await _mentorService.CreateAsync(user);
            }

            await _userManager.UpdateAsync(user);
        }

        /// <summary>
        /// Creates mentor
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreateMentorAsync(int userId, MentorRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new NotFoundException("User with this id was not found");
            }

            var previousRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, previousRoles);
            await _userManager.AddToRoleAsync(user, role.ToString());

            if (user.Mentor != null)
            {
                throw new BadRequestException("User is already mentor");
            }

            await _mentorService.CreateAsync(user);
            await _userManager.UpdateAsync(user);
        }
    }
}
