using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.IO.Compression;

namespace OnlineShop.Service.Services.FileExcute
{
    public class FileDownload : IFileDownload
    {
        private readonly ILogger<FileDownload> _logger;
        private readonly IConfiguration _configuration;

        public FileDownload(ILogger<FileDownload> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        private void GetPathInfo(string fullPath, string outputPath, ref List<Dictionary<string, string>> infos, List<string>? suffixs = null)
        {
            if (System.IO.Directory.Exists(fullPath))
            {
                DirectoryInfo dir = new DirectoryInfo(fullPath);
                var dirList = dir.GetDirectories().ToList();
                foreach (var dirItem in dirList)
                {
                    GetPathInfo(Path.Combine(fullPath, dirItem.Name), Path.Combine(outputPath, dirItem.Name), ref infos, suffixs);
                }

                var files = dir.GetFiles();
                foreach (var file in files)
                {
                    if (suffixs != null && suffixs.Exists(x => file.FullName.Contains(x)) == false)
                    {
                        continue;
                    }
                    var _outputPath = Path.Combine(outputPath, file.Name);
                    var _fullPath = Path.Combine(fullPath, file.Name);
                    //infos.Add(new Dictionary<string, string>() { { _outputPath, _fullPath } });
                    infos.Add(new Dictionary<string, string>() { { "fullPath", _fullPath }, { "outputPath", _outputPath } });
                }
            }
            else
            {
                if (System.IO.File.Exists(fullPath))
                {
                    if (suffixs != null && suffixs.Exists(x => fullPath.Contains(x)) == false)
                    {
                        return;
                    }
                    infos.Add(new Dictionary<string, string>() { { "fullPath", fullPath }, { "outputPath", outputPath } });
                }
                return;
            }
        }

        public async Task<byte[]> DownloadFile(string path, List<string>? suffixs = null)
        {
            try
            {
                List<Dictionary<string, string>> infos = new List<Dictionary<string, string>>();
                if (System.IO.File.Exists(path))
                {
                    _logger.LogInformation("{0} is file", path);
                    GetPathInfo(path, "DownloadFile",ref infos, suffixs);                   
                }
                else if (System.IO.Directory.Exists(path))
                {
                    _logger.LogInformation("{0} is directory", path);
                    DirectoryInfo dir = new DirectoryInfo(path);
                    GetPathInfo(path, dir.Name, ref infos, suffixs);
                }
                else
                {
                    throw new FileNotFoundException(String.Format("File not found"));
                }
                using (var ms = new MemoryStream())
                {
                    using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, true))
                    {
                        foreach(var info in infos)
                        {
                            var fileName = info["outputPath"];
                            byte[] buffer = System.IO.File.ReadAllBytes(info["fullPath"]);

                            var zipEntry = archive.CreateEntry(fileName, CompressionLevel.Fastest);
                            using (var zipStream = zipEntry.Open())
                            {
                                await zipStream.WriteAsync(buffer, 0, buffer.Length);
                            }
                        }
                    }
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error is occurred when upload file: {0}", ex.Message), ex);
            }
            //return null;
        }
    }
}
