using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.IconPacks;
using Microsoft.Win32;

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

public partial class Sticky : Window
{
    // Характеристики стикера
    private readonly string baseStickyName = "Sticky";
    private int fontnum;
    private double _opacitySticky = 1;
    private readonly string[,] FontColor = {
        // Заголовок, остальная часть стикера
        { "#E7CFFF", "#F2E6FF" },  // Фиолетовый
        { "#FFCCE5", "#FFE4F1" },  // Розовый
        { "#CBF1C4", "#E4F9E0" },  // Зеленый
        { "#CDE9FF", "#E2F1FF" },  // Голубой
        { "#FFF2AB", "#FFF7D1" },  // Желтый
        { "#494745", "#696969" },  // Черный
        { "#E1DFDD", "#F3F2F1" }   // Серый
    };
    
    private int _stickyWidth = 300;
    private int _stickyHeight = 335;
    private Point _windowsСoordinates;
    
    private bool _changeStickyState = true;
    private bool _thumbtack;
    private bool _isMaximized; 
    private double _baseSizeFont = 16;

    private bool stateLock; // есть ли защита на стикере
    private bool _Lockinformation; // текущее состояние стикера
    private string _stickyPassword;

    private string _stickyName;

    public Sticky()
    {
        InitializeComponent();
        
        MainStickyName.Text = _stickyName = baseStickyName;
        installIcon();
    }

    private void installIcon()
    {
        string imagePath = "";
        if (fontnum == 0) imagePath = "pack://application:,,,/Resource/ImageTaskBar/purple_sticky_icon.png";
        else if (fontnum == 1)
            imagePath = "pack://application:,,,/Resource/ImageTaskBar/pink_sticky_icon.png";
        else if (fontnum == 2)
            imagePath = "pack://application:,,,/Resource/ImageTaskBar/green_sticky_icon.png";
        else if (fontnum == 3)
            imagePath = "pack://application:,,,/Resource/ImageTaskBar/blue_sticky_icon.png";
        else if (fontnum == 4)
            imagePath = "pack://application:,,,/Resource/ImageTaskBar/yellow_sticky_icon.png";
        else if (fontnum == 5)
            imagePath = "pack://application:,,,/Resource/ImageTaskBar/dark_sticky_icon.png";
        else if (fontnum == 6)
            imagePath = "pack://application:,,,/Resource/ImageTaskBar/gray_sticky_icon.png";
        var path = new Uri(imagePath);
        this.Icon = new BitmapImage(path);
    }

    public Sticky(string name)
    {
        InitializeComponent();
        this.Title = name;
        
        try
        {
            WorkingFiles.checkStickerAvailability(name);
            MainStickyName.Text = _stickyName = name;

            GetDataSticky();
            StartInitiation();
        }
        catch (DirectoryNotFoundException ex)
        {
            new Thread(() => MessageBox.Show(ex.Message, "ошибка", MessageBoxButton.OK)).Start();
            Close();
        }
        catch (FileNotFoundException ex)
        {
            new Thread(() => MessageBox.Show(ex.Message, "ошибка", MessageBoxButton.OK)).Start();
            Close();
        }
        this.Closing += MainWindow_Closing;
        
        installIcon();
    }

    private void GetDataSticky()
    {
        Dictionary<string, string> data =  WorkingFiles.readDataSticker(this._stickyName);

        foreach (var index in data)
        {
            if (index.Key == "Password")
            {
                string passwd = Cryptography.Decrypt(index.Value);
                if (passwd != "None")
                {
                    stateLock = true;
                    _Lockinformation = true;
                    _stickyPassword = passwd;
                }
            }
            else if (index.Key == "Color")
                fontnum = Convert.ToInt32(index.Value);
            else if (index.Key == "Position")
            {
                string[] pos = index.Value.Split("; ");
                _windowsСoordinates = new Point(Convert.ToInt32(pos[0]), Convert.ToInt32(pos[1]));
            }
            else if (index.Key == "Size")
            {
                _stickyHeight = Convert.ToInt32(index.Value) + 35;
                _stickyWidth = Convert.ToInt32(index.Value);
            }
            else if (index.Key == "Opasity")
                _opacitySticky = Convert.ToDouble(index.Value);
            else if (index.Key == "ExitState")
            {
                _changeStickyState = Convert.ToBoolean(Convert.ToInt32(index.Value));
                if (!Convert.ToBoolean(Convert.ToInt32(index.Value)))
                    stateOnlySticky();
            }
        }
    }
    
