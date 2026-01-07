namespace WebApiDemo.DTOs
{
    public class PageDTO<T> where T : class 
    {
        public int RecordCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public List<T> Items { get; set; } = new List<T>();
    }
}
