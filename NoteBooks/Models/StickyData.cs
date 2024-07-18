using StickyNotes.Scripts;

namespace StickyNotes.Models;

public class StickyData
{
    public bool Security { get; set; }
    public string? Password { get; set; }
    public ColorSticky.stickyColor Color { get; set; }
    public StickerPosition? Position { get; set; }
    public StickerSize Size { get; set; }
    public double Opacity { get; set; }

    public StickyData(bool security, ColorSticky.stickyColor color, StickerSize size, double opacity, string? password = null, StickerPosition? position = null) =>
        (Security, Password, Color, Position, Size, Opacity) = (security, password, color, position, size, opacity);
}

public class StickerPosition
{
    public int posX { get; set; }
    public int posY { get; set; }
}

public class StickerSize
{
    public int width { get; set; }
    public int height { get; set; }
}