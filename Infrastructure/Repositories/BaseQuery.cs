using Dapper;
using System.Data;

namespace Infrastructure.Repositories
{
    public interface IBaseQuery
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters = null);
        Task<T> QuerySingleAsync<T>(string sql, object parameters = null);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null);
    }

    public class BaseQuery : IBaseQuery
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

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters = null)
        {
            return await _connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }
    }
}
