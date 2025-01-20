using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using StickyNotes.Scripts;

namespace StickyNotes.FrameMainWindows;

public partial class WindowsMainMenu : Page
{
    public WindowsMainMenu()
    {
        InitializeComponent();
    }
    
    private MainWindow _window;
    private int SelectMenuFrame = 1;
    private bool CheckOnDelete = FileSettings.deleteNotification;
    
    public WindowsMainMenu(MainWindow window)
    {
        InitializeComponent();
        _window = window;

        CreateStickyFrame();
        createAnimationOrPressed(StickyButton);
    }
    
    public class Sticky
    {
        public string Character { get; set; }
        public string Number { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string CurrentDate { get; set; }
        public Brush BgColor { get; set; }
        public string IconF { get; set; }
        public string IconT { get; set; }
    }

    private BrushConverter converter = new BrushConverter();
    private Dictionary<int, string> NameSticky = new Dictionary<int, string>();
    private ObservableCollection<Sticky> sticky = new ObservableCollection<Sticky>();

    private void CreateStickyFrame()
    {
        NameSticky.Clear();
        sticky.Clear();
        int num = 1;
        try
        {
            foreach (var sticker in Sticker.getAllDataSticker().Stickers)
            {
                sticky.Add(new Sticky
                {
                    Character = sticker.Name.Substring(0, 1),
                    Number = num.ToString(),
                    Name = sticker.Name,
                    StartDate = $"{sticker.DateStart.Day:D2}.{sticker.DateStart.Month:D2}.{sticker.DateStart.Year:D2}",
                    CurrentDate = $"{sticker.CurrentDateUpdate.Day:D2}.{sticker.CurrentDateUpdate.Month:D2}.{sticker.CurrentDateUpdate.Year:D2}",
                    BgColor = (Brush)converter.ConvertFromString(ColorSticky.color((ColorSticky.stickyColor)sticker.Color)[3]),
                    IconF = sticker.StickerFavorite ? "Star" : "StarOutline",
                    IconT = sticker.StickerThumbtack ? "Pin" : "PinOutline"
                });
                NameSticky.Add(num - 1, sticker.Name);
                num++;
            }
        }
        catch (NullReferenceException) { }

        stickyDataGrid.ItemsSource = sticky;
        TitleNumSticky.Text = $"Стикеров: {num - 1}";
    }

    private void CreateThumbtackSticky()
    {
        NameSticky.Clear();
        sticky.Clear();
        int num = 1;

        foreach (var sticker in Sticker.getAllDataSticker().Stickers)
        {
            if (sticker.StickerThumbtack)
            {
                sticky.Add(new Sticky
                {
                    Character = sticker.Name.Substring(0, 1),
                    Number = num.ToString(),
                    Name = sticker.Name,
                    StartDate = $"{sticker.DateStart.Day:D2}.{sticker.DateStart.Month:D2}.{sticker.DateStart.Year:D2}",
                    CurrentDate = $"{sticker.CurrentDateUpdate.Day:D2}.{sticker.CurrentDateUpdate.Month:D2}.{sticker.CurrentDateUpdate.Year:D2}",
                    BgColor = (Brush)converter.ConvertFromString(ColorSticky.color((ColorSticky.stickyColor)sticker.Color)[3]),
                    IconF = sticker.StickerFavorite ? "Star" : "StarOutline",
                    IconT = sticker.StickerThumbtack ? "Pin" : "PinOutline"
                });
                NameSticky.Add(num - 1, sticker.Name);
                num++;
            }
        }

        stickyDataGrid.ItemsSource = sticky;
        TitleNumSticky.Text = $"Стикеров: {num - 1}";
    }

