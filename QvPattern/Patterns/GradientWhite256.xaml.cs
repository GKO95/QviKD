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

namespace QviKD.Modules.QvPattern
{
    /// <summary>
    /// Interaction logic for GradientWhite256.xaml
    /// </summary>
    public partial class GradientWhite256 : Page
    {
        public GradientWhite256()
        {
            InitializeComponent();

            for (int index = 0; index < MAX(DEPTH.BIT8); index++)
            {
                GradientWhite256Content.ColumnDefinitions.Add(new ColumnDefinition());
                Rectangle rectangle = new()
                {
                    Fill = new SolidColorBrush(Color.FromRgb((byte)index, (byte)index, (byte)index)),
                    Margin = new Thickness(0),
                }; 
                rectangle.SetValue(Grid.ColumnProperty, index);
                GradientWhite256Content.Children.Add(rectangle);
            }
        }

        private static uint MAX(DEPTH depth) => Convert.ToUInt32(Math.Pow(2, (double)depth));

        enum DEPTH : uint
        { 
            BIT8    = 8,
            BIT10   = 10,
            BIT12   = 12,
            BIT14   = 14,
        }
    }
}
