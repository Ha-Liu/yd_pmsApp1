using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using yd_pmsApp1.Models;

namespace yd_pmsApp1.Services
{
    // Services/UserService.cs
    /// <summary>
    /// 提供用户会话信息的管理服务。
    /// </summary>
    public class UserService
    {
        /// <summary>
        /// 当前登录用户信息。
        /// </summary>
        private UserInfo _currentUser;

        /// <summary>
        /// 获取或设置当前登录用户信息。
        /// 当设置新的用户信息时，会触发 CurrentUserChanged 事件。
        /// </summary>
        public UserInfo CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                CurrentUserChanged?.Invoke(this, _currentUser);
            }
        }

        /// <summary>
        /// 当当前用户信息更改时触发的事件。
        /// 订阅此事件可以监听用户登录或注销等状态变化。
        /// </summary>
        public event EventHandler<UserInfo> CurrentUserChanged;

        /// <summary>
        /// 设置当前登录用户的信息。
        /// </summary>
        /// <param name="user">要设置为当前用户的 UserInfo 对象。</param>
        public void SetCurrentUser(UserInfo user)
        {
            CurrentUser = user;
        }

        /// <summary>
        /// 清除当前登录用户信息，通常在用户注销时调用。
        /// </summary>
        public void ClearCurrentUser()
        {
            CurrentUser = null;
        }
    }
}