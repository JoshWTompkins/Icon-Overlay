using Avalonia.Media.Imaging;

public class IconEntry
{
    public required string Name { get; init; }
    public required string Path { get; init; }
    public Bitmap Bitmap { get; init; } = null!;
}