    private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        Dictionary<string, string> stickyData = new Dictionary<string, string>
        {
            {"Color", fontnum.ToString()},
            {"Position", $"{Convert.ToInt32(this._windowsСoordinates.X)}; {Convert.ToInt32(this._windowsСoordinates.Y)}"},
            {"Size", _stickyWidth.ToString()},
            {"Opasity", Math.Round(_opacitySticky, 1).ToString(CultureInfo.GetCultureInfo("ru-RU"))},
            {"ExitState", Convert.ToInt32(_changeStickyState).ToString()}
        };
        WorkingFiles.updateDataSticker(_stickyName, stickyData);
            
        SaveToFile();
    }

    private void SaveToFile()
    {
        TextRange doc = new TextRange(MainRichTextBox.Document.ContentStart, MainRichTextBox.Document.ContentEnd);
        using (FileStream fs = File.Create(WorkingFiles.getPathDataSticker(_stickyName)))
            doc.Save(fs, DataFormats.Rtf);
    }

    private void LoadTextSticky()
    {
        try
        {
            TextRange doc = new TextRange(MainRichTextBox.Document.ContentStart, MainRichTextBox.Document.ContentEnd);
            using (FileStream fs = new FileStream(WorkingFiles.getPathDataSticker(_stickyName), FileMode.Open))
                doc.Load(fs, DataFormats.Rtf);
        }
        catch {}
    }

    private void StartInitiation()
    {
        InstallColor();
        SettingStickySize();
        UpdateBackgroundFontSticky();
        LoadTextSticky();
        
        this.Top = this._windowsСoordinates.X;
        this.Left = this._windowsСoordinates.Y;
        
        MainRichTextBox.Focus();
        if (stateLock) SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
        if (stateLock) LockSticky();
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
        if (!stateLock) return;
        
        stateOnlySticky();
        _Lockinformation = true;
        CheckMark.Kind = PackIconMaterialKind.Eye;
        MainRichTextBox.Opacity = 0;
    }

    private void UnlockSticky()
    {
        if (!stateLock) return;
        
        var passwordWindow = new PasswordWindow();

        if (passwordWindow.ShowDialog() == true && passwordWindow.Password == _stickyPassword)
        {
            _Lockinformation = false;
            CheckMark.Kind = PackIconMaterialKind.Pencil;
            MainRichTextBox.Opacity = 1;
        }
        else
        {
            MessageBox.Show("Введенный вами пароль не верен или\nВвод пароля был отменен", "Проверка пароля", MessageBoxButton.OK);
        }
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
        var color = (Color)ColorConverter.ConvertFromString(FontColor[fontnum, 1]);
        color.A = (byte)(_opacitySticky * 255);
            
        SomeSticky.Background = new SolidColorBrush(color);
    }

    private void stateOnlySticky()
    {
        _changeStickyState = false;
        BottonMenuSettings.Height = 0;
        ShowInTaskbar = false;
        CheckMark.Kind = PackIconMaterialKind.Pencil;
        MainRichTextBox.IsReadOnly = !_changeStickyState;
    }

    private void StateReadOnlySticky()
    {
        if (_Lockinformation) UnlockSticky();
        
        if (_changeStickyState) stateOnlySticky();
        else
        {
            _changeStickyState = true;
            BottonMenuSettings.Height = 30;
            ShowInTaskbar = true;
            CheckMark.Kind = PackIconMaterialKind.Check;
            MainRichTextBox.IsReadOnly = !_changeStickyState;
        }
    }

    private void UpdateBackgroundFontSticky()
    {
        if (fontnum >= FontColor.GetLength(0)) fontnum = 0;

        MainRichTextBox.Foreground = new SolidColorBrush(Colors.Black);
        if (fontnum == 5) MainRichTextBox.Foreground = new SolidColorBrush(Colors.White);

        BorderInform.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(FontColor[fontnum, 0]));
        installIcon();
    }
    private string RandomColorStickyName()
    {
        string[] listColor = { "#ECC300", "#7E00FC", "#E80000", "#2F96E5", "#F69F1E", "#EF0090", "#F05D63", "#0ECF4F", "#461BDA", "#EC7616" };
        
        Random random = new Random();
        int index = random.Next(listColor.Length);
        return listColor[index];
    }

    private void RichTextBox_OnPreviewKeyDown(object sender, KeyEventArgs e) // Валидация событий на текстовом поле
    {
        if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.A)
        {
            var sticky = new Sticky();
            sticky.Show();
        }
        else if (e.Key == Key.Enter && _Lockinformation)
                UnlockSticky();
        else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.Y)
            StateReadOnlySticky();
        else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.L && !_Lockinformation)
            LockSticky();
        else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.Q)
        {
            foreach(Window window in App.Current.Windows)
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
        else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.T)
        {
            _thumbtack = !_thumbtack;
            string text = "Стикер был " + (_thumbtack? "закреплен!": "откреплен!");
            MessageBox.Show(text, "StickyNotes", MessageBoxButton.OK);
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
        
        else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.OemPlus && _stickyWidth < 750)
            ModifyStickySize(50);
        else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.OemMinus && _stickyWidth > 250)
            ModifyStickySize(-50);
        
        else if (e.KeyboardDevice.Modifiers == (ModifierKeys.Control | ModifierKeys.Shift) && e.Key == Key.S)
        {
            if (this._stickyName == baseStickyName)
            {
                Random rnd = new Random();
                this._stickyName = $"{baseStickyName}_{rnd.Next(11111, 99999)}";
                MainStickyName.Text = this._stickyName;
                this.Title = this._stickyName;

                Dictionary<string, string> stickyData = new Dictionary<string, string>()
                {
                    {"Password", "None"},
                    {"Color", fontnum.ToString()},
                    {"Position", $"{Convert.ToInt32(this._windowsСoordinates.X)}; {Convert.ToInt32(this._windowsСoordinates.Y)}"},
                    {"Size", _stickyWidth.ToString()},
                    {"Opasity", Math.Round(_opacitySticky, 1).ToString(CultureInfo.GetCultureInfo("ru-RU"))},
                    {"ExitState", Convert.ToInt32(_changeStickyState).ToString()}
                };
                string date = $"{DateTime.Now.Year:D2}-{DateTime.Now.Month:D2}-{DateTime.Now.Day:D2}";
                Dictionary<string, string> settingsData = new Dictionary<string, string>()
                {
                    {"Color", RandomColorStickyName()},
                    {"Date", date},
                    {"Favorite", "0"},
                    {"Thumbtack", "0"},
            
                };
                                
                WorkingFiles.createSticker(_stickyName, stickyData, settingsData);
                                
                this.Closing += MainWindow_Closing;
            }
            SaveToFile();
        }
    }

    private void ModifyStickySize(int windowsSize)
    {
        _stickyWidth += windowsSize;
        _stickyHeight += windowsSize;
        SettingStickySize();
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

    private void Sticky_OnDeactivated(object? sender, EventArgs e)
    {
        BorderInform.Height = 10;
        MainStickyName.Visibility = Visibility.Hidden;
        
        if (_changeStickyState)
            BottonMenuSettings.Height = 0;
    }

    private void Sticky_OnActivated(object? sender, EventArgs e)
    {
        BorderInform.Height = 35;
        MainStickyName.Visibility = Visibility.Visible;

        if (_changeStickyState) BottonMenuSettings.Height = 30;
    }

    private void MainRichTextBox_OnSelectionChanged(object sender, RoutedEventArgs e)
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
}