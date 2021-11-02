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

namespace QviKD
{
    /// <summary>
    /// Interaction logic for MainContent.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
#if DEBUG
            for(int index = 0; index < 30; index++)
            {
                MainPage_AppendButton(index);
            }
#else
            // Append buttons that each represents a monitor detected by the system.
            for (int index = 0; index < Database.Displays.Count; index++)
            {
                MainPage_AppendButton(index);
            }

            // Append button that is for accessing modules that is not dependent to any monitor.
            MainPage_AppendButton(Database.Displays.Count, "Modules");
#endif
        }

        private void MainPage_AppendButton(int index, string content = null)
        {
            // Expand row of the grid if the index is out of range.
            while (index / MainPageContent.ColumnDefinitions.Count >= MainPageContent.RowDefinitions.Count)
            {
                MainPageContent.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(0, GridUnitType.Auto),
                });
            }

            // Create a button and add it to the grid.
            Button button = new()
            {
                Name = $"MainMenuButton{index}",
                Margin = new Thickness(8),
                Content = index < Database.Displays.Count && content == null ? Database.Displays[index].EDID.DisplayName : content,
                MaxWidth = MainPageContent.MaxWidth / MainPageContent.ColumnDefinitions.Count * 0.8,
                MaxHeight = MaxWidth,
            };
            button.SetValue(Grid.ColumnProperty, index % MainPageContent.ColumnDefinitions.Count);
            button.SetValue(Grid.RowProperty, index / MainPageContent.ColumnDefinitions.Count);
            _ = MainPageContent.Children.Add(button);

            // Square the button.
            Binding binding = new("ActualWidth")
            {
                RelativeSource = RelativeSource.Self,
            };
            _ = button.SetBinding(HeightProperty, binding);

            // Add Click event to the button.
            button.Click += new RoutedEventHandler(MainPage_ClickButton);
        }

        private void MainPage_ClickButton(object sender, RoutedEventArgs e)
        {
            // Pass the button (or its corresponding monitor index) via arbitrary data within the page element.
            int index = MainPageContent.Children.IndexOf((Button)sender);

            if (index < Database.Displays.Count)
            {
                (Tag as MainWindow).GoTo(index);
                _ = NavigationService.Navigate(new Uri("Pages/MonitorPage.xaml", UriKind.Relative));
            }
            else if (index == Database.Displays.Count)
            {
                (Tag as MainWindow).GoTo(MainWindow.PAGES.MODULES);
                _ = NavigationService.Navigate(new Uri("Pages/ModulePage.xaml", UriKind.Relative));
            }
            else
            {
            
            }
        }
    }
}
