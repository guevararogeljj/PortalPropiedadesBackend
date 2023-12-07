namespace BusinessLogic.Contracts
{
    public interface IAzureSasTokenService
    {
        public string GetToken(int hourexpired);
    }
}
