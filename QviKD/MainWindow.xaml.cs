using System;
using System.Collections.Generic;
using System.Globalization;
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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _ = new EnumDisplays();
            _ = new EnumModules();
            MainWindowClientContentLoading.Visibility = Visibility.Collapsed;
        }

        private void Window_Closed(object sender, EventArgs e)
        {            
            Application.Current.Shutdown();
        }

        private PAGES PageNavigation { get; set; } = PAGES.MAIN;
        public int PageIndex => (int)PageNavigation;
        public void GoTo(int index)  => GoTo((PAGES)index);
        public void GoTo(PAGES page) => PageNavigation = page;
        public enum PAGES : int
        {
            MAIN    = -1,
            MODULES = -2,
            SETTING = -3,
        }

        private void MainWindowCaptionButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void MainWindowCaptionButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized) WindowState = WindowState.Normal;
            else WindowState = WindowState.Maximized;
        }
        private void MainWindowCaptionButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MainWindowContent_LoadCompleted(object sender, NavigationEventArgs e)
        {
            // Allow the page to access the MainWindow object via its arbitrary data, namely "Tag" property.
            (e.Content as Page).Tag = this;

            // Disable Go Back navigation by removing recent entry history every time the frame navigates.
            if (MainWindowClientContentFrame.CanGoBack)
            {
                JournalEntry entry = MainWindowClientContentFrame.RemoveBackEntry();
                while (entry != null)
                {
                    entry = MainWindowClientContentFrame.RemoveBackEntry();
                }
            }
        }

        /// <summary>
        /// Print message for debugging; DEBUG-mode exclusive.
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        private void DebugMessage(string msg)
        {
            System.Diagnostics.Debug.WriteLine($"'{GetType().Name}.cs' {msg}");
        }
    }
}
