using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace main_service.Services;

/// <summary>
/// This will be the service that will handle all, image related operations
/// At the time of writing, this service only works locally assuming you are running Azurite.
/// </summary>
public interface IBlobService
{
    Task<Stream> GetImage(string filename);
    Task<(int, string)> UploadImage(string fileName, IFormFile file);
    Task<(int, string)> DeleteImage(string url);
    Task<bool> ImageExists(string filename);
}

public class BlobService : IBlobService
{
    
    private readonly IConfiguration _configuration;

    public BlobService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>Retrieves an image file as a stream from Azure Blob Storage.</summary>
    /// <param name="filename">The name of the image file to retrieve.</param>
    /// <returns> A Task containing a stream of bytes of an image file or a default image stream.</returns>
    public async Task<Stream> GetImage(string filename)
    {
        if (filename == null)
        {
            throw new Exception("Filename is null");
        }

        var containerClient = GetBlobContainerClient();
        try
        {
            var blobClient = containerClient.GetBlobClient(filename);
            var response = await blobClient.DownloadAsync();
            return response.Value.Content;
        }
        catch (Exception e)
        {
            // Instead we return a default image, that is stored locally
            var defaultImage = await File.ReadAllBytesAsync("wwwroot/images/no-image.jpg");
            return new MemoryStream(defaultImage);
        }
    }

    /// <summary>Uploads an image to the specified file name.</summary>
    /// <param name="fileName">The name of the file to upload the image to.</param>
    /// <param name="file">The image file to upload.</param>
    /// <returns>A tuple containing an integer status code and a string message:
    /// - Status code 1 indicates successful upload with the file name.
    /// - Status code 0 indicates failure with an appropriate error message.
    /// </returns>
    public async Task<(int, string)> UploadImage(string fileName, IFormFile file)
    {
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        var fileBytes = memoryStream.ToArray();

        var imageExists = await ImageExists(fileName);
        if (imageExists)
        {
            return (0, "Image already exists");
        }

        var containerClient = GetBlobContainerClient();
        var blobClient = containerClient.GetBlobClient(fileName);
        var blobHttpHeader = new BlobHttpHeaders
        {
            ContentType = "image/png"
        };
        var response = await blobClient.UploadAsync(new MemoryStream(fileBytes), blobHttpHeader);
        if (response.GetRawResponse().Status == 201)
        {
            return (1, fileName);
        }

        return (0, "Image not uploaded");
    }

    
    /// <summary>
    /// Deletes an image from Azure Blob Storage.
    /// </summary>
    /// <param name="url">The name of the file that needs to be deleted.</param>
    /// <returns> A tuple containing an integer status code and a string message:
    /// - Status code 1 indicates successful deletion.
    /// - Status code 0 indicates failure with an appropriate error message.
    /// </returns>
    public async Task<(int, string)> DeleteImage(string url)
    {
        var containerClient = GetBlobContainerClient();
        var blobClient = containerClient.GetBlobClient(url);
        var response = await blobClient.DeleteIfExistsAsync();
        if (response.Value)
        {
            return (1, "Image deleted");
        }

        return (0, "Image not found");
    }
    
    /// <summary>
    /// This will check if an image exists in the blob storage
    /// </summary>
    /// <param name="filename">The name of the file to check for existence.</param>
    /// <returns>A Task containing a boolean value indicating whether the image exists.</returns>
    public async Task<bool> ImageExists(string filename)
    {
        var containerClient = GetBlobContainerClient();
        var blobClient = containerClient.GetBlobClient(filename);
        return await blobClient.ExistsAsync();
    }
    
    /// <summary>
    /// This will get the blob container client
    /// This is done using Azurite or Azure
    /// If the container does not exist, it will be created 
    /// </summary>
    /// <returns> A BlobContainerClient object that represents the container in Azure Blob Storage.</returns>
    private BlobContainerClient GetBlobContainerClient()
    {
        var connectionString = Environment.GetEnvironmentVariable("AZURITE_CONNECTION_STRING") ?? _configuration["Azurite:ConnectionString"]; 
        var containerName = Environment.GetEnvironmentVariable("AZURITE_CONTAINER") ?? _configuration["Azurite:Container"];
        var blobServiceClient = new BlobServiceClient(connectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        containerClient.CreateIfNotExists();
        return containerClient;
    }
}