using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface ITFAService
    {
        Task<Response> ValidateCode(string email, string code, string apptitle);
        Task<Response> GenerateRegisterCode(string email, string code, string apptitle);


    }
}
