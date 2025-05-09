using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yd_pmsApp1.Models
{
    // Models/UserInfo.cs
    public class UserInfo
    {
        public string UserId { get; set; }
        public string Session { get; set; }
        public string Account { get; set; }
        public string LoginAccount { get; set; }  // 登录时输入的账号
    }

}
