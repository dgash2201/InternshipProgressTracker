using AutoMapper;
using AutoMapper.QueryableExtensions;
using InternshipProgressTracker.Entities;
using InternshipProgressTracker.Entities.Enums;
using InternshipProgressTracker.Exceptions;
using InternshipProgressTracker.Models.Users;
using InternshipProgressTracker.Services.Mentors;
using InternshipProgressTracker.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Admins
{
    /// <summary>
    /// Service for admins
    /// </summary>
    public class AdminService : IAdminService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMentorService _mentorService;
        private readonly IPhotoManager _photoManager;
        private readonly IMapper _mapper;

        public AdminService(
            UserManager<User> userManager,
            IMentorService mentorService,
            IPhotoManager photoManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mentorService = mentorService;
            _photoManager = photoManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets list of all users
        /// </summary>
        /// <returns></returns>
        public async Task<IReadOnlyCollection<UserResponseDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
        {
            var users = await _userManager
                .Users
                .Include(u => u.Roles)
                .Include(u => u.Student)
                .ThenInclude(s => s.StudyPlanProgresses)
                .Include(u => u.Mentor)
                .ThenInclude(m => m.StudentStudyPlanProgresses)
                .ProjectTo<UserResponseDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            return users.AsReadOnly();
        }

        /// <summary>
        /// Adds admin role to user
        /// </summary>
        public async Task<UserResponseDto> CreateAdminAsync(int userId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager
                .Users
                .Include(u => u.Roles)
                .Include(u => u.Student)
                .ThenInclude(s => s.StudyPlanProgresses)
                .Include(u => u.Mentor)
                .ThenInclude(m => m.StudentStudyPlanProgresses)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

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

            return _mapper.Map<UserResponseDto>(user);
        }

        /// <summary>
        /// Creates mentor
        /// </summary>
        public async Task<UserResponseDto> CreateMentorAsync(int userId, MentorRole role, CancellationToken cancellationToken = default)
        {
            var user = await _userManager
                .Users
                .Include(u => u.Roles)
                .Include(u => u.Student)
                .ThenInclude(s => s.StudyPlanProgresses)
                .Include(u => u.Mentor)
                .ThenInclude(m => m.StudentStudyPlanProgresses)
                .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);

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

            return _mapper.Map<UserResponseDto>(user);
        }
    }
}
