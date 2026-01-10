using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using zjq.Services;
using zjq.Models;

namespace zjq.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly SelfRescuerService _selfRescuerService;

        [ObservableProperty]
        private string statusText;

        [ObservableProperty]
        private int totalRescuersCount;

        [ObservableProperty]
        private int expiredRescuersCount;

        [ObservableProperty]
        private int maintenanceRescuersCount;

        [ObservableProperty]
        private int normalRescuersCount;

        public MainPageViewModel(SelfRescuerService selfRescuerService)
        {
            _selfRescuerService = selfRescuerService;
            Title = "自救器管理系统";
            _ = LoadSystemStatusAsync();
        }

        private async Task LoadSystemStatusAsync()
        {
            try
            {
                IsBusy = true;
                var allRescuers = await _selfRescuerService.GetAllSelfRescuersAsync();
                var expiredRescuers = await _selfRescuerService.GetExpiredSelfRescuersAsync();
                var maintenanceRescuers = await _selfRescuerService.GetSelfRescuersNeedingMaintenanceAsync();

                TotalRescuersCount = allRescuers.Count;
                ExpiredRescuersCount = expiredRescuers.Count;
                MaintenanceRescuersCount = maintenanceRescuers.Count;
                NormalRescuersCount = allRescuers.Count - expiredRescuers.Count - maintenanceRescuers.Count;

                StatusText = $"总自救器数: {TotalRescuersCount}\n" +
                             $"过期数量: {ExpiredRescuersCount}\n" +
                             $"需要维护: {MaintenanceRescuersCount}\n" +
                             $"正常状态: {NormalRescuersCount}";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading system status: {ex.Message}");
                StatusText = "加载系统状态时发生错误";
                TotalRescuersCount = 0;
                ExpiredRescuersCount = 0;
                MaintenanceRescuersCount = 0;
                NormalRescuersCount = 0;
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ViewAllSelfRescuersAsync()
        {
            await Shell.Current.GoToAsync("//SelfRescuerListPage");
        }

        [RelayCommand]
        private async Task AddSelfRescuerAsync()
        {
            await Shell.Current.GoToAsync("//AddSelfRescuerPage");
        }

        [RelayCommand]
        private async Task CheckExpiryAsync()
        {
            try
            {
                IsBusy = true;
                var expiredRescuers = await _selfRescuerService.GetExpiredSelfRescuersAsync();
                if (expiredRescuers.Count == 0)
                {
                    await App.Current.MainPage.DisplayAlert("过期检查", "没有发现过期的自救器", "确定");
                }
                else
                {
                    string message = $"发现 {expiredRescuers.Count} 个过期自救器:\n";
                    foreach (var rescuer in expiredRescuers)
                    {
                        string expiryDateText = "未过期";
                        if (rescuer.SelfRescueValidEnd.HasValue)
                        {
                            expiryDateText = rescuer.SelfRescueValidEnd.Value.ToString("yyyy-MM-dd");
                        }
                       if (!string.IsNullOrWhiteSpace(rescuer.SelfRescueId) && rescuer.SelfRescueId.Length >= 8)
                        {
                            try
                            {
                                string dateStr = rescuer.SelfRescueId.Substring(0, 8);
                                if (System.Text.RegularExpressions.Regex.IsMatch(dateStr, "^\\d{8}$"))
                                {
                                    if (DateTime.TryParseExact(dateStr, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                                    {
                                        expiryDateText = parsedDate.AddYears(3).ToString("yyyy-MM-dd");
                                    }
                                }
                            }
                            catch
                            {
                                // 解析失败，保持默认值
                            }
                        }
                        message += $"- {rescuer.SelfRescueId} (过期日期: {expiryDateText})\n";
                    }
                    await App.Current.MainPage.DisplayAlert("过期检查", message, "确定");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking expiry: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("错误", "检查过期自救器时发生错误", "确定");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task CheckMaintenanceAsync()
        {
            try
            {
                IsBusy = true;
                var maintenanceRescuers = await _selfRescuerService.GetSelfRescuersNeedingMaintenanceAsync();
                if (maintenanceRescuers.Count == 0)
                {
                    await App.Current.MainPage.DisplayAlert("维护提醒", "没有需要维护的自救器", "确定");
                }
                else
                {
                    string message = $"发现 {maintenanceRescuers.Count} 个需要维护的自救器:\n";
                    foreach (var rescuer in maintenanceRescuers)
                    {
                        message += $"- {rescuer.SelfRescueId} (上次维护日期: {rescuer.CheckTime?.ToString("yyyy-MM-dd") ?? "未检查"})\n";
                    }
                    await App.Current.MainPage.DisplayAlert("维护提醒", message, "确定");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error checking maintenance: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("错误", "检查需要维护的自救器时发生错误", "确定");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}