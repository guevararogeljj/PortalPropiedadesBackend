namespace LeadsInjection.Contracts
{
    public interface ILeadService
    {
        /// <summary>
        /// injection a lead to the portal for process campains in crm
        /// </summary>
        /// <param name="enpoint">url service leads injection</param>
        /// <param name="formid">id create for infra to injetion to leads</param>
        /// <param name="medio">get from catalog crm</param>
        /// <param name="apikey">key for user enpoint leads </param>
        /// <param name="canal">get from catalog crm</param>
        /// <param name="fuente">get from catalog crm</param>
        /// <param name="data"> dictionary with the client information for injection keys [Fullname][Email][Phone] be mandatory</param>
        /// <returns></returns>
        Task<Dictionary<string, object>> LeadInjection(string enpoint, string formid, string medio, string apikey, string canal, string fuente, Dictionary<string, object> data);

        /// <summary>
        /// create the directory with the correct keys for leadsservice
        /// </summary>
        /// <param name="fullname">client name</param>
        /// <param name="phone">client phone number</param>
        /// <param name="email">client email</param>
        /// <returns></returns>
        Dictionary<string, object> ClientInfoModel(string fullname, string phone, string email);
    }
}
