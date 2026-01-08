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
        private string selfRescueId = string.Empty;

        [ObservableProperty]
        private string selfRescueModel = string.Empty;

        [ObservableProperty]
        private string selfRescueCompany = string.Empty;

        [ObservableProperty]
        private string selfRescueName = string.Empty;

        [ObservableProperty]
        private string selfRescueSafeCode = string.Empty;

        [ObservableProperty]
        private string selfRescueValidDate = string.Empty;

        [ObservableProperty]
        private DateTime? selfRescueValidStart;

        [ObservableProperty]
        private DateTime? selfRescueValidEnd;

        [ObservableProperty]
        private string selfRescueInfo = string.Empty;

        [ObservableProperty]
        private string selfRescueUrl = string.Empty;

        [ObservableProperty]
        private string selfRescueIsValid = string.Empty;

        [ObservableProperty]
        private int deviceType = 0;

        [ObservableProperty]
        private byte? processingStatus;

        [ObservableProperty]
        private int? processingCount;

        [ObservableProperty]
        private int? employeeId;

        [ObservableProperty]
        private string inspectorName = string.Empty;

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
                await App.Current.MainPage.DisplayAlert("错误", "请输入出厂编号", "确定");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelfRescueInfo))
            {
                await App.Current.MainPage.DisplayAlert("错误", "请输入自救器信息", "确定");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelfRescueUrl))
            {
                await App.Current.MainPage.DisplayAlert("错误", "请输入扫码URL", "确定");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelfRescueModel))
            {
                await App.Current.MainPage.DisplayAlert("错误", "请输入自救器类型", "确定");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelfRescueSafeCode))
            {
                await App.Current.MainPage.DisplayAlert("错误", "请输入安标", "确定");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelfRescueName))
            {
                await App.Current.MainPage.DisplayAlert("错误", "请输入自救器名称", "确定");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelfRescueIsValid))
            {
                await App.Current.MainPage.DisplayAlert("错误", "请输入有效性", "确定");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelfRescueCompany))
            {
                await App.Current.MainPage.DisplayAlert("错误", "请输入生产厂家", "确定");
                return;
            }

            if (string.IsNullOrWhiteSpace(SelfRescueValidDate))
            {
                await App.Current.MainPage.DisplayAlert("错误", "请输入有效时间范围", "确定");
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
                    await App.Current.MainPage.DisplayAlert("成功", "自救器添加成功", "确定");
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("错误", "添加失败，请重试", "确定");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error saving self rescuer: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("错误", $"添加失败: {ex.Message}", "确定");
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
                    await App.Current.MainPage.DisplayAlert("设备不支持", "当前设备没有摄像头，无法使用扫码功能", "确定");
                    return;
                }
                
                // 检查摄像头权限
                var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
                if (status != PermissionStatus.Granted)
                {
                    status = await Permissions.RequestAsync<Permissions.Camera>();
                    if (status != PermissionStatus.Granted)
                    {
                        await App.Current.MainPage.DisplayAlert("权限不足", "需要摄像头权限才能扫码", "确定");
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
                await App.Current.MainPage.DisplayAlert("错误", "扫码功能初始化失败，请检查摄像头权限", "确定");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void ParseQRCodeData(string qrCodeText)
        {
            try
            {
                IsBusy = true;
                // 检查是否是URL格式
                if (qrCodeText.StartsWith("http"))
                {
                    await HandleUrlQRCode(qrCodeText);
                }
                // 假设二维码数据格式为JSON
                else if (qrCodeText.StartsWith("{") && qrCodeText.EndsWith("}"))
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
            finally
            {
                IsBusy = false;
            }
        }

        private async Task HandleUrlQRCode(string url)
        {
            try
            {
                IsBusy = true;
                System.Diagnostics.Debug.WriteLine($"处理URL二维码: {url}");

                // 检查URL是否包含code参数
                if (!url.Contains("code="))
                {
                    await App.Current.MainPage.DisplayAlert("错误", "URL中未包含设备编号，请扫描正确格式的二维码", "确定");
                    return;
                }

                // 提取code参数值
                int codeIndex = url.IndexOf("code=") + "code=".Length;
                int endIndex = url.IndexOf("&", codeIndex);
                string deviceCode = endIndex == -1 ? url.Substring(codeIndex) : url.Substring(codeIndex, endIndex - codeIndex);

                System.Diagnostics.Debug.WriteLine($"从URL中提取到设备编号: {deviceCode}");

                // 检查数据库中是否存在相同URL的记录
                var existingRescuers = await _selfRescuerService.GetSelfRescuersByUrlAsync(url);

                if (existingRescuers.Count > 0)
                {
                    // 显示现有记录
                    string message = $"找到 {existingRescuers.Count} 条记录：\n\n";
                    foreach (var rescuer in existingRescuers)
                    {
                        message += $"出厂编号: {rescuer.SelfRescueId}\n";
                        message += $"校验日期: {rescuer.CheckTime?.ToString("yyyy-MM-dd HH:mm") ?? "未校验"}\n";
                        message += $"检查人: {rescuer.InspectorName ?? "未知"}\n\n";
                    }
                    await App.Current.MainPage.DisplayAlert("提示", message, "确定");
                }
                else
                {
                    // 调用API获取设备信息
                    await FetchDeviceInfoFromApi(url, deviceCode);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"处理URL二维码时出错: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("错误", $"处理二维码时出错: {ex.Message}", "确定");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task FetchDeviceInfoFromApi(string url, string deviceCode)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"调用API获取设备信息，设备编号: {deviceCode}");

                // 构建API URL
                string apiUrl = $"http://plm.aqbz.org:9999/prod-api/application/qrcode/product/v1?code={deviceCode}";
                System.Diagnostics.Debug.WriteLine($"API URL: {apiUrl}");
               
                // 创建并配置HttpClient
                var httpClientHandler = new HttpClientHandler
                {
                    // 允许自动重定向
                    AllowAutoRedirect = true,
                    // 不使用默认代理（可能解决某些代理问题）
                    UseProxy = false,
                    // 禁用SSL验证（仅用于测试，生产环境应启用）
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
                    // 禁用自动压缩（可能解决某些网络问题）
                    AutomaticDecompression = System.Net.DecompressionMethods.None
                };

                using var httpClient = new HttpClient(httpClientHandler);
                
                // 设置超时时间
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                
                // 添加必要的请求头
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json, text/plain, */*");
                httpClient.DefaultRequestHeaders.Add("Accept-Language", "zh-CN,zh;q=0.9,en;q=0.8");
                httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
                httpClient.DefaultRequestHeaders.Add("Pragma", "no-cache");

                // 记录请求头信息
                System.Diagnostics.Debug.WriteLine("请求头信息:");
                foreach (var header in httpClient.DefaultRequestHeaders)
                {
                    System.Diagnostics.Debug.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
                }
                
                System.Diagnostics.Debug.WriteLine("开始发送API请求...");
                
                // 发送请求
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
                
                System.Diagnostics.Debug.WriteLine($"API响应状态码: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine($"API响应: {json}");

                    // 解析API响应
                    var apiResponse = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(json);

                    if (apiResponse.TryGetProperty("data", out System.Text.Json.JsonElement data))
                    {
                        // 提取设备信息并填充表单
                        SelfRescueId = data.GetProperty("serialNumber").GetString() ?? Guid.NewGuid().ToString();
                        SelfRescueUrl = url;
                        SelfRescueInfo = deviceCode;
                        
                        // 设备类型 - 根据自救器名称判断
                        if (data.TryGetProperty("oldProductInfo", out System.Text.Json.JsonElement productInfo1))
                        {
                            string productName = productInfo1.GetProperty("productName").GetString() ?? "";
                            DeviceType = productName.Contains("压缩氧") ? 1 : 0;
                        }
                        else
                        {
                            DeviceType = 0;
                        }

                        if (data.TryGetProperty("oldProductInfo", out System.Text.Json.JsonElement productInfo))
                        {
                            SelfRescueModel = productInfo.GetProperty("productType").GetString() ?? "未知型号";
                            SelfRescueSafeCode = productInfo.GetProperty("productSafeCode").GetString() ?? deviceCode;
                            SelfRescueName = productInfo.GetProperty("productName").GetString() ?? "未知设备";
                            SelfRescueIsValid = productInfo.GetProperty("safetyStatus").GetString() ?? "有效";
                            SelfRescueCompany = productInfo.GetProperty("enterpriseName").GetString() ?? "未知厂家";
                            SelfRescueValidDate = productInfo.GetProperty("validity").GetString() ?? "未知有效期";

                            // 处理日期
                            if (productInfo.TryGetProperty("datePub", out System.Text.Json.JsonElement datePub))
                            {
                                if (DateTime.TryParse(datePub.GetString(), out var validStart))
                                    SelfRescueValidStart = validStart;
                            }
                            if (productInfo.TryGetProperty("dateEnd", out System.Text.Json.JsonElement dateEnd))
                            {
                                if (DateTime.TryParse(dateEnd.GetString(), out var validEnd))
                                    SelfRescueValidEnd = validEnd;
                            }
                        }

                        await App.Current.MainPage.DisplayAlert("成功", "设备信息获取成功", "确定");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("API响应格式不正确，缺少data字段");
                        await App.Current.MainPage.DisplayAlert("错误", "API响应格式不正确，缺少data字段", "确定");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"API请求失败，状态码: {response.StatusCode}");
                    // 尝试读取错误响应内容
                    try
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        System.Diagnostics.Debug.WriteLine($"错误响应内容: {errorContent}");
                        await App.Current.MainPage.DisplayAlert("错误", $"API请求失败: {response.StatusCode}\n{errorContent}", "确定");
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"读取错误响应失败: {ex.Message}");
                        await App.Current.MainPage.DisplayAlert("错误", $"API请求失败，状态码: {response.StatusCode}", "确定");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"=== API调用异常详情 ===");
                System.Diagnostics.Debug.WriteLine($"异常消息: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"异常类型: {ex.GetType().FullName}");
                System.Diagnostics.Debug.WriteLine($"异常堆栈: {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"内部异常消息: {ex.InnerException.Message}");
                    System.Diagnostics.Debug.WriteLine($"内部异常类型: {ex.InnerException.GetType().FullName}");
                    System.Diagnostics.Debug.WriteLine($"内部异常堆栈: {ex.InnerException.StackTrace}");
                    
                    // 检查内部异常是否为SocketException
                    if (ex.InnerException is System.Net.Sockets.SocketException socketEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Socket错误码: {socketEx.ErrorCode}");
                        System.Diagnostics.Debug.WriteLine($"Socket错误详情: {socketEx.SocketErrorCode}");
                    }
                }
                
                // 检查是否为AggregateException
                if (ex is AggregateException aggEx)
                {
                    System.Diagnostics.Debug.WriteLine($"AggregateException包含 {aggEx.InnerExceptions.Count} 个内部异常:");
                    foreach (var innerEx in aggEx.InnerExceptions)
                    {
                        System.Diagnostics.Debug.WriteLine($"- {innerEx.GetType().Name}: {innerEx.Message}");
                    }
                }
                
                // 区分不同类型的异常
                if (ex is HttpRequestException httpEx)
                {
                    System.Diagnostics.Debug.WriteLine($"HTTP异常详情: {httpEx.InnerException?.Message}");
                    await App.Current.MainPage.DisplayAlert("网络错误", $"网络请求失败: {httpEx.Message}\n可能原因: 网络连接问题、服务器不可达或超时\n详细信息: {httpEx.InnerException?.Message}", "确定");
                }
                else if (ex is TaskCanceledException)
                {
                    await App.Current.MainPage.DisplayAlert("超时错误", "请求超时，请检查网络连接或服务器响应时间", "确定");
                }
                else if (ex is System.Net.Sockets.SocketException socketEx)
                {
                    System.Diagnostics.Debug.WriteLine($"Socket异常错误码: {socketEx.ErrorCode}");
                    System.Diagnostics.Debug.WriteLine($"Socket异常SocketErrorCode: {socketEx.SocketErrorCode}");
                    await App.Current.MainPage.DisplayAlert("网络错误", $"网络连接失败: {socketEx.Message}\n错误码: {socketEx.ErrorCode}\nSocket错误: {socketEx.SocketErrorCode}", "确定");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("错误", $"获取设备信息时出错: {ex.Message}\n异常类型: {ex.GetType().Name}", "确定");
                }
            }
        }

   }
}
