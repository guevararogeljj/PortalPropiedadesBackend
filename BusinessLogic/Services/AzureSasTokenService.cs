using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using BusinessLogic.Contracts;
using Microsoft.Extensions.Configuration;

namespace BusinessLogic.Services
{
    internal class AzureSasTokenService : IAzureSasTokenService
    {
        private readonly IConfiguration _configuration;
        private readonly BlobContainerClient _containerClient;

        public AzureSasTokenService(IConfiguration configuration, BlobContainerClient containerClient)
        {
            this._configuration = configuration;
            this._containerClient = containerClient;
        }
        public string GetToken(int timeexpired)
        {
            try
            {
                if (_containerClient.CanGenerateSasUri)
                {
                    BlobSasBuilder sasBuilder = new BlobSasBuilder()
                    {
                        BlobContainerName = _containerClient.Name,
                        Resource = "c"
                    };

                    sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(timeexpired);
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
    }
}
