using System;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace StickyNotes
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            
            OpenMainMenu(MainMenu);
            installColorTheme();
            
            this.Icon = new BitmapImage(new Uri("pack://application:,,,/Resource/ImageTaskBar/apps.png"));
        }

        private void installColorTheme()
        {
            FileSettings.themeChange(FileSettings.themeStyle);
            
            var t = new Thread(() => autoUpdateTheme());
            t.IsBackground = true;
            t.Start();
        }

        public void autoUpdateTheme()
        {
            while (FileSettings.themeStyle == themeNameStyle.system)
            {
                FileSettings.setNewColorTheme(themeNameStyle.system);
                Thread.Sleep(500);
            }
            FileSettings.setNewColorTheme(FileSettings.themeStyle);
        }
        

        private bool IsMaximized ;
        private void Border_MouseDows(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void WindowsMaksimum()
        {
            WindowState = WindowState.Maximized;
            IsMaximized = true;
        }

        private void WindowsBase()
        {
            WindowState = WindowState.Normal;
            IsMaximized = false;
        }

        private void Border_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount != 2) return;
            
            if (IsMaximized)
                WindowsBase();
            else
                WindowsMaksimum();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        
        public void OpenMainMenu(object sender)
        {
            createAnimationOrPressed(sender);
            OpenMainMenu();
        }
        public void OpenCreateMenu(object sender)
        {
            createAnimationOrPressed(sender);
            OpenCreateMenu();
        }
        public void OpenInformMenu(object sender)
        {
            createAnimationOrPressed(sender);
            OpenInformMenu();
        }
        public void OpenSettingsMenu(object sender)
        {
            createAnimationOrPressed(sender);
            OpenSettingsMenu();
        }
        
        public void OpenMainMenu()
        {
            MainFrame.Content = new FrameMainWindows.WindowsMainMenu(this);
        }
        public void OpenCreateMenu()
        {
            MainFrame.Content = new FrameMainWindows.WindowsNewSticky(this);
        }
        public void OpenInformMenu()
        {
            // MainFrame.Content = new FrameMainWindows.WindowsNewSticky(this);
        }
        public void OpenSettingsMenu()
        {
            MainFrame.Content = new FrameMainWindows.WindowsSetting(this);
        }

        private void createAnimationOrPressed(object sender)
        {
            Button btn = (Button)sender;
            btn.Background = (Brush)converter.ConvertFromString("#7b5cd6");
            btn.Foreground = new SolidColorBrush(Colors.White);

            try
            {
                if (_currButton[0].Name != btn.Name)
                {
                    _currButton[0].Background = new SolidColorBrush(Colors.Transparent);
                    _currButton[0].Foreground = (Brush)converter.ConvertFromString("#d0c0ff");
                    _currButton[0] = btn;
                }
            }
            catch
            {
                _currButton.Add(btn);
            }
        }

        private List<Button> _currButton = new List<Button>(1);
        private void HomeButtonBown(object sender, RoutedEventArgs e)
        {
            OpenMainMenu(sender);
        }
        
        private void CreateButtonBown(object sender, RoutedEventArgs e)
        {
            OpenCreateMenu(sender);
        }
        
        private void InformButtonBown(object sender, RoutedEventArgs e)
        {
            OpenInformMenu(sender);
        }
        
        private void SettingsButtonBown(object sender, RoutedEventArgs e)
        {
            OpenSettingsMenu(sender);
        }
        
        private void WindowsSize_OnClick(object sender, RoutedEventArgs e)
        {
            if (IsMaximized) WindowsBase();
            else WindowsMaksimum();
        }

        private void WindowsMinimizide_OnClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        
        private BrushConverter converter = new BrushConverter();
        private List<Button> _buttons = new List<Button>();
        
        private void UIElement_OnMouseEnter(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            btn.Foreground = new SolidColorBrush(Colors.White);
            
            try
            {
                _buttons[0] = btn;
            }
            catch (ArgumentOutOfRangeException)
            {
                _buttons.Add(btn);
            }
        }

        private void UIElement_OnMouseLeave(object sender, MouseEventArgs e)
        {
            if (_currButton[0].Name != _buttons[0].Name)
                _buttons[0].Foreground = (Brush)converter.ConvertFromString("#d0c0ff");
        }
    }
}