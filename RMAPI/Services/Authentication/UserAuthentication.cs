using AutoMapper;
using Common.Dto;
using CoreValidation.Requests.Authentication;
using CoreValidation.ValidRequests.Authentication;
using Helper.BCryptHelper;
using Helper.NLog;
using Helper.Dictionary;
using Infrastructure.Repositories;
using RMAPI.ConfigApp;
using Shared;

namespace RMAPI.Services.Authentication
{
    public class UserAuthentication
    {
        private readonly BaseCommand<User> _baseCommand;
        private readonly ConfigJWT _jwt;
        private readonly IMapper _mapper;

        private readonly LoginValidation _loginValidator = new LoginValidation();
        private readonly RegisterValidation _registerValidator = new RegisterValidation();

        public UserAuthentication(BaseCommand<User> baseCommand, ConfigJWT jwt, IMapper mapper)
        {
            _baseCommand = baseCommand;
            _jwt = jwt;
            _mapper = mapper;
        }

        public UserDto Login(Login request)
        {
            try
            {
                var loginValid = _loginValidator.Validate(request);
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
                if (existedUser.IsActive != 1)
                    throw new Exception("Tài khoản chưa được xác minh.");
                var token = _jwt.GenerateToken(existedUser.UserId, existedUser.Email);
                UserDto userDto = _mapper.Map<UserDto>(existedUser);
                userDto.StatusAccount = Dictionary.StatusAccountDic.ContainsKey(existedUser.IsActive)
                                           ? Dictionary.StatusAccountDic[existedUser.IsActive]
                                           : "Không xác định";
                userDto.Token = token;
                return userDto;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public UserDto Register(Register request)
        {
            try
            {
                var RegisterValid = _registerValidator.Validate(request);
                if (!RegisterValid.IsValid)
                {
                    var errorList = RegisterValid.Errors.FirstOrDefault();
                    throw new Exception(errorList?.ToString());
                }
                var existedUser = _baseCommand.FindByCondition(x => x.Email == request.Email).FirstOrDefault();
                if (existedUser != null)
                    throw new Exception("Email đã được sử dụng");
                User user = new User()
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = BCryptHelper.HashPassword(request.Password),
                    IsActive = 0,
                    RoleId = 2,
                };
                _baseCommand.Create(user);
                UserDto userDto = _mapper.Map<UserDto>(user);
                userDto.StatusAccount = Dictionary.StatusAccountDic[user.IsActive];
                return userDto;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }
    }
}
