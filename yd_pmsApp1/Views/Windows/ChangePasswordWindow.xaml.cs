using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using yd_pmsApp1.Services;
using Newtonsoft.Json.Linq;
using Wpf.Ui;
using System.Windows.Input;
using System.Windows.Controls;

namespace yd_pmsApp1.Views.Windows
{
    public partial class ChangePasswordWindow : Window
    {
        public ChangePasswordWindow()
        {
            InitializeComponent();
        }

        private async void Confirm_Click(object sender, RoutedEventArgs e)
        {
            var userService = App.Services.GetService<UserService>();
            var currentUser = userService?.CurrentUser;

            if (currentUser == null)
            {
                MessageBox.Show("用户信息不存在，无法修改密码。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string oldPassword = OldPasswordBox.Password;
            string newPassword = NewPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("所有字段均为必填项。", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("新密码与确认密码不一致。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //判断新旧密码是否相同
            if (oldPassword == newPassword)
            {
                MessageBox.Show("新密码不能与旧密码相同。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // 调用修改密码 API
                string response = await ApiService.ChangePasswordAsync(currentUser.UserId, currentUser.Session, oldPassword, newPassword);

                // 根据接口文档解析API响应
                var jsonResponse = JObject.Parse(response);
                var code = jsonResponse["ANSWER"]?["HDR"]?["CODE"]?.ToString();
                var text = jsonResponse["ANSWER"]?["HDR"]?["TEXT"]?.ToString();

                if (code == "0") // 接口文档中成功为0
                {
                    MessageBox.Show(text ?? "密码修改成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                    // 清除当前用户信息
                    userService.ClearCurrentUser();

                    // 重启应用程序
                    RestartApplication();
                }
                else
                {
                    var debugMsg = jsonResponse["ANSWER"]?["HDR"]?["DEBUG_MSG"]?.ToString();
                    MessageBox.Show($"修改密码失败：{text ?? "未知错误"} {debugMsg}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"修改密码失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show($"重启应用程序失败: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 添加PasswordBox_KeyDown事件处理方法，避免在XAML中引用不存在的方法
        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (sender == OldPasswordBox)
                {
                    NewPasswordBox.Focus();
                }
                else if (sender == NewPasswordBox)
                {
                    ConfirmPasswordBox.Focus();
                }
                else if (sender == ConfirmPasswordBox)
                {
                    Confirm_Click(sender, new RoutedEventArgs());
                }
            }
        }
    }
}
