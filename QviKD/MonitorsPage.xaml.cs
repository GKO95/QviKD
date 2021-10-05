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

using QviKDLib;

namespace QviKD
{
    /// <summary>
    /// Interaction logic for MonitorsPage.xaml
    /// </summary>
    public partial class MonitorsPage : Page
    {
        public MonitorsPage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Database.Displays.Count > 0)
            {
                for (int index = 0; index < Database.Displays.Count; index++)
                {
                    if (index % MonitorsPageContent.ColumnDefinitions.Count == 0) MonitorsPageContent.RowDefinitions.Add(new RowDefinition());
                    Button button = new()
                    {
                        Template = (ControlTemplate)Resources["tplMonitorButton"],
                        Content = Database.Displays[index].DeviceName,
                    };

                    MonitorsPageContent.Children.Add(button);
                    button.SetValue(Grid.RowProperty, index / MonitorsPageContent.ColumnDefinitions.Count);
                    button.SetValue(Grid.ColumnProperty, index & MonitorsPageContent.ColumnDefinitions.Count);
                }
            }
            else
            {
                // TODO: Add code here...
            }
        }
    }
}
