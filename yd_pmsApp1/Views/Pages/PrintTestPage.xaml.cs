using Wpf.Ui.Abstractions.Controls;
using yd_pmsApp1.ViewModels.Pages;

namespace yd_pmsApp1.Views.Pages
{
    /// <summary>
    /// PrintTestPage.xaml 的交互逻辑
    /// </summary>
    public partial class PrintTestPage : INavigableView<PrintTestViewModel>
    {
        public PrintTestViewModel ViewModel { get; }

        public PrintTestPage(PrintTestViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
            DataContext = this;
        }
    }
}