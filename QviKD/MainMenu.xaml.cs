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
    /// Interaction logic for MainContent.xaml
    /// </summary>
    public partial class MainMenu : UserControl
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void MainMenu_Loaded(object sender, RoutedEventArgs e)
        {
#if !DEBUG
            for(int index = 0; index < 30; index++)
            {
                MainMenu_AppendButton(index);
            }
#else
            // Append buttons that each represents a monitor detected by the system.
            for (int index = 0; index < Database.Displays.Count; index++)
            {
                MainMenu_AppendButton(index);
            }

            // Append button that is for accessing modules that is not dependent to any monitor.
            MainMenu_AppendButton(Database.Displays.Count, "Modules");
#endif
        }

        private void MainMenu_AppendButton(int index, string content = null)
        {
            // Expand row of the grid if the index is out of range.
            while (index / MainMenuContent.ColumnDefinitions.Count >= MainMenuContent.RowDefinitions.Count)
            {
                MainMenuContent.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(0, GridUnitType.Auto),
                });
            }

            // Create a button and add it to the grid.
            Button button = new()
            {
                Name = $"MainMenuButton{index}",
                Margin = new Thickness(8),
                Content = index < Database.Displays.Count && content == null ? Database.Displays[index].DeviceName : content,
                MaxWidth = MainMenuContent.MaxWidth / MainMenuContent.ColumnDefinitions.Count * 0.8,
                MaxHeight = MaxWidth,
            };
            button.SetValue(Grid.ColumnProperty, index % MainMenuContent.ColumnDefinitions.Count);
            button.SetValue(Grid.RowProperty, index / MainMenuContent.ColumnDefinitions.Count);
            _ = MainMenuContent.Children.Add(button);

            // Square the button.
            Binding binding = new("ActualWidth")
            {
                RelativeSource = RelativeSource.Self,
            };
            _ = button.SetBinding(HeightProperty, binding);

            // Add Click event to the button.
            button.Click += new RoutedEventHandler(MainMenu_ClickButton);
        }

        private void MainMenu_ClickButton(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;

            

            _ = MessageBox.Show($"{button.Content}");
        }
    }
}
