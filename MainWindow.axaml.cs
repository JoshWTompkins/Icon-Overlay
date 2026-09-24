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

    private double _sizeIncrement;
    private double _positionRemainder = 0;
    private float _scale = 3; 

    void ChangeSize(float delta)
    {
        
        _scale = Math.Clamp(_scale + delta, 0.5f, 10.0f);
        var newSize = _scale * _sizeIncrement;

        var positionDelta = (Width - newSize)/2;

        _positionRemainder += positionDelta;

        var pixelDelta = (int)_positionRemainder;
        _positionRemainder -= pixelDelta;

        Position = new PixelPoint(
            Position.X + pixelDelta,
            Position.Y + pixelDelta
        );

        Width = Height = newSize;

        
        

    }
    private void OnOpened(object? sender, EventArgs e)
    {
        var screen = Screens.ScreenFromWindow(this);

        if (screen is null)
            return;

        var area = screen.WorkingArea;

        // 5% of width
        
        _sizeIncrement = area.Width * 0.02;
        Width = Height = _sizeIncrement * 3;
        

        // 20px margin from top-right
        Position = new PixelPoint(
            area.X + area.Width - (int)Width - 20,
            area.Y + 20
        );
    }


    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);
        switch (e.KeySymbol)
        {
            case "=":
                ChangeSize(1);
                break;

            case "-":
                ChangeSize(-1);
                break;

            case { } symbol:
                _mainWindowViewModel.HandleShortcut(symbol[0]);
                break;

        }
        
    }
    
    private double _scrollAccumulator = 0;
    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);

        // handle application scaling:

        if (e.KeyModifiers.HasFlag(KeyModifiers.Control))
        {
            ChangeSize((float) e.Delta.Y);
            return;
        }


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
            _positionRemainder = 0;
        }
    }

    

}