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
    /// Interaction logic for IntegerBox.xaml
    /// </summary>
    public partial class IntBox : TextBox
    {
        public IntBox()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(input: e.Text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Text != string.Empty)
            {
                if (!Regex.IsMatch(Text, @"^([0-9])*$"))
                {
                    e.Handled = false;
                    Text = Regex.Replace(Text, @"([^0-9])*", "");
                    CaretIndex = Text.Length;
                }

                if (MaxValue > 0 && Convert.ToInt32(Text) > MaxValue)
                {
                    e.Handled = false;
                    while (Convert.ToInt32(Text) > MaxValue)
                    {
                        Text = Text.Remove(Text.Length - 1);
                    }
                    CaretIndex = Text.Length;
                }
            }
        }

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(IntBox), new PropertyMetadata(0));
    }
}
