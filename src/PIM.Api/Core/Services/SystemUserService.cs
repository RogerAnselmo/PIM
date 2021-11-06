using System.Threading.Tasks;
using PIM.Api.Core.Models;
using PIM.Api.Data.Repositories;
using PIM.Api.TransferObjects.Responses.Base;

namespace PIM.Api.Core.Services
{
    public class SystemUserService
    {
        private readonly SystemUserRepository _systemUserRepository;

        public SystemUserService(SystemUserRepository systemUserRepository)
        {
            _systemUserRepository = systemUserRepository;
        }

        public async Task<BaseResponse> SaveAsync(SystemUser user)
        {
            var userWithSameUserName = await _systemUserRepository.GetByUserName(user.UserName);
            if (userWithSameUserName != null)
                return new BaseResponse($"UserName {user.UserName} is already on use", false);

            await _systemUserRepository.SaveAndCommitAsync(user);
            return new BaseResponse("User successfully created ", true);
        }
    }
}
