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
        private string selfRescueId;

        [ObservableProperty]
        private string selfRescueModel;

        [ObservableProperty]
        private string selfRescueCompany;

        [ObservableProperty]
        private string selfRescueName;

        [ObservableProperty]
        private string selfRescueSafeCode;

        [ObservableProperty]
        private string selfRescueValidDate;

        [ObservableProperty]
        private DateTime? selfRescueValidStart;

        [ObservableProperty]
        private DateTime? selfRescueValidEnd;

        [ObservableProperty]
        private string selfRescueInfo;

        [ObservableProperty]
        private string selfRescueUrl;

        [ObservableProperty]
        private string selfRescueIsValid;

        [ObservableProperty]
        private int deviceType = 0;

        [ObservableProperty]
        private byte? processingStatus;

        [ObservableProperty]
        private int? processingCount;

        [ObservableProperty]
        private int? employeeId;

        [ObservableProperty]
        private string inspectorName;

        [ObservableProperty]
        private float? positivePressure;

        [ObservableProperty]
        private DateTime? positivePressureTime;

        [ObservableProperty]
        private float? negativePressure;

        [ObservableProperty]
        private DateTime? negativePressureTime;

        [ObservableProperty]
        private float? exhaustPressure;

        [ObservableProperty]
        private DateTime? exhaustPressureTime;

        [ObservableProperty]
        private float? quantitativeOxygen;

        [ObservableProperty]
        private DateTime? quantitativeOxygenTime;

        [ObservableProperty]
        private float? manualOxygen;

        [ObservableProperty]
        private DateTime? manualOxygenTime;

        [ObservableProperty]
        private float? temp;

        [ObservableProperty]
        private float? hs;

        [ObservableProperty]
        private byte? verifyResult;

        [ObservableProperty]
        private DateTime? checkTime;

        public AddSelfRescuerViewModel(SelfRescuerService selfRescuerService)
        {
            _selfRescuerService = selfRescuerService;
            Title = "添加自救器";
        }

        [RelayCommand]
        private async Task SaveAsync()
        {
            // 验证输入
            if (string.IsNullOrWhiteSpace(SelfRescueId))
            {
                await App.Current.MainPage.DisplayAlertAsync("错误", "请输入出厂编号", "确定");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelfRescueInfo))
            {
                await App.Current.MainPage.DisplayAlertAsync("错误", "请输入自救器信息", "确定");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelfRescueUrl))
            {
                await App.Current.MainPage.DisplayAlertAsync("错误", "请输入扫码URL", "确定");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelfRescueModel))
            {
                await App.Current.MainPage.DisplayAlertAsync("错误", "请输入自救器类型", "确定");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelfRescueSafeCode))
            {
                await App.Current.MainPage.DisplayAlertAsync("错误", "请输入安标", "确定");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelfRescueName))
            {
                await App.Current.MainPage.DisplayAlertAsync("错误", "请输入自救器名称", "确定");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelfRescueIsValid))
            {
                await App.Current.MainPage.DisplayAlertAsync("错误", "请输入有效性", "确定");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelfRescueCompany))
            {
                await App.Current.MainPage.DisplayAlertAsync("错误", "请输入生产厂家", "确定");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelfRescueValidDate))
            {
                await App.Current.MainPage.DisplayAlertAsync("错误", "请输入有效时间范围", "确定");
                return;
            }

            // 创建新的自救器对象
            var rescuer = new SelfRescuer
            {
                SelfRescueId = SelfRescueId,
                CreateTime = DateTime.Now,
                CheckTime = CheckTime,
                VerifyResult = VerifyResult,
                Temp = Temp,
                Hs = Hs,
                SelfRescueInfo = SelfRescueInfo,
                SelfRescueUrl = SelfRescueUrl,
                SelfRescueModel = SelfRescueModel,
                SelfRescueSafeCode = SelfRescueSafeCode,
                SelfRescueName = SelfRescueName,
                SelfRescueIsValid = SelfRescueIsValid,
                SelfRescueCompany = SelfRescueCompany,
                SelfRescueValidDate = SelfRescueValidDate,
                SelfRescueValidStart = SelfRescueValidStart,
                SelfRescueValidEnd = SelfRescueValidEnd,
                ProcessingStatus = ProcessingStatus,
                ProcessingCount = ProcessingCount,
                EmployeeId = EmployeeId,
                DeviceType = DeviceType,
                InspectorName = InspectorName,
                PositivePressure = PositivePressure,
                PositivePressureTime = PositivePressureTime,
                NegativePressure = NegativePressure,
                NegativePressureTime = NegativePressureTime,
                ExhaustPressure = ExhaustPressure,
                ExhaustPressureTime = ExhaustPressureTime,
                QuantitativeOxygen = QuantitativeOxygen,
                QuantitativeOxygenTime = QuantitativeOxygenTime,
                ManualOxygen = ManualOxygen,
                ManualOxygenTime = ManualOxygenTime
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
                // 示例格式: {"SelfRescueId":"SR-12345","SelfRescueModel":"ABC-120","SelfRescueCompany":"制造商A","SelfRescueName":"自救器名称","SelfRescueSafeCode":"安标编号","SelfRescueValidDate":"3年"}
                if (qrCodeText.StartsWith("{") && qrCodeText.EndsWith("}"))
                {
                    // 简单解析JSON
                    var data = System.Text.Json.JsonSerializer.Deserialize<System.Collections.Generic.Dictionary<string, string>>(qrCodeText);
                    if (data != null)
                    {
                        if (data.TryGetValue("SelfRescueId", out var selfRescueId))
                            SelfRescueId = selfRescueId;
                        if (data.TryGetValue("SelfRescueModel", out var selfRescueModel))
                            SelfRescueModel = selfRescueModel;
                        if (data.TryGetValue("SelfRescueCompany", out var selfRescueCompany))
                            SelfRescueCompany = selfRescueCompany;
                        if (data.TryGetValue("SelfRescueName", out var selfRescueName))
                            SelfRescueName = selfRescueName;
                        if (data.TryGetValue("SelfRescueSafeCode", out var selfRescueSafeCode))
                            SelfRescueSafeCode = selfRescueSafeCode;
                        if (data.TryGetValue("SelfRescueValidDate", out var selfRescueValidDate))
                            SelfRescueValidDate = selfRescueValidDate;
                        if (data.TryGetValue("SelfRescueInfo", out var selfRescueInfo))
                            SelfRescueInfo = selfRescueInfo;
                        if (data.TryGetValue("SelfRescueUrl", out var selfRescueUrl))
                            SelfRescueUrl = selfRescueUrl;
                        if (data.TryGetValue("SelfRescueIsValid", out var selfRescueIsValid))
                            SelfRescueIsValid = selfRescueIsValid;
                        if (data.TryGetValue("SelfRescueValidStart", out var selfRescueValidStart))
                        {
                            if (DateTime.TryParse(selfRescueValidStart, out var date))
                                SelfRescueValidStart = date;
                        }
                        if (data.TryGetValue("SelfRescueValidEnd", out var selfRescueValidEnd))
                        {
                            if (DateTime.TryParse(selfRescueValidEnd, out var date))
                                SelfRescueValidEnd = date;
                        }
                    }
                }
                else
                {
                    // 如果不是JSON格式，假设只是出厂编号
                    SelfRescueId = qrCodeText;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error parsing QR code data: {ex.Message}");
                // 如果解析失败，将整个二维码内容作为出厂编号
                SelfRescueId = qrCodeText;
            }
        }
    }
}
