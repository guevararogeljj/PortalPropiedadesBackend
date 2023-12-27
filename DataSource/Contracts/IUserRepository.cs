using DataSource.Entities;
using DataSource.Interfaces;

namespace DataSource.Contracts
{
    public interface IUserRepository : IRepository<TUSERS>
    {
        TUSERS FindByEmail(TUSERS entity);
        TRUSERSTATUSREGISTER GetStatusRegistration(int id, string description);
        Dictionary<string, object> UserInformation(TUSERS user);
        bool IsLogin(TUSERS user, string token);
        TUSERS FindByCellphone(TUSERS entity);
        Dictionary<string, object> UserInfoWebdox(TUSERS user);
        Task<TUSERS> FindByCellphoneOrEmail(TUSERS entity);
        Task<TUSERS?> GetByCellphoneOrEmail(string? cell, string? email);
    }
}
