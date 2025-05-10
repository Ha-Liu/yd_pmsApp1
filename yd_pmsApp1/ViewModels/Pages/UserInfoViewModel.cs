using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using yd_pmsApp1.Services;
using yd_pmsApp1.Views.Windows;
using Wpf.Ui.Abstractions.Controls;
using Newtonsoft.Json.Linq;

namespace yd_pmsApp1.ViewModels.Pages
{
    public partial class UserInfoViewModel : ObservableObject, INavigationAware
    {
        private readonly UserService _userService;

        [ObservableProperty]
        private string? _userAccount = string.Empty;

        [ObservableProperty]
        private string? _userId = string.Empty;

        [ObservableProperty]
        private string? _loginAccount = string.Empty;

        public UserInfoViewModel(UserService userService)
        {
            _userService = userService;
            LoadUserInfo();
        }

        public async Task OnNavigatedFromAsync()
        {
            // 页面离开逻辑
            await Task.CompletedTask;
        }

        public async Task OnNavigatedToAsync()
        {
            // 页面加载逻辑
            LoadUserInfo();
            await Task.CompletedTask;
        }

        private void LoadUserInfo()
        {
            var currentUser = _userService.CurrentUser;
            if (currentUser != null)
            {
                UserAccount = currentUser.Account;
                UserId = currentUser.UserId;
                LoginAccount = currentUser.LoginAccount;
            }
            else
            {
                UserAccount = "未登录";
                UserId = "未知";
                LoginAccount = "未登录";
            }
        }

        [RelayCommand]
        private void ChangePassword()
        {
            try
            {
                // 每次创建新的 ChangePasswordWindow 实例
                var changePasswordWindow = new ChangePasswordWindow
                {
                    Owner = Application.Current.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                // 显示窗口
                changePasswordWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"打开修改密码窗口失败: {ex.Message}", "错误",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task LogoutAsync()
        {
            // 确认用户是否要登出
            var result = MessageBox.Show("确定要退出当前账号吗？", "确认退出",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var currentUser = _userService.CurrentUser;
                    if (currentUser == null)
                    {
                        MessageBox.Show("用户未登录，无需退出。", "提示",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    // 调用登出 API
                    var response = await ApiService.LogoutAsync(currentUser.UserId, currentUser.Session);

                    // 根据接口文档解析API响应
                    var jsonResponse = JObject.Parse(response);
                    var code = jsonResponse["ANSWER"]?["HDR"]?["CODE"]?.ToString();
                    var text = jsonResponse["ANSWER"]?["HDR"]?["TEXT"]?.ToString();

                    if (code == "0") // 接口文档中成功为0
                    {
                        // 清除当前用户信息
                        _userService.ClearCurrentUser();

                        // 显示成功消息
                        MessageBox.Show($"{text ?? "已成功退出账号"}", "提示",
                            MessageBoxButton.OK, MessageBoxImage.Information);

                        // 重启应用程序
                        RestartApplication();
                    }
                    else
                    {
                        var debugMsg = jsonResponse["ANSWER"]?["HDR"]?["DEBUG_MSG"]?.ToString();
                        MessageBox.Show($"退出失败: {text ?? "未知错误"} {debugMsg}", "错误",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"退出账号失败: {ex.Message}", "错误",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /// <summary>
        /// 重启应用程序
        /// </summary>
        private void RestartApplication()
        {
            // 获取当前应用程序路径
            string appPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

            // 创建新进程启动信息
            var startInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = appPath,
                UseShellExecute = true
            };

            try
            {
                // 启动新实例
                System.Diagnostics.Process.Start(startInfo);

                // 关闭当前应用程序
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"重启应用程序失败: {ex.Message}", "错误",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
