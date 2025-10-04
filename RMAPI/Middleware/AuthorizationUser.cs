using Infrastructure.Repositories;

namespace RMAPI.Middleware
{
    public static class AuthorizationUser
    {
        public static async Task<bool> HasPermissionAsync(int userId, string permission, IBaseQuery baseQuery)
        {
            // 1. Check theo Role
            const string roleSql = @"
                SELECT r.FunctionIdList
                FROM [User] u
                JOIN [Role] r ON u.RoleId = r.RoleId
                WHERE u.UserId = @UserId";

            var functionIdListRaw = await baseQuery.QuerySingleAsync<string>(roleSql, new { UserId = userId });

            if (!string.IsNullOrWhiteSpace(functionIdListRaw))
            {
                var functionIds = functionIdListRaw
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(id => int.Parse(id.Trim()))
                    .ToList();

                if (functionIds.Any())
                {
                    const string getFunctionCodesSql = @"
                        SELECT FunctionCode
                        FROM [Function]
                        WHERE FunctionId IN @Ids";

                    var functionCodes = await baseQuery.QueryAsync<string>(getFunctionCodesSql, new { Ids = functionIds });
                    if (functionCodes.Contains(permission))
                        return true;
                }
            }

            // 2. Check theo quyền riêng (UserFunction)
            const string userFunctionSql = @"
                SELECT f.FunctionCode
                FROM [UserFunction] uf
                JOIN [Function] f ON uf.FunctionId = f.FunctionId
                WHERE uf.UserId = @UserId";

            var userFunctionCodes = await baseQuery.QueryAsync<string>(userFunctionSql, new { UserId = userId });

            return userFunctionCodes.Contains(permission);
        }

    }
}
