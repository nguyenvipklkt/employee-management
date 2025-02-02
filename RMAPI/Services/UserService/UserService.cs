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

namespace RMAPI.Services.UserService
{
    public class UserService
    {
        private readonly BaseCommand<User> _baseCommand;
        private readonly ConfigJWT _jwt;
        private readonly IMapper _mapper;

        public UserService(BaseCommand<User> baseCommand, ConfigJWT jwt, IMapper mapper)
        {
            _baseCommand = baseCommand;
            _jwt = jwt;
            _mapper = mapper;
        }

        public UserDto GetProfile(int userId)
        {
            try
            {
                var user = _baseCommand.FindByCondition(x => x.UserId == userId).FirstOrDefault();
                UserDto userDto = _mapper.Map<UserDto>(user);
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
