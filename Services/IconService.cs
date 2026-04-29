using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Avalonia.Media.Imaging;
using Avalonia.Platform;



public class IconService
{
    private List<IconEntry> IconEntries;
    private Dictionary<char, List<IconEntry>> IconShortcuts;
    private int ShortcutIndex = 0;
    private int IconIndex = 0;
    private char? LastKey = null;

    
    public IconService()
    {
        string iconFolder = Path.Combine(AppContext.BaseDirectory, "Icons");
        IconEntries = GetIconEntries(iconFolder);
        IconShortcuts = GetIconShortcuts(IconEntries);
        
    }

    private List<IconEntry> GetIconEntries(string iconFolder)
    {
        var files = Directory.GetFiles(iconFolder, "*.png");

        List<IconEntry> icons = files
            .Select(path => new IconEntry
            {
                Path = path,
                Name = Path.GetFileNameWithoutExtension(path).ToLower(),
                Bitmap = new Bitmap(path)
            })
            .ToList();
        return icons;
    }

    private Dictionary<char, List<IconEntry>> GetIconShortcuts(List<IconEntry> icons)
    {
        Dictionary<char, List<IconEntry>> iconShortcuts = icons
            .GroupBy(icon => char.ToLower(icon.Name[0]))
            .ToDictionary(
                g => g.Key,
                g => g.OrderBy(i => i.Name).ToList()
            );
        return iconShortcuts;
    }

    public Bitmap? GetIconFromShortcut(char shortcut)
    {
        if (!IconShortcuts.TryGetValue(shortcut, out List<IconEntry>? iconList) || iconList.Count == 0) return null;
        ShortcutIndex = LastKey switch
        {
            var x when x == shortcut => (ShortcutIndex + 1) % iconList.Count,
            _ => 0
        };
        LastKey = shortcut;

        // get icon, find its position in full list
        IconEntry icon = iconList[ShortcutIndex];
        int newIconIndex = IconEntries.IndexOf(icon);

        // Deals with the edge case where scroll position == shortcut position, leaving icon unchanged
        if(newIconIndex == IconIndex)
        {
            ShortcutIndex = (ShortcutIndex + 1) % iconList.Count;
            icon = iconList[ShortcutIndex];
            newIconIndex = IconEntries.IndexOf(icon);
        }
        IconIndex = newIconIndex;
        return iconList[ShortcutIndex].Bitmap;
    }

    public Bitmap IncrementIconIndex(int increment)
    {
        IconIndex = (IconIndex + increment) % IconEntries.Count();
        IconIndex += IconIndex >= 0 ? 0 : IconEntries.Count();
        LastKey = null;
        return IconEntries[IconIndex].Bitmap;
    }
}