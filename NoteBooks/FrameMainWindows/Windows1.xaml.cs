using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace NoteBooks.FrameMainWindows;

public partial class Windows1 : Page
{
    public Windows1()
    {
        InitializeComponent();
    }
    
    private MainWindow _window;
    private int SelectMenuFrame = 1;
    private bool CheckOnDelete = WorkingFiles.checkDeleteSticker();
    
    public Windows1(MainWindow window)
    {
        InitializeComponent();
        _window = window;

        CreateStickyFrame();
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
        
        foreach (KeyValuePair<string,List<string>> data in  WorkingFiles.getStickyDataProgram())
        {
            sticky.Add(new Sticky
            {
                Character = data.Key.Substring(0, 1), 
                Number = num.ToString(), 
                Name = data.Key, 
                StartDate = data.Value[1], 
                CurrentDate = data.Value[2],
                BgColor = (Brush)converter.ConvertFromString(data.Value[0]),
                IconF = (Convert.ToBoolean(Convert.ToInt32(data.Value[3])))? "Star": "StarOutline",
                IconT = (Convert.ToBoolean(Convert.ToInt32(data.Value[4])))? "Pin": "PinOutline"
            });
            NameSticky.Add(num-1, data.Key);
            num++;
        }
        
        stickyDataGrid.ItemsSource = sticky;
        TitleNumSticky.Text = $"Стикеров: {num-1}";
    }

    private void CreateThumbtackSticky()
    {
        NameSticky.Clear();
        sticky.Clear();
        int num = 1;

        foreach (KeyValuePair<string,List<string>> data in WorkingFiles.getDataThumbtackSticky())
        {
            sticky.Add(new Sticky
            {
                Character = data.Key.Substring(0, 1), 
                Number = num.ToString(), 
                Name = data.Key, 
                StartDate = data.Value[1], 
                CurrentDate = data.Value[2],
                BgColor = (Brush)converter.ConvertFromString(data.Value[0]),
                IconF = (Convert.ToBoolean(Convert.ToInt32(data.Value[3])))? "Star": "StarOutline",
                IconT = "Pin"
            });
            NameSticky.Add(num-1, data.Key);
            num++;
        }
        
        stickyDataGrid.ItemsSource = sticky;
        TitleNumSticky.Text = $"Закрепленных стикеров: {num-1}";
    }

