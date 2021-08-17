using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Utils
{
    public class PhotoManager : IPhotoManager
    {
        private readonly BlobContainerClient _blobContainer;

        public PhotoManager(BlobServiceClient blobService)
        {
            _blobContainer = blobService.GetBlobContainerClient("photocontainer");
        }

        public async Task<FileContentResult> GetAsync(string photoName, CancellationToken cancellationToken = default)
        {
            var blob = _blobContainer.GetBlobClient(photoName);

            await using var memoryStream = new MemoryStream();
            await blob.DownloadToAsync(memoryStream, cancellationToken);
            var properties = await blob.GetPropertiesAsync(cancellationToken: cancellationToken);

            return new FileContentResult(memoryStream.ToArray(), properties.Value.ContentType);
        }

        public async Task<string> UploadAsync(IFormFile photo, CancellationToken cancellationToken = default)
        {
            var name = photo.FileName + Guid.NewGuid().ToString();
            var blob = _blobContainer.GetBlobClient(name);

            await using var readStream = photo.OpenReadStream();
            var response = await blob.UploadAsync(readStream, new BlobHttpHeaders { ContentType = photo.ContentType }, cancellationToken: cancellationToken);

            return name;
        }
    }
}
