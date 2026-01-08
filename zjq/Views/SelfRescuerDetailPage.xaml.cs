using Microsoft.Maui.Controls;
using zjq.ViewModels;

namespace zjq.Views
{
    public partial class SelfRescuerDetailPage : ContentPage
    {
        private readonly SelfRescuerDetailViewModel _viewModel;

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
                // 使用 Shell 导航参数
                var navigationParameters = Shell.Current.CurrentState?.Location?.Query;
                if (!string.IsNullOrEmpty(navigationParameters))
                {
                    // 直接解析查询字符串
                    var query = navigationParameters.TrimStart('?');
                    var paramPairs = query.Split('&');
                    
                    foreach (var pair in paramPairs)
                    {
                        var keyValue = pair.Split('=');
                        if (keyValue.Length == 2 && keyValue[0] == "Id" && int.TryParse(keyValue[1], out var id))
                        {
                            System.Diagnostics.Debug.WriteLine($"Received navigation parameter: Id={id}");
                            await _viewModel.LoadSelfRescuerAsync(id);
                            break;
                        }
                    }
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