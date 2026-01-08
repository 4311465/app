using Microsoft.Extensions.Logging;
using zjq.Services;
using zjq.Repositories;
using zjq.ViewModels;
using ZXing.Net.Maui;
using ZXing.Net.Maui.Controls;

namespace zjq
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBarcodeReader()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // 注册服务
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<SelfRescuerRepository>();
            builder.Services.AddSingleton<MaintenanceRecordRepository>();
            builder.Services.AddSingleton<UsageRecordRepository>();
            builder.Services.AddSingleton<StatusTypeRepository>();
            builder.Services.AddSingleton<SelfRescuerService>();

            // 注册 ViewModel
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<AddSelfRescuerViewModel>();
            builder.Services.AddTransient<SelfRescuerListViewModel>();
            builder.Services.AddTransient<SelfRescuerDetailViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
