using Wpf.Ui.Abstractions.Controls;
using yd_pmsApp1.ViewModels.Pages;

namespace yd_pmsApp1.Views.Pages
{
    /// <summary>
    /// UserInfoPage.xaml 的交互逻辑
    /// </summary>
    public partial class UserInfoPage : INavigableView<UserInfoViewModel>
    {
        public UserInfoViewModel ViewModel { get; }
        public UserInfoPage(UserInfoViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = this;
        }
    }
}
