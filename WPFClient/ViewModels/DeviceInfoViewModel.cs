using RestSharp;
using Shared.Constants;
using Shared.DTOs;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.DTOs;
using WPFClient.HttpClients;
using WPFClient.Models;
using WPFClient.MsgEvents;

namespace WPFClient.ViewModels
{
    public class DeviceInfoViewModel: BindableBase,IDialogAware
    {
        private readonly HttpRestClient HttpClient;

        private readonly IEventAggregator Aggregator;

        private DeviceModel _DeviceInfo;
        public DeviceModel DeviceInfo
        {
            get { return _DeviceInfo; }
            set { _DeviceInfo = value; RaisePropertyChanged(); }
        }

        public DeviceInfoViewModel(HttpRestClient httpRestClient, IEventAggregator aggregator) 
        {
            DeviceInfo = new DeviceModel();
            HttpClient = httpRestClient;
            Aggregator = aggregator;
            SubmitCmm = new DelegateCommand(SubmitForm);
        }

        public List<StatusOption> DeviceTypeOptions { get; set; } = new()
        {
            new StatusOption { Name = "モニター", Value = 1 },
            new StatusOption { Name = "マウス", Value = 2 },
            new StatusOption { Name = "キーボード", Value = 3 },
        };

        public List<StatusOption> StatusOptions { get; set; } = new()
        {
            new StatusOption { Name = "非アクティブ", Value = 1 },
            new StatusOption { Name = "アクティブ", Value = 2 },
            new StatusOption { Name = "メンテナンス", Value = 3 },
            new StatusOption { Name = "廃止", Value = 4 }
        };

        public DialogCloseListener RequestClose { get; set; }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.TryGetValue<DeviceModel>("DeviceInfo", out var deviceModel) && deviceModel != null)
            {
                DeviceInfo = deviceModel;
            }
        }

        public DelegateCommand SubmitCmm { get; set; }

        private void SubmitForm()
        {
            if (DeviceInfo.Status == 0 || DeviceInfo.DeviceType == 0  
                || DeviceInfo.DeviceName == "" || DeviceInfo.DeviceId == "")
            {
                Aggregator.GetEvent<MsgEvent>().Publish("必須項目ご入力ください");
                return;
            }
            ApiRequest apiRequest = new ApiRequest()
            {
                Route = $"Device/insert",
                Method = Method.Post,
                ContentType = "application/json",
                Parameters = DeviceInfo
            };
            CommonDTO<PageDTO<DeviceModel>> respone = HttpClient.Execute<PageDTO<DeviceModel>>(apiRequest);
            if (respone.Code == ErrorCode.Success && respone.Data != null)
            {
                Aggregator.GetEvent<MsgEvent>().Publish("作成完了しました");
                RequestClose.Invoke(new DialogResult(ButtonResult.OK));
                return;
            }
            else
            {
                Aggregator.GetEvent<MsgEvent>().Publish($"Error: {respone.Message}");
            }
        }
    }

}
