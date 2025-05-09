using Wpf.Ui.Abstractions.Controls;
using yd_pmsApp1.ViewModels.Pages;

namespace yd_pmsApp1.Views.Pages
{
    public partial class SettingsPage : INavigableView<SettingsViewModel>
    {
        public SettingsViewModel ViewModel { get; }

        public SettingsPage(SettingsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
