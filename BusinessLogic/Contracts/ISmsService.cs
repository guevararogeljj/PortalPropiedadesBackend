namespace BusinessLogic.Contracts
{
    public interface ISmsService
    {
        Task<Dictionary<string, object>> SendSmsOtp(string cellphone);

        Task<Dictionary<string, object>> ValidateSmsOtp(string cellphone, string code);
    }
}
