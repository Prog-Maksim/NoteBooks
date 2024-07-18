using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using StickyNotes.Models;

namespace StickyNotes.Scripts;

public class Sticker: ColorSticky
{
    public static bool checkIsAvailabilityFile(string nameSticker)
    {
        var stickers = getAllDataSticker().Stickers;
        if (stickers != null && stickers.Any(sticker => sticker.Name == nameSticker))
        {
            return true;
        }

        return false;
    }

    public static void addNewStickerContextMenu(FileInfo pathSticker)
    {
        StickersList stickers = getAllDataSticker();
        var sticker = CreateBaseSticker(pathSticker);
     
        if (stickers.Stickers != null)
        {
            stickers.Stickers.Add(sticker);
            updateAllStickerData(stickers);
        }
        else
        {
            List<Models.Sticker> stickersList = new List<Models.Sticker> { sticker };
            updateAllStickerData(new StickersList(stickersList));
        }
    }

    private static Models.Sticker CreateBaseSticker(FileInfo pathSticker)
    {
        StickyData stickyData = new StickyData(
            security: false, 
            color: stickyColor.purple, 
            size: new StickerSize {width = 300, height = 325}, 
            opacity: 1, 
            position: new StickerPosition {posX = 100, posY = 100}
            );
        Models.Sticker sticker = new Models.Sticker(
            name: pathSticker.Name.Replace(".rtf", ""), 
            dateStart: DateTime.Now,
            currentDateUpdate: DateTime.Now,
            color: 0, 
            stickerThumbtack: false, 
            stickerFavorite: false, 
            stickerCurrentPath: pathSticker.ToString(),
            stickyData: stickyData
        );

        return sticker;
    }

    public static void createNewSticker(Models.Sticker dataSticker)
    {
        string file_path = Path.Combine(ClassRegistry.mainBasePath, "StickyData.json");
        
        StickersList data = getAllDataSticker();

        data.Stickers!.Add(dataSticker);

        string jsonData = JsonSerializer.Serialize(data);
        File.WriteAllText(file_path, jsonData);
    }

    public static bool getResultDublicateStickerName(string nameSticker)
    {
        StickersList stickersList = getAllDataSticker();

        if (stickersList.Stickers == null) return false;
        return stickersList.Stickers.Any(sticker => sticker.Name == nameSticker);
    }

    public static StickersList getAllDataSticker()
    {
        string file_path = Path.Combine(ClassRegistry.mainBasePath, "StickyData.json");

        using FileStream fs = new FileStream(file_path, FileMode.OpenOrCreate);
        StickersList? data = JsonSerializer.Deserialize<StickersList?>(fs);
        if (data != null)
            return data;
            
        StickersList list = new StickersList
        {
            Stickers = new List<Models.Sticker>()
        };
        return list;
    }

    private static void updateAllStickerData(StickersList data)
    {
        string file_path = Path.Combine(ClassRegistry.mainBasePath, "StickyData.json");
        
        string jsonData = JsonSerializer.Serialize(data);
        File.WriteAllText(file_path, jsonData);
    }

    public static void InstallOpenSticky(string stickyName)
    {
        string path = Path.Combine(ClassRegistry.PathOpenStickers, $"~{stickyName}");
        Directory.CreateDirectory(path);
    }

    public static void DeleteOpenSticky(string stickyName)
    {
        string path = Path.Combine(ClassRegistry.PathOpenStickers, $"~{stickyName}");
        Directory.Delete(path);
    }

    public static string getPathDataSticker(string nameSticker)
    {
        return getDataSticker(nameSticker).StickerCurrentPath;
    }

    public static StickyData readDataSticker(string nameSticker)
    {
        return getDataSticker(nameSticker).StickyData;
    }

    private static Models.Sticker getDataSticker(string nameSticker)
    {
        string file_path = Path.Combine(ClassRegistry.mainBasePath, "StickyData.json");
        using FileStream fs = new FileStream(file_path, FileMode.OpenOrCreate);
        StickersList? data = JsonSerializer.Deserialize<StickersList>(fs);
        
        if (data is { Stickers: not null })
            return data.Stickers.FirstOrDefault(sticker => sticker.Name == nameSticker)!;
        throw new NullReferenceException("данный стикер не найден");
    }

    public static void updateStickerData(string nameSticker, StickyData stickyData)
    {
        StickersList stickersList = getAllDataSticker();
        foreach (var sticker in stickersList.Stickers!.Where(sticker => sticker.Name == nameSticker))
        {
            sticker.StickyData = stickyData;
        }
        
        updateAllStickerData(stickersList);
    }

    public static void updateStateStickerFavorite(string nameSticker)
    {
        StickersList stickersList = getAllDataSticker();
        foreach (var sticker in stickersList.Stickers!.Where(sticker => sticker.Name == nameSticker))
        {
            sticker.StickerFavorite = !sticker.StickerFavorite;
        }
        
        updateAllStickerData(stickersList);
    } 
    
    public static void updateStateStickerThumbtack(string nameSticker)
    {
        StickersList stickersList = getAllDataSticker();
        foreach (var sticker in stickersList.Stickers!.Where(sticker => sticker.Name == nameSticker))
        {
            sticker.StickerThumbtack = !sticker.StickerThumbtack;
        }
        
        updateAllStickerData(stickersList);
    }

    public static void deleteSticker(string nameSticker)
    {
        StickersList stickersList = getAllDataSticker();

        for (int i = 0; i < stickersList.Stickers!.Count; i++)
        {
            if (stickersList.Stickers[i].Name != nameSticker) continue;
            if (!Directory.Exists(Path.Combine(ClassRegistry.PathOpenStickers, $"~{nameSticker}")))
            {
                File.Delete(stickersList.Stickers[i].StickerCurrentPath);
                stickersList.Stickers.RemoveAt(i);
            }
            else
            {
                MessageBox.Show("Невозможно удалить стикер т.к он открыт!", "StickyNotes", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        updateAllStickerData(stickersList);
    }
}