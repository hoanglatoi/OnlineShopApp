namespace OnlineShop.Service.Services.FileExcute
{
    public interface IFileDelete
    {
        public Task<bool> DeleteFile(string filepath);
        public Task<bool> DeleteFolder(string path);

        public Task<bool> DeleteFileInFolder(string path, List<string>? suffixs = null);
    }
}
