namespace CoreValidation.Requests.Table
{
    public class UpdateTableRequest
    {
        public int TableId { get; set; } // Mã bàn
        public string TableNumber { get; set; } = string.Empty; // Số hiệu bàn
        public string QRCodeUrl { get; set; } = string.Empty; // Link đến QR
        public string Status { get; set; } = string.Empty; // Trạng thái (trống, đang phục vụ...)
        public int Capacity { get; set; } // Sức chứa
    }
}
