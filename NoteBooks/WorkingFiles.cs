using System;
using System.Collections.Generic;
using System.IO;

namespace NoteBooks;

public class WorkingFiles
{
    private static readonly string basePathFolder = "baseFolder_StickyData";

    #region Техническое задание

        // Проверка наличия данных стикера <- name -> bool
        // Обновление данных стикера <- name, Dictionary -> bool
        // Чтение данных стикера <- name -> Dictionary
        // Возврат пути хранения текста стикера <- name -> string
        // Обновляем статус открытия/закрытия стикера
        // Удаление данных о стикере
        // Обновление имя стикера
        // Обновление цвета заголовка стикера
        // Обновление даты последнего изменения данных стикера
        // Добавление/удаление стикера в избранное
        // Обновляем информацию о стикере закреплен/откреплен
        // Функция обновления даты изменения стикера
        // Создание данных стикера, добавление информации о стикере <- Dictionary, name -> bool

    #endregion

    public static void checkStickerAvailability(string nameSticker)
    {
        if (Directory.Exists($"{basePathFolder}/{nameSticker}"))
        {
            if (!File.Exists($"{basePathFolder}/{nameSticker}/{nameSticker}.txt")) 
                throw new FileNotFoundException("Конфигурационные данные стикера не найдены!");
        }
        else
            throw new DirectoryNotFoundException("Каталог с данными не найден!");
    }

    public static bool checkIsOpenFile(string nameSticker)
    {
        Dictionary<string, List<string>> data = getStickyDataProgram();
        if (Convert.ToInt32(data[nameSticker][5]) == 0)
            return true;
        return false;
    }

    public static List<string> getThumbtackSticky()
    {
        List<string> data = new List<string>();
        foreach (KeyValuePair<string,List<string>> index in getStickyDataProgram())
        {
            if (Convert.ToInt32(index.Value[4]) == 1)
                data.Add(index.Key);
        }

        return data;
    }

    private static string currentDate()
    {
        DateTime dateTime = DateTime.Now;
        string date = $"{dateTime.Year:D2}-{dateTime.Month:D2}-{dateTime.Day:D2}";

        return date;
    }

    public static Dictionary<string, List<string>> getStickyDataProgram()
    {
        string path = "ProgramData.txt";
        Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();
        
        using (StreamReader reader = new StreamReader(path))
        {
            string? line;
            string name = "# Stickers";
            int currentPosition = 0;
            int needPosition = 100;
            while ((line = reader.ReadLine()) != null)
            {
                if (line == "") continue;
                currentPosition++;

                if (line == name)
                    needPosition = currentPosition;

                if (currentPosition > needPosition)
                {
                    string[] parts = line.Split(new string[] { ", " }, StringSplitOptions.None);
                    
                    string nameSticker = parts[0];
                    List<string> properties = new List<string>();
                    for (int i = 1; i < parts.Length; i++)
                        properties.Add(parts[i]);
                    
                    data.Add(nameSticker, properties);
                }
            }
        }

        return data;
    }

    public static Dictionary<string, List<string>> getDataFavoriteSticky()
    {
        Dictionary<string, List<string>> newDataSticky = new Dictionary<string, List<string>>();

        foreach (KeyValuePair<string,List<string>> index in getStickyDataProgram())
        {
            if (index.Value[3] == "1")
            {
                newDataSticky.Add(index.Key, index.Value);
            }
        }

        return newDataSticky;
    }
    
    public static Dictionary<string, List<string>> getDataThumbtackSticky()
    {
        Dictionary<string, List<string>> newDataSticky = new Dictionary<string, List<string>>();

        foreach (KeyValuePair<string,List<string>> index in getStickyDataProgram())
        {
            if (index.Value[4] == "1")
            {
                newDataSticky.Add(index.Key, index.Value);
            }
        }

        return newDataSticky;
    }

    public static void updateFavoriteStatus(string nameSticker)
    {
        string path = "ProgramData.txt";
        string input;

        using (StreamReader reader = new StreamReader(path))
        {
            input = reader.ReadToEnd();
        }
        string[] text = input.Split("\n");
        int num_text = 0;
        foreach (string index in text)
        {
            if (index.Split(", ")[0] == nameSticker)
            {
                string[] txt = text[num_text].Split(", ");
                if (Convert.ToInt32(txt[4]) == 0)
                    txt[4] = txt[4].Replace('0', '1');
                else if (Convert.ToInt32(txt[4]) == 1)
                    txt[4] = txt[4].Replace('1', '0');
                
                text[num_text] = string.Join(", ", txt);
                break;
            }
            num_text++;
        }
        string newText = string.Join("\n", text);

        using (StreamWriter writer = new StreamWriter(path, false))
        {
            writer.WriteLine(newText);
        }
    }

    public static void deleteSticker(string nameSticker)
    {
        try
        {
            string filePath = "ProgramData.txt";
            string[] lines = File.ReadAllLines(filePath);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Split(", ")[0] == nameSticker)
                {
                    lines[i] = null;
                    break;
                }
            }

            string result = string.Join("\n", lines);

            File.WriteAllText(filePath, result);

