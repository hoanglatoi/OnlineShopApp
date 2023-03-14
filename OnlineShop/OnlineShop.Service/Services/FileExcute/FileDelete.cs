using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.IO.Compression;

namespace OnlineShop.Service.Services.FileExcute
{
    public class FileDelete : IFileDelete
    {
        private readonly ILogger<FileDelete> _logger;
        private readonly IConfiguration _configuration;

        public FileDelete(ILogger<FileDelete> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<bool> DeleteFile(string filepath)
        {
            bool ret = true;
            if (System.IO.File.Exists(filepath) == false)
            {
                //throw new DirectoryNotFoundException(String.Format("「Filepath:{0}」 not found", filepath));
                _logger.LogInformation("{0} is not existed, return true", filepath);
                return true;
            }
            var file = new FileInfo(filepath);
            file.Delete();
            _logger.LogInformation("Delete file:{0} is succeed", filepath);
            return ret;
        }

        public async Task<bool> DeleteFolder(string path)
        {
            bool ret = true;
            if (System.IO.Directory.Exists(path) == false)
            {
                //throw new DirectoryNotFoundException(String.Format("「Folder:{0}」 not found", path));
                _logger.LogInformation("{0} is not existed, return true", path);
                return true;
            }
            var folder = new DirectoryInfo(path);
            folder.Delete(true);
            _logger.LogInformation("Delete folder:{0} is succeed", path);
            return ret;
        }

        public async Task<bool> DeleteFileInFolder(string path, List<string>? suffixs = null)
        {
            bool ret = true;
            if (System.IO.Directory.Exists(path) == false)
            {
                //throw new DirectoryNotFoundException(String.Format("「Folder:{0}」 not found", path));
                _logger.LogInformation("{0} is not existed, return true", path);
                return true;
            }
            ret = await DeleteFile(path, suffixs);
            _logger.LogInformation("Delete file in folder:{0} is succeed", path);
            return ret;
        }

        private async Task<bool> DeleteFile(string fullPath, List<string>? suffixs = null)
        {
            if (System.IO.Directory.Exists(fullPath) == false)
            {
                //throw new DirectoryNotFoundException(String.Format("「Folder:{0}」not found", fullPath));
                return true;
            }

            bool ret = true;
            DirectoryInfo dir = new DirectoryInfo(fullPath);
            var dirList = dir.GetDirectories().ToList();
            foreach (var dirItem in dirList)
            {
                var dirPath = dirItem.FullName;
                ret = await DeleteFile(fullPath, suffixs);
                if (ret == false)
                {
                    return false;
                }
            }

            var files = dir.GetFiles();
            foreach (var file in files)
            {
                if (suffixs != null && suffixs.Exists(x => file.FullName.Contains(x)) == false)
                {
                    continue;
                }
                file.Delete();
            }
            return ret;
        }
    }
}
