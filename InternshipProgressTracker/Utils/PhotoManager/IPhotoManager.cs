using Microsoft.AspNetCore.Http;
using Microsoft.Graph;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Utils
{
    /// <summary>
    /// Interface of photo manager
    /// </summary>
    public interface IPhotoManager
    {
        Task<string> UploadAsync(IFormFile photo, CancellationToken cancellationToken = default);
        Task<string> UploadAsync(IProfilePhotoContentRequest photoRequest, CancellationToken cancellationToken = default);
    }
}
