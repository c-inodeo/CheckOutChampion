using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckOutChampion.Services.Interface
{
    public interface IAzureBlobStorageService
    {
        Task DeleteFileAsync(string imageUrl);
        Task<string> UploadFileAsync(Stream fileStream, string filename);
    }
}
