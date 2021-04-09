using System;
using System.Threading.Tasks;
using Authentication_Service.Dal.Interface;
using Authentication_Service.Models.Dto;

namespace Authentication_Service.Dal
{
    public class UserDal : IUserDal
    {
        public UserDal(DataContext context)
        {

        }

        public async Task<UserDto> Find(string username)
        {
            throw new NotImplementedException();
        }

        public async Task Delete(Guid uuid)
        {
            throw new NotImplementedException();
        }
    }
}