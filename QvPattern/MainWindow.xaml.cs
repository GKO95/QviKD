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

namespace QviKD.Modules.QvPattern
{
    /*==============================================================
    |                   >> PROJECT GUIDELINE <<                    |
    |                                                              |
    |   This comment section provides guideline on how to utilize  |
    | the project for creating custom modules and widgets.         |
    |                                                              |
    |     1. Avoid changing codes under #region directives.        |
    |                                                              |
    |     2. Specify whether the project is designed as            |
    |          module (#define MODULE) or widget (#define WIDGET). |
    |                                                              |
    |     3. DO NOT ATTEMPT TO MODIFY THE BASE CLASS!              |
    |                                                              |
    |   Appropriate operation cannot be guarnateed if failed to    |
    | observe the guideline above, and the original author do not  |
    | hold any responsibility follow by such actions.              |
    ==============================================================*/

    #region Compile Conditioning
#if MODULE && WIDGET
#error Conflicting module availability between definitions, MODULE and WIDGET.
#elif !MODULE && !WIDGET
#warning Missing module availability definition, compile as default MODULE.
#endif
    #endregion

    /// <summary>
    /// Interaction logic for MainModule.xaml
    /// </summary>
    public partial class MainWindow : ModuleWindow
    {
        #region Module Constuctor
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
        #endregion

        /// <summary>
        /// Static method that defines set of monitors the module should be available.
        /// </summary>
        public static new bool IsAvailable(string monitor, Type type)
        {
            #region Module Availability
            // Select where the module is going to be available from:
            ModuleAvailableAsToolkit
#if SUBMODULE
                = true;     // * ToolkitPage.xaml
#else
                = false;    // * MonitorPage.xaml
#endif
            #endregion

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
