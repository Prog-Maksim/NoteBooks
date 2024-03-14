using System.Windows;

namespace NoteBooks;

public partial class PasswordWindow : Window
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