namespace Object.Setting
{
    public class APIResponse
    {
        public string Code { get; set; } = "Ok";
        public string Message { get; set; } = "Thành công";
        public object? Data { get; set; }
    }
}
