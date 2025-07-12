using Infrastructure.Repositories;

namespace RMAPI.Helpers
{
    public static class AuthorizationUser
    {
        public static async Task<bool> HasPermissionAsync(
            int userId,
            string permission,
            BaseQuery baseQuery)
        {
            const string sql = @"
                SELECT r.UserFunctionIdList
                FROM [User] u
                JOIN [Role] r ON u.RoleId = r.RoleId
                WHERE u.UserId = @UserId";

            var result = await baseQuery.QuerySingleAsync<string>(sql, new { UserId = userId });

            if (string.IsNullOrWhiteSpace(result))
                return false;

            var permissions = result.Split(',', StringSplitOptions.RemoveEmptyEntries);
            return permissions.Contains(permission);
        }
    }
}
