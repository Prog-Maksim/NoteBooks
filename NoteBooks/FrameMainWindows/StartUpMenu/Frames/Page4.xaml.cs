using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StickyNotes.FrameMainWindows.StartUpMenu.Frames;

public partial class Page4 : Page
{
    private StartMenu menu;
    public Page4(StartMenu menu)
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

    private void Ellipse_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        Ellipse obj = (Ellipse)sender;

        if (obj.Name == "purple")
            ImageObj.Source = new BitmapImage(new Uri("/Resource/SturtUpMenuImage/ColorSticky/PurpleSticky.png", UriKind.Relative));
        else if (obj.Name == "pink")
            ImageObj.Source = new BitmapImage(new Uri("/Resource/SturtUpMenuImage/ColorSticky/PinkSticky.png", UriKind.Relative));
        else if (obj.Name == "green")
            ImageObj.Source = new BitmapImage(new Uri("/Resource/SturtUpMenuImage/ColorSticky/GreenSticky.png", UriKind.Relative));
        else if (obj.Name == "blue")
            ImageObj.Source = new BitmapImage(new Uri("/Resource/SturtUpMenuImage/ColorSticky/BlueSticky.png", UriKind.Relative));
        else if (obj.Name == "yellow")
            ImageObj.Source = new BitmapImage(new Uri("/Resource/SturtUpMenuImage/ColorSticky/YellowSticky.png", UriKind.Relative));
        else if (obj.Name == "black")
            ImageObj.Source = new BitmapImage(new Uri("/Resource/SturtUpMenuImage/ColorSticky/BlackSticky.png", UriKind.Relative));
        else if (obj.Name == "gray")
            ImageObj.Source = new BitmapImage(new Uri("/Resource/SturtUpMenuImage/ColorSticky/GraySticky.png", UriKind.Relative));
    }
}