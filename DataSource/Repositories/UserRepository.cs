using DataSource.Contracts;
using DataSource.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataSource.Repositories
{
    internal class UserRepository : Repository<TUSERS>, IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(DbContextOptions<AppDbContext> options, ILogger<UserRepository> logger) : base(options)
        {
            this._logger = logger;
        }

        public TUSERS FindByEmail(TUSERS entity)
        {
            return this._dbset.Include(x => x.TUSERSINFO).Where(x => x.EMAIL == entity.EMAIL).FirstOrDefault();
        }

        public TUSERS FindByCellphone(TUSERS entity)
        {
            return this._dbset.Include(x => x.TUSERSINFO).Where(x => x.CELLPHONE == entity.CELLPHONE).FirstOrDefault();
        }

        public TRUSERSTATUSREGISTER GetStatusRegistration(int id, string description)
        {
            var status = this._context.TCREGISTERSTATUS.Where(x => x.DESCRIPTION == description).FirstOrDefault();
            var userstatus = new TRUSERSTATUSREGISTER() { IDUSER = id, IDREGISTERSTATUS = status.ID, CREATED_AT = DateTime.Now };

            return userstatus;
        }

        public bool IsLogin(TUSERS user, string token)
        {
            var userStatus = this.FindByCellphone(user);

            if (userStatus == null)
            {
                return false;
            }

            if (userStatus.TOKEN == null)
            {
                return false;
            }

            return userStatus.TOKEN.Equals(token);
        }

        public Dictionary<string, object> UserInformation(TUSERS email)
        {
            var userInfo = this.FindByCellphone(email);
            string fullname = string.Empty;
            if (userInfo.TUSERSINFO != null)
            {
                fullname = $"{userInfo.TUSERSINFO.NAMES} {userInfo.TUSERSINFO.LASTNAME} {userInfo.TUSERSINFO.LASTNAME2}";
            }
            var cellphonevalidated = (userInfo.CELLPHONE_VALIDATED_AT is null ? false : true);
            var emailvalidated = (userInfo.EMAIL_VALIDATED_AT is null ? false : true);
            var requiredcode = userInfo.REQUIREDCODE;
            var contractsign = userInfo.CONTRACT_SIGN_AT;
            var result = new Dictionary<string, object>();
            result.Add("name", fullname);
            result.Add("email", userInfo.EMAIL ?? String.Empty);
            result.Add("cellphone", userInfo.CELLPHONE);
            result.Add("validatedcellphone", cellphonevalidated);
            result.Add("validatedemail", emailvalidated);
            result.Add("requiredcode", requiredcode);
            result.Add("contractsign", contractsign);

            return result;
        }

        public Dictionary<string, object> UserInfoWebdox(TUSERS user)
        {

            var data = this._dbset.Where(x => x.ID == user.ID).FirstOrDefault();

            if (data != null)
            {
                var fullname = string.Format("{0} {1} {2}", data.TUSERSINFO.NAMES, data.TUSERSINFO.LASTNAME, data.TUSERSINFO.LASTNAME2);
                var entitybirth = data.TUSERSINFO.HOMETOWN;
                var birthday = data.TUSERSINFO.BIRTHDAY;
                var occupation = data.TUSERSINFO.IDOCCUPATIONNavigation.DESCRIPTION;
                var expeditionplace = data.TUSERSINFO.PLACEDOCUMENT;
                var rfc = data.TUSERSINFO.RFC;
                var address1 = data.TUSERSINFO.ADDRESS;
                var cellphone = data.CELLPHONE;
                var email = data.EMAIL;
                var documentnumber = data.TUSERSINFO.NUMBERDOCUMENT;

                var result = new Dictionary<string, object>();

                result.Add("FULLNAME", fullname);
                result.Add("ENTITYBIRTH", entitybirth);
                result.Add("BIRTHDAY", birthday);
                result.Add("OCCUPATION", occupation);
                result.Add("EXPEDITIONPLACE", expeditionplace);
                result.Add("RFC", rfc);
                result.Add("ADDRESS1", address1);
                result.Add("CELLPHONE", cellphone);
                result.Add("EMAIL", email);
                result.Add("DOCUMENTNUMBER", documentnumber);

                return result;
            }

            return new Dictionary<string, object>();
        }

        public async Task <TUSERS> FindByCellphoneOrEmail(TUSERS entity)
        {
            var data = new TUSERS();
            data = await this._dbset.Include(x => x.TUSERSINFO)
                .Where(x => x.CELLPHONE!.Equals(entity.CELLPHONE!)).FirstOrDefaultAsync();
           if(data is null)
                data = await this._dbset.Include(x => x.TUSERSINFO)
                .Where(x => x.EMAIL!.Equals(entity.EMAIL!)).FirstOrDefaultAsync();

            return data!;
            //return await this._dbset.Include(x => x.TUSERSINFO)
                //.Where(x => x.EMAIL!.Equals(entity.EMAIL!) || x.CELLPHONE!.Equals(entity.CELLPHONE!)).FirstOrDefaultAsync();
        }
    }
}
