using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace NoteBooks
{
    public partial class MainWindow: Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            OpenMainMenu();
            OpenThumbrackSticky();
            
            try
            {
                string executablePath = System.Reflection.Assembly.GetEntryAssembly().Location;
                string executableDirectory = System.IO.Path.GetDirectoryName(executablePath);
                string imagePath = $"{executableDirectory}/Icons/apps.png";
                this.Icon = new BitmapImage(new Uri(imagePath));
            }
            catch {}
            
            GC.Collect(5);
        }
        
        private void OpenThumbrackSticky()
        {
            List<string> data = WorkingFiles.getThumbtackSticky();
            foreach (string index in data)
            {
                if (WorkingFiles.checkIsOpenFile(index))
                {
                    Sticky sticky = new Sticky(index);
                    sticky.Show();
                }
            }
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
        
        public void OpenMainMenu()
        {
            MainFrame.Content = new FrameMainWindows.Windows1(this);
        }
        public void OpenCreateMenu()
        {
            MainFrame.Content = new FrameMainWindows.WindowsNewSticky(this);
        }
        public void OpenStatsMenu()
        {
            MainFrame.Content = new FrameMainWindows.WindowsStats();
        }
        public void OpenSettingsMenu()
        {
            MainFrame.Content = new FrameMainWindows.WindowsSetting(this);
        }

        private void HomeButtonBown(object sender, RoutedEventArgs e)
        {
            OpenMainMenu();
        }
        
        private void CreateButtonBown(object sender, RoutedEventArgs e)
        {
            OpenCreateMenu();
        }
        
        private void StatsButtonBown(object sender, RoutedEventArgs e)
        {
            OpenStatsMenu();
        }
        
        private void SettingsButtonBown(object sender, RoutedEventArgs e)
        {
            OpenSettingsMenu();
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

        private void ButtonBase_OnClick_1(object sender, RoutedEventArgs e)
        {
            GC.Collect(5);
        }
    }
}
