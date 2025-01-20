using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using StickyNotes.Models;

namespace StickyNotes.Scripts;

public static class FileSettings
{
    private static readonly string programSetting = Path.Combine(ClassRegistry.mainBasePath, "ProgramSettings.json");
    public static readonly string stickersData = Path.Combine(ClassRegistry.mainBasePath, "StickyData.json");

    public static bool autoStart
    {
        get
        {
            return GetProgramSettings().AutoStart;
        }
        set
        {
            UpdateConfigProperty(config => config.AutoStart = value);
        }
    }

    public static bool deleteNotification
    {
        get
        {
            return GetProgramSettings().DeleteNotification;
        }
        set
        {
            UpdateConfigProperty(config => config.DeleteNotification = value);
        }
    }

    public static themeNameStyle themeStyle
    {
        get
        {
            return GetProgramSettings().Theme;
        }
        private set
        {
            UpdateConfigProperty(config => config.Theme = value);
        }
    }

    public static bool closeMenu
    {
        get
        {
            return GetProgramSettings().StickerSetings.CloseMenuLeaveFocus;
        }
        set
        {
            UpdateConfigProperty(config => config.StickerSetings.CloseMenuLeaveFocus = value);
        }
    }

    public static bool closeMenuAnimation
    {
        get
        {
            return GetProgramSettings().StickerSetings.CloseAnimation;
        }
        set
        {
            UpdateConfigProperty(config => config.StickerSetings.CloseAnimation = value);
        }
    }

    public static double delayTopMenu
    {
        get
        {
            return GetProgramSettings().StickerSetings.DelayAnimationTopMenu;
        }
        set
        {
            UpdateConfigProperty(config => config.StickerSetings.DelayAnimationTopMenu = value);
        }
    }
    
    public static double delayBottonMenu
    {
        get
        {
            return GetProgramSettings().StickerSetings.DelayAnimationBottonMenu;
        }
        set
        {
            UpdateConfigProperty(config => config.StickerSetings.DelayAnimationBottonMenu = value);
        }
    }
    
    
    private static Config GetProgramSettings()
    {
        if (!File.Exists(programSetting))
            createSystemFiles();
        
        Config? config;
        using (FileStream fs = new FileStream(programSetting, FileMode.Open))
            config = JsonSerializer.Deserialize<Config>(fs);
        
        if (config == null)
            throw new InvalidOperationException("Не удалось десериализовать конфигурационный файл.");

        return config;
    }

    private static void UpdateConfigProperty(Action<Config> updateAction)
    {
        if (!File.Exists(programSetting))
            createSystemFiles();

        Config? config;
        using (FileStream fs = new FileStream(programSetting, FileMode.Open))
            config = JsonSerializer.Deserialize<Config>(fs);

        if (config == null)
            throw new InvalidOperationException("Не удалось десериализовать конфигурационный файл.");
        
        updateAction(config);
        
        string jsonData = JsonSerializer.Serialize(config);
        File.WriteAllText(programSetting, jsonData);
    }
    
    
    public static void setNewColorTheme(themeNameStyle theme)
    {
        try
        {
            if (theme == themeNameStyle.light)
            {
                var uri = new Uri("ColorTheme/LightTheme.xaml", UriKind.Relative);
                ResourceDictionary? resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDict);
            }
            else if (theme == themeNameStyle.dark)
            {
                var uri = new Uri("ColorTheme/DarkTheme.xaml", UriKind.Relative);
                ResourceDictionary? resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
                Application.Current.Resources.Clear();
                Application.Current.Resources.MergedDictionaries.Add(resourceDict);
            }
            else if (theme == themeNameStyle.system)
            {
                themeNameStyle currentThemeStyle = ClassRegistry.getCurrentThemeStyle();
                setNewColorTheme(currentThemeStyle);
            }
        }
        catch
        {
            // ignore
        }
    }
    
    public static void themeChange(themeNameStyle theme)
    {
        setNewColorTheme(theme);
        FileSettings.themeStyle = theme;
    }
    
    public static void createSystemFiles()
    {
        createSystemSettings();
        createFileStickersList();
        CreateStudySticker();
    }
    
    private static void createSystemSettings()
    {
        if (File.Exists(programSetting))
            File.Delete(programSetting);
        
        using FileStream fs = new FileStream(programSetting, FileMode.OpenOrCreate);
        StickerSetings stickerSetings = new StickerSetings(true, true, 0.5, 0.1);
        Config systemSettings = new Config(themeNameStyle.light, true, false, stickerSetings);
            
        JsonSerializer.Serialize(fs, systemSettings);
    }

    private static void createFileStickersList()
    {
        if (File.Exists(stickersData))
            File.Delete(stickersData);
        
        using FileStream fs = new FileStream(stickersData, FileMode.OpenOrCreate);
        StickersList stickersList = new StickersList
        {
            Stickers = new List<Models.Sticker>()
        };
        JsonSerializer.Serialize(fs, stickersList);
    }

    private static void CreateStudySticker()
    {
        if (File.Exists("Inform_2_0_0.rtf"))
            Sticker.addNewStickerContextMenu(new FileInfo("Inform_2_0_0.rtf"));
    }
}