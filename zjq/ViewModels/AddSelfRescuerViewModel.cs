using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using zjq.Models;
using zjq.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;

namespace zjq.ViewModels
{
    public partial class AddSelfRescuerViewModel : BaseViewModel
    {
        private readonly SelfRescuerService _selfRescuerService;

        [ObservableProperty]
        private string serialNumber;

        [ObservableProperty]
        private string model;

        [ObservableProperty]
        private string manufacturer;

        [ObservableProperty]
        private DateTime manufactureDate = DateTime.Now;

        [ObservableProperty]
        private DateTime expiryDate = DateTime.Now;

        [ObservableProperty]
        private StatusType selectedStatus;

        [ObservableProperty]
        private string location;

        [ObservableProperty]
        private string notes;

        [ObservableProperty]
        private ObservableCollection<StatusType> statusTypes;

        public AddSelfRescuerViewModel(SelfRescuerService selfRescuerService)
        {
            _selfRescuerService = selfRescuerService;
            Title = "添加自救器";
            _ = LoadStatusTypesAsync();
        }

        private async Task LoadStatusTypesAsync()
        {
            try
            {
                IsBusy = true;
                var statusTypesList = await _selfRescuerService.GetAllStatusTypesAsync();
                StatusTypes = new ObservableCollection<StatusType>(statusTypesList);
                SelectedStatus = StatusTypes.FirstOrDefault(); // 默认选择第一个状态
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading status types: {ex.Message}");
                StatusTypes = new ObservableCollection<StatusType>();
                // 创建一个默认状态，以防加载失败
                StatusTypes.Add(new StatusType { Id = 1, Name = "正常", Description = "正常可用状态" });
                SelectedStatus = StatusTypes.FirstOrDefault();
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            // 验证输入
            if (string.IsNullOrWhiteSpace(SerialNumber))
            {
                await App.Current.MainPage.DisplayAlertAsync("错误", "请输入序列号", "确定");
                return;
            }

            // 创建新的自救器对象
            var rescuer = new SelfRescuer
            {
                SerialNumber = SerialNumber,
                Model = Model,
                Manufacturer = Manufacturer,
                ManufactureDate = ManufactureDate,
                ExpiryDate = ExpiryDate,
                StatusId = SelectedStatus?.Id ?? 1,
                Location = Location,
                Notes = Notes,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            try
            {
                IsBusy = true;
                // 保存到数据库
                var result = await _selfRescuerService.AddSelfRescuerAsync(rescuer);
                if (result > 0)
                {
                    await App.Current.MainPage.DisplayAlertAsync("成功", "自救器添加成功", "确定");
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlertAsync("错误", "添加失败，请重试", "确定");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving self rescuer: {ex.Message}");
                await App.Current.MainPage.DisplayAlertAsync("错误", $"添加失败: {ex.Message}", "确定");
            }
            finally
            {
                IsBusy = false;
            }
        }


        [RelayCommand]
        private async Task CancelAsync()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        [RelayCommand]
        private async Task ScanQRCodeAsync()
        {
            try
            {
                IsBusy = true;
                
                // 检查设备是否支持条形码扫描（是否有摄像头）
                if (!ZXing.Net.Maui.BarcodeScanning.IsSupported)
                {
                    await App.Current.MainPage.DisplayAlertAsync("设备不支持", "当前设备没有摄像头，无法使用扫码功能", "确定");
                    return;
                }
                
                // 检查摄像头权限
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                    if (status != PermissionStatus.Granted)
                    {
                        await App.Current.MainPage.DisplayAlertAsync("权限不足", "需要摄像头权限才能扫码", "确定");
                        return;
                    }
                }

                // 导航到扫码页面
                var scanPage = new zjq.Views.QRScanPage();
                scanPage.ScanCompleted += (sender, result) =>
                {
                    ParseQRCodeData(result);
                };
                await App.Current.MainPage.Navigation.PushAsync(scanPage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error scanning QR code: {ex.Message}");
                await App.Current.MainPage.DisplayAlertAsync("错误", "扫码功能初始化失败，请检查摄像头权限", "确定");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ParseQRCodeData(string qrCodeText)
        {
            try
            {
                // 假设二维码数据格式为JSON
                // 示例格式: {"SerialNumber":"SR-12345","Model":"ABC-120","Manufacturer":"制造商A","ManufactureDate":"2024-01-01","ExpiryDate":"2027-01-01"}
                if (qrCodeText.StartsWith("{") && qrCodeText.EndsWith("}"))
                {
                    // 简单解析JSON (实际项目中可使用Newtonsoft.Json或System.Text.Json)
                    var data = System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, string>>(qrCodeText);
                    if (data != null)
                    {
                        if (data.TryGetValue("SerialNumber", out var serialNumber))
                            SerialNumber = serialNumber;
                        if (data.TryGetValue("Model", out var model))
                            Model = model;
                        if (data.TryGetValue("Manufacturer", out var manufacturer))
                            Manufacturer = manufacturer;
                        if (data.TryGetValue("ManufactureDate", out var manufactureDate))
                        {
                            if (DateTime.TryParse(manufactureDate, out var date))
                                ManufactureDate = date;
                        }
                        if (data.TryGetValue("ExpiryDate", out var expiryDate))
                        {
                            if (DateTime.TryParse(expiryDate, out var date))
                                ExpiryDate = date;
                        }
                    }
                }
                else
                {
                    // 如果不是JSON格式，假设只是序列号
                    SerialNumber = qrCodeText;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error parsing QR code data: {ex.Message}");
                // 如果解析失败，将整个二维码内容作为序列号
                SerialNumber = qrCodeText;
            }
        }
    }
}
