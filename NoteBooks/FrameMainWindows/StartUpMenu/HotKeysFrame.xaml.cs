using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StickyNotes.FrameMainWindows.StartUpMenu
{
    /// <summary>
    /// Логика взаимодействия для HotKeysFrame.xaml
    /// </summary>
    public partial class HotKeysFrame : UserControl
    {
        public HotKeysFrame()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        public string hotKeys { get; set; }
    }
}
