namespace Object.Model
{
    public class BaseModel
    {
        public DateTime CreateAt { get; set; } = DateTime.Now; 
        public DateTime UpdateAt { get; set; } = default;
        public bool IsDeleted { get; set; } = false;
    }
}
