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
            string excelPath = @"..\..\..\..\ServiceGeneratorTool\DbSchema.xlsx";

            switch (choice)
            {
                case "1":
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
