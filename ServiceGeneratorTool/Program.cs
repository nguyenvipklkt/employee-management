using System;
using System.IO;

namespace ServiceGeneratorTool
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("===== SERVICE GENERATOR TOOL =====");
            Console.WriteLine("1. Generate specific Entity");
            Console.WriteLine("2. Generate Entities from Excel file (DbSchema.xlsx)");
            Console.Write("Choose (1 or 2): ");
            var choice = Console.ReadLine();

            string sharedPath = @"..\..\..\..\Object\Model";
            string dtoOutputPath = @"..\..\..\..\Object\Dto";
            string excelPath = @"..\..\..\..\ServiceGeneratorTool\DbSchema.xlsx";

            switch (choice)
            {
                case "1":
                    Console.Write("🔤 Enter Entity Name (e.g., Ingredient): ");
                    var entityName = Console.ReadLine()?.Trim();

                    if (string.IsNullOrWhiteSpace(entityName))
                    {
                        Console.WriteLine("❌ Invalid entity name.");
                        break;
                    }

                    string serviceOutputPath = $@"..\..\..\..\RMAPI\Services\{entityName}Service";
                    string controllerOutputPath = $@"..\..\..\..\RMAPI\Controllers\{entityName}Controller";

                    Directory.CreateDirectory(serviceOutputPath);
                    Directory.CreateDirectory(controllerOutputPath);

                    Console.WriteLine($"📦 Generating for entity: {entityName}");

                    var resDto = ServiceGenerator.GenerateDto(entityName, sharedPath, dtoOutputPath);
                    if (resDto != 1)
                    {
                        Console.WriteLine("❌ Failed to generate DTO.");
                        break;
                    }

                    var resService = ServiceGenerator.GenerateService(entityName, serviceOutputPath);
                    if (resService != 1)
                    {
                        Console.WriteLine("❌ Failed to generate Service.");
                        break;
                    }

                    var resController = ServiceGenerator.GenerateController(entityName, controllerOutputPath);
                    if (resController != 1)
                    {
                        Console.WriteLine("❌ Failed to generate Controller.");
                        break;
                    }

                    Console.WriteLine($"✅ Successfully generated Dto, Service, and Controller for '{entityName}'");
                    break;

                case "2":
                    ServiceGenerator.GenerateEntitiesFromExcel(excelPath, sharedPath, sharedPath);
                    Console.WriteLine("✔️ Generated entity from Excel");
                    break;

                default:
                    Console.WriteLine("❌Your option is not validated!");
                    break;
            }

            Console.WriteLine("Click any key to exit...");
            Console.ReadKey();
        }
    }
}
