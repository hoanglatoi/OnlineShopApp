using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace OnlineShop.Service.Services.FileExcute
{
    public interface IFileUpload
    {
        Task<bool> BufferUploadFile(string folderName, IFormFile file, List<string>? suffixs = null);
        Task<bool> StreamUploadFile(string folderName, MultipartReader reader, MultipartSection section, CancellationToken ct, List<string>? _suffixs = null);
    }
}
