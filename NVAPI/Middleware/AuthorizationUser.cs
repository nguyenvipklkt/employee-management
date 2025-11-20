using Infrastructure.Repositories;

namespace NVAPI.Middleware
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
                SELECT r.FunctionCodeList
                FROM [User] u
                JOIN [Role] r ON u.RoleCode = r.RoleCode
                WHERE u.UserId = @UserId AND u.IsDeleted = 0 AND r.IsDeleted = 0";

            var functionCodeListRaw = await baseQuery.QueryFirstOrDefaultAsync<string?>(roleSql, new { UserId = userId });

            if (!string.IsNullOrWhiteSpace(functionCodeListRaw))
            {
                if (functionCodeListRaw.Contains(permission))
                    return true;
            }

            // 4. Check theo quyền riêng (UserFunction)
            const string userFunctionSql = @"
                SELECT FunctionCode
                FROM [UserFunction]
                WHERE UserId = @UserId AND IsDeleted = 0";

            var userFunctionCodes = await baseQuery.QueryAsync<string>(userFunctionSql, new { UserId = userId }) ?? new List<string>();

            return userFunctionCodes.Contains(permission);
        }

    }
}
