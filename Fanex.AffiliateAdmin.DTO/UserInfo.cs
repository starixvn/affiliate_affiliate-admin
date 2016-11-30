using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.AffiliateAdmin.DTO
{
    public class UserInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserSalt { get; set; }
        public string Password { get; set; }
    }
}
