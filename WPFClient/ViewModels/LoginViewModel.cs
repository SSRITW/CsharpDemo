using RestSharp;
using Shared.Constants;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFClient.HttpClients;
using WPFClient.MsgEvents;

namespace WPFClient.ViewModels
{
    public class LoginViewModel : BindableBase, IDialogAware
    {
        private readonly HttpRestClient HttpClient;
        public string Title { get; set; } = "Login";
        public DialogCloseListener RequestClose { get; set; }

        private readonly IEventAggregator Aggregator;

        public DelegateCommand LoginCmm { get; set; }

        public DelegateCommand RegisterCmm { get; set; }

        private AccountDTO _AccountInfo;

        public AccountDTO AccountInfo
        {
            get { return _AccountInfo; }
            set { 
                _AccountInfo = value; 
                RaisePropertyChanged();
            }
        }

        private int _SelectedIndex;

        public int SelectedIndex
        {
            get { return _SelectedIndex; }
            set { 
                _SelectedIndex = value; 
                RaisePropertyChanged();
            }
        }
        public LoginViewModel(HttpRestClient httpRestClient, IEventAggregator aggregator)
        {
            LoginCmm = new DelegateCommand(LoginExecute);
            SwitchInfoCmm = new DelegateCommand(SwitchInfo);
            RegisterCmm = new DelegateCommand(RegisterExecute);
            AccountInfo = new AccountDTO();
            HttpClient = httpRestClient;
            Aggregator = aggregator;
        }

        private void LoginExecute()
        {
            if (string.IsNullOrEmpty(Account)|| string.IsNullOrEmpty(Pwd)) 
            {
                Aggregator.GetEvent<MsgEvent>().Publish("必須項目ご入力ください");
                return; 
            }
            string password = Md5Hepler.GetMd5(Pwd);
            ApiRequest apiRequest = new ApiRequest()
            {
                Route = $"Account/Login?account={Account}&password={password}",
                Method = Method.Get,
                ContentType = "application/json"
            };
            CommonDTO<AccountDTO> respone = HttpClient.Execute<AccountDTO>(apiRequest);
            if (respone.Code == ErrorCode.Success)
            {
                Aggregator.GetEvent<MsgEvent>().Publish("ログインしました");
                RequestClose.Invoke(new DialogResult(ButtonResult.OK));
            }
            else
            {
                Aggregator.GetEvent<MsgEvent>().Publish($"Error: {respone.Message}");
            }
        }

        private void RegisterExecute()
        {
            if (string.IsNullOrEmpty(AccountInfo.Nickname) 
                || string.IsNullOrEmpty(AccountInfo.Password) 
                || string.IsNullOrEmpty(AccountInfo.Account))
            {
                Aggregator.GetEvent<MsgEvent>().Publish("必須項目ご入力ください");
                return;
            }
            if (AccountInfo.Password != Pwd)
            {
                Aggregator.GetEvent<MsgEvent>().Publish("パスワードが一致しません");
                return;
            }
            AccountInfo.Password = Md5Hepler.GetMd5(AccountInfo.Password);
            ApiRequest apiRequest = new ApiRequest()
            {
                Route = "Account/Register",
                Method = Method.Post,
                Parameters = AccountInfo,
                ContentType = "application/json"
            };
            CommonDTO<object> respone = HttpClient.Execute<object>(apiRequest);
            if (respone.Code == ErrorCode.Success)
            {
                Aggregator.GetEvent<MsgEvent>().Publish("新規登録が完了しました");
                SelectedIndex = 0;
            }
            else
            {
                Aggregator.GetEvent<MsgEvent>().Publish($"Error: {respone.Message}");
            }
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
        }

        public DelegateCommand SwitchInfoCmm { get; set; }

        private void SwitchInfo()
        {
            SelectedIndex = SelectedIndex == 0 ? 1:0;
            Pwd = string.Empty;
        }


        private string _Account;

        public string Account
        {
            get { return _Account; }
            set { _Account = value; RaisePropertyChanged(); }
        }

        private string _Pwd;
        public string Pwd
        {
            get { return _Pwd; }
            set
            {
                _Pwd = value;
                RaisePropertyChanged();
            }
        }
    }
}
