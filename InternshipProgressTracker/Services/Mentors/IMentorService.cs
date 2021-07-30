using InternshipProgressTracker.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Services.Mentors
{
    public interface IMentorService
    {
        Task CreateAsync(User user);
    }
}
