using Microsoft.Maui.Controls;
using ZXing.Net.Maui;
using System;
using CommunityToolkit.Mvvm.Input;

namespace zjq.Views
{
    public partial class QRScanPage : ContentPage
    {
        public event EventHandler<string> ScanCompleted;
        
        public QRScanPage()
        {
            InitializeComponent();
            
            cameraBarcodeReaderView.Options = new BarcodeReaderOptions
            {
                Formats = BarcodeFormats.All,
                AutoRotate = true,
                Multiple = false
            };
        }
        
        private void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
        {
            if (e.Results.Length > 0)
            {
                Dispatcher.Dispatch(async () =>
                {
                    cameraBarcodeReaderView.IsDetecting = false;
                    var result = e.Results[0].Value;
                    ScanCompleted?.Invoke(this, result);
                    await Navigation.PopAsync();
                });
            }
        }
        
        [RelayCommand]
        private async Task CancelAsync()
        {
            await Navigation.PopAsync();
        }
    }
}