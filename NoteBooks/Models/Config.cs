using System.Text.Json.Serialization;

namespace NoteBooks.Models;

public class Config
{
    public themeNameStyle Theme { get; set; }
    public bool DeleteNotification { get; set; }
    public bool AutoStart { get; set; }
    public StickerSetings StickerSetings { get; set; }

    [JsonConstructor]
    public Config(themeNameStyle theme, bool deleteNotification, bool autoStart, StickerSetings stickerSetings) =>
        (Theme, DeleteNotification, AutoStart, StickerSetings) = (theme, deleteNotification, autoStart, stickerSetings);
}