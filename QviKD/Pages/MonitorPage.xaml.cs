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
using System.Globalization;
using QviKD.Types;

namespace QviKD
{
    /// <summary>
    /// Interaction logic for MonitorMenu.xaml
    /// </summary>
    public partial class MonitorPage : Page
    {
        private Display Display;
        private readonly static InUse2IsEnableConverter Converter = new();

        public MonitorPage()
        {
            InitializeComponent();
        }

        private void MonitorPage_Loaded(object sender, RoutedEventArgs e)
        {
            int PageIndex = (Tag as MainWindow).PageIndex;
            if (PageIndex < Database.Displays.Count && PageIndex > (int)MainWindow.PAGES.MAIN)
            {
                Display = Database.Displays[(Tag as MainWindow).PageIndex];
                MonitorPageNavigationTitle.Text = Display.EDID.DisplayName;

                MonitorControlEDID.Display = Display;

                // For each modules detected and stored in the Database...
                foreach (Module module in Database.Modules)
                {
                    if (module.IsAvailable(Display))
                    {
                        Button button = new()
                        {
                            Name = $"MonitorPageModule{module.AssemblyName.Name}Button",
                            Content = module.AssemblyName.Name,
                            Tag = module,
                        };
                        button.SetBinding(IsEnabledProperty, new Binding("InUse")
                        {
                            Source = Display,
                            Mode = BindingMode.OneWay,
                            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                            Converter = Converter,
                        });
                        button.Click += new RoutedEventHandler(MonitorPageModule_ClickButton);
                        MonitorPageModules.Children.Add(button);
                    }
                }
            }
            else
            {
                MonitorPageNavigationTitle.Text = "RETURN";
                MonitorPageContentList.Visibility = Visibility.Collapsed;
                MonitorPageContentError.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Return to the main page.
        /// </summary>
        private void MonitorPageNavigationBack_Click(object sender, RoutedEventArgs e)
        {
            (Tag as MainWindow).GoTo(MainWindow.PAGES.MAIN);
            _ = NavigationService.Navigate(new Uri("Pages/MainPage.xaml", UriKind.Relative));
        }

        /// <summary>
        /// Create an module instance from the library on run-time.
        /// </summary>
        private void MonitorPageModule_ClickButton(object sender, RoutedEventArgs e)
        {
            object wnd = Activator.CreateInstance(((Module)((Button)sender).Tag).Type, Display);
            (wnd as ModuleWindow).Show();
        }
    }

    /// <summary>
    /// Converter for binding data between <i>Display.InUse</i> and <i>Button.IsEnable</i> property.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InUse2IsEnableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException($"The {targetType} of the binding target is incompatible with Boolean data type.");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
