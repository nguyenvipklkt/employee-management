using AutoMapper;
using Common.Common;
using Common.Enum;
using CoreValidation.Requests.Authentication;
using Helper.BCryptHelper;
using Helper.EmailHelper;
using Helper.NLog;
using Infrastructure.Repositories;
using Object.Dto;
using Object.Model;
using RMAPI.ConfigApp;

namespace RMAPI.Services.Authentication
{
    public class UserAuthentication
    {
        private readonly BaseCommand<User> _baseCommand;
        private readonly ConfigJWT _jwt;
        private readonly IMapper _mapper;

        private readonly EmailHelper _emailHelper;


        public UserAuthentication(BaseCommand<User> baseCommand, ConfigJWT jwt, IMapper mapper, EmailHelper emailHelper)
        {
            _baseCommand = baseCommand;
            _jwt = jwt;
            _mapper = mapper;
            _emailHelper = emailHelper;
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
                userDto.StatusAccount = CoreEnum.StatusAccountDic.ContainsKey(existedUser.IsActive)
                                           ? CoreEnum.StatusAccountDic[existedUser.IsActive]
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
                    CreateAt  = DateTime.UtcNow,
                };
                _baseCommand.Create(user);
                UserDto userDto = _mapper.Map<UserDto>(user);
                userDto.StatusAccount = CoreEnum.StatusAccountDic[user.IsActive];
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
                await _emailHelper.SendVerificationEmail(email, code);
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
                userDto.StatusAccount = CoreEnum.StatusAccountDic.ContainsKey(user.IsActive)
                                            ? CoreEnum.StatusAccountDic[user.IsActive]
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
