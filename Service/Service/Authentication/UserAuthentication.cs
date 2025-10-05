using AutoMapper;
using Common.Common;
using Common.Enum;
using Common.Enum.ErrorEnum;
using CoreValidation.Requests.Authentication;
using Helper.BCryptHelper;
using Helper.EmailHelper;
using Helper.NLog;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Object.Dto;
using Object.Model;
using Service.Config;
using Service.Worker;

namespace Service.Service.Authentication
{
    public interface IUserAuthentication
    {
        UserDto Login(Login request, string ipAddress);
        UserDto Register(Register request);
        User VerifyCode(VerifyCode request);
        Task<bool> SendEmail(string email);
        UserDto RefreshAccessToken(RefreshRequestToken request, int userId);
    }
        
    public class UserAuthentication : IUserAuthentication
    {
        private readonly IBaseCommand<User> _baseUserCommand;
        private readonly IBaseCommand<OTP> _baseOTPCommand;
        private readonly IBaseCommand<RefreshToken> _baseRefreshTokenCommand;
        private readonly ConfigJWT _jwt;
        private readonly IMapper _mapper;

        private readonly EmailHelper _emailHelper;


        public UserAuthentication(IBaseCommand<User> baseUserCommand, IBaseCommand<OTP> baseOTPCommand, ConfigJWT jwt, IMapper mapper, EmailHelper emailHelper, IBaseCommand<RefreshToken> baseRefreshTokenCommand)
        {
            _baseUserCommand = baseUserCommand;
            _baseOTPCommand = baseOTPCommand;
            _jwt = jwt;
            _mapper = mapper;
            _emailHelper = emailHelper;
            _baseRefreshTokenCommand = baseRefreshTokenCommand;
        }

        public UserDto Login(Login request, string ipAddress)
        {
            try
            {
                var existedUser = _baseUserCommand.FindByCondition(x => x.Email == request.Email).FirstOrDefault();
                if (existedUser == null)
                    throw new Exception(ErrorMem.GetErrorNameByCode(UserError.USER_NOT_FOUND));
                var isValidPassword = BCryptHelper.VerifyPassword(request.Password ?? "", existedUser.Password);
                if (isValidPassword == false)
                    throw new Exception("Nhập sai thông tin tài khoản hoặc mật khẩu");
                if (existedUser.IsActive != 1)
                    throw new Exception("Tài khoản chưa được xác minh.");

                var token = _jwt.GenerateToken(existedUser.UserId, existedUser.Email);

                // Thêm đoạn sinh refresh token và lưu lại
                var newRefreshToken = _jwt.GenerateRandomToken();

                // Revoke all old tokens
                var oldTokens = _baseRefreshTokenCommand.FindByCondition(r =>
                    r.UserId == existedUser.UserId && r.IsActive == 1).ToList();

                foreach (var t in oldTokens)
                {
                    t.IsActive = 0;
                    t.RevokedAt = DateTime.UtcNow;
                    t.ReplacedByToken = newRefreshToken; // hoặc null nếu không cần
                    _baseRefreshTokenCommand.UpdateByEntity(t);
                }

                var newRefresh = new RefreshToken
                {
                    UserId = existedUser.UserId,
                    Token = newRefreshToken,
                    CreateAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(7),
                    IsActive = 1,
                    CreatedByIp = ipAddress
                };

                _baseRefreshTokenCommand.Create(newRefresh);

                UserDto userDto = _mapper.Map<UserDto>(existedUser);
                userDto.StatusAccount = CoreEnum.StatusAccountDic.ContainsKey(existedUser.IsActive)
                                           ? CoreEnum.StatusAccountDic[existedUser.IsActive]
                                           : "Không xác định";
                userDto.Token = token;
                userDto.RefreshToken = newRefreshToken;
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
                var existedUser = _baseUserCommand.FindByCondition(x => x.Email == request.Email).FirstOrDefault();
                if (existedUser != null)
                    throw new Exception("Email đã được sử dụng");
                User user = new User()
                {
                    Name = request.Name,
                    Email = request.Email,
                    Password = BCryptHelper.HashPassword(request.Password),
                    IsActive = 0,
                    RoleId = 2,
                    CreateAt = DateTime.UtcNow,
                };
                _baseUserCommand.Create(user);
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
                var now = DateTime.UtcNow;

                var expiredOtps = _baseOTPCommand
                    .FindByCondition(o => o.OTPType == "AU"
                                      && o.Status == 1
                                      && o.ExpiryTime < now)
                    .ToList();
                foreach (var e in expiredOtps)
                {
                    e.Status = 0;
                    _baseOTPCommand.UpdateByEntity(e);
                }

                var otp = _baseOTPCommand
                    .FindByCondition(o => o.Description == request.VerificationCode
                                      && o.OTPType == "AU"
                                      && o.Status == 1
                                      && o.ExpiryTime >= now)
                    .OrderByDescending(o => o.ExpiryTime)
                    .FirstOrDefault();

                if (otp == null)
                    throw new Exception("Mã xác thực không chính xác hoặc đã hết hạn.");

                otp.Status = 0;
                _baseOTPCommand.UpdateByEntity(otp);

                var siblingOtps = _baseOTPCommand
                    .FindByCondition(o => o.UserId == otp.UserId
                                      && o.OTPType == "AU"
                                      && o.Status == 1)
                    .AsTracking()
                    .ToList();
                foreach (var s in siblingOtps)
                {
                    s.Status = 0;
                    _baseOTPCommand.UpdateByEntity(s);
                }

                var user = _baseUserCommand
                    .FindByCondition(u => u.UserId == otp.UserId)
                    .FirstOrDefault();
                if (user == null)
                    throw new Exception("Người dùng không tồn tại.");

                user.IsActive = 1;
                _baseUserCommand.UpdateByEntity(user);

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
                var user = _baseUserCommand
                    .FindByCondition(u => u.Email == email)
                    .FirstOrDefault();
                if (user == null)
                    throw new Exception("Tài khoản gmail này chưa được đăng ký.");

                var oldOtps = _baseOTPCommand
                    .FindByCondition(o => o.UserId == user.UserId
                                      && o.OTPType == "AU"
                                      && o.Status == 1)
                    .ToList();
                foreach (var o in oldOtps)
                {
                    o.Status = 0;
                    _baseOTPCommand.UpdateByEntity(o);
                }

                string code = CommonFunc.GenerateOTP();
                var newOtp = new OTP
                {
                    UserId = user.UserId,
                    Description = code,
                    ExpiryTime = DateTime.UtcNow.AddMinutes(5),
                    OTPType = "AU",
                    Status = 1
                };
                _baseOTPCommand.Create(newOtp);

                await _emailHelper.SendVerificationEmail(email, code);
                return true;
            }
            catch (Exception ex)
            {
                BaseNLog.logger.Error(ex);
                throw;
            }
        }

        public UserDto RefreshAccessToken(RefreshRequestToken request, int userId)
        {
            try
            {
                var oldToken = _baseRefreshTokenCommand.FindByCondition(r => r.Token == request.RefreshToken && r.UserId == userId && r.IsActive == 1 && r.ExpiresAt > DateTime.UtcNow).FirstOrDefault();

                if (oldToken == null)
                    throw new Exception("Refresh token không hợp lệ hoặc đã hết hạn.");

                var user = _baseUserCommand.FindOrFail(userId);
                var newAccessToken = _jwt.GenerateToken(user.UserId, user.Email);

                var userDto = _mapper.Map<UserDto>(user);
                userDto.Token = newAccessToken;
                userDto.RefreshToken = request.RefreshToken; // Giữ nguyên token cũ
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
