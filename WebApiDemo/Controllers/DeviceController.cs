using Microsoft.AspNetCore.Mvc;
using Shared.Constants;
using Shared.DTOs;
using Shared.Enums;
using System;
using System.Linq.Expressions;
using WebApiDemo.DTOs;
using WebApiDemo.Models;
using WebApiDemo.Services.Interfaces;
using static Shared.Enums.DeviceEnum;

namespace WebApiDemo.Controllers
{
    /// <summary>
    /// 設備API
    /// </summary>
    [ApiController]
    //[Route("[controller]/[action]")]
    [Route("[controller]")]
    public class DeviceController : ControllerBase
    {
        private IDeviceService _deviceService;
        public DeviceController(IDeviceService deviceService) {
            _deviceService = deviceService;
        }

        [HttpGet]
        [Route("{id}")]
        public CommonDTO<DeviceModel>? Find(Guid id)
        {
            CommonDTO<DeviceModel> result = new CommonDTO<DeviceModel>();
            result.Data =  _deviceService.Find<DeviceModel>(id);
            result.Code = ErrorCode.Success;
            return result;
        }

        [HttpGet("list")]
        public CommonDTO<PageDTO<DeviceModel>> GetPage(int status, int deviceType, string? deviceName, int orderByType, int pageSize, int pageIndex, bool isAsc = true)
        {
            CommonDTO<PageDTO<DeviceModel>> result = new CommonDTO<PageDTO<DeviceModel>>();
            if (pageSize > Page.MaxPageSize || pageSize < Page.MinPageSize)
            {
                result.Code = ErrorCode.InvalidParameter;
                result.Message = $"pageSize should be between {Page.MinPageSize} and {Page.MaxPageSize}";
                return result;
            }

            Expression<Func<DeviceModel, bool>> funcWhere;
            funcWhere = funcWhere =>
                (status == (int)DeviceEnum.Status.Default || funcWhere.Status == status) &&
                (deviceType == (int)DeviceEnum.Type.Default || funcWhere.DeviceType == deviceType) &&
                (string.IsNullOrEmpty(deviceName) || funcWhere.DeviceName.Contains(deviceName));
            Expression<Func<DeviceModel,object>> funcOrderBy;
            switch (orderByType)
            {
                case (int)DeviceEnum.OrderByType.UpdateTime:
                    funcOrderBy = funcOrderBy => funcOrderBy.UpdateTime;
                    break;
                case (int)DeviceEnum.OrderByType.LastMaintainTime:
                    funcOrderBy = funcOrderBy => funcOrderBy.LastMaintainTime;
                    break;
                default:
                    funcOrderBy = funcOrderBy => funcOrderBy.CreateTime;
                    break;
            }
            result.Data = _deviceService.QueryPage(funcWhere, pageSize, pageIndex, funcOrderBy, isAsc);
            result.Code = ErrorCode.Success;
            return result;
        }

        [HttpPost("insert")]
        public DeviceModel Insert(DeviceModel model)
        {   
            return _deviceService.Insert(model);
        }

        [HttpPost("insert_batch")]
        public List<DeviceModel> InsertBatch(List<DeviceModel> models)
        {
            return _deviceService.InsertBatch(models);
        }

        [HttpPut("update")]
        public CommonDTO<object> Update(Guid id, string deviceId, string deviceName, int deviceType, int status)
        {
            CommonDTO<object> result = new CommonDTO<object>();
            
            if (id == Guid.Empty)
            {
                result.Code = ErrorCode.InvalidParameter;
                result.Message = "id is empty";
                return result;
            }

            int rowNum = _deviceService.Update<DeviceModel>(id, s =>
                s.SetProperty(d => d.UpdateTime, DateTime.UtcNow)
                 .SetProperty(d => d.DeviceId, deviceId) // 直接使用 deviceId
                 .SetProperty(d => d.DeviceType, deviceType)
                 .SetProperty(d => d.DeviceName, deviceName)
                 .SetProperty(d => d.Status, status)
            );

            if (rowNum == 0)
            {
                result.Code = ErrorCode.NotFound;
                result.Message = "Device not found";
                return result;
            }
            
            result.Code = ErrorCode.Success;
            result.Message = $"{rowNum} rows affected";
            return result;
        }
        [HttpPut("maintain")]
        public CommonDTO<object> Maintain(Guid id, string maintainerName)
        {
            CommonDTO<object> result = new CommonDTO<object>();
            if (id == Guid.Empty)
            {
                result.Code = ErrorCode.InvalidParameter;
                result.Message = "id is empty";
                return result;
            }
            
            int rowNum = _deviceService.Update<DeviceModel>(id, s => s
                .SetProperty(d => d.LastMaintainerName, maintainerName)
                .SetProperty(d => d.LastMaintainTime, DateTime.UtcNow)
                .SetProperty(d => d.UpdateTime, DateTime.UtcNow)
            );
            
            if (rowNum == 0)
            {
                result.Code = ErrorCode.NotFound;
                result.Message = "Device not found";
                return result;
            }
            
            result.Code = ErrorCode.Success;
            result.Message = $"{rowNum} rows affected";
            return result;
        }

        [HttpDelete]
        public CommonDTO<object> Delete(DeviceModel model)
        {
            CommonDTO<object> result = new CommonDTO<object>();
            int rowNum = _deviceService.Delete(model);
            result.Message = $"{rowNum} rows affected";
            return result;
        }

        [HttpDelete("delete_batch")]
        public CommonDTO<object> DeleteBatch(List<DeviceModel> models)
        {
            CommonDTO<object> result = new CommonDTO<object>();
            int rowNum = _deviceService.DeleteBatch(models);
            result.Message = $"{rowNum} rows affected";
            return result;
        }
    }
}
