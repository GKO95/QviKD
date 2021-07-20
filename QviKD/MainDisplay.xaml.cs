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
            for (int index = 0; index < Database.Displays.Count; index++)
            {
                System.Diagnostics.Debug.WriteLine(MainDisplayListStack.Height);
                MainDisplayListStack.Children.Add(new MainDisplayItem()
                { 
                    Height = MainDisplayListStack.ActualHeight,
                    Width = MainDisplayListStack.ActualHeight,
                });
            }
        }

        private void MainDisplayList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            foreach (MainDisplayItem element in MainDisplayListStack.Children)
            {
                element.Height = MainDisplayListStack.ActualHeight;
                element.Width = MainDisplayListStack.ActualHeight;
            }
        }
    }

    public class MainDisplayItem : UserControl
    {
        public MainDisplayItem()
        {
            Background = new SolidColorBrush(Color.FromArgb(255, 40, 40, 40));
            Margin = new Thickness(4, 4, 4, 4);
        }
    }
}
