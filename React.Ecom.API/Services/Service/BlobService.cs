using React.Ecom.API.Services.IService;
using React.Ecom.API.Utility;
namespace React.Ecom.API.Services.Service
{
    public class BlobService : IBlobService
    {
        private readonly string _rootpath;
        private readonly IWebHostEnvironment _environment;

        public BlobService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _rootpath = Path.Combine(_environment.WebRootPath,SD.SD_Storage_Container);
            if (!Directory.Exists(_rootpath))
            {
                Directory.CreateDirectory(_rootpath);
            }
        }
        public async Task<bool> DeleteBlob(string blobName)
        {
            bool isDeleted = false;
            if (!string.IsNullOrEmpty(blobName))
            {
                string filePath = Path.Combine(_rootpath, blobName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    isDeleted = true;
                }
            }
            return isDeleted;
        }

        public async Task<string> GetBlob(string blobName)
        {
            string fileData = string.Empty;
            if (!string.IsNullOrEmpty(blobName))
            {
                string filePath = Path.Combine(_rootpath, blobName);
                if (File.Exists(filePath))
                {
                    var fileByte = File.ReadAllBytes(filePath);
                    fileData = Convert.ToBase64String(fileByte);
                }
            }
            return await Task.FromResult(fileData);
        }

        public async Task<string> UploadBlob(string blobName, IFormFile file)
        {
            if (!string.IsNullOrEmpty(blobName))
            {
                string fileName = Path.GetFileName(blobName);
                using FileStream stream = new(Path.Combine(_rootpath, fileName), FileMode.Create);
                file.CopyTo(stream);
            }
            return await Task.FromResult(blobName);
        }
    }
}
