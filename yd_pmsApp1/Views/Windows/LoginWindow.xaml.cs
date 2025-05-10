using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Wpf.Ui;

namespace yd_pmsApp1.Views.Windows
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            // 注册窗口关闭事件
            Closed += LoginWindow_Closed;
        }
        private void LoginWindow_Closed(object sender, EventArgs e)
        {
            // 如果主窗口没有显示，则退出应用程序
            if (Application.Current.Windows.OfType<MainWindow>().All(w => !w.IsVisible))
            {
                Application.Current.Shutdown();
            }
        }
        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("用户名和密码不能为空。", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // 调用 API 验证登录
                string response = await ApiService.LoginAsync(username, password);

                // 解析响应数据
                var responseJson = JObject.Parse(response);
                string? code = responseJson["ANSWER"]?["HDR"]?["CODE"]?.ToString();

                if (code == "0")
                {
                    // 获取用户数据
                    var userData = responseJson["ANSWER"]?["DATA"]?.FirstOrDefault();
                    if (userData != null)
                    {
                        // 创建用户信息对象
                        var userInfo = new Models.UserInfo
                        {
                            UserId = userData["USER_ID"]?.ToString(),
                            Session = userData["SESSION"]?.ToString(),
                            Account = userData["ACCOUNT"]?.ToString(),
                            LoginAccount = username  // 保存登录时输入的账号
                        };

                        // 获取 UserService 并存储用户信息
                        var userService = App.Services.GetService<Services.UserService>();
                        userService?.SetCurrentUser(userInfo);

                        // 登录成功，打开主窗口
                        var mainWindow = App.Services.GetRequiredService<INavigationWindow>() as MainWindow;
                        mainWindow.ShowWindow();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("获取用户信息失败", "登录失败", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    // 登录失败，显示错误信息
                    string text = responseJson["ANSWER"]?["HDR"]?["TEXT"]?.ToString() ?? "登录失败";
                    MessageBox.Show(text, "登录失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // 捕获异常并显示错误信息
                MessageBox.Show($"登录时发生错误：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // 模拟点击登录按钮
                LoginButton_Click(LoginButton, new RoutedEventArgs());
            }
        }
    }
}
