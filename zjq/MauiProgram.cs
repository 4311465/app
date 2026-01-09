using Microsoft.Extensions.Logging;
using zjq.Services;
using zjq.Repositories;
using zjq.ViewModels;
using zjq.Views;
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
                    fonts.AddFont("OpenSansRegular.ttf", "OpenSansRegular");
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

            // 注册页面
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<SelfRescuerListPage>();
            builder.Services.AddTransient<SelfRescuerDetailPage>();
            builder.Services.AddTransient<AddSelfRescuerPage>();
            builder.Services.AddTransient<MaintenanceRecordPage>();
            builder.Services.AddTransient<UsageRecordPage>();
            builder.Services.AddTransient<StatisticsPage>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
