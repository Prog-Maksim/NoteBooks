﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using MahApps.Metro.IconPacks;
using Microsoft.Win32;
using NoteBooks.Models;

namespace NoteBooks;

public class Cryptography
{
    private const int Key = 3;

    public static string Encrypt(string input)
    {
        StringBuilder encrypted = new StringBuilder();

        foreach (char c in input)
        {
            encrypted.Append((char)(c + Key));
        }

        return encrypted.ToString();
    }

    public static string Decrypt(string input)
    {
        StringBuilder decrypted = new StringBuilder();

        foreach (char c in input)
        {
            decrypted.Append((char)(c - Key));
        }

        return decrypted.ToString();
    }
}

public partial class Sticky
{
    // Характеристики стикера
    private readonly string baseStickyName = "Sticky";
    private ColorSticky.stickyColor fontnum;
    private double _opacitySticky = 1;
    
    private int _stickyWidth = 300;
    private int _stickyHeight = 335;
    private Point _windowsСoordinates;
    
    private bool _changeStickyState = true; // отвечает за состояние стикера
    private bool _thumbtack;
    private bool _isMaximized; 
    private double _baseSizeFont = 16;

    // Безопасность стикера
    private bool _stateSecurity;
    private bool _lockState;
    private string _stickyPassword;

    // Настройки анимации стикера
    private bool _stickyFocusAnimation = true;
    private bool _stickyAnimation = true;
    private double _timeSpanTopMenu = 0.3;
    private double _timeSpanBottonMenu = 0.1;

    private string _stickyName;

    public Sticky()
    {
        InitializeComponent();

        MainStickyName.Text = _stickyName = baseStickyName;
        installIcon();
        installStickySettings();
    }

    public Sticky(string name)
    {
        InitializeComponent();
        
        MainStickyName.Text = _stickyName = name;
        GetDataSticky();
        StartInitiation();
        installStickySettings();
        
        this.Closing += MainWindow_Closing;
        Sticker.InstallOpenSticky(name);
    }

    private void installStickySettings()
    {
        _stickyFocusAnimation = FileSettings.closeMenu;
        _stickyAnimation = FileSettings.closeMenuAnimation;
        _timeSpanTopMenu = FileSettings.delayTopMenu;
        _timeSpanBottonMenu = FileSettings.delayBottonMenu;
    }

    private void GetDataSticky()
    {
        StickyData data = Sticker.readDataSticker(this._stickyName);

        _stateSecurity = data.Security;
        _lockState = data.Security;
        if (data.Security)
            _stickyPassword = Cryptography.Decrypt(data.Password!);

        fontnum = data.Color;
        _windowsСoordinates = new Point(data.Position[0], data.Position[1]);
        _stickyWidth = data.Size[0];
        _stickyHeight = data.Size[1];
        _opacitySticky = data.Opacity;
    }
    
    private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        List<int> pos = new List<int>(2) {Convert.ToInt32(this._windowsСoordinates.X), Convert.ToInt32(this._windowsСoordinates.Y)};
        List<int> size = new List<int>(2) { Convert.ToInt32(this.Width), Convert.ToInt32(this.Height) };
        
        StickyData stickyData = new StickyData(security: _stateSecurity, color: fontnum, size: size,
            opacity: _opacitySticky, position: pos, password: _stickyPassword);
        Sticker.updateStickerData(_stickyName, stickyData);
            
