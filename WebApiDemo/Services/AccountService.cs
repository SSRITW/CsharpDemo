using Shared.Constants;
using Shared.DTOs;
using Shared.Models;
using WebApiDemo.Data;
using WebApiDemo.Services.Interfaces;

namespace WebApiDemo.Services
{
    public class AccountService : BaseService, IAccountService
    {
        public AccountService(AppDbContext db) : base(db)
        {
        }

        /// <summary>
        /// 新規登録
        /// </summary>
        /// <param name="model"></param>
        /// <returns>0:成功、1:unknown 2:既存アカウント</returns>
        public CommonDTO<AccountModel> Register(AccountModel model)
        {
            CommonDTO<AccountModel> result = new CommonDTO<AccountModel>();
            try
            {
                AccountModel data = base.Query<AccountModel>(t => t.Account == model.Account).FirstOrDefault();
                if (data != null)
                {
                    result.Code = ErrorCode.Existing;
                    result.Message = "Account already exists.";
                    return result;
                }
                base.Insert<AccountModel>(model);
                result.Code = ErrorCode.Success;
                result.Data = model;
            }
            catch (Exception ex)
            {
                result.Code = ErrorCode.UnknownError;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
