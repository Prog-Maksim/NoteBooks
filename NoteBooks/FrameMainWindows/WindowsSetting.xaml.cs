using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace NoteBooks.FrameMainWindows;

public partial class WindowsSetting : Page
{
    private MainWindow app;
    private string CurrentTheme;
    
    public WindowsSetting(MainWindow app)
    {
        InitializeComponent();

        this.app = app;
        InitializeWindows();
    }

    private void InitializeWindows()
    {
        if (IsRunAsAdmin()) ButtonIsAdministrator.Visibility = Visibility.Hidden;
        else CheckBoxAutoStart.IsEnabled = false;

        if (checkIsAutoStartProgram())
        {
            CheckBoxAutoStart.IsChecked = true;
            TextAutoStart.Text = "Включено";
        }

        DeleteSticker.IsChecked = true;
    }

    private void IsAdministrator()
    {
        try
        {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.UseShellExecute = true;
            proc.WorkingDirectory = Environment.CurrentDirectory;
            proc.FileName = Path.GetFullPath(Process.GetCurrentProcess().MainModule.FileName);
            proc.Verb = "runas";
            Process.Start(proc);
            this.app.Close();
        }
        catch { }
    }


    static internal bool IsRunAsAdmin()
    {
        WindowsIdentity id = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(id);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
    
    private string PathToFileProject()
    {
        string path = Path.GetFullPath(Process.GetCurrentProcess().MainModule.FileName);
        
        Process p = new Process();
        p.StartInfo.FileName = path.Replace("NoteBooks.exe", "main.exe");
        p.Start();
        
        string newPath = path.Replace("NoteBooks.exe", "StickyNotes.lnk");
        return newPath;
    }
    
    private bool checkIsAutoStartProgram()
    {
        using (RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", false))
        {
            if (key.GetValue("NoteBook") != null)
                return true;
            return false;
        }
    }
    
    private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
    {
        if (!checkIsAutoStartProgram())
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            key.SetValue("NoteBook", PathToFileProject());
            key.Close();
            TextAutoStart.Text = "Включено";
        }
    }
    private void ToggleButton_OnUnchecked(object sender, RoutedEventArgs e)
    {
        if (checkIsAutoStartProgram())
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            key.DeleteValue("NoteBook");
            key.Close();
            TextAutoStart.Text = "Выключено";
        }
    }

    private void DeleteSticker_OnChecked(object sender, RoutedEventArgs e)
    {
        try
        {
            TextDeleteSticker.Text = "Запрашивать";
        }
        catch (Exception exception) {}
    }

    private void DeleteSticker_OnUnchecked(object sender, RoutedEventArgs e)
    {
        TextDeleteSticker.Text = "Не запрашивать";
    }

    private void ButtonIsAdministrator_OnClick(object sender, RoutedEventArgs e)
    {
        IsAdministrator();
    }
    

    private void Save_OnClick(object sender, RoutedEventArgs e)
    {
        // Сохранение обновленных настроек
        clearinformMessage();
    }

    private async Task clearinformMessage()
    {
        MessageText.Visibility = Visibility.Visible;

        await Task.Delay(1500);
        MessageText.Visibility = Visibility.Hidden;
    }
    
    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
        if (Convert.ToBoolean(LightRadioButton.IsChecked))
            CurrentTheme = "Light";
        else if (Convert.ToBoolean(DarkRadioButton.IsChecked))
            CurrentTheme = "Dark";
        else if (Convert.ToBoolean(SystemRadioButton.IsChecked))
            CurrentTheme = "System";
    }
}