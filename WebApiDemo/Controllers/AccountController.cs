using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.DTOs;
using Shared.Models;
using WebApiDemo.Services.Interfaces;

namespace WebApiDemo.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpGet]
        public CommonDTO<AccountDTO> Login(string account, string password)
        {
            CommonDTO<AccountDTO> result = new CommonDTO<AccountDTO>();

            AccountModel accountInfo = _accountService.Query<AccountModel>(t => t.Account == account).FirstOrDefault();
            if (accountInfo == null)
            {
                result.Code = ErrorCode.NotFound;
                result.Message = "アカウントが存在しません";
                return result;
            }
            if (accountInfo.Password != password)
            {
                result.Code = ErrorCode.PasswordIncorrect;
                result.Message = "パスワードが間違っています";
                return result;
            }
            result.Code = ErrorCode.Success;
            result.Message = "ログインしました";
            result.Data = _mapper.Map<AccountModel, AccountDTO>(accountInfo);
            return result;
        }

        /// <summary>
        /// 注册新账号。
        /// </summary>
        /// <param name="accountInfo">账号信息。</param>
        /// <returns>注册结果。</returns>
        [HttpPost]
        public CommonDTO<Object> Register(AccountDTO accountInfo)
        {
            AccountModel accountModel = _mapper.Map<AccountDTO, AccountModel>(accountInfo);
            CommonDTO<AccountModel> resultInfo = _accountService.Register(accountModel);
            CommonDTO<Object> result = new CommonDTO<Object>();
            result.Code = ErrorCode.Success;
            result.Message = resultInfo.Message;
            return result;
        }
    }
}
