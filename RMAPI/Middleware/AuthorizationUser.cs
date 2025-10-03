using Infrastructure.Repositories;

namespace RMAPI.Middleware
{
    public static class AuthorizationUser
    {
        public static async Task<bool> HasPermissionAsync(
            int userId,
            string permission,
            IBaseQuery baseQuery)
        {
            const string sql = @"
                SELECT r.UserFunctionIdList
                FROM [User] u
                JOIN [Role] r ON u.RoleId = r.RoleId
                JOIN [UserFunction] uf on r.UserFunctionIdList like '%' + CAST(uf.UserFunctionId as varchar) + '%'
                WHERE u.UserId = @UserId";

            var result = await baseQuery.QuerySingleAsync<string>(sql, new { UserId = userId });

            if (string.IsNullOrWhiteSpace(result))
                return false;

            var permissions = result.Split(',', StringSplitOptions.RemoveEmptyEntries);
            return permissions.Contains(permission);
        }
    }
}
