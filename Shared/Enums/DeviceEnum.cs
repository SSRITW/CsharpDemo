using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Enums
{
    public static class DeviceEnum
    {
        public enum Type
        {
            Default = 0,
            Sensor = 1,
            Actuator = 2,
            Controller = 3,
            Gateway = 4
        }

        public enum Status 
        {
            Default = 0,
            Inactive = 1,
            Active = 2,
            Maintenance = 3,
            Decommissioned = 4
        }

        public enum OrderByType
        {
            CreateTime = 0,
            UpdateTime = 1,
            LastMaintainTime = 2
        }
    }
}
