using Microsoft.Maui.Controls;
using zjq.ViewModels;

namespace zjq.Views
{
    public partial class SelfRescuerListPage : ContentPage
    {
        public SelfRescuerListPage(SelfRescuerListViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}