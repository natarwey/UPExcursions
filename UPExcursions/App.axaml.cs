using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using UPExcursions.Data;
using UPExcursions.Views;

namespace UPExcursions
{
    public partial class App : Application
    {
        public static AppDbContext dbContext { get; set; } = new AppDbContext();

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new AuthWindow();
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}