    private void CreateFavoriteSticky()
    {
        sticky.Clear();
        NameSticky.Clear();
        int num = 1;

        foreach (KeyValuePair<string,List<string>> data in WorkingFiles.getDataFavoriteSticky())
        {
            sticky.Add(new Sticky
            {
                Character = data.Key.Substring(0, 1), 
                Number = num.ToString(), 
                Name = data.Key, 
                StartDate = data.Value[1], 
                CurrentDate = data.Value[2],
                BgColor = (Brush)converter.ConvertFromString(data.Value[0]),
                IconF = "Star",
                IconT = (Convert.ToBoolean(Convert.ToInt32(data.Value[4])))? "Pin": "PinOutline"
            });
            
            NameSticky.Add(num-1, data.Key);
            num++;
        }
        
        stickyDataGrid.ItemsSource = sticky;
        TitleNumSticky.Text = $"Избранных стикеров: {num-1}";
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
        WorkingFiles.updateFavoriteStatus(NameSticky[stickyDataGrid.SelectedIndex]);
        openCurrentMenu();
    }
    private void Change_ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        WorkingFiles.updateThumbtackStatus(NameSticky[stickyDataGrid.SelectedIndex]);
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
        WorkingFiles.deleteSticker(name);
        openCurrentMenu();
    }

    private int CheckOpenMenu = 1;
    private void StickyButton_OnClick(object sender, RoutedEventArgs e)
    {
        CreateStickyFrame();
        SelectMenuFrame = 1;
    }

    private void ThumbtackButton_OnClick(object sender, RoutedEventArgs e)
    {
        SelectMenuFrame = 2;
        CreateThumbtackSticky();
    }

    private void FavoritesButton_OnClick(object sender, RoutedEventArgs e)
    {
        SelectMenuFrame = 3;
        CreateFavoriteSticky();
    }
    
    private void checkBox_Checked(object sender, RoutedEventArgs e)
    {
        // MessageBox.Show("Отмечен");
    }
 
    private void checkBox_Unchecked(object sender, RoutedEventArgs e)
    {
        // MessageBox.Show("не отмечен");
    }

    private void StickyDataGrid_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        try
        {
            string name = NameSticky[stickyDataGrid.SelectedIndex];
            if (!WorkingFiles.checkIsOpenFile(name)) return;
            try
            {
                NoteBooks.Sticky sticky = new NoteBooks.Sticky(name);
                sticky.Show();
            }
            catch {}
        }
        catch {}
    }

    private void TxtSearch_OnKeyUp(object sender, KeyEventArgs e)
    {
        if (txtSearch.Text == "") openCurrentMenu();
        else
        {
            Dictionary<string, List<string>> data = WorkingFiles.searchSticky(txtSearch.Text);
            CreateSearchSticky(data);
        }
    }
    
    private void CreateSearchSticky(Dictionary<string, List<string>> dataSticker)
    {
        sticky.Clear();
        NameSticky.Clear();
        int num = 1;

        foreach (KeyValuePair<string,List<string>> data in dataSticker)
        {
            if (SelectMenuFrame == 1)
            {
                sticky.Add(new Sticky
                {
                    Character = data.Key.Substring(0, 1), 
                    Number = num.ToString(), 
                    Name = data.Key, 
                    StartDate = data.Value[1], 
                    CurrentDate = data.Value[2],
                    BgColor = (Brush)converter.ConvertFromString(data.Value[0]),
                    IconF = (Convert.ToBoolean(Convert.ToInt32(data.Value[3])))? "Star": "StarOutline",
                    IconT = (Convert.ToBoolean(Convert.ToInt32(data.Value[4])))? "Pin": "PinOutline"
                });
                
                NameSticky.Add(num-1, data.Key);
                num++;
            }
            else if (SelectMenuFrame == 2)
            {
                if (int.Parse(data.Value[4]) == 1)
                {
                    sticky.Add(new Sticky
                    {
                        Character = data.Key.Substring(0, 1), 
                        Number = num.ToString(), 
                        Name = data.Key, 
                        StartDate = data.Value[1], 
                        CurrentDate = data.Value[2],
                        BgColor = (Brush)converter.ConvertFromString(data.Value[0]),
                        IconF = (Convert.ToBoolean(Convert.ToInt32(data.Value[3])))? "Star": "StarOutline",
                        IconT = (Convert.ToBoolean(Convert.ToInt32(data.Value[4])))? "Pin": "PinOutline"
                    });
                
                    NameSticky.Add(num-1, data.Key);
                    num++;
                }
            }
            else if (SelectMenuFrame == 3)
            {
                if (int.Parse(data.Value[3]) == 1)
                {
                    sticky.Add(new Sticky
                    {
                        Character = data.Key.Substring(0, 1), 
                        Number = num.ToString(), 
                        Name = data.Key, 
                        StartDate = data.Value[1], 
                        CurrentDate = data.Value[2],
                        BgColor = (Brush)converter.ConvertFromString(data.Value[0]),
                        IconF = (Convert.ToBoolean(Convert.ToInt32(data.Value[3])))? "Star": "StarOutline",
                        IconT = (Convert.ToBoolean(Convert.ToInt32(data.Value[4])))? "Pin": "PinOutline"
                    });
                
                    NameSticky.Add(num-1, data.Key);
                    num++;
                }
            }
        }
        
        stickyDataGrid.ItemsSource = sticky;
        TitleNumSticky.Text = $"Найдено стикеров: {num-1}";
    }
}