        SaveToFile();
        Sticker.DeleteOpenSticky(_stickyName);
    }
    
    private void installIcon()
    {
        string imagePath = "";
        if (fontnum == ColorSticky.stickyColor.purple) 
            imagePath = "pack://application:,,,/Resource/ImageTaskBar/purple_sticky_icon.png";
        else if (fontnum == ColorSticky.stickyColor.pink)
            imagePath = "pack://application:,,,/Resource/ImageTaskBar/pink_sticky_icon.png";
        else if (fontnum == ColorSticky.stickyColor.green)
            imagePath = "pack://application:,,,/Resource/ImageTaskBar/green_sticky_icon.png";
        else if (fontnum == ColorSticky.stickyColor.blue)
            imagePath = "pack://application:,,,/Resource/ImageTaskBar/blue_sticky_icon.png";
        else if (fontnum == ColorSticky.stickyColor.yellow)
            imagePath = "pack://application:,,,/Resource/ImageTaskBar/yellow_sticky_icon.png";
        else if (fontnum == ColorSticky.stickyColor.black)
            imagePath = "pack://application:,,,/Resource/ImageTaskBar/dark_sticky_icon.png";
        else if (fontnum == ColorSticky.stickyColor.gray)
            imagePath = "pack://application:,,,/Resource/ImageTaskBar/gray_sticky_icon.png";
        var path = new Uri(imagePath);
        this.Icon = new BitmapImage(path);
    }
    
    private void SaveToFile()
    {
        TextRange doc = new TextRange(MainRichTextBox.Document.ContentStart, MainRichTextBox.Document.ContentEnd);
        using (FileStream fs = File.Create(Sticker.getPathDataSticker(_stickyName)))
            doc.Save(fs, DataFormats.Rtf);
    }
    
    private void LoadTextSticky()
    {
        try
        {
            TextRange doc = new TextRange(MainRichTextBox.Document.ContentStart, MainRichTextBox.Document.ContentEnd);
            using (FileStream fs = new FileStream(Sticker.getPathDataSticker(_stickyName), FileMode.Open))
                doc.Load(fs, DataFormats.Rtf);
        }
        catch { }
    }

    private void StartInitiation()
    {
        InstallColor();
        SettingStickySize();
        UpdateBackgroundFontSticky();
        LoadTextSticky();
        
        this.Title = _stickyName;
        
        this.Top = this._windowsСoordinates.X;
        this.Left = this._windowsСoordinates.Y;
        
        MainRichTextBox.Focus();
        if (_stateSecurity) SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
        if (_stateSecurity) LockSticky();
    }
    
    private void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
    {
        switch (e.Reason)
        {
            case SessionSwitchReason.SessionLock:
                LockSticky();
                break;
            case SessionSwitchReason.SessionUnlock:
                CheckMark.Kind = PackIconMaterialKind.Eye;
                break;
        }
    }

    private void LockSticky()
    {
        if (!_stateSecurity) return;
        
        stateOnlySticky();
        _lockState = true;
        CheckMark.Kind = PackIconMaterialKind.Eye;
        MainRichTextBox.Opacity = 0;
    }

    private bool UnlockSticky()
    {
        if (!_stateSecurity) return true;
        
        var passwordWindow = new PasswordWindow();

        if (passwordWindow.ShowDialog() == true && passwordWindow.Password == _stickyPassword)
        {
            _lockState = false;
            CheckMark.Kind = PackIconMaterialKind.Pencil;
            MainRichTextBox.Opacity = 1;
            return true;
        }
        MessageBox.Show("Введенный вами пароль не верен или\nВвод пароля был отменен", "Проверка пароля", MessageBoxButton.OK);
        return false;
    }

    private void Exit(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void Border_MouseDows(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton != MouseButton.Left || !_changeStickyState) return;
        try
        {
            DragMove();

            this._windowsСoordinates = new Point(this.Top, this.Left);
        }
        catch {}
    }

    private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount != 2 || !_changeStickyState) return;
        
        if (_isMaximized)
        {
            WindowState = WindowState.Normal;
            Width = _stickyWidth;
            Height = _stickyHeight;

            _isMaximized = false;
        }
        else
        {
            WindowState = WindowState.Maximized;
            _isMaximized = true;
        }
    }

    private void BorderInform_OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount != 2 || !_changeStickyState) return;
        if (this._changeStickyState)
            this.WindowState = WindowState.Minimized;
    }

    private void Strikeout()
    {
        if (MainRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty) == TextDecorations.Strikethrough)
            MainRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
        else
            MainRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Strikethrough);
    }

    private void Underline()
    {
        if (MainRichTextBox.Selection.GetPropertyValue(Inline.TextDecorationsProperty) == TextDecorations.Underline)
            MainRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
        else
            MainRichTextBox.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
    }

    private void Bold()
    {
        if (MainRichTextBox.Selection.GetPropertyValue(TextElement.FontWeightProperty).Equals(FontWeights.Bold))
            MainRichTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
        else
            MainRichTextBox.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
    }

    private void Italic()
    {
        if (MainRichTextBox.Selection.GetPropertyValue(TextElement.FontStyleProperty) is FontStyle style && style == FontStyles.Italic)
            MainRichTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
        else
            MainRichTextBox.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
    }

    private void InstallColor()
    {
        var color = (Color)ColorConverter.ConvertFromString(ColorSticky.color(fontnum)[1]);
        color.A = (byte)(_opacitySticky * 255);

        var selectionColor = (Color)ColorConverter.ConvertFromString(ColorSticky.color(fontnum)[2]);
        MainRichTextBox.SelectionBrush = new SolidColorBrush(selectionColor);            
        SomeSticky.Background = new SolidColorBrush(color);
    }

    private void stateOnlySticky()
    {
        _changeStickyState = false;
        if (_stickyAnimation)
            StickyCloseBottonMenuAnimation();
        else
            BottonMenuSettings.Height = 0;
        ShowInTaskbar = false;
        ResizeMode = ResizeMode.NoResize;
        CheckMark.Kind = PackIconMaterialKind.Pencil;
        MainRichTextBox.IsReadOnly = !_changeStickyState;
    }

    private void StateReadOnlySticky()
    {
        if (_lockState)
            if (!UnlockSticky())
                return;
        
        if (_changeStickyState) stateOnlySticky();
        else
        {
            _changeStickyState = true;
            if (_stickyAnimation)
                StickyOpenBottonMenuAnimation();
            else
                BottonMenuSettings.Height = 30;
            ShowInTaskbar = true;
            CheckMark.Kind = PackIconMaterialKind.Check;
            ResizeMode = ResizeMode.CanResize;
            MainRichTextBox.IsReadOnly = !_changeStickyState;
        }
    }

    private void UpdateBackgroundFontSticky()
    {
        if (fontnum > ColorSticky.stickyColor.gray) fontnum = ColorSticky.stickyColor.purple;
        
        MainRichTextBox.Foreground = new SolidColorBrush(Colors.Black);
        if (fontnum == ColorSticky.stickyColor.black) MainRichTextBox.Foreground = new SolidColorBrush(Colors.White);

        BorderInform.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(ColorSticky.color(fontnum)[0]));
        installIcon();
    }

    private void RichTextBox_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.A)
        {
            var sticky = new Sticky();
            sticky.Show();
        }
        else if (e.Key == Key.Enter && _lockState)
                UnlockSticky();
        else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.Y)
            StateReadOnlySticky();
        else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.L && !_lockState)
            LockSticky();
        else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.Q)
        {
            try
            {
                foreach (Window window in App.Current.Windows)
                {
                    if (window is MainWindow)
                    {
                        window.WindowState = WindowState.Normal;
                        return;
                    }
                }

                var mainWindow = new MainWindow();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
            }
        }
        else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.T)
        {
            _thumbtack = !_thumbtack;
            Sticker.updateStateStickerThumbtack(this._stickyName);
            string text = "Стикер был " + (_thumbtack? "закреплен!": "откреплен!");
            new Thread(() => MessageBox.Show(text, "StickyNotes", MessageBoxButton.OK)).Start();
        }
        
        else if (!_changeStickyState) return;
        
        else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.X)
        {
            fontnum++;

            UpdateBackgroundFontSticky();
            InstallColor();
        }
        else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.S)
            Strikeout();
        else if (e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Alt) && e.Key == Key.Oem4)
        {
            if (_opacitySticky >= 0.2) _opacitySticky -= 0.1;
            InstallColor();
        }
        else if (e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Alt) && e.Key == Key.Oem6)
        {
            if (_opacitySticky <= 0.9) _opacitySticky += 0.1;
            InstallColor();
        }
        else if (e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift ) && e.Key == Key.Oem4)
        {
            if (_baseSizeFont > 5) _baseSizeFont -= 1;
            MainRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, _baseSizeFont);
        }
        else if (e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift) && e.Key == Key.Oem6)
        {
            if (_baseSizeFont < 30) _baseSizeFont += 1;
            MainRichTextBox.Selection.ApplyPropertyValue(Inline.FontSizeProperty, _baseSizeFont);
        }
        
        else if (e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift) && e.Key == Key.B)
            MainRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
        else if (e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift) && e.Key == Key.R)
            MainRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
        else if (e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift) && e.Key == Key.G)
            MainRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
        else if (e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift) && e.Key == Key.Y)
            MainRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Yellow);
        else if (e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift) && e.Key == Key.W)
            MainRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.White);
        else if (e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift) && e.Key == Key.O)
            MainRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Orange);
        else if (e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift) && e.Key == Key.D)
            MainRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
        else if (e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift) && e.Key == Key.P)
            MainRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Purple);
        else if (e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift) && e.Key == Key.F)
            MainRichTextBox.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Gray);
        else if (e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift) && e.Key == Key.S)
        {
            if (this._stickyName == baseStickyName)
            {
                Random rnd = new Random();
                this._stickyName = $"{baseStickyName}_{rnd.Next(11111, 99999)}";
                MainStickyName.Text = this._stickyName;
                
                List<int> pos = new List<int>(2) {Convert.ToInt32(this._windowsСoordinates.X), Convert.ToInt32(this._windowsСoordinates.Y)};
                List<int> size = new List<int>(2) { Convert.ToInt32(this.Width), Convert.ToInt32(this.Height) };

                StickyData sticky = new StickyData(
                    security: false,
                    color: this.fontnum,
                    size: size,
                    opacity: this._opacitySticky,
                    position: pos
                    );

                Models.Sticker stickerData = new Models.Sticker(
                    name: this._stickyName,
                    color: (int)fontnum,
                    dateStart: DateTime.Now,
                    currentDateUpdate: DateTime.Now,
                    stickerThumbtack: this._thumbtack,
                    stickerFavorite: false,
                    stickerCurrentPath: Path.Combine(ClassRegistry.PathStickyFolder, $"{this._stickyName}.rtf"),
                    stickyData: sticky
                    );
                Sticker.createNewSticker(stickerData);
                Directory.CreateDirectory(Path.Combine(ClassRegistry.PathOpenStickers, $"~{this._stickyName}"));
                this.Closing += MainWindow_Closing;
            }
            SaveToFile();
        }
    }

    private void SettingStickySize()
    {
        Height = _stickyHeight;
        Width = _stickyWidth;
    }

    private void StateBoldFont(object sender, RoutedEventArgs e)
    {
        if (_changeStickyState) Bold();
    }

    private void StateItalicFont(object sender, RoutedEventArgs e)
    {
        if (_changeStickyState) Italic();
    }

    private void StateUnderlineFont(object sender, RoutedEventArgs e)
    {
        if (_changeStickyState) Underline();
    }

    private void StateStrikeoutFont(object sender, RoutedEventArgs e)
    {
        if (_changeStickyState) Strikeout();
    }

    private void CheckMark_OnMouseDown(object sender, RoutedEventArgs e)
    {
        SaveToFile();
        StateReadOnlySticky();
    }

    private void new_Sticky(object sender, RoutedEventArgs e)
    {
        var sticky = new Sticky();
        sticky.Show();
    }

    private void StickyCloseBottonMenuAnimation()
    {
        DoubleAnimation menuHeightAnimation = new DoubleAnimation();
        menuHeightAnimation.To = 0;
        menuHeightAnimation.Duration = new Duration(TimeSpan.FromSeconds(_timeSpanBottonMenu));

        BottonMenuSettings.BeginAnimation(HeightProperty, menuHeightAnimation);
    }

    private void StickyOpenBottonMenuAnimation()
    {
        DoubleAnimation menuHeightAnimation = new DoubleAnimation();
        menuHeightAnimation.To = 30;
        menuHeightAnimation.Duration = new Duration(TimeSpan.FromSeconds(_timeSpanBottonMenu));

        BottonMenuSettings.BeginAnimation(HeightProperty, menuHeightAnimation);
    }

    private void StickyCloseMenu()
    {
        BorderInform.Height = 10;
        MainStickyName.Visibility = Visibility.Hidden;
        if (_changeStickyState) BottonMenuSettings.Height = 0;
    }

    private void StickyOpenMenu()
    {
        BorderInform.Height = 35;
        MainStickyName.Visibility = Visibility.Visible;
        if (_changeStickyState) BottonMenuSettings.Height = 30;
    }

    private void StickyCloseMenuAnimation()
    {
        DoubleAnimation heightAnimation = new DoubleAnimation();
        heightAnimation.To = 10;
        heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(_timeSpanTopMenu));

        BorderInform.BeginAnimation(HeightProperty, heightAnimation);
        
        if (_changeStickyState)
            StickyCloseBottonMenuAnimation();
        
        MainStickyName.Visibility = Visibility.Hidden;
    }

    private void StickyOpenMenuAnimation()
    {
        DoubleAnimation heightAnimation = new DoubleAnimation();
        heightAnimation.To = 35;
        heightAnimation.Duration = new Duration(TimeSpan.FromSeconds(_timeSpanTopMenu));

        BorderInform.BeginAnimation(HeightProperty, heightAnimation);
        
        if (_changeStickyState)
            StickyOpenBottonMenuAnimation();
        
        MainStickyName.Visibility = Visibility.Visible;
    }

    private void Sticky_OnDeactivated(object? sender, EventArgs e)
    {
        if (!_stickyFocusAnimation) return;
        
        if (_stickyAnimation)
            StickyCloseMenuAnimation();
        else
            StickyCloseMenu();
    }

    private void Sticky_OnActivated(object? sender, EventArgs e)
    {
        if (!_stickyFocusAnimation) return;
        
        if (_stickyAnimation)
            StickyOpenMenuAnimation();
        else
            StickyOpenMenu();
    }

    private void MainRichTextBox_OnSelectionChanged(object sender, RoutedEventArgs e)
    {
        try
        {
            if (MainRichTextBox.Selection.Text.Length < 1)
            {
                IconBold.Foreground = new SolidColorBrush(Colors.Gray);
                IconItalic.Foreground = new SolidColorBrush(Colors.Gray);
                IconStrikeout.Foreground = new SolidColorBrush(Colors.Gray);
                IconUnderline.Foreground = new SolidColorBrush(Colors.Gray);
            }
            else
            {
                if (MainRichTextBox.Selection.GetPropertyValue(TextElement.FontWeightProperty).Equals(FontWeights.Bold))
                    IconBold.Foreground = new SolidColorBrush(Colors.Black);
                if (MainRichTextBox.Selection.GetPropertyValue(TextElement.FontStyleProperty) is FontStyle style && style == FontStyles.Italic)
                    IconItalic.Foreground = new SolidColorBrush(Colors.Black);
            
                TextRange selectionRange = new TextRange(MainRichTextBox.Selection.Start, MainRichTextBox.Selection.End);

                if (selectionRange.GetPropertyValue(Inline.TextDecorationsProperty).Equals(TextDecorations.Underline))
                    IconUnderline.Foreground = new SolidColorBrush(Colors.Black);
                if (selectionRange.GetPropertyValue(Inline.TextDecorationsProperty).Equals(TextDecorations.Strikethrough))
                    IconStrikeout.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
        catch { }
    }
}