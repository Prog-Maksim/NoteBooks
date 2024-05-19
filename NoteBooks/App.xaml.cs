using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using NoteBooks.Models;


namespace NoteBooks
{
    public partial class App
    {
        void App_Startup(object sender, StartupEventArgs e)
        {
            if (e.Args.Length == 0)
            {
                sturtUpProgram(WindowState.Normal);
            }
            else if (e.Args[0] == "/StartMinimized")
            {
                sturtUpProgram(WindowState.Minimized);
            }
            else if (e.Args[0] == "/OpenSticker")
            {
                int openSticker = openThumbtackStickers();
                if (openSticker == 0) Application.Current.Shutdown();
            }
            else
            {
                try
                {
                    if (File.Exists(e.Args[0]))
                    {
                        string name = new FileInfo(e.Args[0]).Name.Replace(".rtf", "");

                        if (!Directory.Exists(Path.Combine(ClassRegistry.PathOpenStickers, $"~{name}")))
                        {
                            if (Sticker.checkIsAvailabilityFile(name))
                            {
                                openStickers(name);
                            }
                            else
                            {
                                Sticker.addNewStickerContextMenu(new FileInfo(e.Args[0]));
                                openStickers(name);
                            }
                        }
                        else
                            Application.Current.Shutdown();
                    }
                    else
                        Application.Current.Shutdown();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message + "\n" + exception.StackTrace, "StickyNotes", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Shutdown();
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
                MainWindow mainWindow = new MainWindow();
                mainWindow.WindowState = state;
                mainWindow.Show();
            }
            catch (InvalidOperationException) { }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
            
            openThumbtackStickers();
        }

        private int openThumbtackStickers()
        {
            StickersList stickersList = Sticker.getAllDataSticker();
            int openSticker = 0;

            foreach (var sticker in stickersList.Stickers)
            {
                if (sticker.StickerThumbtack && !Directory.Exists(Path.Combine(ClassRegistry.PathOpenStickers, $"~{sticker.Name}")))
                {
                    openStickers(sticker.Name);
                    openSticker++;
                }
            }

            return openSticker;
        }

        private void openStickers(string name)
        {
            var sticky = new Sticky(name);
            sticky.Show();
        }
    }
}