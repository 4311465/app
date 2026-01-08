using Microsoft.Maui.Controls;
using zjq.ViewModels;

namespace zjq.Views
{
    [QueryProperty(nameof(Id), "Id")]
    public partial class SelfRescuerDetailPage : ContentPage
    {
        private readonly SelfRescuerDetailViewModel _viewModel;

        public string Id { get; set; }

        public SelfRescuerDetailPage(SelfRescuerDetailViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            
            try
            {
                System.Diagnostics.Debug.WriteLine($"Received navigation parameter: Id={Id}");
                
                int id = 0;
                if (!string.IsNullOrEmpty(Id) && int.TryParse(Id, out id) && id > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Extracted Id: {id}");
                    await _viewModel.LoadSelfRescuerAsync(id);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Invalid Id parameter: {Id}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in OnNavigatedTo: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }
    }
}