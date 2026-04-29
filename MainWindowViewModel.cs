using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace IconOverlay;
public partial class MainWindowViewModel: ObservableObject

{
    
    [ObservableProperty]
    private Bitmap? _imageFromBinding = null;

}