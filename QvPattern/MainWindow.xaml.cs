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

namespace QviKD.Module.QvPattern
{
    /// <summary>
    /// Interaction logic for MainModule.xaml
    /// </summary>
    public partial class MainWindow : ModuleWindow
    {
        public MainWindow() : base()
        {
        
        }

        public MainWindow(Display display) : base(display)
        {
            
        }

        public static new bool IsValidMonitor(string monitor)
        {
            if (monitor is null)
            {
                throw new ArgumentNullException(nameof(monitor));
            }

            return true;
        }

        protected override void ModuleWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }

}