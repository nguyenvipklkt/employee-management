using ClosedXML.Excel;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace ServiceGeneratorTool
{
    public static class ServiceGenerator
    {
        static string dtoOutputPath = @"..\..\..\..\Common\Dto";

        /// <summary>
        /// Generate service from entity
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public static int GenerateService(string entityName, string outputPath)
        {
            string serviceName = $"{entityName}Service";
            string dtoName = $"{entityName}Dto";
            string lowerEntity = char.ToLowerInvariant(entityName[0]) + entityName.Substring(1);
            var sb = new StringBuilder();
            sb.AppendLine("using AutoMapper;");
            sb.AppendLine("using Common.Dto;");
            sb.AppendLine("using Helper.NLog;");
            sb.AppendLine("using Infrastructure.Repositories;");
            sb.AppendLine("using RMAPI.ConfigApp;");
            sb.AppendLine("using Shared;");
            sb.AppendLine();    
            sb.AppendLine($"namespace RMAPI.Services.{entityName}Service");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {serviceName}");
            sb.AppendLine("    {");
            sb.AppendLine($"        private readonly BaseCommand<{entityName}> _baseCommand;");
            sb.AppendLine("        private readonly ConfigJWT _jwt;");
            sb.AppendLine("        private readonly IMapper _mapper;");
            sb.AppendLine();
            sb.AppendLine($"        public {serviceName}(BaseCommand<{entityName}> baseCommand, ConfigJWT jwt, IMapper mapper)");
            sb.AppendLine("        {");
            sb.AppendLine("            _baseCommand = baseCommand;");
            sb.AppendLine("            _jwt = jwt;");
            sb.AppendLine("            _mapper = mapper;");
            sb.AppendLine("        }");
            sb.AppendLine();

            // Add<Entity>
            sb.AppendLine($"        public {dtoName} Add{entityName}({dtoName} dto)");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine($"                var entity = _mapper.Map<{entityName}>(dto);");
            sb.AppendLine("                var result = _baseCommand.Create(entity);");
            sb.AppendLine($"                return _mapper.Map<{dtoName}>(result);");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                BaseNLog.logger.Error(ex);");
            sb.AppendLine("                throw;");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();

            // Get<Entity>ById
            sb.AppendLine($"        public {dtoName} Get{entityName}ById(int id)");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                var entity = _baseCommand.FindOrFail(id);");
            sb.AppendLine($"                return _mapper.Map<{dtoName}>(entity);");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                BaseNLog.logger.Error(ex);");
            sb.AppendLine("                throw;");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();

            // Update<Entity>
            sb.AppendLine($"        public {dtoName} Update{entityName}({dtoName} dto)");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine($"                var entity = _mapper.Map<{entityName}>(dto);");
            sb.AppendLine("                var result = _baseCommand.UpdateByEntity(entity);");
            sb.AppendLine($"                return _mapper.Map<{dtoName}>(result);");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                BaseNLog.logger.Error(ex);");
            sb.AppendLine("                throw;");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();

            // Delete<Entity>
            sb.AppendLine($"        public bool Delete{entityName}(int id)");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine($"                var entity = _baseCommand.FindOrFail(id);");
            sb.AppendLine("                return _baseCommand.DeleteByEntity(entity);");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                BaseNLog.logger.Error(ex);");
            sb.AppendLine("                throw;");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();

            // GetAll<Entity>
            sb.AppendLine($"        public List<{dtoName}> GetAll{entityName}s()");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                var entities = _baseCommand.FindAll().ToList();");
            sb.AppendLine($"                return _mapper.Map<List<{dtoName}>>(entities);");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                BaseNLog.logger.Error(ex);");
            sb.AppendLine("                throw;");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();

            // Get<Entity>ByCondition
            sb.AppendLine($"        public List<{dtoName}> Get{entityName}sByCondition(Func<{entityName}, bool> predicate)");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine("                var result = _baseCommand.FindAll().Where(predicate).ToList();");
            sb.AppendLine($"                return _mapper.Map<List<{dtoName}>>(result);");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                BaseNLog.logger.Error(ex);");
            sb.AppendLine("                throw;");
            sb.AppendLine("            }");
            sb.AppendLine("        }");

            sb.AppendLine("    }");
            sb.AppendLine("}");

            // Write file
            string filePath = Path.Combine(outputPath, $"{serviceName}.cs");
            if (File.Exists(filePath))
            {
                Console.WriteLine($"✅ Existed: {filePath},skip.");
                return 1;
            }
            File.WriteAllText(filePath, sb.ToString());
            Console.WriteLine($"✔️ Service file created at: {filePath}");
            return 1;
        }

        /// <summary>
        /// Generate DTO from entity
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="sharedPath"></param>
        /// <param name="dtoOutputPath"></param>
        /// <returns></returns>
        public static int GenerateDto(string entityName, string sharedPath, string dtoOutputPath)
        {
            string entityFile = Path.Combine(sharedPath, $"{entityName}.cs");

            if (!File.Exists(entityFile))
            {
                Console.WriteLine($"❌ Do not find entity: {entityFile}");
                return 0;
            }

            string content = File.ReadAllText(entityFile);

            SyntaxTree tree = CSharpSyntaxTree.ParseText(content);
            CompilationUnitSyntax root = tree.GetCompilationUnitRoot();

            var classNode = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
                                .FirstOrDefault(c => c.Identifier.Text == entityName);

            if (classNode == null)
            {
                Console.WriteLine($"❌ Do not find class {entityName}");
                return 0;
            }

            var properties = classNode.Members
                .OfType<PropertyDeclarationSyntax>()
                .Select(p => p.WithAttributeLists(new SyntaxList<AttributeListSyntax>()));

            var dtoClass = SyntaxFactory.ClassDeclaration($"{entityName}Dto")
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddMembers(properties.ToArray());

            var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("Common.Dto"))
                .AddMembers(dtoClass);

            var dtoUnit = SyntaxFactory.CompilationUnit()
                .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")))
                .AddMembers(namespaceDeclaration)
                .NormalizeWhitespace();

            string dtoCode = dtoUnit.ToFullString();

            Directory.CreateDirectory(dtoOutputPath);
            string outputFile = Path.Combine(dtoOutputPath, $"{entityName}Dto.cs");
            if (File.Exists(outputFile))
            {
                Console.WriteLine($"✅ Existed: {outputFile},skip.");
                return 1;
            }
            File.WriteAllText(outputFile, dtoCode); return 1;
        }

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
                sb.AppendLine("namespace Shared");
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
                if (!string.IsNullOrEmpty(tableName))
                {
                    string outputServicePath = $@"..\..\..\..\RMAPI\Services\{tableName}Service";
                    Directory.CreateDirectory(outputServicePath);
                    var resDto = GenerateDto(tableName, sharedPath, dtoOutputPath);
                    if (resDto == 1)
                    {
                        var resService = GenerateService(tableName, outputServicePath);
                        if (resService == 1)
                        {
                            string controllerOutputPath = $@"..\..\..\..\RMAPI\Controllers\{tableName}Controller";
                            if (!Directory.Exists(controllerOutputPath))
                            {
                                Directory.CreateDirectory(controllerOutputPath);
                            }
                            var resController = GenerateController(tableName, controllerOutputPath);
                            if (resController == 1)
                            {
                                Console.WriteLine($"✔️ Created Dto ,Service and Controller for entity '{tableName}'");
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generate controller from entity
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public static int GenerateController(string entityName, string outputPath)
        {
            string serviceName = $"{entityName}Service";
            string controllerName = $"{entityName}Controller";
            string lowerEntity = char.ToLowerInvariant(entityName[0]) + entityName.Substring(1);
            string route = entityName.ToLower();

            var sb = new StringBuilder();
            sb.AppendLine("using Common.Enum;");
            sb.AppendLine("using Common.Dto;");
            sb.AppendLine("using Microsoft.AspNetCore.Authorization;");
            sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine($"using RMAPI.Services.{entityName}Service;");
            sb.AppendLine();
            sb.AppendLine($"namespace RMAPI.Controllers.{entityName}Controller");
            sb.AppendLine("{");
            sb.AppendLine("    [ApiController]");
            sb.AppendLine($"    [Route(\"api/{route}\")]");
            sb.AppendLine("    [Authorize]");
            sb.AppendLine($"    public class {controllerName} : BaseApiController<" + controllerName + ">");
            sb.AppendLine("    {");
            sb.AppendLine($"        private readonly {serviceName} _" + lowerEntity + "Service;");
            sb.AppendLine();
            sb.AppendLine($"        public {controllerName}({serviceName} " + lowerEntity + "Service)");
            sb.AppendLine("        {");
            sb.AppendLine($"            _{lowerEntity}Service = {lowerEntity}Service;");
            sb.AppendLine("        }");
            sb.AppendLine();

            // GET ALL
            sb.AppendLine("        [HttpGet]");
            sb.AppendLine($"        [Route(\"getAll\")]");
            sb.AppendLine("        public APIResponse GetAll()");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine($"                var res = _{lowerEntity}Service.GetAll{entityName}s();");
            sb.AppendLine("                return new APIResponse { Data = res };");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                return NG(ex);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();

            // GET BY ID
            sb.AppendLine("        [HttpGet]");
            sb.AppendLine("        [Route(\"getById\")]");
            sb.AppendLine("        public APIResponse GetById(int id)");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine($"                var res = _{lowerEntity}Service.Get{entityName}ById(id);");
            sb.AppendLine("                return new APIResponse { Data = res };");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                return NG(ex);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();

            // POST - ADD
            sb.AppendLine("        [HttpPost]");
            sb.AppendLine("        [Route(\"add\")]");
            sb.AppendLine($"        public APIResponse Add({entityName}Dto dto)");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine($"                var res = _{lowerEntity}Service.Add{entityName}(dto);");
            sb.AppendLine("                return new APIResponse { Data = res };");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                return NG(ex);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();

            // PUT - UPDATE
            sb.AppendLine("        [HttpPut]");
            sb.AppendLine("        [Route(\"update\")]");
            sb.AppendLine($"        public APIResponse Update({entityName}Dto dto)");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine($"                var res = _{lowerEntity}Service.Update{entityName}(dto);");
            sb.AppendLine("                return new APIResponse { Data = res };");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                return NG(ex);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();

            // DELETE
            sb.AppendLine("        [HttpDelete]");
            sb.AppendLine("        [Route(\"delete\")]");
            sb.AppendLine("        public APIResponse Delete(int id)");
            sb.AppendLine("        {");
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine($"                var res = _{lowerEntity}Service.Delete{entityName}(id);");
            sb.AppendLine("                return new APIResponse { Data = res };");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine("                return NG(ex);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");

            sb.AppendLine("    }");
            sb.AppendLine("}");

            string filePath = Path.Combine(outputPath, $"{controllerName}.cs");
            if (File.Exists(filePath))
            {
                Console.WriteLine($"✅ Existed: {filePath},skip.");
                return 1;
            }
            File.WriteAllText(filePath, sb.ToString());
            Console.WriteLine($"✔️ Controller file created at: {filePath}");
            return 1;
        }

    }
}
