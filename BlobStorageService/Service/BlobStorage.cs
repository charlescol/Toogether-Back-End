using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorageService.Service
{
    public interface IBlobStorage
    {
        public Task<string> StoreImageInBlobServiceStorage(byte[] data, string imgNameWithExt, string containerName);
    }
    public class BlobStorage : IBlobStorage
    {
        public async Task<string> StoreImageInBlobServiceStorage(byte[] data, string imgNameWithExt, string containerName)
        {
            string blobServiceName = Environment.GetEnvironmentVariable("BLOB_SERVICE_NAME");
            string blobServiceKey = Environment.GetEnvironmentVariable("BLOB_SERVICE_KEY");
            StorageCredentials creden = new StorageCredentials(blobServiceName, blobServiceKey);
            CloudStorageAccount acc = new CloudStorageAccount(creden, useHttps: true);
            CloudBlobClient client = acc.CreateCloudBlobClient();
            string blobServiceEndPoint = Environment.GetEnvironmentVariable("BLOB_SERVICE_ENDPOINT");

            CloudBlobContainer cont = new CloudBlobContainer(new Uri(blobServiceEndPoint + containerName), creden);
            await cont.CreateIfNotExistsAsync();
            await cont.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            CloudBlockBlob cblob = cont.GetBlockBlobReference(imgNameWithExt);

            using (var stream = new MemoryStream(data, writable: false))
            {
                cblob.Properties.ContentType = "image/" + Path.GetExtension(imgNameWithExt).Remove(0, 1);
                await cblob.UploadFromStreamAsync(stream);
            }
            return cblob.Uri.AbsoluteUri;
        }
    }
}
