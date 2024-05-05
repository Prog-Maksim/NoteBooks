using System.Text.Json.Serialization;

namespace NoteBooks.Models;

public class Config
{
    public themeNameStyle Theme { get; set; }
    public bool DeleteNotification { get; set; }
    public bool AutoStart { get; set; }
    public string SavePathData { get; set; }
    public StickerSetings StickerSetings { get; set; }

    [JsonConstructor]
    public Config(themeNameStyle theme, bool deleteNotification, bool autoStart, string savePathData, StickerSetings stickerSetings) =>
        (Theme, DeleteNotification, AutoStart, SavePathData, StickerSetings) = (theme, deleteNotification, autoStart, savePathData, stickerSetings);
}