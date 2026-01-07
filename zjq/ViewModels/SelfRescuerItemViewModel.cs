using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace zjq.ViewModels
{
    public partial class SelfRescuerItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private int id;

        [ObservableProperty]
        private string serialNumber = string.Empty;

        [ObservableProperty]
        private string model = string.Empty;

        [ObservableProperty]
        private string location = string.Empty;

        [ObservableProperty]
        private int statusId;

        [ObservableProperty]
        private string statusName = string.Empty;

        [ObservableProperty]
        private DateTime expiryDate;

        [ObservableProperty]
        private string expiryDateText = string.Empty;

        [ObservableProperty]
        private Microsoft.Maui.Graphics.Color statusColor = Microsoft.Maui.Graphics.Colors.White;
    }
}