using System.Windows;

namespace StickyNotes.OtherWindowsProgram;

public partial class PasswordWindow
{
    public PasswordWindow()
    {
        InitializeComponent();
        passwordBox.Focus();
    }
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
    }
    
    public string Password
    {
        get { return passwordBox.Password; }
    }
}