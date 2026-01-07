namespace WebApiDemo.Models
{
    public abstract class BaseModel
    {
        public DateTime CreateTime{ get; set; } = DateTime.UtcNow;

        public DateTime UpdateTime{ get; set; } = DateTime.UtcNow;
    }
}
