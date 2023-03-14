using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using System.IO.Compression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace OnlineShop.Service.Services.FileExcute
{
    public class FileUpload : IFileUpload
    {
        private readonly ILogger<FileUpload> _logger;
        private readonly IConfiguration _configuration;
        //private readonly string _rootFolder;

        public FileUpload(ILogger<FileUpload> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            //_rootFolder = configuration.GetSection("Paths:ReportPicture").Value;
        }

        public async Task<bool> BufferUploadFile(string folderName, IFormFile file, List<string>? suffixs = null)
        {
            try
            {
                if (file.Length > 0)
                {
                    //string path = Path.Combine(_rootFolder, folderName);
                    string path = folderName;
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    if (file.FileName.ToLower().Contains(".zip") == false)
                    {
                        _logger.LogInformation("Because {0} is not a .zip file, don't need extract, save file directly", file.FileName);
                        if (suffixs != null && suffixs.Exists(x => file.FileName.ToLower().Contains(x)) == false)
                        {
                            _logger.LogInformation("Can not find any file which have extension in {0}, so skip", suffixs);
                            return false;
                        }

                        using (var fileStream = new FileStream(Path.Combine(path, file.FileName), FileMode.Create))
                        {
                            await file.CopyToAsync(fileStream);
                        }
                    }
                    else
                    {
                        // if file is zip file, extract
                        using (var memoryStream = new MemoryStream())
                        {
                            file.CopyTo(memoryStream);
                            using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
                            {
                                foreach (ZipArchiveEntry entry in archive.Entries)
                                {
                                    if (suffixs != null && suffixs.Exists(x => entry.Name.ToLower().Contains(x)) == false)
                                    {
                                        _logger.LogInformation("{0} have extension is invalid, so skip", entry.Name);
                                        continue;
                                    }

                                    // create folder if folder not existed
                                    var filePath = Path.Combine(path, entry.FullName);
                                    if (Directory.Exists(filePath.Replace(entry.Name, "")) == false)
                                    {
                                        Directory.CreateDirectory(filePath.Replace(entry.Name, ""));
                                    }
                                    // extract
                                    entry.ExtractToFile(Path.Combine(path, entry.FullName), true);
                                }
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    throw new Exception("Size of File is zero");
                    //return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error is occurred when upload file: {0}", ex.Message), ex);
            }
        }

        public async Task<bool> StreamUploadFile(string folderName, MultipartReader reader, MultipartSection? section, CancellationToken ct, List<string>? suffixs = null)
        {
            try
            {
                if (suffixs != null)
                {
                    for (int i = 0; i < suffixs.Count; i++)
                    {
                        suffixs[i] = suffixs[i].ToLower();
                    }
                }

                while (section != null)
                {
                    ct.ThrowIfCancellationRequested();

                    var hasContentDispositionHeader = ContentDispositionHeaderValue.TryParse(
                        section.ContentDisposition, out var contentDisposition
                    );

                    if (hasContentDispositionHeader)
                    {
                        if (contentDisposition != null && contentDisposition.DispositionType.Equals("form-data") &&
                        (!string.IsNullOrEmpty(contentDisposition.FileName.Value) ||
                        !string.IsNullOrEmpty(contentDisposition.FileNameStar.Value)))
                        {
                            //string filePath = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, "UploadedFiles"));
                            //string filePath = Path.Combine(_rootFolder, folderName);
                            string filePath = folderName;
                            byte[] fileArray;
                            using (var memoryStream = new MemoryStream())
                            {
                                await section.Body.CopyToAsync(memoryStream);
                                fileArray = memoryStream.ToArray();


                                if (contentDisposition.FileName.Value.ToLower().Contains(".zip") == false)
                                {
                                    if (suffixs != null && suffixs.Exists(x => contentDisposition.FileName.Value.ToLower().Contains(x)) == false)
                                    {
                                        continue;
                                    }

                                    using (var fileStream = System.IO.File.Create(Path.Combine(filePath, contentDisposition.FileName.Value)))
                                    {
                                        await fileStream.WriteAsync(fileArray);
                                    }
                                }
                                else
                                {
                                    // if file is zip file, extract
                                    using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
                                    {
                                        foreach (ZipArchiveEntry entry in archive.Entries)
                                        {
                                            if (suffixs != null && suffixs.Exists(x => entry.Name.ToLower().Contains(x)) == false)
                                            {
                                                continue;
                                            }

                                            // create folder if folder not existed
                                            var path = Path.Combine(filePath, entry.FullName);
                                            if (Directory.Exists(path.Replace(entry.Name, "")) == false)
                                            {
                                                Directory.CreateDirectory(path.Replace(entry.Name, ""));
                                            }
                                            // extract
                                            entry.ExtractToFile(Path.Combine(filePath, entry.FullName), true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    section = await reader.ReadNextSectionAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error is occurred when upload file: {0}", ex.Message), ex);
            }

            return true;
        }
    }
}