            Directory.Delete($"{basePathFolder}/{nameSticker}", true);
        }
        catch {}
    }

    public static Dictionary<string, List<string>> searchSticky(string query)
    {
        Dictionary<string, List<string>> dataSticky = getStickyDataProgram();
        Dictionary<string, List<string>> newDataSticky = new Dictionary<string, List<string>>();

        foreach (KeyValuePair<string,List<string>> index in dataSticky)
        {
            if (index.Key.Contains(query))
                newDataSticky.Add(index.Key, index.Value);
        }

        return newDataSticky;
    }
    
    public static bool checkDeleteSticker()
    {
        string path = "ProgramData.txt";
        
        using (StreamReader reader = new StreamReader(path))
        {
            string input = reader.ReadToEnd();
            string[] texts = input.Split("\n");
            return Convert.ToBoolean(Convert.ToInt32(texts[3]));
        }
    }

    public static void updateThumbtackStatus(string nameSticker)
    {
        string path = "ProgramData.txt";
        string input;

        using (StreamReader reader = new StreamReader(path))
        {
            input = reader.ReadToEnd();
        }
        string[] text = input.Split("\n");
        int num_text = 0;
        foreach (string index in text)
        {
            if (index.Split(", ")[0] == nameSticker)
            {
                string[] txt = text[num_text].Split(", ");
                if (Convert.ToInt32(txt[5]) == 0)
                    txt[5] = txt[5].Replace('0', '1');
                else if (Convert.ToInt32(txt[5]) == 1)
                    txt[5] = txt[5].Replace('1', '0');
                
                text[num_text] = string.Join(", ", txt);
                break;
            }
            num_text++;
        }
        string newText = string.Join("\n", text);

        using (StreamWriter writer = new StreamWriter(path, false))
        {
            writer.WriteLine(newText);
        }
    }

    public static Dictionary<string, string> readDataSticker(string nameSticker)
    {
        string path = $"{basePathFolder}/{nameSticker}/{nameSticker}.txt";
        Dictionary<string, string> data = new Dictionary<string, string>();
        
        using (StreamReader reader = new StreamReader(path))
        {
            string? line;
            int currendPosition = 0;
            int needPosition = 0;
            string field = "";
            while ((line = reader.ReadLine()) != null)
            {
                if (line == "") break;
                currendPosition++;
                if (line[0] == '#')
                {
                    field = line.Substring(2);
                    needPosition = currendPosition + 1;
                }

                if (currendPosition == needPosition)
                    data.Add(field, line);
            }
        }

        return data;
    }

    public static string getPathDataSticker(string nameSticker)
    {
        return $"{basePathFolder}/{nameSticker}/{nameSticker}.rtf";
    }

    private static void updateData(Dictionary<string, string> data, string nameSticker)
    {
        string path = $"{basePathFolder}/{nameSticker}/{nameSticker}.txt";
        string text = "";

        foreach (var inform in data)
        {
            text = text + $"# {inform.Key}\n{inform.Value}\n";
        }
        using (StreamWriter writer = new StreamWriter(path, false))
        {
            writer.WriteLine(text);
        }
    }

    public static void updateDataStickerPassword(string nameSticker, string data)
    {
        Dictionary<string, string> stickerData = readDataSticker(nameSticker);

        stickerData["Password"] = data;
        updateData(stickerData, nameSticker);
    }
    public static void updateDataStickerColor(string nameSticker, string data)
    {
        Dictionary<string, string> stickerData = readDataSticker(nameSticker);
        
        stickerData["Color"] = data;
        updateData(stickerData, nameSticker);
    }
    public static void updateDataStickerPosition(string nameSticker, string data)
    {
        Dictionary<string, string> stickerData = readDataSticker(nameSticker);
        
        stickerData["Position"] = data;
        updateData(stickerData, nameSticker);
    }
    public static void updateDataStickerSize(string nameSticker, string data)
    {
        Dictionary<string, string> stickerData = readDataSticker(nameSticker);
        
        stickerData["Size"] = data;
        updateData(stickerData, nameSticker);
    }
    public static void updateDataStickerOpasity(string nameSticker, string data)
    {
        Dictionary<string, string> stickerData = readDataSticker(nameSticker);
        
        stickerData["Opasity"] = data;
        updateData(stickerData, nameSticker);
    }
    public static void updateDataStickerState(string nameSticker, string data)
    {
        Dictionary<string, string> stickerData = readDataSticker(nameSticker);
        
        stickerData["ExitState"] = data;
        updateData(stickerData, nameSticker);
    }

    public static void updateDataSticker(string nameSticker, Dictionary<string, string> dataSticker)
    {
        foreach (var data in dataSticker)
        {
            if (data.Key == "Password")
                updateDataStickerPassword(nameSticker, data.Value);
            else if (data.Key == "Color")
                updateDataStickerColor(nameSticker, data.Value);
            else if (data.Key == "Position")
                updateDataStickerPosition(nameSticker, data.Value);
            else if (data.Key == "Size")
                updateDataStickerSize(nameSticker, data.Value);
            else if (data.Key == "Opasity")
                updateDataStickerOpasity(nameSticker, data.Value);
            else if (data.Key == "ExitState")
                updateDataStickerState(nameSticker, data.Value);
        }
    }

    public static void createSticker(string name, Dictionary<string, string> dataSticker, Dictionary<string, string> settingsData)
    {
        Directory.CreateDirectory($"{basePathFolder}/{name}");
        updateData(dataSticker, name);
        
        using (StreamWriter writer = new StreamWriter("ProgramData.txt", true))
        {
            writer.WriteLine($"{name}, {settingsData["Color"]}, {settingsData["Date"]}, {settingsData["Date"]}, {settingsData["Favorite"]}, {settingsData["Thumbtack"]}, 1");
        }
    }
}