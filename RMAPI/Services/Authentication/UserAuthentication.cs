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
using RMAPI.ServiceRegistration;
using Common.Common;
using CoreValidation.ValidatorFunc;
using System.IdentityModel.Tokens.Jwt;

namespace RMAPI.Services.Authentication
{
    public class UserAuthentication
    {
        private readonly BaseCommand<User> _baseCommand;
        private readonly ConfigJWT _jwt;
        private readonly IMapper _mapper;

        private readonly EmailRegistration _emailRegistration;


        public UserAuthentication(BaseCommand<User> baseCommand, ConfigJWT jwt, IMapper mapper, EmailRegistration emailRegistration)
        {
            _baseCommand = baseCommand;
            _jwt = jwt;
            _mapper = mapper;
            _emailRegistration = emailRegistration;
        }

        public UserDto Login(Login request)
        {
            try
            {
                var existedUser = _baseCommand.FindByCondition(x => x.Email == request.Email).FirstOrDefault();
                if (existedUser == null)
                    throw new Exception("Người dùng không tồn tại");
                var isValidPassword = BCryptHelper.VerifyPassword(request.Password ?? "", existedUser.Password);
                if (isValidPassword == false)
                    throw new Exception("Nhập sai thông tin tài khoản hoặc mật khẩu");
                if (existedUser.IsActive != 1)
                    throw new Exception("Tài khoản chưa được xác minh.");
                var token = _jwt.GenerateToken(existedUser.UserId, existedUser.Email);

                // Thêm đoạn sinh refresh token và lưu lại
                var refreshToken = _jwt.GenerateRandomToken();
                existedUser.RefreshToken = refreshToken;
                existedUser.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7); // token sống 7 ngày
                _baseCommand.UpdateByEntity(existedUser);

                UserDto userDto = _mapper.Map<UserDto>(existedUser);
                userDto.StatusAccount = Dictionary.StatusAccountDic.ContainsKey(existedUser.IsActive)
                                           ? Dictionary.StatusAccountDic[existedUser.IsActive]
                                           : "Không xác định";
                userDto.Token = token;
                userDto.RefreshToken = refreshToken;
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

        public User VerifyCode(VerifyCode request)
        {
            try
            {
                var user = _baseCommand.FindByCondition(u => u.Email == request.Email && u.OTP == request.VerificationCode).FirstOrDefault();
                if (user == null)
                    throw new Exception("Mã xác thực không chính xác hoặc email không tồn tại.");

                user.IsActive = 1;
                user.OTP = string.Empty;
                _baseCommand.UpdateByEntity(user);

                return user;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public async Task<bool> SendEmail(string email)
        {
            try
            {
                var user = _baseCommand.FindByCondition(u => u.Email == email).FirstOrDefault();
                if (user == null)
                    throw new Exception("Tài khoản gmail này chưa được đăng ký.");
                string code = CommonFunc.GenerateOTP();
                user.OTP = code;
                _baseCommand.UpdateByEntity(user);
                await _emailRegistration.SendVerificationEmail(email, code);
                return true;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public UserDto RefreshToken(RefreshRequestToken request, int userId)
        {
            try
            {
                // Tìm user theo email và refresh token hợp lệ
                var user = _baseCommand.FindByCondition(u =>
                    u.UserId == userId &&
                    u.RefreshToken == request.RefreshToken &&
                    u.RefreshTokenExpiry > DateTime.UtcNow
                ).FirstOrDefault();

                if (user == null)
                    throw new Exception("Refresh token không hợp lệ hoặc đã hết hạn.");

                // Tạo access token mới và refresh token mới
                var newAccessToken = _jwt.GenerateToken(user.UserId, user.Email);
                var newRefreshToken = _jwt.GenerateRandomToken();

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
                _baseCommand.UpdateByEntity(user);

                // Trả về UserDto chứa token mới
                var userDto = _mapper.Map<UserDto>(user);
                userDto.Token = newAccessToken;
                userDto.RefreshToken = newRefreshToken;
                userDto.StatusAccount = Dictionary.StatusAccountDic.ContainsKey(user.IsActive)
                                            ? Dictionary.StatusAccountDic[user.IsActive]
                                            : "Không xác định";
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
