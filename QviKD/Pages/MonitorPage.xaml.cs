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
        private Display Display;

        public MonitorPage()
        {
            InitializeComponent();
        }

        private void MonitorPage_Loaded(object sender, RoutedEventArgs e)
        {
            Display = Database.Displays[(Tag as MainWindow).Page];

            MonitorPageHeaderTitle.Content = Display.EDID.DisplayName;

            MonitorPageInformationIsPrimary.Content = Display.IsPrimary ? "Yes" : "No";
            MonitorPageInformationResolution.Content = $"{Display.Rect.right - Display.Rect.left} x {Display.Rect.bottom - Display.Rect.top}";
            MonitorPageInformationPosition.Content = $"({Display.Rect.left}, {Display.Rect.top})";

            MonitorPageInformationDescription.Content = Display.Description;

            MonitorPageInformationDeviceName.Content = Display.DeviceName;
            MonitorPageInformationDeviceID.Content = Display.DeviceID;

            // For each modules detected and stored in the Database...
            foreach (Module module in Database.Modules)
            {
                try
                {
                    if ((bool)module.Type.GetMethod("IsAvailable",
                        System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { Display.EDID }))
                    {
                        Button button = new()
                        {
                            Name = $"MonitorPageModule{module.AssemblyName.Name}Button",
                            Content = module.AssemblyName.Name,
                            Tag = module,
                        };
                        MonitorPageModules.Children.Add(button);
                        button.Click += new RoutedEventHandler(MonitorPageModule_ClickButton);
                    }
                }
                catch (NullReferenceException)
                { 
                    
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
            ModuleWindow wnd = new(Display, ((Button)sender).Tag as Module);
            wnd.Show();
        }
    }
}
