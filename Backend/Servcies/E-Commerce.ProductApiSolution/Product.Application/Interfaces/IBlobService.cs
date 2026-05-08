using Microsoft.AspNetCore.Http;
namespace Product.Application.Interfaces;

public interface IBlobService
{
    Task<string> UploadImageAsync(IFormFile file);
}