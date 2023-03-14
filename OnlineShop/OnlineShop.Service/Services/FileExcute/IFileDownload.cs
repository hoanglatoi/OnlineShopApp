using Microsoft.AspNetCore.WebUtilities;

namespace OnlineShop.Service.Services.FileExcute
{
    public interface IFileDownload
    {
        Task<byte[]> DownloadFile(string path, List<string>? suffixs = null);
    }
}
