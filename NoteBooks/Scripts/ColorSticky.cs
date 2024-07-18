using System.Collections.Generic;

namespace StickyNotes.Scripts;

public class ColorSticky
{
    private static readonly string[,] FontColor = {
        // Заголовок, остальная часть стикера
        { "#E7CFFF", "#F2E6FF", "#d0a2ff", "#c300ff" },  // Фиолетовый
        { "#FFCCE5", "#FFE4F1", "#ffacd5", "#ff05ac" },  // Розовый
        { "#CBF1C4", "#E4F9E0", "#afffa0", "#0db900" },  // Зеленый
        { "#CDE9FF", "#E2F1FF", "#a7d8ff", "#00d6ff" },  // Голубой
        { "#FFF2AB", "#FFF7D1", "#ffee94", "#d5cf00" },  // Желтый
        { "#494745", "#696969", "#3c3c3c", "#0b0b0b" },  // Черный
        { "#E1DFDD", "#F3F2F1", "#bebebe", "#b8b8b8" }   // Серый
    };
    
    public enum stickyColor
    {
        purple,
        pink,
        green,
        blue,
        yellow,
        black,
        gray
    }

    public static List<string> color(stickyColor color)
    {
        List<string> result = new List<string>();
        int num;
        
        if (color == stickyColor.purple)
            num = 0;
        else if (color == stickyColor.pink)
            num = 1;
        else if (color == stickyColor.green)
            num = 2;
        else if (color == stickyColor.blue)
            num = 3;
        else if (color == stickyColor.yellow)
            num = 4;
        else if (color == stickyColor.black)
            num = 5;
        else if (color == stickyColor.gray)
            num = 6;
        else
            num = 0;
        
        result.Add(FontColor[num, 0]);
        result.Add(FontColor[num, 1]);
        result.Add(FontColor[num, 2]);
        result.Add(FontColor[num, 3]);
        return result;
    }
}