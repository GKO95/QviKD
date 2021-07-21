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
    /// Interaction logic for MainTabsDisplay.xaml
    /// </summary>
    public partial class MainDisplay : UserControl
    {
        public MainDisplay()
        {
            InitializeComponent();
        }

        private void MainDisplayList_Loaded(object sender, RoutedEventArgs e)
        {
            double size = MainDisplayListScroll.ActualHeight - (MainDisplayListScroll.Padding.Top + MainDisplayListScroll.Padding.Bottom);

            for (int index = 0; index < Database.Displays.Count; index++)
            {
                MainDisplayListStack.Children.Add(new MainDisplayItem()
                {
                    Background = new SolidColorBrush(Color.FromArgb(255, 40, 40, 40)),
                    Margin = new Thickness(0, 0, 8, 0),
                    Height = size,
                    Width = size,
                });
            }
        }

        private void MainDisplayList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (MainDisplayListStack.Children.Count > 0)
            {
                double size = MainDisplayListScroll.ActualHeight - (MainDisplayListScroll.Padding.Top + MainDisplayListScroll.Padding.Bottom);
                foreach (MainDisplayItem item in MainDisplayListStack.Children)
                {
                    item.Height = size;
                    item.Width = size;
                }
            }
        }
    }

    public class MainDisplayItem : UserControl
    {
        public MainDisplayItem()
        {
            
        }
    }
}
