namespace BlazorApp.Services
{
    public interface IBlobService
    {
        Task<string> UploadFileToBlobAsync(string strFileName, string contecntType, Stream fileStream);
        Task<bool> DeleteFileToBlobAsync(string strFileName);
    }
}
