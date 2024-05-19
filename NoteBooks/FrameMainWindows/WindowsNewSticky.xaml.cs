using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;
using NoteBooks.Models;

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
    
    // TODO: Реализовать проверку на дубликат стикера
    private void CreateSticky_OnClick(object sender, RoutedEventArgs e)
    {
        if (TextBox_NameSticky.Text == "")
        {
            MessageBox.Show("Имя стикера не должно быть пустым", "StickyNotes", MessageBoxButton.OK);
            return;
        }

        if (Sticker.getResultDublicateStickerName(TextBox_NameSticky.Text))
        {
            MessageBox.Show("Стикера с данным именем уже существует", "StickyNotes", MessageBoxButton.OK);
            return;
        }

        List<int> size = new List<int>(2) { stickySize, stickySize + 35 };
        List<int> pos = new List<int>(2) { 250, 250 };
        string password = (PasswordBox.Text != "") ? Cryptography.Encrypt(PasswordBox.Text): Cryptography.Encrypt("None");

        StickyData sticky = new StickyData(
            security: PasswordBox.Text.Length > 0 ? true : false,
            color: stickyColor,
            size: size,
            opacity: stickyOpacity,
            password: password,
            position: pos);

        Models.Sticker stickerData = new Models.Sticker(
            name: TextBox_NameSticky.Text,
            color: (int)stickyColor,
            dateStart: DateTime.Now,
            currentDateUpdate: DateTime.Now,
            stickerThumbtack: stickyThumbtack,
            stickerFavorite: false,
            stickerCurrentPath: Path.Combine(ClassRegistry.PathStickyFolder, $"{TextBox_NameSticky.Text}.rtf"),
            stickyData: sticky);

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

        Sticker.createNewSticker(stickerData);
        
        var sticker = new Sticky(TextBox_NameSticky.Text);
        sticker.Show();
    }

    private int stickySize;
    private void ComboBox_Size_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        switch (Convert.ToInt32(ComboBox_Size.SelectedIndex.ToString()))
        {
            case 0:
                stickySize = 250;
                break;
            case 1:
                stickySize = 300;
                break;
            case 2:
                stickySize = 350;
                break;
            case 3:
                stickySize = 400;
                break;
            case 4:
                stickySize = 450;
                break;
            case 5:
                stickySize = 500;
                break;
            case 6:
                stickySize = 550;
                break;
            case 7:
                stickySize = 600;
                break;
            case 8:
                stickySize = 650;
                break;
            case 9:
                stickySize = 700;
                break;
            case 10:
                stickySize = 750;
                break;
        }
    }

    private BrushConverter converter = new BrushConverter();
    private void StateColorStickyPreview(ColorSticky.stickyColor color)
    {
        MainFrame.Background = (Brush)converter.ConvertFromString(ColorSticky.color(color)[1]);
        HeaderMenu.Background = (Brush)converter.ConvertFromString(ColorSticky.color(color)[0]);
    }

    private ColorSticky.stickyColor stickyColor;
    private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        Border borderColor = (Border)sender;

        switch (borderColor.Name)
        {
            case "Purple":
                stickyColor = ColorSticky.stickyColor.purple;
                break;
            case "Pink":
                stickyColor = ColorSticky.stickyColor.pink;
                break;
            case "Green":
                stickyColor = ColorSticky.stickyColor.green;
                break;
            case "Blue":
                stickyColor = ColorSticky.stickyColor.blue;
                break;
            case "Yellow":
                stickyColor = ColorSticky.stickyColor.yellow;
                break;
            case "Black":
                stickyColor = ColorSticky.stickyColor.black;
                break;
            case "Gray":
                stickyColor = ColorSticky.stickyColor.gray;
                break;
        }
        
        StateColorStickyPreview(stickyColor);
    }
    
    private double stickyOpacity = 1;
    private void ComboBox_Opacity_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        switch (ComboBox_Opacity.SelectedIndex)
        {
            case 0:
                stickyOpacity = 1;
                break;
            case 1:
                stickyOpacity = 0.9;
                break;
            case 2:
                stickyOpacity = 0.8;
                break;
            case 3:
                stickyOpacity = 0.7;
                break;
            case 4:
                stickyOpacity = 0.6;
                break;
            case 5:
                stickyOpacity = 0.5;
                break;
            case 6:
                stickyOpacity = 0.4;
                break;
            case 7:
                stickyOpacity = 0.3;
                break;
            case 8:
                stickyOpacity = 0.2;
                break;
            case 9:
                stickyOpacity = 0.1;
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
}