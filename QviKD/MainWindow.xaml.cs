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
            _ = new EnumDisplays();
            _ = new EnumModules();
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

        private void MainWindowCaptionButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
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
            if (MainWindowContent.CanGoBack)
            {
                JournalEntry entry = MainWindowContent.RemoveBackEntry();
                while (entry != null)
                {
                    entry = MainWindowContent.RemoveBackEntry();
                }
            }
        }
    }

    /// <summary>
    /// Converter for changing between Resize and Maximize icon on a caption button.
    /// </summary>
    [ValueConversion(typeof(WindowState), typeof(ImageSource))]
    public class CaptionButtonIconConverter : IValueConverter
    {
        // Binding source → Binding target
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(ImageSource))
                throw new InvalidOperationException($"The {targetType} of the value is incompatible with ImageSource type.");

            return ((WindowState)value == WindowState.Maximized)
            ? BitmapFrame.Create(Application.GetResourceStream(new Uri("Images/iconmonstr-restore-thin-32.png", UriKind.Relative)).Stream)
            : BitmapFrame.Create(Application.GetResourceStream(new Uri("Images/iconmonstr-maximize-thin-32.png", UriKind.Relative)).Stream);
        }

        // Binding target → Binding source
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
