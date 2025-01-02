using Dapper;
using System.Data;

namespace Infrastructure.Repositories
{
    public class BaseQuery
    {
        private readonly IDbConnection _connection;

        public BaseQuery(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null)
        {
            return await _connection.QueryAsync<T>(sql, parameters);
        }

        public async Task<T> QuerySingleAsync<T>(string sql, object parameters = null)
        {
            return await _connection.QuerySingleOrDefaultAsync<T>(sql, parameters);
        }
    }
}
