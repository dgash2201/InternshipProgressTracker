using InternshipProgressTracker.Database;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        public AdminService(InternshipProgressTrackerDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets list of all users
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<User>> GetAllUsersAsync()
        {
            var users = await _dbContext
                .Users
                .ToListAsync();

            return users.AsReadOnly();
        }

        /// <summary>
        /// Sets role to user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task SetUserRoleAsync(int userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
            {
                throw new NotFoundException("User with this id was not found");
            }

            var previousRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, previousRoles);
            await _userManager.AddToRoleAsync(user, role);
            await _userManager.UpdateAsync(user);
        }
    }
}
