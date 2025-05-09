using Wpf.Ui.Abstractions.Controls;
using yd_pmsApp1.ViewModels.Pages;

namespace yd_pmsApp1.Views.Pages
{
    public partial class DashboardPage : INavigableView<DashboardViewModel>
    {
        public DashboardViewModel ViewModel { get; }

        public DashboardPage(DashboardViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
