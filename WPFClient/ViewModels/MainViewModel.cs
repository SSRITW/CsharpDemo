using RestSharp;
using Shared.Constants;
using Shared.DTOs;
using Shared.Enums;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WebApiDemo.DTOs;
using WPFClient.HttpClients;
using WPFClient.Models;
using WPFClient.MsgEvents;

namespace WPFClient.ViewModels
{
    public class MainViewModel : BindableBase, IDialogAware
    {
        private readonly HttpRestClient HttpClient;
        public string Title { get; set; } = "Main";

        public MainViewModel(HttpRestClient httpClient, IDialogService dialogService)
        {
            HttpClient = httpClient;
            RefreshDeviceListCmm = new DelegateCommand(RefreshDeviceList);
            Status = 0;
            DeviceType = 0;
            PageSize = 3;
            PageNum = 1;
            DialogService = dialogService;
            OpenDeviceInfoDialogCmm = new DelegateCommand<DeviceModel>(OpenDeviceInfoDialog);
            RefreshDeviceList();
        }

        public List<StatusOption> StatusOptions { get; set; } = new()
        {
            new StatusOption { Name = "全部", Value = 0 }, 
            new StatusOption { Name = "非アクティブ", Value = 1 },
            new StatusOption { Name = "アクティブ", Value = 2 },
            new StatusOption { Name = "メンテナンス", Value = 3 },
            new StatusOption { Name = "廃止", Value = 4 }
        };

        public List<StatusOption> DeviceTypeOptions { get; set; } = new()
        {
            new StatusOption { Name = "全部", Value = 0 },
            new StatusOption { Name = "モニター", Value = 1 },
            new StatusOption { Name = "マウス", Value = 2 },
            new StatusOption { Name = "キーボード", Value = 3 },
        };

        public List<int> PageSizeOptions { get; } = new() { 3, 10, 20, 50 };

        private readonly IDialogService DialogService;

        private int _Status;
        public int Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        private int _DeviceType;
        public int DeviceType
        {
            get { return _DeviceType; }
            set { _DeviceType = value; }
        }

        private string _DeviceName;
        public string DeviceName
        {
            get { return _DeviceName; }
            set { _DeviceName = value; }
        }

        private int _PageSize;

        public int PageSize
        {
            get { return _PageSize; }
            set 
            { 
                _PageSize = value; 
                RaisePropertyChanged(); 
                RefreshDeviceList();
            }
        }

        private int _PageNum;
        public int PageNum
        {
            get { return _PageNum; }
            set { _PageNum = value; RaisePropertyChanged(); }
        }

        private int _RecordCount;
        public int RecordCount
        {
            get { return _RecordCount; }
            set { _RecordCount = value; RaisePropertyChanged(); }
        }

        private int _TotalPages;
        public int TotalPages
        {
            get { return _TotalPages; }
            set { _TotalPages = value; RaisePropertyChanged(); }
        }

        private List<DeviceModel> _DeviceList;
        public List<DeviceModel> DeviceList 
        {
            get { return _DeviceList; }
            set
            {
                _DeviceList = value;
                RaisePropertyChanged();
            }
        }

        public DialogCloseListener RequestClose { get; set; }

        public DelegateCommand RefreshDeviceListCmm {  get; set; }

        private void RefreshDeviceList()
        {
            if (PageSize == 0 || PageNum == 0)
            {
                return; 
            }
            ApiRequest apiRequest = new ApiRequest()
            {
                Route = $"Device/list?pageSize={PageSize}&pageNum={PageNum}",
                Method = Method.Get,
                ContentType = "application/json"
            };
            CommonDTO<PageDTO<DeviceModel>> respone = HttpClient.Execute<PageDTO<DeviceModel>>(apiRequest);
            if (respone.Code == ErrorCode.Success && respone.Data !=null)
            {
                DeviceList = respone.Data.Items;
                RecordCount = respone.Data.RecordCount;
                TotalPages = (RecordCount / PageSize) + ((RecordCount % PageSize) > 0 ? 1:0);
            }
        }

        public DelegateCommand<DeviceModel> OpenDeviceInfoDialogCmm { get; set; }
        private void OpenDeviceInfoDialog(DeviceModel? deviceInfo)
        {
            var param = new DialogParameters();
            param.Add("DeviceInfo", deviceInfo);
            DialogService.ShowDialog("DeviceInfo", param, callback =>
            {
                if (callback.Result == ButtonResult.OK)
                {
                    RefreshDeviceList();
                }
            });
        }

        public DelegateCommand FirstPageCommand => new(() =>
        {
            PageNum = 1;
            RefreshDeviceList();
        });

        public DelegateCommand PrevPageCommand => new(() =>
        {
            if (PageNum > 1)
            {
                PageNum--;
                RefreshDeviceList();
            }
        });
        public DelegateCommand NextPageCommand => new(() =>
        {
            if (PageNum < TotalPages)
            {
                PageNum++;
                RefreshDeviceList();
            }
        });

        public DelegateCommand LastPageCommand => new(() =>
        {
            PageNum = TotalPages;
            RefreshDeviceList();
        });

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
    }
}
