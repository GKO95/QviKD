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
        private readonly string[] Monitors = {
            // Add monitors here...

        };

        public MainWindow() : base()
        {
            InitializeComponent();
        }

        public MainWindow(Display display) : base(display)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Condition that defines which monitor should the module be available.
        /// </summary>
        protected static bool IsMonitor(string monitor)
        {
            return false;
        }

        protected override void ModuleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            base.ModuleWindow_Loaded(sender, e);
        }
    }

}