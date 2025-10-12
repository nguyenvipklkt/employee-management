using AutoMapper;
using Infrastructure.Repositories;
using Object.Model;
using OfficeOpenXml;

namespace Service.Service.TemplateService
{
    public interface ITemplateService
    {
        string GenerateImportTemplate(int userId, int warehouseId);
    }
    public class TemplateService: ITemplateService
    {
        private readonly IBaseCommand<Warehouse> _baseWarehouseCommand;
        private readonly IBaseCommand<Department> _baseDepartmentCommand;
        private readonly IMapper _mapper;

        public TemplateService(IBaseCommand<Warehouse> baseWarehouseCommand, IBaseCommand<Department> baseDepartmentCommand, IMapper mapper)
        {
            _baseWarehouseCommand = baseWarehouseCommand;
            _baseDepartmentCommand = baseDepartmentCommand;
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

                    ws.Cells["B3"].Value = department.DepartmentName;
                    ws.Cells["B4"].Value = department.DepartmentAddress;
                    ws.Cells["B5"].Value = department.DepartmentId;

                    ws.Cells["B6"].Value = warehouse.Name;
                    ws.Cells["B7"].Value = warehouse.WarehouseId;
                    ws.Cells["B8"].Value = DateTime.Now.ToString("dd/MM/yyyy");

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

    }
}
