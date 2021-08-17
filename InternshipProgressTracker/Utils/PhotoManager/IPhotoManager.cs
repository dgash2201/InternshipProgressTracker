using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Utils
{
    public interface IPhotoManager
    {
        Task<FileContentResult> GetAsync(string name, CancellationToken cancellationToken = default);
        Task<string> UploadAsync(IFormFile photo, CancellationToken cancellationToken = default);
    }
}
