namespace React.Ecom.API.Services.IService
{
    public interface IBlobService
    {
        Task<string> GetBlob(string blobName);
        Task<bool> DeleteBlob(string blobName);
        Task<string> UploadBlob(string blobName , IFormFile file);
    }
}
