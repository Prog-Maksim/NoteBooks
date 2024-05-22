using System.Windows;
using System.Windows.Controls;

namespace StickyNotes.FrameMainWindows.StartUpMenu.Frames;

public partial class Page3 : Page
{
    private StartMenu menu;
    public Page3(StartMenu menu)
    {
        InitializeComponent();
        this.menu = menu;
    }
    
    private void NextButton_OnClick(object sender, RoutedEventArgs e)
    {
        menu.slideNext();
    }
    
    private void BackButton_OnClick(object sender, RoutedEventArgs e)
    {
        menu.slideBack();
    }
}