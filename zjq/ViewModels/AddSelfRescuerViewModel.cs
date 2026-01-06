using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using zjq.Models;
using zjq.Services;

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
                await App.Current.MainPage.DisplayAlert("错误", "请输入序列号", "确定");
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
    }
}