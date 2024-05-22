using System.Windows;
using System.Windows.Controls;

namespace StickyNotes.FrameMainWindows.StartUpMenu.Frames;

public partial class Page8 : Page
{
    private StartMenu menu;
    public Page8(StartMenu menu)
    {
        InitializeComponent();
        this.menu = menu;
    }
    
    private void NextButton_OnClick(object sender, RoutedEventArgs e)
    {
        this.menu.openMainMenu();
    }
    
    private void BackButton_OnClick(object sender, RoutedEventArgs e)
    {
        menu.slideBack();
    }
}