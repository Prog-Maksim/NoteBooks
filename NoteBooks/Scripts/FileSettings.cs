﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Windows;
using StickyNotes.Models;

namespace StickyNotes.Scripts;

public static class FileSettings
{
    private static readonly string path = Path.Combine(ClassRegistry.mainBasePath, "ProgramSettings.json");

    public static bool autoStart
    {
        get
        {
            using FileStream fs = new FileStream(path, FileMode.Open);
            Config? config = JsonSerializer.Deserialize<Config>(fs);
            return config!.AutoStart;
        }
        set
        {
            Config? config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
            }

            config!.AutoStart = value;
            string jsonData = JsonSerializer.Serialize(config);
            File.WriteAllText(path, jsonData);
        }
    }

    public static bool deleteNotification
    {
        get
        {
            using FileStream fs = new FileStream(path, FileMode.Open);
            Config? config = JsonSerializer.Deserialize<Config>(fs);
            return config!.DeleteNotification;
        }
        set
        {
            Config? config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
            }

            config!.DeleteNotification = value;
            string jsonData = JsonSerializer.Serialize(config);
            File.WriteAllText(path, jsonData);
        }
    }

    public static themeNameStyle themeStyle
    {
        get
        {
            using FileStream fs = new FileStream(path, FileMode.Open);
            var config = JsonSerializer.Deserialize<Config>(fs);
            return config!.Theme;
        }
        private set
        {
            Config? config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
            }
        
            config!.Theme = value;
            string jsonData = JsonSerializer.Serialize(config);
            File.WriteAllText(path, jsonData);
        }
    }

    public static bool closeMenu
    {
        get
        {
            using FileStream fs = new FileStream(path, FileMode.Open);
            var config = JsonSerializer.Deserialize<Config>(fs);
            return config!.StickerSetings.CloseMenuLeaveFocus;
        }
        set
        {
            Config? config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
            }
        
            config!.StickerSetings.CloseMenuLeaveFocus = value;
            string jsonData = JsonSerializer.Serialize(config);
            File.WriteAllText(path, jsonData);
        }
    }

    public static bool closeMenuAnimation
    {
        get
        {
            using FileStream fs = new FileStream(path, FileMode.Open);
            var config = JsonSerializer.Deserialize<Config>(fs);
            return config!.StickerSetings.CloseAnimation;
        }
        set
        {
            Config? config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
            }
        
            config!.StickerSetings.CloseAnimation = value;
            string jsonData = JsonSerializer.Serialize(config);
            File.WriteAllText(path, jsonData);
        }
    }

    public static double delayTopMenu
    {
        get
        {
            using FileStream fs = new FileStream(path, FileMode.Open);
            var config = JsonSerializer.Deserialize<Config>(fs);
            return config!.StickerSetings.DelayAnimationTopMenu;
        }
        set
        {
            Config? config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
            }
        
            config!.StickerSetings.DelayAnimationTopMenu = value;
            string jsonData = JsonSerializer.Serialize(config);
            File.WriteAllText(path, jsonData);
        }
    }
    
    public static double delayBottonMenu
    {
        get
        {
            using FileStream fs = new FileStream(path, FileMode.Open);
            var config = JsonSerializer.Deserialize<Config>(fs);
            return config!.StickerSetings.DelayAnimationBottonMenu;
        }
        set
        {
            Config? config;
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                config = JsonSerializer.Deserialize<Config>(fs);
            }
        
            config!.StickerSetings.DelayAnimationBottonMenu = value;
            string jsonData = JsonSerializer.Serialize(config);
            File.WriteAllText(path, jsonData);
        }
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
        string progSettings = Path.Combine(ClassRegistry.mainBasePath, "ProgramSettings.json");
        string stickersData = Path.Combine(ClassRegistry.mainBasePath, "StickyData.json");
            
        createSystemSettings(progSettings);
        createFileStickersList(stickersData);
        CreateStudySticker();
    }
    
    private static void createSystemSettings(string file)
    {
        using FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
        StickerSetings stickerSetings = new StickerSetings(true, true, 0.5, 0.1);
        Config systemSettings = new Config(themeNameStyle.light, true, false, stickerSetings);
            
        JsonSerializer.Serialize(fs, systemSettings);
    }

    private static void createFileStickersList(string file)
    {
        using FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
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