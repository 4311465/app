using Microsoft.Maui.Controls;
using zjq.ViewModels;

namespace zjq.Views
{
    public partial class AddSelfRescuerPage : ContentPage
    {
        public AddSelfRescuerPage(AddSelfRescuerViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}