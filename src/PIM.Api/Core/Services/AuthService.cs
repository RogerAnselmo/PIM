using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PIM.Api.Data.Repositories;
using PIM.Api.Infra.AuthEngine.Interface;
using PIM.Api.TransferObjects.Requests;
using PIM.Api.TransferObjects.Responses;

namespace PIM.Api.Core.Services
{
    public class AuthService
    {
        private readonly ITokenProvider _jwtProvider;
        private readonly SystemUserRepository _userRepository;
        private readonly string _hash;

        public AuthService(IConfiguration configuration,ITokenProvider jwtProvider, SystemUserRepository userRepository)
        {
            _jwtProvider = jwtProvider;
            _userRepository = userRepository;
            _hash = configuration.GetSection("token:Hash").Value;
        }

        public virtual async Task<AuthenticatedUser> Authenticate(LoginRequestModel loginInfo)
        {
            var user = await _userRepository.GetByUserNameAndPassword(loginInfo.UserName, loginInfo.Password);

            if (user == null)
                return new AuthenticatedUser("UserName or password incorrect", false);

            var token = _jwtProvider.GenerateToken(_hash, user);

            return new AuthenticatedUser(user.UserName, token);
        }
    }
}
