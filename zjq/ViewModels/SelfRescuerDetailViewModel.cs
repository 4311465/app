using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using zjq.Models;
using zjq.Services;

namespace zjq.ViewModels
{
    public partial class SelfRescuerDetailViewModel : BaseViewModel
    {
        private readonly SelfRescuerService _selfRescuerService;

        [ObservableProperty]
        private SelfRescuer? selfRescuer;

        [ObservableProperty]
        private string serialNumber = string.Empty;

        [ObservableProperty]
        private string model = string.Empty;

        [ObservableProperty]
        private string location = string.Empty;

        [ObservableProperty]
        private string statusName = string.Empty;

        [ObservableProperty]
        private string expiryDateText = string.Empty;

        [ObservableProperty]
        private string company = string.Empty;

        [ObservableProperty]
        private string safeCode = string.Empty;

        [ObservableProperty]
        private string validDate = string.Empty;

        [ObservableProperty]
        private string deviceType = string.Empty;

        public SelfRescuerDetailViewModel(SelfRescuerService selfRescuerService)
        {
            _selfRescuerService = selfRescuerService;
            Title = "自救器详情";
        }

        [RelayCommand]
        private async Task BackAsync()
        {
            await Shell.Current.GoToAsync("//SelfRescuerListPage");
        }

        public async Task LoadSelfRescuerAsync(int id)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Attempting to load self rescuer with Id: {id}");
                IsBusy = true;
                
                // 检查服务是否可用
                if (_selfRescuerService == null)
                {
                    System.Diagnostics.Debug.WriteLine("SelfRescuerService is null!");
                    return;
                }
                
                // 重置属性值
                SerialNumber = string.Empty;
                Model = string.Empty;
                Location = string.Empty;
                StatusName = string.Empty;
                ExpiryDateText = string.Empty;
                Company = string.Empty;
                SafeCode = string.Empty;
                ValidDate = string.Empty;
                DeviceType = string.Empty;
                
                SelfRescuer = await _selfRescuerService.GetSelfRescuerByIdAsync(id);
                
                System.Diagnostics.Debug.WriteLine($"Loaded self rescuer: {SelfRescuer}");
                
                if (SelfRescuer != null)
                {
                    System.Diagnostics.Debug.WriteLine($"SelfRescuer data: Id={SelfRescuer.Id}, SelfRescueId={SelfRescuer.SelfRescueId}, Model={SelfRescuer.SelfRescueModel}");
                    
                    SerialNumber = SelfRescuer.SelfRescueId;
                    Model = SelfRescuer.SelfRescueModel;
                    Location = SelfRescuer.SelfRescueCompany; // 使用厂家作为位置信息
                    Company = SelfRescuer.SelfRescueCompany;
                    SafeCode = SelfRescuer.SelfRescueSafeCode;
                    ValidDate = SelfRescuer.SelfRescueValidDate;
                    DeviceType = SelfRescuer.DeviceType == 0 ? "化学氧" : "压缩氧";

                    // 确定状态名称
                    StatusName = "正常";
                    
                    // 检查是否过期
                    if (SelfRescuer.SelfRescueValidEnd.HasValue && SelfRescuer.SelfRescueValidEnd.Value < System.DateTime.Now)
                    {
                        StatusName = "过期";
                    }
                    // 检查是否临期
                    else if (SelfRescuer.CheckTime.HasValue)
                    {
                        var currentDate = System.DateTime.Now;
                        if ((SelfRescuer.DeviceType == 1 && SelfRescuer.CheckTime.Value.AddDays(160) <= currentDate && SelfRescuer.CheckTime.Value.AddDays(180) >= currentDate) ||
                            (SelfRescuer.DeviceType == 0 && SelfRescuer.CheckTime.Value.AddDays(80) <= currentDate && SelfRescuer.CheckTime.Value.AddDays(90) >= currentDate))
                        {
                            StatusName = "临期";
                        }
                    }

                    // 计算过期日期
                    System.DateTime expiryDate = System.DateTime.Now.AddYears(3);
                    if (SelfRescuer.SelfRescueValidEnd.HasValue)
                    {
                        expiryDate = SelfRescuer.SelfRescueValidEnd.Value;
                    }
                    else if (!string.IsNullOrWhiteSpace(SelfRescuer.SelfRescueId) && SelfRescuer.SelfRescueId.Length >= 8)
                    {
                        try
                        {
                            string dateStr = SelfRescuer.SelfRescueId.Substring(0, 8);
                            if (System.Text.RegularExpressions.Regex.IsMatch(dateStr, "^\\d{8}$"))
                            {
                                if (System.DateTime.TryParseExact(dateStr, "yyyyMMdd", null, System.Globalization.DateTimeStyles.None, out System.DateTime parsedDate))
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

                    ExpiryDateText = $"过期日期: {expiryDate.ToString("yyyy-MM-dd")}";
                    
                    System.Diagnostics.Debug.WriteLine("SelfRescuer properties updated successfully");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"No self rescuer found with Id: {id}");
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading self rescuer details: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
