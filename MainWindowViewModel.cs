using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace IconOverlay;
public partial class MainWindowViewModel: ObservableObject

{
    
    [ObservableProperty]
    private Bitmap? _imageFromBinding = null;
    private IconService _iconService = new IconService();

    public void HandleShortcut(char shortcut)
    {
        Bitmap? icon = _iconService.GetIconFromShortcut(shortcut);
        if(icon != null) ImageFromBinding = icon;
    }

}