using Microsoft.AspNetCore.Http;

namespace School.Service.Services._Interfaces
{
    public interface IFileService
    {
        public Task<string> UploadImage(string Location, IFormFile file);
    }
}
