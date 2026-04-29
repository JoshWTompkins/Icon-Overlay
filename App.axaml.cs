using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace IconOverlay;

public partial class App : Application
{
    private readonly MainWindowViewModel _mainWindowViewModel = new();
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }
    

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow(_mainWindowViewModel);
        }

        base.OnFrameworkInitializationCompleted();
    }
}