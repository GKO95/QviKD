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
using QviKD.Types;

namespace QviKD
{
    /// <summary>
    /// Interaction logic for MonitorMenu.xaml
    /// </summary>
    public partial class MonitorPage : Page
    {
        private Display display;

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

            // For each modules detected and stored in the Database...
            foreach (Module module in Database.Modules)
            {
                // 
                if ((bool)module.Type.GetMethod("IsAvailable",
                    System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { display.EDID.DisplayName , GetType() } ))
                {
                    Button button = new()
                    {
                        Name = $"MonitorPageModule{module.AssemblyName.Name}",
                        Content = module.AssemblyName.Name,
                        Tag = module.Type,
                    };
                    MonitorPageModules.Children.Add(button);
                    button.Click += new RoutedEventHandler(MonitorPageModule_ClickButton);
                }

            }


        }

        private void MonitorPageHeaderBack_Click(object sender, RoutedEventArgs e)
        {
            (Tag as MainWindow).GoTo(MainWindow.PAGES.MAIN);
            _ = NavigationService.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
        }

        private void MonitorPageModule_ClickButton(object sender, RoutedEventArgs e)
        {
            if (Activator.CreateInstance((Type)((Button)sender).Tag, new Display[] { display }) is ModuleWindow wnd)
            {
                wnd.Show();
            }
        }
    }
}
