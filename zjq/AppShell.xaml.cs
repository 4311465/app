namespace zjq
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            
            // 配置路由
            Routing.RegisterRoute(nameof(Views.SelfRescuerDetailPage), typeof(Views.SelfRescuerDetailPage));
            Routing.RegisterRoute(nameof(Views.AddSelfRescuerPage), typeof(Views.AddSelfRescuerPage));
            Routing.RegisterRoute(nameof(Views.MaintenanceRecordPage), typeof(Views.MaintenanceRecordPage));
            Routing.RegisterRoute(nameof(Views.UsageRecordPage), typeof(Views.UsageRecordPage));
            Routing.RegisterRoute(nameof(Views.StatisticsPage), typeof(Views.StatisticsPage));
        }
    }
}
