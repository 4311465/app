using Microsoft.Maui.Controls;
using zjq.ViewModels;
using zjq.Services;
using zjq.Repositories;

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