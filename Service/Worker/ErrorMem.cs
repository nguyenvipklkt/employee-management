using Dapper;
using Object.Model;
using System.Data;

namespace Service.Worker
{
    public class ErrorMem
    {
        private readonly IDbConnection _connection;
        public static List<ErrorDefinition> ErrorsInMemory { get; private set; } = new();

        public ErrorMem(IDbConnection connection)
        {
            _connection = connection;
        }

        public void LoadErrorsToMemory()
        {
            string sql = "SELECT * FROM ErrorDefinition";
            var errors = _connection.Query<ErrorDefinition>(sql).ToList();
            ErrorsInMemory = errors;
            Console.WriteLine($"✔️ Loaded {errors.Count} error definitions to memory.");
        }

        public static string GetErrorNameById(string errorId)
        {
            return ErrorsInMemory.FirstOrDefault(e => e.ErrorId == errorId)?.ErrorName ?? "";
        }
    }
}
