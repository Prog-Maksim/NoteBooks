using System.Windows;
using System.Windows.Input;
using StickyNotes.FrameMainWindows.StartUpMenu.Frames;

namespace StickyNotes.FrameMainWindows.StartUpMenu;

public partial class StartMenu : Window
{
    public StartMenu()
    {
        InitializeComponent();

        MainFrame.Content = new Page1(this);
    }
    
    private void WindowsMinimizide_OnClick(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }
    
    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
    
    private void Border_MouseDows(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            DragMove();
    }

    private int currentMenuOpen = 1;
    
    public void slideNext()
    {
        if (currentMenuOpen < 8)
            currentMenuOpen++;
        
        openPage(currentMenuOpen);
    }

    public void slideBack()
    {
        if (currentMenuOpen > 0)
            currentMenuOpen--;
        
        openPage(currentMenuOpen);
    }
    
    private void openPage(int page)
    {
        ProgressBar.Value = page;

        if (page == 1)
        {
            ProgressBar.Value = 0;
            MainFrame.Content = new Page1(this);
        }
        else if (page == 2)
            MainFrame.Content = new Page2(this);
        else if (page == 3)
            MainFrame.Content = new Page3(this);
        else if (page == 4)
            MainFrame.Content = new Page4(this);
        else if (page == 5)
            MainFrame.Content = new Page5(this);
        else if (page == 6)
            MainFrame.Content = new Page6(this);
        else if (page == 7)
            MainFrame.Content = new Page7(this);
        else if (page == 8)
            MainFrame.Content = new Page8(this);
    }

    public void openMainMenu()
    {
        MainWindow window = new MainWindow();
        window.Show();
        
        this.Close();
    }
}