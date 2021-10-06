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
#if DEBUG
            for(int index = 0; index < 6; index++)
            {
                if (index % MainMenuContent.ColumnDefinitions.Count == 0)
                {
                    MainMenuContent.RowDefinitions.Add(new RowDefinition()
                    { 
                        Height = new GridLength(0, GridUnitType.Auto),
                    });
                }

                Button button = new()
                {
                    Margin = new Thickness(8),
                };
                button.SetValue(Grid.ColumnProperty, index % MainMenuContent.ColumnDefinitions.Count);
                button.SetValue(Grid.RowProperty, index / MainMenuContent.ColumnDefinitions.Count);
                MainMenuContent.Children.Add(button);

                Binding binding = new("ActualWidth")
                {
                    RelativeSource = RelativeSource.Self,
                };
                button.SetBinding(HeightProperty, binding);
            }

#else
            if (Database.Displays.Count > 0)
            {
                for (int index = 0; index < Database.Displays.Count; index++)
                {
                    if (index % MainMenuContent.ColumnDefinitions.Count == 0)
                    {
                        MainMenuContent.RowDefinitions.Add(new RowDefinition()
                        {
                            Height = new GridLength(0, GridUnitType.Auto),
                        });
                    }

                    Button button = new()
                    {
                        Margin = new Thickness(8),
                    };
                    button.SetValue(Grid.ColumnProperty, index % MainMenuContent.ColumnDefinitions.Count);
                    button.SetValue(Grid.RowProperty, index / MainMenuContent.ColumnDefinitions.Count);
                    MainMenuContent.Children.Add(button);

                    Binding binding = new("ActualWidth")
                    {
                        RelativeSource = RelativeSource.Self,
                    };
                    button.SetBinding(HeightProperty, binding);
                }
            }
#endif
        }
    }
}
