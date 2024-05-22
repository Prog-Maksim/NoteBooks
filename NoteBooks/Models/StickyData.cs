using System.Collections.Generic;

namespace StickyNotes.Models;

public class StickyData
{
    public bool Security { get; set; }
    public string? Password { get; set; }
    public ColorSticky.stickyColor Color { get; set; }
    public List<int>? Position { get; set; }
    public List<int> Size { get; set; }
    public double Opacity { get; set; }

    public StickyData(bool security, ColorSticky.stickyColor color, List<int> size, double opacity, string? password = null, List<int>? position = null) =>
        (Security, Password, Color, Position, Size, Opacity) = (security, password, color, position, size, opacity);
}