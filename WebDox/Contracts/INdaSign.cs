using WebDox.Models;

namespace WebDox.Contracts
{
    public interface INdaSign
    {
        Task<Response> Authentication();
        Task<Response> NewRequest(FormRequest data);
        Task<Response> AddSigner(Signer signer);
        Task<Response> InitiateSignatures(InitiateSigners data);
    }
}
