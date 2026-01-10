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
        private SelfRescuerItemViewModel? selectedSelfRescuer;

        partial void OnSelectedSelfRescuerChanged(SelfRescuerItemViewModel? oldValue, SelfRescuerItemViewModel? newValue)
        {
            if (newValue != null)
            {
                _ = SelfRescuerSelectedCommand.ExecuteAsync(null);
            }
        }

        public SelfRescuerListViewModel(SelfRescuerService selfRescuerService)
        {
            _selfRescuerService = selfRescuerService;
            Title = "自救器列表";
            SelfRescuers = new ObservableCollection<SelfRescuerItemViewModel>();
        }

        /// <summary>
        /// 加载自救器数据（供页面的 OnAppearing 方法调用）
        /// </summary>
        public async Task InitializeAsync()
        {
            await LoadSelfRescuersAsync();
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

                        // 1. Calculate Expiration Date
                        DateTime expiryDate = DateTime.Now.AddYears(3); // Default fallback
                        DateTime? idExpiryDate = null;

                        // Try parsing from ID first
                        if (!string.IsNullOrWhiteSpace(r.SelfRescueId) && r.SelfRescueId.Length >= 8)
                        {
                            try
                            {
                                string dateStr = r.SelfRescueId.Substring(0, 8);
                                if (System.Text.RegularExpressions.Regex.IsMatch(dateStr, "^\\d{8}$"))
                                {
                                    if (DateTime.TryParseExact(dateStr, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                                    {
                                        idExpiryDate = parsedDate.AddYears(3);
                                        // If ValidEnd is null, use ID date as the primary expiry date for display
                                        if (!r.SelfRescueValidEnd.HasValue)
                                        {
                                            expiryDate = idExpiryDate.Value;
                                        }
                                    }
                                }
                            }
                            catch
                            {
                                // Ignore parsing errors
                            }
                        }

                        // If ValidEnd exists, it usually takes precedence for display, unless we want to show the *earliest*?
                        // Existing logic preferred ValidEnd. Let's keep ValidEnd as the primary "ExpiryDate" if present.
                        if (r.SelfRescueValidEnd.HasValue)
                        {
                            expiryDate = r.SelfRescueValidEnd.Value;
                        }

                        // 2. Determine Status
                        string statusName = "正常";

                        // Check Expired (ValidEnd < Now OR IdExpiry < Now)
                        bool isExpiredByValidEnd = r.SelfRescueValidEnd.HasValue && r.SelfRescueValidEnd.Value < DateTime.Now;
                        bool isExpiredById = idExpiryDate.HasValue && idExpiryDate.Value < DateTime.Now;

                        if (isExpiredByValidEnd || isExpiredById)
                        {
                            statusName = "过期";
                            // Ideally show the date that caused expiration if different? 
                            // For now keeping `expiryDate` logic as "ValidEnd > IdDate > Default". 
                            // But correctness might suggest showing the *actual* expired date.
                            // If expired by ID but ValidEnd is future (unlikely but possible if data conflict), showing ValidEnd might be confusing if status is "Expired".
                            // Let's stick to the user's specific request about the *logic*. display date is secondary.
                        }
                        // Check Near Expiry
                        else if (r.CheckTime.HasValue)
                        {
                            var currentDate = DateTime.Now;
                            if ((r.DeviceType == 1 && r.CheckTime.Value.AddDays(160) <= currentDate && r.CheckTime.Value.AddDays(180) >= currentDate) ||
                                (r.DeviceType == 0 && r.CheckTime.Value.AddDays(80) <= currentDate && r.CheckTime.Value.AddDays(90) >= currentDate))
                            {
                                statusName = "临期";
                            }
                        }
                        
                        rescuerViewModels.Add(new SelfRescuerItemViewModel
                        {
                            Id = r.Id,
                            SerialNumber = r.SelfRescueId,
                            Model = r.SelfRescueModel,
                            Location = r.SelfRescueCompany, // 使用厂家作为临时位置信息
                            StatusId = 1, // 默认状态为正常
                            StatusName = statusName,
                            ExpiryDate = expiryDate,
                            ExpiryDateText = $"过期日期: {expiryDate.ToString("yyyy-MM-dd")}",
                            StatusColor = GetStatusColor(statusName)
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

        private Microsoft.Maui.Graphics.Color GetStatusColor(string statusName)
        {
            switch (statusName)
            {
                case "正常":
                    return Microsoft.Maui.Graphics.Colors.LightGreen;
                case "临期":
                    return Microsoft.Maui.Graphics.Colors.Yellow;
                case "过期":
                    return Microsoft.Maui.Graphics.Colors.Red;
                case "损坏":
                    return Microsoft.Maui.Graphics.Colors.Orange;
                case "在使用中":
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
            try
            {
                System.Diagnostics.Debug.WriteLine($"SelfRescuerSelectedAsync called. SelectedSelfRescuer: {SelectedSelfRescuer}");
                
                if (SelectedSelfRescuer != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Navigating from ViewModel to detail page for SelfRescuerId: {SelectedSelfRescuer.Id}");
                    
                    // 构建包含参数的完整路由字符串，避免路由匹配歧义
                    string route = $"{nameof(Views.SelfRescuerDetailPage)}?Id={SelectedSelfRescuer.Id}";
                    
                    System.Diagnostics.Debug.WriteLine($"Navigation route: {route}");
                    
                    await Shell.Current.GoToAsync(route);
                    
                    System.Diagnostics.Debug.WriteLine("Navigation completed successfully");
                    SelectedSelfRescuer = null;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("SelectedSelfRescuer is null!");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in SelfRescuerSelectedAsync: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
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