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
using QviKD.Types;

namespace QviKD.Module.QvPattern
{
    /// <summary>
    /// Interaction logic for MainModule.xaml
    /// </summary>
    public partial class MainWindow : ModuleWindow
    {
        // Constructor: instantiated modules via ToolkitPage.xaml
        // ...independent from monitor models.
        public MainWindow() : base()
        {
            InitializeComponent();
        }

        // Constructor: instantiated modules via MonitorPage.xaml
        // ...should be available only on specific monitor models.
        public MainWindow(Display display) : base(display)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Static method that defines set of monitors the module should be available.
        /// </summary>
        public static new bool IsAvailable(string monitor, Type type)
        {
            // Select where the module is going to be available from:
            // * MonitorPage.xaml : false
            // * ToolkitPage.xaml : true
            ModuleAvailableAsToolkit = false;

            // Add name of monitors here to have modules available
            // ...or leave empty for all monitors.
            Monitors.UnionWith(new HashSet<string>
            {
                // Add monitors here...
                
            });
            return ModuleWindow.IsAvailable(monitor, type);
        }
        
        protected override void ModuleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Comment "ModuleWindow_Loaded" method to prevent window from maximizing.
            base.ModuleWindow_Loaded(sender, e);

            // Insert code here...

        }
    }

}