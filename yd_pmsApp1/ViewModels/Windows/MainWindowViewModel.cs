using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace yd_pmsApp1.ViewModels.Windows
{
    /// <summary>
    /// MainWindow 的 ViewModel，为主应用程序窗口提供数据绑定和逻辑。
    /// </summary>
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _applicationTitle = "yd_pmsApp1";

        /// <summary>
        /// 导航视图中显示的菜单项集合。
        /// 每个菜单项代表一个可导航到的页面。
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "Home",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            },
            new NavigationViewItem()
            {
                Content = "Data",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DataHistogram24 },
                TargetPageType = typeof(Views.Pages.DataPage)
            },
            //添加一个打印测试的页面
            new NavigationViewItem()
            {
                Content = "打印测试",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Print24 },
                TargetPageType = typeof(Views.Pages.PrintTestPage)
            }
        };

        /// <summary>
        /// 导航视图中显示的页脚菜单项集合。
        /// 通常用于设置或其他辅助操作。
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "用户",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Person24 },
                TargetPageType = typeof(Views.Pages.UserInfoPage)
            },
            new NavigationViewItem()
            {
                Content = "设置",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Settings24 },
                TargetPageType = typeof(Views.Pages.SettingsPage)
            }
        };
        /// <summary>
        /// 托盘菜单项集合，通常用于系统托盘或上下文菜单操作。
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };
    }
}
