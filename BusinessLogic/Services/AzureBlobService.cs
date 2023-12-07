using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using BusinessLogic.Contracts;
using DataSource.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Services
{
    internal class AzureBlobService : IAzureBlobService
    {
        private readonly IConfiguration _configuration;
        private BlobContainerClient _containerClient;
        private readonly BlobServiceClient _serviceClient;
        private readonly IParametersRepository _parametersRepository;
        private readonly ILogger<AzureBlobService> _logger;

        public AzureBlobService(IConfiguration configuration, BlobServiceClient serviceClient, IParametersRepository parametersRepository, ILogger<AzureBlobService> logger)
        {
            this._configuration = configuration;
            this._serviceClient = serviceClient;
            this._parametersRepository = parametersRepository;
            this._logger = logger;
        }
        public string GetToken(string container, int hourexpire)
        {
            try
            {
                _containerClient = _serviceClient.GetBlobContainerClient(container);
                if (_containerClient.CanGenerateSasUri)
                {
                    BlobSasBuilder sasBuilder = new BlobSasBuilder()
                    {
                        BlobContainerName = _containerClient.Name,
                        Resource = "c"
                    };

                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(hourexpire);
                    sasBuilder.SetPermissions(BlobContainerSasPermissions.Read);

                    Uri sasUri = _containerClient.GenerateSasUri(sasBuilder);

                    return sasUri.Query;
                }

                return null;

            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<Dictionary<string, object>> UploadFileStream(Stream pFile, string pFileName, string containernamedocuments)
        {
            _containerClient = _serviceClient.GetBlobContainerClient(containernamedocuments);

            BlobClient blobClient = _containerClient.GetBlobClient(pFileName);

            var blobfile = await blobClient.UploadAsync(pFile, true);

            if (blobfile.GetRawResponse().Status == 201)
            {
                var result = new Dictionary<string, object>();
                result.Add("Url", blobClient.Uri.AbsoluteUri);
                result.Add("FullPath", blobClient.Name);

                return result;
            }

            return new Dictionary<string, object>();
        }

        public async Task<Dictionary<string, object>> ImageByPath(string path, string blobcontainer)
        {
            _containerClient = _serviceClient.GetBlobContainerClient(blobcontainer);
            Dictionary<string, object> result = new Dictionary<string, object>();

            var blobClient = _containerClient.GetBlobClient(path);
            using (var stream = new MemoryStream())
            {
                var file = await blobClient.DownloadToAsync(stream, CancellationToken.None);
                result.Add("File", stream.ToArray());
            }

            return result;
        }


        public async Task<string> ImageByPathToBase64(string path, string blobcontainer)
        {
            _containerClient = _serviceClient.GetBlobContainerClient(blobcontainer);//_configuration.GetValue<string>("BlobStorage:ContainerNameImages")
            Dictionary<string, object> result = new Dictionary<string, object>();

            var blobClient = _containerClient.GetBlobClient(path);
            try
            {
                using (var stream = new MemoryStream())
                {
                    var file = await blobClient.DownloadToAsync(stream, CancellationToken.None);
                    var base64 = Convert.ToBase64String(stream.ToArray());
                    return base64;
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Error al tratar de recuperar archivo de blobl storage propiedad: {path}", ex);
                return String.Empty;
            }
        }
    }
}
