using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace zjq.ViewModels
{
    public partial class SelfRescuerItemViewModel : ObservableObject
    {
        [ObservableProperty]
        private int id;

        [ObservableProperty]
        private string serialNumber;

        [ObservableProperty]
        private string model;

        [ObservableProperty]
        private string location;

        [ObservableProperty]
        private int statusId;

        [ObservableProperty]
        private string statusName;

        [ObservableProperty]
        private DateTime expiryDate;

        [ObservableProperty]
        private string expiryDateText;

        [ObservableProperty]
        private Microsoft.Maui.Graphics.Color statusColor;
    }
}