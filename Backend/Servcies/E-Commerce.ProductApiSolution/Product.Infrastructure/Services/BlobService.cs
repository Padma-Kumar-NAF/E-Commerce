using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Product.Application.Interfaces;

namespace Product.Infrastructure.Services;

public class BlobService : IBlobService
{
    private readonly BlobContainerClient _container;

    public BlobService(IConfiguration configuration)
    {
        var connectionString =
            configuration["BlobStorage:ConnectionString"];

        var containerName =
            configuration["BlobStorage:ContainerName"];

        BlobServiceClient client =
            new BlobServiceClient(connectionString);

        _container =
            client.GetBlobContainerClient(containerName);
    }

    public async Task<string> UploadImageAsync(IFormFile file)
    {
        var fileName =
            Guid.NewGuid() + Path.GetExtension(file.FileName);

        var blobClient =
            _container.GetBlobClient(fileName);

        using var stream = file.OpenReadStream();

        await blobClient.UploadAsync(stream);

        return blobClient.Uri.ToString();
    }
}