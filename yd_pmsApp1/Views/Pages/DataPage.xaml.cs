using Wpf.Ui.Abstractions.Controls;
using yd_pmsApp1.ViewModels.Pages;

namespace yd_pmsApp1.Views.Pages
{
    public partial class DataPage : INavigableView<DataViewModel>
    {
        public DataViewModel ViewModel { get; }

        public DataPage(DataViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
