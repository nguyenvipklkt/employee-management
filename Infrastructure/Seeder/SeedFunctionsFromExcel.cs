using Dapper;
using OfficeOpenXml;
using System.Data;

namespace Infrastructure.Seeder
{
    public static class FunctionSeeder
    {
        public static void SeedFunctionsFromExcel(IDbConnection dbConnection, string excelPath)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            if (!File.Exists(excelPath))
                return;

            using var package = new ExcelPackage(new FileInfo(excelPath));
            var worksheet = package.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                var functionCode = worksheet.Cells[row, 2]?.Text?.Trim();
                var functionName = worksheet.Cells[row, 3]?.Text?.Trim();

                if (string.IsNullOrWhiteSpace(functionCode) || string.IsNullOrWhiteSpace(functionName))
                    continue;

                const string checkSql = "SELECT COUNT(1) FROM [Function] WHERE FunctionCode = @FunctionCode";
                var exists = dbConnection.ExecuteScalar<int>(checkSql, new { FunctionCode = functionCode });

                if (exists == 0)
                {
                    const string insertSql = @"
                        INSERT INTO [Function] (FunctionCode, FunctionName, CreateAt, IsDeleted)
                        VALUES (@FunctionCode, @FunctionName, GETDATE(), 0)";
                    dbConnection.Execute(insertSql, new { FunctionCode = functionCode, FunctionName = functionName });
                }
            }
        }
    }
}
