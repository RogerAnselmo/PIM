using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PIM.Api.Core.Models;
using PIM.Api.Data.Context;
using PIM.Api.Data.Repositories.Base;

namespace PIM.Api.Data.Repositories
{
    public class SystemUserRepository : BaseRepository<SystemUser>
    {
        public SystemUserRepository(ApplicationContext db) : base(db)
        {
        }

        public Task<SystemUser> GetByUserName(string userName) =>
            Db.SystemUsers.Where(x => x.UserName.ToUpper()
                    .Equals(userName.ToUpper()))
                .FirstOrDefaultAsync();

        public Task<SystemUser> GetByUserNameAndPassword(string userName,
            string password) =>
            Db.SystemUsers.Where(x => x.UserName.ToUpper()
                                          .Equals(userName.ToUpper()) &&
                                      x.Password.Equals(password))
                .FirstOrDefaultAsync();
    }
}
