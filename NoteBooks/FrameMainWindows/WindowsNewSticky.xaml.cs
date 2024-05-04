using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NoteBooks.FrameMainWindows;

public partial class WindowsNewSticky : Page
{
    private MainWindow app;
    public WindowsNewSticky(MainWindow window)
    {
        InitializeComponent();

        app = window;
    }

    private void SettingsMenuButton_OnClick(object sender, RoutedEventArgs e)
    {
        app.OpenSettingsMenu();
    }

    private string RandomColorStickyName()
    {
        string[] listColor = { "#ECC300", "#7E00FC", "#E80000", "#2F96E5", "#F69F1E", "#EF0090", "#F05D63", "#0ECF4F", "#461BDA", "#EC7616" };
        
        Random random = new Random();
        int index = random.Next(listColor.Length);
        return listColor[index];
    }

    private void CreateSticky_OnClick(object sender, RoutedEventArgs e)
    {
        if (TextBox_NameSticky.Text == "")
        {
            MessageBox.Show("Имя стикера не должно быть пустым", "Ошибка", MessageBoxButton.OK);
            return;
        }
        
        if (!IsNameStickyCheck(TextBox_NameSticky.Text))
        {
            MessageBox.Show("Данное имя стикера уже занято", "Ошибка", MessageBoxButton.OK);
            return;
        }

        string date = $"{DateTime.Now.Year:D2}-{DateTime.Now.Month:D2}-{DateTime.Now.Day:D2}";
        
        // Цвет: RandomColorStickyName()
        // Имя: TextBox_NameSticky.Text
        // Дата: date
        // Пароль: PasswordBox.Text
        // Цвет стикера: stickyColor
        // Размеры: stickySize
        // Координаты: "100, 100"
        // Статус закрепления стикера: Convert.ToInt32(stickyThumbtack)
        // Прозрачность: stickyOpacity
        // Избранный стикер: 0
        string password = (PasswordBox.Text != "") ? Cryptography.Encrypt(PasswordBox.Text) : Cryptography.Encrypt("None");

        Dictionary<string, string> dataSticker = new Dictionary<string, string>()
        {
            {"Password", password},
            {"Color", stickyColor.ToString()},
            {"Position", "350; 350"},
            {"Size", stickySize},
            {"Opasity", stickyOpacity},
            {"ExitState", "1"}
        };

        Dictionary<string, string> settingsData = new Dictionary<string, string>()
        {
            {"Color", RandomColorStickyName()},
            {"Date", date},
            {"Favorite", "0"},
            {"Thumbtack", Convert.ToInt32(stickyThumbtack).ToString()},
            
        };
        
        WorkingFiles.createSticker(TextBox_NameSticky.Text, dataSticker, settingsData);
        
        var sticky = new Sticky(TextBox_NameSticky.Text);
        sticky.Show();
    }

    private string stickySize;
    private void ComboBox_Size_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        switch (Convert.ToInt32(ComboBox_Size.SelectedIndex.ToString()))
        {
            case 0:
                stickySize = "250";
                break;
            case 1:
                stickySize = "300";
                break;
            case 2:
                stickySize = "350";
                break;
            case 3:
                stickySize = "400";
                break;
            case 4:
                stickySize = "450";
                break;
            case 5:
                stickySize = "500";
                break;
            case 6:
                stickySize = "550";
                break;
            case 7:
                stickySize = "600";
                break;
            case 8:
                stickySize = "650";
                break;
            case 9:
                stickySize = "700";
                break;
            case 10:
                stickySize = "750";
                break;
        }
    }

    private BrushConverter converter = new BrushConverter();
    private void StateColorStickyPreview(string[] color)
    {
        MainFrame.Background = (Brush)converter.ConvertFromString(color[1]);
        HeaderMenu.Background = (Brush)converter.ConvertFromString(color[0]);
    }

    private int stickyColor;
    private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        Border borderColor = (Border)sender;

        string[] color = new string[2];
        switch (borderColor.Background.ToString())
        { 
            case "#FFD7AFFF":
                stickyColor = 0;
                color = new[] { "#E7CFFF", "#F2E6FF" };
                break;
            case "#FFFFAFDF":
                stickyColor = 1;
                color = new[] { "#FFCCE5", "#FFE4F1" };
                break;
            case "#FFA1EF9B":
                stickyColor = 2;
                color = new[] { "#CBF1C4", "#E4F9E0" };
                break;
            case "#FF9EDFFF":
                stickyColor = 3;
                color = new[] { "#CDE9FF", "#E2F1FF" };
                break;
            case "#FFFFE66E":
                stickyColor = 4;
                color = new[] { "#FFF2AB", "#FFF7D1" };
                break;
            case "#FF767676":
                stickyColor = 5;
                color = new[] { "#494745", "#696969" };
                break;
            case "#FFE0E0E0":
                stickyColor = 6;
                color = new[] { "#E1DFDD", "#F3F2F1" };
                break;
        }
        StateColorStickyPreview(color);
    }
    
    private string stickyOpacity = "1";
    private void ComboBox_Opacity_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        switch (ComboBox_Opacity.SelectedIndex)
        {
            case 0:
                stickyOpacity = "1";
                break;
            case 1:
                stickyOpacity = "0,9";
                break;
            case 2:
                stickyOpacity = "0,8";
                break;
            case 3:
                stickyOpacity = "0,7";
                break;
            case 4:
                stickyOpacity = "0,6";
                break;
            case 5:
                stickyOpacity = "0,5";
                break;
            case 6:
                stickyOpacity = "0,4";
                break;
            case 7:
                stickyOpacity = "0,3";
                break;
            case 8:
                stickyOpacity = "0,2";
                break;
            case 9:
                stickyOpacity = "0,1";
                break;
        }
    }

    private bool stickyThumbtack;
    private void Thumbtack_OnChecked(object sender, RoutedEventArgs e)
    {
        stickyThumbtack = true;
    }

    private void Thumbtack_OnUnchecked(object sender, RoutedEventArgs e)
    {
        stickyThumbtack = false;
    }
    
    private bool IsNameStickyCheck(string text)
    {
        Dictionary<string, List<string>> data = WorkingFiles.getStickyDataProgram();
        foreach (KeyValuePair<string,List<string>> index in data)
        {
            if (index.Key == text)
                return false;
        }
        
        return true;
    }
}