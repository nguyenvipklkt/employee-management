namespace CoreValidation.Requests.Warehouse
{
    public class UpdateWarehouseRequest
    {
        public int WarehouseId { get; set; }
        public string WarehouseName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
    }
}
