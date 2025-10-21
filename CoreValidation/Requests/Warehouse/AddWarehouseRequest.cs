namespace CoreValidation.Requests.Warehouse
{
    public class AddWarehouseRequest
    {
        public string WarehouseName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
    }
}
