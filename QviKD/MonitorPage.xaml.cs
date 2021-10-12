using System;
using System.Reflection;
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

namespace QviKD
{
    /// <summary>
    /// Interaction logic for MonitorMenu.xaml
    /// </summary>
    public partial class MonitorPage : Page
    {
        private DISPLAY display;

        public MonitorPage()
        {
            InitializeComponent();
        }

        private void MonitorPage_Loaded(object sender, RoutedEventArgs e)
        {
            display = Database.Displays[(Tag as MainWindow).Page];

            MonitorPageHeaderTitle.Content = display.EDID.DisplayName;

            MonitorPageInformationIsPrimary.Content = display.IsPrimary ? "Yes" : "No";
            MonitorPageInformationResolution.Content = $"{display.Rect.right - display.Rect.left} x {display.Rect.bottom - display.Rect.top}";
            MonitorPageInformationPosition.Content = $"({display.Rect.left}, {display.Rect.top})";

            MonitorPageInformationDescription.Content = display.Description;

            MonitorPageInformationDeviceName.Content = display.DeviceName;
            MonitorPageInformationDeviceID.Content = display.DeviceID;

        }

        private void MonitorPageHeaderBack_Click(object sender, RoutedEventArgs e)
        {
            (Tag as MainWindow).GoTo(MainWindow.PAGES.MAIN);
            _ = NavigationService.Navigate(new Uri("MainPage.xaml", UriKind.Relative));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Assembly assembly = Database.Modules[0];
            Window obj = (Window)assembly.CreateInstance("QviKD.Modules.Pattern.MainModule");


            obj.Show();
        }
    }
}
