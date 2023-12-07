namespace BusinessLogic.Contracts
{
    public interface IIsiService
    {
        Task<bool> StatusSoldByCredit(string credit, string token, List<string> soldstatus);
        Task<string> GenerateToken();
    }
}
