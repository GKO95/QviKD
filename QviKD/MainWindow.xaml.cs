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

using QviKD.Functions;

namespace QviKD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            _ = new EnumDisplays();
            _ = new EnumModules();
        }

        private void MainWindowContent_LoadCompleted(object sender, NavigationEventArgs e)
        {
            // Allow the page to access the MainWindow object via its arbitrary data, namely "Tag" property.
            (e.Content as Page).Tag = this;

            // Disable Go Back navigation by removing recent entry history every time the frame navigates.
            if (MainWindowContent.CanGoBack)
            {
                JournalEntry entry = MainWindowContent.RemoveBackEntry();
                while (entry != null)
                {
                    entry = MainWindowContent.RemoveBackEntry();
                }
            }
        }

        private PAGES PageNavigation { get; set; } = PAGES.MAIN;
        public int Page => (int)PageNavigation;

        public void GoTo(int index)  => GoTo((PAGES)index);
        public void GoTo(PAGES page) => PageNavigation = page;

        public enum PAGES : int
        {
            MAIN    = -1,
            MODULES = -2,
            SETTING = -3,
        }
    }
}
