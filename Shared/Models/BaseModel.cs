namespace WebApiDemo.Models
{
    public abstract class BaseModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        
        public DateTime CreateTime{ get; set; } = DateTime.UtcNow;

        public DateTime UpdateTime{ get; set; } = DateTime.UtcNow;
    }
}
