using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace StickyNotes.FrameMainWindows;

public partial class WindowsSetting : Page
{
    private MainWindow app;
    private themeNameStyle CurrentTheme;
    
    public WindowsSetting(MainWindow app)
    {
        InitializeComponent();
        InitializeProgram();

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
    }

    private void InitializeProgram()
    {
        # region theme
        
        themeNameStyle themeStyle = FileSettings.themeStyle;
        if (themeStyle == themeNameStyle.light)
            LightRadioButton.IsChecked = true;
        else if (themeStyle == themeNameStyle.dark)
            DarkRadioButton.IsChecked = true;
        else if (themeStyle == themeNameStyle.system)
            SystemRadioButton.IsChecked = true;
        
        # endregion
        
        # region autostart

        CheckBoxAutoStart.IsChecked = FileSettings.autoStart;
        
        # endregion
        
        # region deleteNotification

        DeleteSticker.IsChecked = FileSettings.deleteNotification;

        # endregion
        
        # region stickySettings

        CloseMenuCheckBox.IsChecked = FileSettings.closeMenu;
        AnimationCheckBox.IsChecked = FileSettings.closeMenuAnimation;
        Slider1.Value = FileSettings.delayTopMenu;
        Slider2.Value = FileSettings.delayBottonMenu;

        Slider1.ValueChanged += Slider1_OnValueChanged;
        Slider2.ValueChanged += Slider2_OnValueChanged;

        # endregion

        LightRadioButton.Click += RadioButton_Checked;
        DarkRadioButton.Click += RadioButton_Checked;
        SystemRadioButton.Click += RadioButton_Checked;
    }
    
    static internal bool IsRunAsAdmin()
    {
        WindowsIdentity id = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(id);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
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
            key.SetValue("NoteBook", Path.GetFullPath(Process.GetCurrentProcess().MainModule.FileName));
            key.Close();
            TextAutoStart.Text = "Включено";
            FileSettings.autoStart = true;
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

        FileSettings.autoStart = false;
    }

    private void DeleteSticker_OnChecked(object sender, RoutedEventArgs e)
    {
        try
        {
            TextDeleteSticker.Text = "Запрашивать";
            FileSettings.deleteNotification = true;
        }
        catch (Exception exception) {}
    }

    private void DeleteSticker_OnUnchecked(object sender, RoutedEventArgs e)
    {
        TextDeleteSticker.Text = "Не запрашивать";
        FileSettings.deleteNotification = false;
    }

    private void ButtonIsAdministrator_OnClick(object sender, RoutedEventArgs e)
    {
        ClassRegistry.IsAdministrator(this.app);
    }
    
    private void RadioButton_Checked(object sender, RoutedEventArgs e)
    {
        if (Convert.ToBoolean(LightRadioButton.IsChecked))
            CurrentTheme = themeNameStyle.light;
        else if (Convert.ToBoolean(DarkRadioButton.IsChecked))
            CurrentTheme = themeNameStyle.dark;
        else if (Convert.ToBoolean(SystemRadioButton.IsChecked))
            CurrentTheme = themeNameStyle.system;
        
        FileSettings.themeChange(CurrentTheme);
    }

    private void CloseMenuCheckBox_OnChecked(object sender, RoutedEventArgs e)
    {
        FileSettings.closeMenu = true;
    }

    private void CloseMenuCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
    {
        FileSettings.closeMenu = false;
    }

    private void AnimationCheckBox_OnChecked(object sender, RoutedEventArgs e)
    {
        FileSettings.closeMenuAnimation = true;
    }

    private void AnimationCheckBox_OnUnchecked(object sender, RoutedEventArgs e)
    {
        FileSettings.closeMenuAnimation = false;
    }

    private void Slider1_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        FileSettings.delayTopMenu = e.NewValue;
    }

    private void Slider2_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        FileSettings.delayBottonMenu = e.NewValue;
    }
}