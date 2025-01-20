using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using StickyNotes.FrameMainWindows.StartUpMenu;
using StickyNotes.Models;
using StickyNotes.OtherWindowsProgram;
using StickyNotes.Scripts;
using Sticker = StickyNotes.Scripts.Sticker;


namespace StickyNotes
{
    public partial class App
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            checkProgramData(e.Args);
        }
        
        private void checkProgramData(string[] arguments)
        {
            if (!ClassRegistry.checkPathFolderIsRegistry())
            {
                ClassRegistry classRegistry = new ClassRegistry();
                ClassRegistry.createFolderData();
                classRegistry.createPathFolderIsRegistry();
                FileSettings.createSystemFiles();

                StartMenu menu = new StartMenu();
                menu.Show();
            }
            else
            {
                if (arguments.Length == 0)
                {
                    sturtUpProgram(WindowState.Normal);
                }
                else if (arguments[0] == "/StartMinimized")
                {
                    sturtUpProgram(WindowState.Minimized);
                }
                else if (arguments[0] == "/OpenSticker")
                {
                    int openSticker = openThumbtackStickers();
                    if (openSticker == 0) Application.Current.Shutdown();
                }
                else
                {
                    openSticker(arguments[0]);
                }
            }
        }

        private void sturtUpProgram(WindowState state)
        {
            string procName = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(procName);
            if (processes.Length > 1)
                Application.Current.Shutdown();
            
            try
            {
                MainWindow mainWindow = new MainWindow
                {
                    WindowState = state
                };
                mainWindow.Show();
            }
            catch (InvalidOperationException) { }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "StickyNotes", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            
            openThumbtackStickers();
        }

        private int openThumbtackStickers()
        {
            StickersList stickersList = Sticker.getAllDataSticker();
            int openSticker = 0;

            try
            {
                foreach (var sticker in stickersList.Stickers!)
                {
                    if (sticker.StickerThumbtack && !Directory.Exists(Path.Combine(ClassRegistry.PathOpenStickers, $"~{sticker.Name}")))
                    {
                        openStickers(sticker.Name);
                        openSticker++;
                    }
                }
            }
            catch
            {
                // ignored
            }

            return openSticker;
        }

        private void openStickers(string name)
        {
            var sticky = new Sticky(name);
            sticky.Show();
        }

        private void openSticker(string nameSticker)
        {
            try
            {
                string name = new FileInfo(nameSticker).Name.Replace(".rtf", "");

                if (!File.Exists(nameSticker) && !Directory.Exists(Path.Combine(ClassRegistry.PathOpenStickers, $"~{name}")))
                    Application.Current.Shutdown();
                    
                    
                if (Sticker.checkIsAvailabilityFile(name))
                    openStickers(name);
                else
                {
                    Sticker.addNewStickerContextMenu(new FileInfo(nameSticker));
                    openStickers(name);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "StickyNotes", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }
    }
}