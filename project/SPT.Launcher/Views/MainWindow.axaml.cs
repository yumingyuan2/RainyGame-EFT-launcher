using SPT.Launcher.ViewModels;
using Avalonia;
using Avalonia.Controls.Notifications;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using Splat;

namespace SPT.Launcher.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            // AttachDevTools is no longer available in newer Avalonia versions
            // this.AttachDevTools();
#endif

            WindowNotificationManager notificationManager = new WindowNotificationManager(this);

            Locator.CurrentMutable.RegisterConstant(notificationManager);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
