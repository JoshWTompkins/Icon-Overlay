using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;



namespace IconOverlay;

public partial class MainWindow : Window
{
    
    private readonly IconService _iconService = new();
    private readonly MainWindowViewModel _mainWindowViewModel;

    //public Bitmap? ImageFromBinding = null;
    public MainWindow(MainWindowViewModel mainWindowViewModel)
    {
        InitializeComponent();
        _mainWindowViewModel = mainWindowViewModel;
        DataContext = mainWindowViewModel;
        _mainWindowViewModel.ImageFromBinding = _iconService.IncrementIconIndex(0);
        Opened += OnOpened;
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnOpened(object? sender, EventArgs e)
    {
        var screen = Screens.ScreenFromWindow(this);

        if (screen is null)
            return;

        var area = screen.WorkingArea;

        // 5% of width
        double size = area.Width * 0.05;

        Width = size;
        Height = size;

        // 20px margin from top-right
        Position = new PixelPoint(
            area.X + area.Width - (int)Width - 20,
            area.Y + 20
        );
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        if(e.KeySymbol == null) return;
        char symbol = e.KeySymbol[0];
        Bitmap? icon = _iconService.GetIconFromShortcut(symbol);
        if(icon != null) _mainWindowViewModel.ImageFromBinding = icon;
        
    }
    
    private double _scrollAccumulator = 0;
    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);
        _scrollAccumulator += e.Delta.Y;

        int steps = (int)_scrollAccumulator;

        if (steps == 0)
            return;

        _scrollAccumulator -= steps;

        _mainWindowViewModel.ImageFromBinding =
            _iconService.IncrementIconIndex(steps);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
{
    base.OnPointerPressed(e);

    if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
    {
        BeginMoveDrag(e);
    }
}


}