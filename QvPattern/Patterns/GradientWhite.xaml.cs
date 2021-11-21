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

namespace QviKD.Modules.QvPattern.Patterns
{
    /// <summary>
    /// Interaction logic for GradientWhite.xaml
    /// </summary>
    public partial class GradientWhite : Page
    {
        public GradientWhite()
        {
            InitializeComponent();

            for (int index = 0; index < MAX(DEPTH.BIT8); index++)
            {
                GradientWhiteContent.ColumnDefinitions.Add(new ColumnDefinition());
                Rectangle rectangle = new()
                {
                    Fill = new SolidColorBrush(Color.FromRgb((byte)index, (byte)index, (byte)index)),
                    Margin = new Thickness(0),
                }; 
                rectangle.SetValue(Grid.ColumnProperty, index);
                GradientWhiteContent.Children.Add(rectangle);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Window wnd = Window.GetWindow(this);
            wnd.KeyDown += HandleKeyPress;
        }

        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {

            }
            else if (e.Key == Key.Down)
            {

            }
        }

        private static uint MAX(DEPTH depth) => Convert.ToUInt32(Math.Pow(2, (double)depth));
    }
}
