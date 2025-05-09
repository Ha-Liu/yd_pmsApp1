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

            try
            {
                // 调用修改密码 API
                string response = await ApiService.ChangePasswordAsync(currentUser.UserId, currentUser.Session, oldPassword, newPassword);
                MessageBox.Show("密码修改成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);

                // 关闭修改密码窗口
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"修改密码失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
