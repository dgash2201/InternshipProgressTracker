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
        /// Uploads photo to the Azure Blob Storage
        /// </summary>
        /// <returns>Url of photo in the Azure Blob Storage</returns>
        public async Task<string> UploadAsync(IFormFile photo, CancellationToken cancellationToken = default)
        {
            await using var photoStream = photo.OpenReadStream();
            return await UploadAsync(photoStream, photo.ContentType, cancellationToken);
        }

        /// <summary>
        /// Get Azure user photo and upload it to Azure Blob Storage
        /// </summary>
        /// <param name="photoRequest">Microsoft Graph API request to get photo content</param>
        /// <returns>Url of photo in the Azure Blob Storage</returns>
        public async Task<string> UploadAsync(IProfilePhotoContentRequest photoRequest, CancellationToken cancellationToken = default)
        {
            await using var photoStream = await photoRequest.GetAsync();
            return await UploadAsync(photoStream, photoRequest.ContentType, cancellationToken);
        }

        private async Task<string> UploadAsync(Stream photoStream, string contentType, CancellationToken cancellationToken = default)
        {
            var photoId = Guid.NewGuid().ToString();
            var blob = _blobContainer.GetBlobClient(photoId);

            await blob.UploadAsync(
                photoStream,
                new BlobHttpHeaders { ContentType = contentType },
                cancellationToken: cancellationToken);

            return blob.Uri.AbsoluteUri;
        }
    }
}
