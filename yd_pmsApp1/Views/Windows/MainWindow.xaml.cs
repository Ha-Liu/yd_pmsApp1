using Wpf.Ui;
using Wpf.Ui.Abstractions;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using yd_pmsApp1.ViewModels.Windows;

namespace yd_pmsApp1.Views.Windows
{
    /// <summary>
    /// 主窗口类，继承自 <see cref="INavigationWindow"/> 接口。
    /// 提供导航功能，并与 ViewModel 绑定以支持 MVVM 模式。
    /// </summary>
    public partial class MainWindow : INavigationWindow
    {
        /// <summary>
        /// 获取与此窗口绑定的 ViewModel。
        /// </summary>
        public MainWindowViewModel ViewModel { get; }

        /// <summary>
        /// 使用依赖注入初始化 <see cref="MainWindow"/> 类的新实例。
        /// </summary>
        /// <param name="viewModel">主窗口的 ViewModel。</param>
        /// <param name="navigationViewPageProvider">导航页面提供程序。</param>
        /// <param name="navigationService">导航服务。</param>
        public MainWindow(
            MainWindowViewModel viewModel,
            INavigationViewPageProvider navigationViewPageProvider,
            INavigationService navigationService
        )
        {
            ViewModel = viewModel;
            DataContext = this;

            // 监听系统主题变化
            SystemThemeWatcher.Watch(this);

            InitializeComponent();
            SetPageService(navigationViewPageProvider);

            // 设置导航服务的导航控件
            navigationService.SetNavigationControl(RootNavigation);
        }

        #region INavigationWindow methods

        /// <summary>
        /// 获取导航控件。
        /// </summary>
        /// <returns>导航控件实例。</returns>
        public INavigationView GetNavigation() => RootNavigation;

        /// <summary>
        /// 导航到指定页面类型。
        /// </summary>
        /// <param name="pageType">目标页面的类型。</param>
        /// <returns>导航是否成功。</returns>
        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        /// <summary>
        /// 设置页面服务提供程序。
        /// </summary>
        /// <param name="navigationViewPageProvider">页面服务提供程序。</param>
        public void SetPageService(INavigationViewPageProvider navigationViewPageProvider) => RootNavigation.SetPageProviderService(navigationViewPageProvider);

        /// <summary>
        /// 显示窗口。
        /// </summary>
        public void ShowWindow() => Show();

        /// <summary>
        /// 关闭窗口。
        /// </summary>
        public void CloseWindow() => Close();

        #endregion INavigationWindow methods

        /// <summary>
        /// 重写窗口关闭事件，确保关闭应用程序。
        /// </summary>
        /// <param name="e">事件参数。</param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // 确保关闭此窗口将开始关闭应用程序的过程。
            Console.WriteLine("MainWindow.OnClosed called"); // 添加日志
            try
            {
                Application.Current.Shutdown();
                Console.WriteLine("Application.Current.Shutdown called"); // 添加日志
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in MainWindow.OnClosed: {ex}"); // 添加日志
            }
        }

        /// <summary>
        /// 获取导航控件（未实现）。
        /// </summary>
        /// <returns>导航控件实例。</returns>
        INavigationView INavigationWindow.GetNavigation()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 设置服务提供程序（未实现）。
        /// </summary>
        /// <param name="serviceProvider">服务提供程序实例。</param>
        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
}
