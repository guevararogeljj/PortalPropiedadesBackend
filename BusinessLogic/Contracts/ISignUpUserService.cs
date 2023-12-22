using BusinessLogic.Models;

namespace BusinessLogic.Contracts
{
    public interface ISignUpUserService
    {
        /// <summary>
        /// Insert new client user to database
        /// </summary>
        /// <param name="attributes">Data dictionary with keys [Name],[Lastname], [Password], [Email], [Cellphone]</param>
        /// <returns>A boolean that represents it was successfully or not</returns>
        Response AddNewUser(Dictionary<string, object> user);

        /// <summary>
        /// Send sms notification to user by cell phone previously  registered
        /// </summary>
        /// <param name="attributes">Data dictionary with user info with keys [Email]</param>
        /// <returns>A boolean that represents it was successfully or not</returns>
        Task<Object> SendCodeSms(Dictionary<string, object> user);

        Task<Response> SendCodeEmail(Dictionary<string, object> data);

        /// <summary>
        /// Validate the cell phone code send it in the registration phase
        /// </summary>
        /// <param name="attributes">Data dictionary with keys [Email], [Code]</param>
        /// <returns>A boolean that represents it was successfully or not</returns>
        Task<Object> ValidateSmsCode(Dictionary<string, object> user);

        Task<Response> ValidateCodeEmail(Dictionary<string, object> user);

        /// <summary>
        /// Returns the register status user more recently
        /// </summary>
        /// <param name="attributes">Data dictionary with keys [Email]</param>
        /// <returns>last register status user</returns>
        object StatusRegisterUser(Dictionary<string, object> user);

        /// <summary>
        /// Validate and create new user information record
        /// </summary>
        /// <param name="files">List of dictionary with keys [EMail], [FileBase64], [FileName], [Size]</param>
        /// <returns></returns>
        Task<object> ValidateIdentification(IEnumerable<Dictionary<string, object>> files);

        /// <summary>
        /// Get user informacion previosly registered
        /// </summary>
        /// <param name="data">dictionary witk key [Email]</param>
        /// <returns></returns>
        Task<Object> UserInformation(Dictionary<string, object> data);

        /// <summary>
        /// Update the field terms of user information
        /// </summary>
        /// <param name="data">dictionaty with key [Email], [Terms]</param>
        /// <returns></returns>
        Task<Object> UpdateUserInformation(Dictionary<string, object> data);

        //Task<Response> EmailValidate(Dictionary<string, object> data);

        Task<Response> PasswordRecovery(Dictionary<string, object> data);

        Task<Response> PasswordRecoveryValidate(Dictionary<string, object> data);

        Task<Response> UpdateWebdoxRestration(string jsonresponse);
    }
}