    private void CreateFavoriteSticky()
    {
        NameSticky.Clear();
        sticky.Clear();
        int num = 1;

        foreach (var sticker in Sticker.getAllDataSticker().Stickers)
        {
            if (sticker.StickerFavorite)
            {
                sticky.Add(new Sticky
                {
                    Character = sticker.Name.Substring(0, 1),
                    Number = num.ToString(),
                    Name = sticker.Name,
                    StartDate = $"{sticker.DateStart.Day:D2}.{sticker.DateStart.Month:D2}.{sticker.DateStart.Year:D2}",
                    CurrentDate = $"{sticker.CurrentDateUpdate.Day:D2}.{sticker.CurrentDateUpdate.Month:D2}.{sticker.CurrentDateUpdate.Year:D2}",
                    BgColor = (Brush)converter.ConvertFromString(ColorSticky.color((ColorSticky.stickyColor)sticker.Color)[3]),
                    IconF = sticker.StickerFavorite ? "Star" : "StarOutline",
                    IconT = sticker.StickerThumbtack ? "Pin" : "PinOutline"
                });
                NameSticky.Add(num - 1, sticker.Name);
                num++;
            }
        }

        stickyDataGrid.ItemsSource = sticky;
        TitleNumSticky.Text = $"Стикеров: {num - 1}";
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        this._window.OpenCreateMenu();
    }

    private void SettingsMenuButton_OnClick(object sender, RoutedEventArgs e)
    {
        this._window.OpenSettingsMenu();
    }

    private void openCurrentMenu()
    {
        if (SelectMenuFrame == 1) CreateStickyFrame();
        else if (SelectMenuFrame == 2) CreateThumbtackSticky();
        else if (SelectMenuFrame == 3) CreateFavoriteSticky();
    }
    
