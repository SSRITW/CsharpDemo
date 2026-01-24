using Shared.DTOs;
using Shared.Models;
using System.Security.Principal;

namespace WebApiDemo.Services.Interfaces
{
    public interface IAccountService : IBaseService
    {
        public CommonDTO<AccountModel> Register(AccountModel model);
    }
}
