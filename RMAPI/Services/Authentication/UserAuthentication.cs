using Common.Dto;
using CoreValidation.Requests.Authentication;
using CoreValidation.ValidRequests.Authentication;
using Helper.BCryptHelper;
using Helper.NLog;
using Infrastructure.Repositories;
using RMAPI.ConfigApp;
using Shared;

namespace RMAPI.Services.Authentication
{
    public class UserAuthentication
    {
        private readonly BaseCommand<User> _baseCommand;
        private readonly ConfigJWT _jwt;

        private readonly LoginValidation _validator = new LoginValidation();

        public UserAuthentication(BaseCommand<User> baseCommand, ConfigJWT jwt)
        {
            _baseCommand = baseCommand;
            _jwt = jwt;
        }

        public UserDto Login(Login request)
        {
            try
            {
                var loginValid = _validator.Validate(request);
                if (!loginValid.IsValid)
                {
                    var errorList = loginValid.Errors.FirstOrDefault();
                    throw new Exception(errorList?.ToString());
                }
                var existedUser = _baseCommand.FindByCondition(x => x.Email == request.Email).FirstOrDefault();
                if (existedUser == null)
                    throw new Exception("Người dùng không tồn tại");
                var isValidPassword = BCryptHelper.VerifyPassword(request.Password ?? "", existedUser.Password);
                if (isValidPassword == false)
                    throw new Exception("Nhập sai thông tin tài khoản hoặc mật khẩu");
                var token = _jwt.GenerateToken(existedUser.UserId, existedUser.Email);
                UserDto user = new UserDto
                {
                    UserId = existedUser.UserId,
                    Email = existedUser.Email,
                    Name = existedUser.Name,
                    Token = token,
                    Dob = existedUser.Dob,
                    Address = existedUser.Address
                };
                return user;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }
    }
}
