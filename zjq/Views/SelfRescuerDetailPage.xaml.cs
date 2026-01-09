using Microsoft.Maui.Controls;
using zjq.ViewModels;

namespace zjq.Views
{
    [QueryProperty(nameof(SelfRescuerId), "Id")]
    public partial class SelfRescuerDetailPage : ContentPage
    {
        private readonly SelfRescuerDetailViewModel _viewModel;

        public string SelfRescuerId { get; set; }

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
                System.Diagnostics.Debug.WriteLine($"Received navigation parameter: Id={SelfRescuerId}");
                
                int id = 0;
                if (!string.IsNullOrEmpty(SelfRescuerId) && int.TryParse(SelfRescuerId, out id) && id > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"Extracted Id: {id}");
                    await _viewModel.LoadSelfRescuerAsync(id);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Invalid Id parameter: {SelfRescuerId}");
                    // 使用 QueryProperty 属性已经足够获取参数
                    // 不再需要尝试从 route parameter 中获取
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