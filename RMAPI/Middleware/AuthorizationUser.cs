using Infrastructure.Repositories;

namespace RMAPI.Middleware
{
    public static class AuthorizationUser
    {
        public static async Task<bool> HasPermissionAsync(int userId, string permission, IBaseQuery baseQuery)
        {
            // 1.Check có permission không
            if (string.IsNullOrWhiteSpace(permission))
                return true;

            // 2.Check có phải super admin không
            const string superAdminSql = @"
                SELECT IsSuperAdmin
                FROM [User]
                WHERE UserId = @UserId AND IsDeleted = 0";
            var isSuperAdmin = await baseQuery.QueryFirstOrDefaultAsync<int?>(superAdminSql, new { UserId = userId });
            if (isSuperAdmin == 1)
                return true;

            // 3. Check theo Role
            const string roleSql = @"
                SELECT r.FunctionIdList
                FROM [User] u
                JOIN [Role] r ON u.RoleId = r.RoleId
                WHERE u.UserId = @UserId AND u.IsDeleted = 0 AND r.IsDeleted = 0";

            var functionIdListRaw = await baseQuery.QueryFirstOrDefaultAsync<string?>(roleSql, new { UserId = userId });

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
                        WHERE FunctionId IN @Ids AND IsDeleted = 0";

                    var functionCodes = await baseQuery.QueryAsync<string>(getFunctionCodesSql, new { Ids = functionIds }) ?? new List<string>();
                    if (functionCodes.Contains(permission))
                        return true;
                }
            }

            // 4. Check theo quyền riêng (UserFunction)
            const string userFunctionSql = @"
                SELECT f.FunctionCode
                FROM [UserFunction] uf
                JOIN [Function] f ON uf.FunctionId = f.FunctionId
                WHERE uf.UserId = @UserId AND uf.IsDeleted = 0 AND f.IsDeleted = 0";

            var userFunctionCodes = await baseQuery.QueryAsync<string>(userFunctionSql, new { UserId = userId }) ?? new List<string>();

            return userFunctionCodes.Contains(permission);
        }

    }
}
