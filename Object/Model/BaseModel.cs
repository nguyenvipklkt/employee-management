namespace Object.Model
{
    public class BaseModel
    {
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public int CreateBy { get; set; }
        public DateTime UpdateAt { get; set; } = default;
        public int UpdateBy { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
