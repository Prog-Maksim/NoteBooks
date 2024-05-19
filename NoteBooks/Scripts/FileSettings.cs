using System.IO;
using System.Text.Json;
using NoteBooks.Models;

namespace NoteBooks;

public class FileSettings
{
    private static string path = Path.Combine(ClassRegistry.mainBasePath, "ProgramSettings.json");

    public static bool autoStart
    {
        get
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                Config config = JsonSerializer.Deserialize<Config>(fs);
                return config.AutoStart;
            }
        }
        set
        {
            Config config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
            }

            config.AutoStart = value;
            string jsonData = JsonSerializer.Serialize(config);
            File.WriteAllText(path, jsonData);
        }
    }

    public static bool deleteNotification
    {
        get
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                Config config = JsonSerializer.Deserialize<Config>(fs);
                return config.DeleteNotification;
            }
        }
        set
        {
            Config config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
            }

            config.DeleteNotification = value;
            string jsonData = JsonSerializer.Serialize(config);
            File.WriteAllText(path, jsonData);
        }
    }

    public static themeNameStyle themeStyle
    {
        get
        {
            Config config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
                return config.Theme;
            }
        }
        set
        {
            Config config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
            }
        
            config.Theme = value;
            string jsonData = JsonSerializer.Serialize(config);
            File.WriteAllText(path, jsonData);
        }
    }

    public static bool closeMenu
    {
        get
        {
            Config config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
                return config.StickerSetings.CloseMenuLeaveFocus;
            }
        }
        set
        {
            Config config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
            }
        
            config.StickerSetings.CloseMenuLeaveFocus = value;
            string jsonData = JsonSerializer.Serialize(config);
            File.WriteAllText(path, jsonData);
        }
    }

    public static bool closeMenuAnimation
    {
        get
        {
            Config config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
                return config.StickerSetings.CloseAnimation;
            }
        }
        set
        {
            Config config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
            }
        
            config.StickerSetings.CloseAnimation = value;
            string jsonData = JsonSerializer.Serialize(config);
            File.WriteAllText(path, jsonData);
        }
    }

    public static double delayTopMenu
    {
        get
        {
            Config config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
                return config.StickerSetings.DelayAnimationTopMenu;
            }
        }
        set
        {
            Config config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
            }
        
            config.StickerSetings.DelayAnimationTopMenu = value;
            string jsonData = JsonSerializer.Serialize(config);
            File.WriteAllText(path, jsonData);
        }
    }
    
    public static double delayBottonMenu
    {
        get
        {
            Config config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
                return config.StickerSetings.DelayAnimationBottonMenu;
            }
        }
        set
        {
            Config config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
            }
        
            config.StickerSetings.DelayAnimationBottonMenu = value;
            string jsonData = JsonSerializer.Serialize(config);
            File.WriteAllText(path, jsonData);
        }
    }
}