    private void Star_ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        string nameSticker = NameSticky[stickyDataGrid.SelectedIndex];
        Sticker.updateStateStickerFavorite(nameSticker);
        openCurrentMenu();
    }
    private void Thumbtack_ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        string nameSticker = NameSticky[stickyDataGrid.SelectedIndex];
        Sticker.updateStateStickerThumbtack(nameSticker);
        openCurrentMenu();
    }
    
    private void Delete_ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (CheckOnDelete)
        {
            var result =  MessageBox.Show("Вы уверены что хотите \nбезвозвратно удалить данный стикер?", "Предупреждение",
                MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                deleteSticker(NameSticky[stickyDataGrid.SelectedIndex]);
        }
        else
            deleteSticker(NameSticky[stickyDataGrid.SelectedIndex]);
    }
    
    private void deleteSticker(string name)
    {
        
        Sticker.deleteSticker(name);
        openCurrentMenu();
    }

    private int CheckOpenMenu = 1;
    private List<Button> _currButton = new List<Button>();
    
    private void createAnimationOrPressed(object sender)
    {
        Button btn = (Button)sender;
        btn.BorderBrush = (Brush)converter.ConvertFromString("#784ff1");

        try
        {
            if (_currButton[0].Name != btn.Name)
            {
                _currButton[0].BorderBrush = new SolidColorBrush(Colors.Transparent);
                _currButton[0] = btn;
            }
        }
        catch
        {
            _currButton.Add(btn);
        }
    }
    
    private void StickyButton_OnClick(object sender, RoutedEventArgs e)
    {
        CreateStickyFrame();
        SelectMenuFrame = 1;
        createAnimationOrPressed(sender);
    }

    private void ThumbtackButton_OnClick(object sender, RoutedEventArgs e)
    {
        SelectMenuFrame = 2;
        CreateThumbtackSticky();
        createAnimationOrPressed(sender);
    }

    private void FavoritesButton_OnClick(object sender, RoutedEventArgs e)
    {
        SelectMenuFrame = 3;
        CreateFavoriteSticky();
        createAnimationOrPressed(sender);
    }

    private void StickyDataGrid_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if(e.ClickCount != 2) return;

        if (stickyDataGrid.SelectedIndex == -1)
            stickyDataGrid.SelectedIndex = 0;
        
        string name = NameSticky[stickyDataGrid.SelectedIndex];
        if (Directory.Exists(Path.Combine(ClassRegistry.PathOpenStickers, $"~{name}"))) return;
        try
        {
            OtherWindowsProgram.Sticky sticky = new OtherWindowsProgram.Sticky(name);
            sticky.Show();
        }
        catch(Exception ex)
        {
            MessageBox.Show(ex.Message, "StickyNotes", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void TxtSearch_OnKeyUp(object sender, KeyEventArgs e)
    {
        if (txtSearch.Text == "") openCurrentMenu();
        else
        {
            List<Models.Sticker> stickers = new List<Models.Sticker>();
            foreach (var sticker in Sticker.getAllDataSticker().Stickers)
            {
                if (sticker.Name.Contains(txtSearch.Text))
                {
                    stickers.Add(sticker);
                }
            }
            
            CreateSearchSticky(stickers);
        }
    }
    
    private void CreateSearchSticky(List<Models.Sticker> dataSticker)
    {
        sticky.Clear();
        NameSticky.Clear();
        int num = 1;

        foreach (var sticker in dataSticker)
        {
            if (SelectMenuFrame == 1)
            {
                sticky.Add(new Sticky
                {
                    Character = sticker.Name.Substring(0, 1),
                    Number = num.ToString(),
                    Name = sticker.Name,
                    StartDate = $"{sticker.DateStart.Day:D2}.{sticker.DateStart.Month:D2}.{sticker.DateStart.Year:D2}",
                    CurrentDate = $"{sticker.CurrentDateUpdate.Day:D2}.{sticker.CurrentDateUpdate.Month:D2}.{sticker.CurrentDateUpdate.Year:D2}",
                    BgColor = (Brush)converter.ConvertFromString(ColorSticky.color((ColorSticky.stickyColor)sticker.Color)[3]),
                    IconF = sticker.StickerFavorite ? "Star" : "StarOutline",
                    IconT = sticker.StickerThumbtack ? "Pin" : "PinOutline"
                });
                NameSticky.Add(num - 1, sticker.Name);
                num++;
            }
            else if (SelectMenuFrame == 2 && sticker.StickerThumbtack)
            {
                sticky.Add(new Sticky
                {
                    Character = sticker.Name.Substring(0, 1),
                    Number = num.ToString(),
                    Name = sticker.Name,
                    StartDate = $"{sticker.DateStart.Day:D2}.{sticker.DateStart.Month:D2}.{sticker.DateStart.Year:D2}",
                    CurrentDate = $"{sticker.CurrentDateUpdate.Day:D2}.{sticker.CurrentDateUpdate.Month:D2}.{sticker.CurrentDateUpdate.Year:D2}",
                    BgColor = (Brush)converter.ConvertFromString(ColorSticky.color((ColorSticky.stickyColor)sticker.Color)[3]),
                    IconF = sticker.StickerFavorite ? "Star" : "StarOutline",
                    IconT = "Pin"
                });
                NameSticky.Add(num - 1, sticker.Name);
                num++;
            }
            else if (SelectMenuFrame == 3 && sticker.StickerFavorite)
            {
                sticky.Add(new Sticky
                {
                    Character = sticker.Name.Substring(0, 1),
                    Number = num.ToString(),
                    Name = sticker.Name,
                    StartDate = $"{sticker.DateStart.Day:D2}.{sticker.DateStart.Month:D2}.{sticker.DateStart.Year:D2}",
                    CurrentDate = $"{sticker.CurrentDateUpdate.Day:D2}.{sticker.CurrentDateUpdate.Month:D2}.{sticker.CurrentDateUpdate.Year:D2}",
                    BgColor = (Brush)converter.ConvertFromString(ColorSticky.color((ColorSticky.stickyColor)sticker.Color)[3]),
                    IconF = "Star",
                    IconT = sticker.StickerThumbtack ? "Pin" : "PinOutline"
                });
                NameSticky.Add(num - 1, sticker.Name);
                num++;
            }
        }
        
        stickyDataGrid.ItemsSource = sticky;
        TitleNumSticky.Text = $"Найдено стикеров: {num-1}";
    }
    
    private List<Button> _buttons = new List<Button>();
    
    private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
    {
        Button btn = (Button)sender;
        btn.BorderBrush = (Brush)converter.ConvertFromString("#784ff1");
            
        try
        {
            _buttons[0] = btn;
        }
        catch
        {
            _buttons.Add(btn);
        }
    }

    private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
    {
        if (_currButton[0].Name != _buttons[0].Name)
            _buttons[0].BorderBrush = new SolidColorBrush(Colors.Transparent);
    }
}