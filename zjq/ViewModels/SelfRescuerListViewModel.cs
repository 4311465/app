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
                        // 确定状态名称
                        string statusName = "正常";
                        
                        // 检查是否过期
                        if (r.SelfRescueValidEnd.HasValue && r.SelfRescueValidEnd.Value < DateTime.Now)
                        {
                            statusName = "过期";
                        }
                        // 检查是否临期
                        else if (r.CheckTime.HasValue)
                        {
                            var currentDate = DateTime.Now;
                            if ((r.DeviceType == 1 && r.CheckTime.Value.AddDays(160) <= currentDate && r.CheckTime.Value.AddDays(180) >= currentDate) ||
                                (r.DeviceType == 0 && r.CheckTime.Value.AddDays(80) <= currentDate && r.CheckTime.Value.AddDays(90) >= currentDate))
                            {
                                statusName = "临期";
                            }
                        }
                        
                        // 计算过期日期
                        DateTime expiryDate = DateTime.Now.AddYears(3);
                        if (r.SelfRescueValidEnd.HasValue)
                        {
                            expiryDate = r.SelfRescueValidEnd.Value;
                        }
                        else if (!string.IsNullOrWhiteSpace(r.SelfRescueId) && r.SelfRescueId.Length >= 8)
                        {
                            try
                            {
                                string dateStr = r.SelfRescueId.Substring(0, 8);
                                if (System.Text.RegularExpressions.Regex.IsMatch(dateStr, "^\\d{8}$"))
                                {
                                    if (DateTime.TryParseExact(dateStr, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                                    {
                                        expiryDate = parsedDate.AddYears(3);
                                    }
                                }
                            }
                            catch
                            {
                                // 解析失败，保持默认值
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