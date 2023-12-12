using BusinessLogic.Models;

namespace BusinessLogic.Contracts
{
    public interface ISignInUserService
    {
        /// <summary>
        /// Validate user credentials to do login
        /// </summary>
        /// <param name="credentials">Data dictionary with user info with keys [Email] and [Password] </param>
        /// <returns>object with success, result, and message</returns>
        Task<Response> SignIn(Dictionary<string, object> credentials);

        /// <summary>
        /// Signout user session to invalidate token
        /// </summary>
        /// <param name="credentials">dictionary with keys [Email]</param>
        /// <returns>object with success, result, and message</returns>
        Task<Response> SignOut(Dictionary<string, object> credentials);

        Task<Response> Profile(Dictionary<string, object> credentials);

        Task<Response> ChangePassword(Dictionary<string, object> credentials);

        Task<Response> ChangeEmail(Dictionary<string, object> credentials);

        Task<Response> UpDoubleAuth(Dictionary<string, object> data);

        Task<Response> ValidateDoubleAuth(Dictionary<string, object> data);

        Task<Response> UpDoubleAuthStatus(Dictionary<string, object> data);

        Task<Response> DownDoubleAuthStatus(Dictionary<string, object> data);

        Task<Response> ChangeCellphone(Dictionary<string, object> data);

        Task<Response> SendChangeCellphoneCodeSms(Dictionary<string, object> user);

        Task<Response> ValidateChangeCellphoneSmsCode(Dictionary<string, object> user);
        Task<Response> ContractInfo(Dictionary<string, object> dictionary);
        Task<Response> NDASignedStatus(Dictionary<string, object> dictionary);
        Task<Response> UpdateDataUser(Dictionary<string, object> data);

        //Task<Response> GenerateContract(Dictionary<string, object> dictionary);
    }
}
