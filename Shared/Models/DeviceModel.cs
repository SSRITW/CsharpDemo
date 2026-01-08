namespace WebApiDemo.Models
{
    public class DeviceModel : BaseModel
    {
        public string DeviceId { get; set; } = string.Empty;
        public string DeviceName { get; set; } = string.Empty;
        public int DeviceType { get; set; } = 0;
        public DateTime LastMaintainTime { get; set; } = DateTime.UtcNow;
        public string LastMaintainerName { get; set; } = string.Empty;
        public int Status { get; set; } = 0;
    }
}
