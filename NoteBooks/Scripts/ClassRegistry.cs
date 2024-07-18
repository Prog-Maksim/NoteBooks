using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using Microsoft.Win32;

namespace StickyNotes.Scripts;

public class ClassRegistry
{
    public ClassRegistry()
    {
        if (!checkProgramIsAdministrator())
            IsAdministrator();
    }
    
    private static bool checkProgramIsAdministrator()
    {
        WindowsIdentity id = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(id);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
    
    private static void IsAdministrator()
    {
        ProcessStartInfo proc = new ProcessStartInfo
        {
            UseShellExecute = true,
            WorkingDirectory = Environment.CurrentDirectory
        };

        var processModule = Process.GetCurrentProcess().MainModule;
        if (processModule != null && processModule.FileName != null)
            proc.FileName = Path.GetFullPath(processModule.FileName);

        proc.Verb = "runas";
        Process.Start(proc);
    }
    
    public static void IsAdministrator(MainWindow app)
    {
        IsAdministrator();
        app.Close();
    }
    
    public static bool checkPathFolderIsRegistry()
    {
        using (RegistryKey currentUserKey = Registry.CurrentUser)
        {
            RegistryKey? mainKey = currentUserKey.OpenSubKey("StickyNotes", false);
            
            if (mainKey?.GetValue("FolderPath") != null)
                return true;
            return false;
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

    public void createPathFolderIsRegistry()
    {
        using (RegistryKey currentUserKey = Registry.CurrentUser)
        {
            RegistryKey mainKey = currentUserKey.CreateSubKey("StickyNotes", true);
            mainKey.SetValue("FolderPath", mainBasePath);
            mainKey.Close();
        }
    }
    
    public static void DeleteFolderData()
    {
        if (Directory.Exists(mainBasePath))
            Directory.Delete(mainBasePath);
    }

    public void deletePathFolderIsRegistry()
    {
        using (RegistryKey currentUserKey = Registry.CurrentUser)
        {
            RegistryKey? mainKey = currentUserKey.OpenSubKey("StickyNotes", true);
            mainKey!.DeleteValue("FolderPath");
            mainKey.Close();
            currentUserKey.DeleteSubKey("StickyNotes");
        }
    }
    
    public static themeNameStyle getCurrentThemeStyle()
    {
        try
        {
            var themeId =
                Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize",
                    "AppsUseLightTheme", "1");
            if (themeId != null && themeId.ToString() == "0")
                return themeNameStyle.dark;
        }
        catch
        {
            // ignore
        }

        return themeNameStyle.light;
    }
}