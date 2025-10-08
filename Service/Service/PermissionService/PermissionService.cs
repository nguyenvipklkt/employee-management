using Common.Enum.ErrorEnum;
using CoreValidation.Requests.Authentication;
using CoreValidation.Requests.Permision;
using Helper.NLog;
using Infrastructure.Repositories;
using Object.Dto;
using Object.Model;
using static Dapper.SqlMapper;

namespace Service.Service.PermissionService
{
    public interface IPermissionService
    {
        bool GrantPermission(int grantorUserId, GrantPermissionRequest request);
        bool RevokePermission(int grantorUserId, RevokePermissionRequest request);
        List<FunctionDto> GetPermissionsOfUser(int targetUserId);
        List<FunctionDto> GetFunctionsUserCanGrant(int grantorUserId);
    }

    public class PermissionService : IPermissionService
    {
        private readonly IBaseCommand<UserFunction> _userFunctionCommand;
        private readonly IBaseCommand<Function> _functionCommand;

        public PermissionService(
            IBaseCommand<UserFunction> userFunctionCommand,
            IBaseCommand<Function> functionCommand)
        {
            _userFunctionCommand = userFunctionCommand;
            _functionCommand = functionCommand;
        }

        public bool GrantPermission(int grantorUserId, GrantPermissionRequest request)
        {
            try
            {
                // Lấy danh sách functionId mà người gán được phép gán
                var grantableFunctionIds = GetFunctionsUserCanGrant(grantorUserId)
                                           .Select(f => f.FunctionId)
                                           .ToHashSet();

                foreach (var functionId in request.FunctionIds)
                {
                    // Nếu không có quyền thì skip
                    if (!grantableFunctionIds.Contains(functionId))
                        continue;

                    // Nếu đã có quyền rồi thì skip
                    var exists = _userFunctionCommand.FindByCondition(x =>
                        x.UserId == request.TargetUserId &&
                        x.FunctionId == functionId
                    ).Any();

                    if (!exists)
                    {
                        var uf = new UserFunction
                        {
                            UserId = request.TargetUserId,
                            FunctionId = functionId,
                            GrantorId = grantorUserId,
                            GrantAt = DateTime.UtcNow
                        };
                        _userFunctionCommand.Create(uf);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public bool RevokePermission(int grantorUserId, RevokePermissionRequest request)
        {
            try
            {
                var userFunction = _userFunctionCommand.FindByCondition(x =>
                                        x.UserId == request.TargetUserId &&
                                        x.FunctionId == request.FunctionId
                                    ).FirstOrDefault();

                if (userFunction != null)
                {
                    // Có thể kiểm tra nếu chỉ cho phép người đã gán mới được thu hồi:
                    // if (userFunction.GrantorId != grantorUserId) return false;
                    userFunction.IsDeleted = true;
                    userFunction.UpdateAt = DateTime.UtcNow;
                    _userFunctionCommand.UpdateByEntity(userFunction);
                }

                return true;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }   
        }

        public List<FunctionDto> GetPermissionsOfUser(int targetUserId)
        {
            try
            {
                var functionIds = _userFunctionCommand.FindByCondition(x => x.UserId == targetUserId)
                                                      .Select(x => x.FunctionId)
                                                      .ToList();

                var functions = _functionCommand.FindByCondition(f => functionIds.Contains(f.FunctionId))
                                                .Select(f => new FunctionDto
                                                {
                                                    FunctionId = f.FunctionId,
                                                    FunctionCode = f.FunctionCode,
                                                    FunctionName = f.FunctionName
                                                }).ToList();

                return functions;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public List<FunctionDto> GetFunctionsUserCanGrant(int grantorUserId)
        {
            try
            {
                // Có thể sửa lại sau nếu dùng logic phân cấp hoặc group quyền
                return GetPermissionsOfUser(grantorUserId);
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }
    }
}
