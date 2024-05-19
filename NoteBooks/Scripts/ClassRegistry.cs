using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Windows;
using Microsoft.Win32;

namespace NoteBooks;

public class ClassRegistry
{
    public ClassRegistry(MainWindow window)
    {
        if (!checkProgramIsAdministrator())
        {
            IsAdministrator(window);
        }
    }
    
    public static bool checkProgramIsAdministrator()
    {
        WindowsIdentity id = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(id);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
    
    public static void IsAdministrator(MainWindow app)
    {
        try
        {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.UseShellExecute = true;
            proc.WorkingDirectory = Environment.CurrentDirectory;
            proc.FileName = Path.GetFullPath(Process.GetCurrentProcess().MainModule.FileName);
            proc.Verb = "runas";
            Process.Start(proc);
            app.Close();
        }
        catch
        {
            MessageBox.Show("Произошла ошибка, повторите попытку позже...", "StickyNotes", MessageBoxButton.OK, MessageBoxImage.Error);
            app.Close();
        }
    }
    
    public static bool checkPathFolderIsRegistry()
    {
        using (RegistryKey currentUserKey = Registry.CurrentUser)
        {
            RegistryKey mainKey = currentUserKey.OpenSubKey("StickyNotes", false);
            
            if (mainKey?.GetValue("FolderPath") != null)
                return true;
            return false;
        }
    }

    public static string getDataForRegistry()
    {
        using (RegistryKey currentUserKey = Registry.CurrentUser)
        {
            RegistryKey mainKey = currentUserKey.OpenSubKey("StickyNotes", false);
            return mainKey.GetValue("FolderPath").ToString();
        }
    }
    
    public static string mainBasePath
    {
        get
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string folderPath = Path.Combine(localAppData, "StickyNotes");
            return folderPath;
        }
    }

    public static string PathStickyFolder
    {
        get
        {
            return Path.Combine(mainBasePath, "StickyFolderData");
        }
    }

    public static string PathOpenStickers
    {
        get
        {
            return Path.Combine(mainBasePath, "OpenStickers");
        }
    }
    
    public static void createFolderData()
    {
        if (!Directory.Exists(mainBasePath))
        {
            Directory.CreateDirectory(mainBasePath);
            string folderPath = Path.Combine(mainBasePath, "StickyFolderData");
            Directory.CreateDirectory(folderPath);
            string folderPath1 = Path.Combine(mainBasePath, "OpenStickers");
            Directory.CreateDirectory(folderPath1);
        }
    }

    public static void DeleteFolderData()
    {
        if (Directory.Exists(mainBasePath))
            Directory.Delete(mainBasePath);
    }

    public void createPathFolderIsRegistry()
    {
        using (RegistryKey currentUserKey = Registry.CurrentUser)
        {
            RegistryKey mainKey = currentUserKey.CreateSubKey("StickyNotes", true);
            mainKey.SetValue("FolderPath", mainBasePath);
            mainKey.Close();
        }
    }

    public void deletePathFolderIsRegistry()
    {
        using (RegistryKey currentUserKey = Registry.CurrentUser)
        {
            RegistryKey mainKey = currentUserKey.OpenSubKey("StickyNotes", true);
            mainKey.DeleteValue("FolderPath");
            mainKey.Close();
            currentUserKey.DeleteSubKey("StickyNotes");
        }
    }

    public static void AddToContextMenu()
    {
        string extension = ".rtf";
        RegistryKey key = Registry.ClassesRoot.OpenSubKey(extension, true);
        if (key == null)
        {
            key = Registry.ClassesRoot.CreateSubKey(extension);
        }

        // Создаем подключение к ключу реестра для добавления команды в контекстное меню
        RegistryKey shellKey = key.CreateSubKey("shell\\sticky");
        shellKey.SetValue("", "sticky");
        
        RegistryKey commandKey = shellKey.CreateSubKey("command");
        commandKey.SetValue("", "\"D:\\важные файлы\\Program C#\\GraphicsApplication\\NoteBooks\\NoteBooks\\bin\\Debug\\net6.0-windows\\NoteBooks.exe %1\"");
        MessageBox.Show("Программа успешно добавлена в реестр");
        key.Close();
    }
    
    public static themeNameStyle getCurrentThemeStyle()
    {
        try
        {
            var themeId = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", "1");
            if (themeId != null && themeId.ToString() == "0")
                return themeNameStyle.dark;
        }
        catch { }

        return themeNameStyle.light;
    }
}