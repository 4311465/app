using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using zjq.Services;

namespace zjq.ViewModels
{
    public partial class SelfRescuerListViewModel : BaseViewModel
    {
        private readonly SelfRescuerService _selfRescuerService;

        [ObservableProperty]
        private ObservableCollection<SelfRescuerItemViewModel> selfRescuers;

        [ObservableProperty]
        private string searchText;

        [ObservableProperty]
        private SelfRescuerItemViewModel selectedSelfRescuer;

        public SelfRescuerListViewModel(SelfRescuerService selfRescuerService)
        {
            _selfRescuerService = selfRescuerService;
            Title = "自救器列表";
            _ = LoadSelfRescuersAsync();
        }

        private async Task LoadSelfRescuersAsync()
        {
            try
            {
                IsBusy = true;
                var rescuers = await _selfRescuerService.GetAllSelfRescuersAsync();
                var rescuerViewModels = new List<SelfRescuerItemViewModel>();
                
                foreach (var r in rescuers)
                {
                    try
                    {
                        var statusName = await _selfRescuerService.GetStatusNameAsync(r.StatusId);
                        rescuerViewModels.Add(new SelfRescuerItemViewModel
                        {
                            Id = r.Id,
                            SerialNumber = r.SerialNumber,
                            Model = r.Model,
                            Location = r.Location,
                            StatusId = r.StatusId,
                            StatusName = statusName,
                            ExpiryDate = r.ExpiryDate,
                            ExpiryDateText = $"过期日期: {r.ExpiryDate.ToString("yyyy-MM-dd")}",
                            StatusColor = GetStatusColor(r.StatusId)
                        });
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error creating self rescuer view model: {ex.Message}");
                    }
                }

                SelfRescuers = new ObservableCollection<SelfRescuerItemViewModel>(rescuerViewModels);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading self rescuers: {ex.Message}");
                SelfRescuers = new ObservableCollection<SelfRescuerItemViewModel>();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private Microsoft.Maui.Graphics.Color GetStatusColor(int statusId)
        {
            switch (statusId)
            {
                case 1: // 正常
                    return Microsoft.Maui.Graphics.Colors.LightGreen;
                case 2: // 需要维护
                    return Microsoft.Maui.Graphics.Colors.Yellow;
                case 3: // 过期
                    return Microsoft.Maui.Graphics.Colors.Red;
                case 4: // 损坏
                    return Microsoft.Maui.Graphics.Colors.Orange;
                case 5: // 在使用中
                    return Microsoft.Maui.Graphics.Colors.LightBlue;
                default:
                    return Microsoft.Maui.Graphics.Colors.White;
            }
        }

        [RelayCommand]
        private async Task AddSelfRescuerAsync()
        {
            await Shell.Current.GoToAsync("//AddSelfRescuerPage");
        }

        [RelayCommand]
        private async Task SelfRescuerSelectedAsync()
        {
            if (SelectedSelfRescuer != null)
            {
                await Shell.Current.GoToAsync($"//SelfRescuerDetailPage?Id={SelectedSelfRescuer.Id}");
                SelectedSelfRescuer = null;
            }
        }

        partial void OnSearchTextChanged(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _ = LoadSelfRescuersAsync();
            }
            else
            {
                var filteredRescuers = SelfRescuers.Where(r =>
                    r.SerialNumber.ToLower().Contains(value.ToLower()) ||
                    (r.Model != null && r.Model.ToLower().Contains(value.ToLower())) ||
                    (r.Location != null && r.Location.ToLower().Contains(value.ToLower()))).ToList();
                
                SelfRescuers = new ObservableCollection<SelfRescuerItemViewModel>(filteredRescuers);
            }
        }

        [RelayCommand]
        private async Task RefreshSelfRescuersAsync()
        {
            await LoadSelfRescuersAsync();
        }
    }
}