using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace QviKD.Controls
{
    /// <summary>
    /// Interaction logic for HexBox.xaml
    /// </summary>
    public partial class HexBox : TextBox
    {
        public HexBox()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9A-Fa-f]+");
            e.Handled = regex.IsMatch(input: e.Text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Text != string.Empty)
            {
                if (!Regex.IsMatch(Text, @"^([0-9A-Fa-f])*$"))
                {
                    e.Handled = false;
                    Text = Regex.Replace(Text, @"([^0-9A-Fa-f])*", "");
                    CaretIndex = Text.Length;
                }
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            while (Text.Length < MaxLength)
            {
                Text = Text.Insert(0, "0");
            }
        }
    }
}
