namespace BusinessLogic.Contracts
{
    public interface IAzureBlobService
    {
        public string GetToken(string container, int hourexpire);

        Task<Dictionary<string, object>> UploadFileStream(Stream pFile, string pFileName, string container);
        Task<Dictionary<string, object>> ImageByPath(string path, string container);
        Task<string> ImageByPathToBase64(string path, string container);
    }
}
