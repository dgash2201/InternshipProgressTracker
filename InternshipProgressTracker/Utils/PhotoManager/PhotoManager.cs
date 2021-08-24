using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Utils
{
    /// <summary>
    /// Works with user avatars
    /// </summary>
    public class PhotoManager : IPhotoManager
    {
        private readonly BlobContainerClient _blobContainer;

        public PhotoManager(BlobServiceClient blobService)
        {
            _blobContainer = blobService.GetBlobContainerClient("photocontainer");
        }

        /// <summary>
        /// Gets photo from Azure Blob Storage
        /// </summary>
        public async Task<FileContentResult> GetAsync(string photoId, CancellationToken cancellationToken = default)
        {
            var blob = _blobContainer.GetBlobClient(photoId);

            await using var memoryStream = new MemoryStream();
            await blob.DownloadToAsync(memoryStream, cancellationToken);
            var properties = await blob.GetPropertiesAsync(cancellationToken: cancellationToken);

            return new FileContentResult(memoryStream.ToArray(), properties.Value.ContentType)
            {
                FileDownloadName = properties.Value.Metadata["photoName"]
            };
        }

        /// <summary>
        /// Uploads photo to the Azure Blob Storage
        /// </summary>
        /// <returns>Unique Id of photo in the Azure Blob Storage</returns>
        public async Task<string> UploadAsync(IFormFile photo, CancellationToken cancellationToken = default)
        {
            var photoId = Guid.NewGuid().ToString();
            var blob = _blobContainer.GetBlobClient(photoId);

            await using var readStream = photo.OpenReadStream();
            var response = await blob.UploadAsync(
                readStream, 
                new BlobHttpHeaders { ContentType = photo.ContentType },
                new Dictionary<string, string> { ["photoName"] = photo.FileName },
                cancellationToken: cancellationToken);

            return photoId;
        }

        /// <summary>
        /// Get Azure user photo and upload it to Azure Blob Storage
        /// </summary>
        /// <param name="photoRequest">Microsoft Graph API request to get photo content</param>
        /// <returns>Unique Id of photo in the Azure Blob Storage</returns>
        public async Task<string> UploadAsync(IProfilePhotoContentRequest photoRequest, CancellationToken cancellationToken = default)
        {
            using var photoStream = await photoRequest.GetAsync();

            if (photoStream == null) return null;

            var photoId = Guid.NewGuid().ToString();
            var blob = _blobContainer.GetBlobClient(photoId);

            var response = await blob.UploadAsync(
                photoStream,
                cancellationToken: cancellationToken);

            return photoId;
        }
    }
}
