using ClosedXML.Excel;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace ServiceGeneratorTool
{
    public static class ServiceGenerator
    {
        /// <summary>
        /// Generate entities from Excel file
        /// </summary>
        /// <param name="excelPath"></param>
        /// <param name="outputPath"></param>
        /// <param name="sharedPath"></param>
        public static void GenerateEntitiesFromExcel(string excelPath, string outputPath, string sharedPath)
        {
            var tableMap = new Dictionary<string, List<(string name, string type, string description)>>();

            using var workbook = new XLWorkbook(excelPath);
            var worksheet = workbook.Worksheets.First();

            foreach (var row in worksheet.RowsUsed().Skip(1)) // Skip header
            {
                var table = row.Cell(1).GetString().Trim();
                var prop = row.Cell(2).GetString().Trim();
                var type = row.Cell(3).GetString().Trim();
                var desc = row.Cell(4).GetString().Trim();

                if (!tableMap.ContainsKey(table))
                    tableMap[table] = new();

                tableMap[table].Add((prop, type, desc));
            }

            foreach (var kvp in tableMap)
            {
                var tableName = kvp.Key;
                var filePath = Path.Combine(sharedPath, $"{tableName}.cs");

                if (File.Exists(filePath))
                {
                    Console.WriteLine($"✅ Existed: {tableName},skip.");
                    continue;
                }

                var sb = new StringBuilder();
                sb.AppendLine("using System;");
                sb.AppendLine("using System.ComponentModel.DataAnnotations;");
                sb.AppendLine("using System.ComponentModel.DataAnnotations.Schema;");
                sb.AppendLine();
                sb.AppendLine("namespace Object.Model");
                sb.AppendLine("{");
                sb.AppendLine($"    public class {tableName} : BaseModel");
                sb.AppendLine("    {");

                foreach (var (name, type, desc) in kvp.Value)
                {
                    // Optional: Add [Key] to Id fields if needed
                    if (name.ToLower().EndsWith("id") && name.ToLower() == $"{tableName.ToLower()}id")
                    {
                        sb.AppendLine("        [Key]");
                        sb.AppendLine("        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]");
                    }

                    if (type == "string") 
                    { 
                        sb.AppendLine($"        public {type} {name} {{ get; set; }} = string.Empty; // {desc}"); 
                    }
                    else 
                    { 
                        sb.AppendLine($"        public {type} {name} {{ get; set; }} // {desc}"); 
                    }
                }

                sb.AppendLine("    }");
                sb.AppendLine("}");

                File.WriteAllText(filePath, sb.ToString());
                Console.WriteLine($"✅ Entity created: {tableName}");
            }
        }
    }
}
