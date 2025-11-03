using AutoMapper;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Object.Model;
using OfficeOpenXml;

namespace Service.Service.TemplateService
{
    public interface ITemplateService
    {
        string GenerateImportTemplate(int userId, int warehouseId);
        Task<bool> ImportExcelToWarehouse(int userId, IFormFile file);
    }
    public class TemplateService: ITemplateService
    {
        private readonly IBaseCommand<Warehouse> _baseWarehouseCommand;
        private readonly IBaseCommand<Department> _baseDepartmentCommand;
        private readonly IBaseCommand<Material> _baseMaterialCommand;
        private readonly IBaseCommand<WarehouseStock> _baseStockCommand;
        private readonly IBaseCommand<History> _baseHistoryCommand;
        private readonly IMapper _mapper;

        public TemplateService(IBaseCommand<Warehouse> baseWarehouseCommand, IBaseCommand<Department> baseDepartmentCommand, IBaseCommand<Material> baseMaterialCommand, IBaseCommand<WarehouseStock> baseStockCommand, IBaseCommand<History> baseHistoryCommand, IMapper mapper)
        {
            _baseWarehouseCommand = baseWarehouseCommand;
            _baseDepartmentCommand = baseDepartmentCommand;
            _baseMaterialCommand = baseMaterialCommand;
            _baseStockCommand = baseStockCommand;
            _baseHistoryCommand = baseHistoryCommand;
            _mapper = mapper;
        }
        public string GenerateImportTemplate(int userId, int warehouseId)
        {
            try
            {
                var warehouse = _baseWarehouseCommand.FindByCondition(x => x.WarehouseId == warehouseId).FirstOrDefault();
                if (warehouse == null)
                    throw new Exception("Kho không tồn tại");
                var department = _baseDepartmentCommand.FindByCondition(x => x.DepartmentId == warehouse.DepartmentId).FirstOrDefault();

                if (department == null)
                    throw new Exception("Cơ sở không tồn tại");

                var pathToTemplate = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates", "MauPhieuNhapKho.xlsx");
                var generatedFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "generated");
                if (!Directory.Exists(generatedFolder))
                    Directory.CreateDirectory(generatedFolder);

                var outputFileName = $"PhieuNhapKho_{warehouse.Name}_{DateTime.Now:ddMMyyyy_HHmmss}.xlsx";
                var outputPath = Path.Combine(generatedFolder, outputFileName);

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var package = new ExcelPackage(new FileInfo(pathToTemplate)))
                {
                    var ws = package.Workbook.Worksheets[0];

                    ws.Cells["B4"].Value = department.DepartmentName;
                    ws.Cells["B5"].Value = department.DepartmentAddress;
                    ws.Cells["B6"].Value = department.DepartmentId;

                    ws.Cells["B7"].Value = warehouse.Name;
                    ws.Cells["B8"].Value = warehouse.WarehouseId;
                    ws.Cells["B9"].Value = DateTime.Now.ToString("dd/MM/yyyy");

                    // Lưu file mới
                    package.SaveAs(new FileInfo(outputPath));
                    return outputFileName;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ImportExcelToWarehouse(int userId, IFormFile file)
        {
            try
            {
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                stream.Position = 0;

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using var package = new ExcelPackage(stream);
                var sheet = package.Workbook.Worksheets.First();

                var materials = new List<Material>();
                var stocks = new List<WarehouseStock>();
                float totalPrice = 0;
                int.TryParse(sheet.Cells[6, 2].Text, out int departmentId);
                if (departmentId == 0)
                    throw new Exception("Mã cơ sở không hợp lệ");
                int.TryParse(sheet.Cells[8, 2].Text, out int warehouseId);
                if (warehouseId == 0)
                    throw new Exception("Mã kho không hợp lệ");

                var department = _baseDepartmentCommand.FindByCondition(x => x.DepartmentId == departmentId).FirstOrDefault();
                if (department == null)
                    throw new Exception("Cơ sở không tồn tại");
                var warehouse = _baseWarehouseCommand.FindByCondition(x => x.WarehouseId == warehouseId && x.DepartmentId == departmentId).FirstOrDefault();
                if (warehouse == null)
                    throw new Exception("Kho không tồn tại");

                for (int row = 14; row <= sheet.Dimension.End.Row; row++)
                {
                    var materialName = sheet.Cells[row, 2].Text.Trim();
                    var quantityText = sheet.Cells[row, 3].Text;
                    var unit = sheet.Cells[row, 4].Text.Trim();
                    var unitPriceText = sheet.Cells[row, 5].Text;
                    var expiredAtText = sheet.Cells[row, 6]?.Text;
                    var note = sheet.Cells[row, 7].Text.Trim();

                    if (string.IsNullOrWhiteSpace(materialName)) continue;

                    float.TryParse(quantityText, out float quantity);
                    float.TryParse(unitPriceText, out float unitPrice);
                    DateTime.TryParse(expiredAtText, out DateTime expiredAt);

                    // 1. Insert vào Material nếu chưa có
                    var material = _baseMaterialCommand.FindByCondition(x => x.Name.ToUpper() == materialName.ToUpper() && x.Unit.ToUpper() == unit.ToUpper()).FirstOrDefault();
                    if (material == null)
                    {
                        material = new Material
                        {
                            Name = materialName,
                            Unit = unit,
                            Price = (long)unitPrice,
                            CreateAt = DateTime.UtcNow,
                            CreateBy = userId,
                        };
                        material = _baseMaterialCommand.Create(material);
                    }

                    // 2. Insert vào WarehouseStock
                    var stock = new WarehouseStock
                    {
                        MaterialId = material.MaterialId,
                        Quantity = quantity,
                        UnitPrice = unitPrice,
                        ImportedAt = DateTime.UtcNow,
                        ExpiredAt = expiredAt,
                        Note = note,
                        WarehouseId = warehouseId,
                        IsUsedUp = false
                    };
                    _baseStockCommand.Create(stock);

                    // Tính tổng tiền
                    totalPrice += quantity * unitPrice;
                }

                // 3. Insert vào bảng History
                var history = new History
                {
                    MaterialIdList = 0, // TODO: cần cách lưu dạng {1,2,3,...}
                    TotalPrice = totalPrice,
                    ImportDate = DateTime.UtcNow,
                    Description = "Nhập kho từ Excel",
                    CreateBy = userId,
                    HistoryType = "I",
                    DepartmentId = departmentId,
                    CreateAt = DateTime.UtcNow
                };
                _baseHistoryCommand.Create(history);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
