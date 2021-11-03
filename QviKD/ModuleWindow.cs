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

namespace QviKD
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:QviKD"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:QviKD;assembly=QviKD"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:ModuleWindow/>
    ///
    /// </summary>
    public abstract class ModuleWindow : Window
    {
        static ModuleWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModuleWindow), new FrameworkPropertyMetadata(typeof(ModuleWindow)));
        }

        protected readonly Display Display;
        protected static readonly HashSet<string> Monitors = new();
        protected static bool ModuleAvailableAsToolkit = false;

        // Constructor w/o Display information
        // : for luminance measurement, calculator, et cetera.
        protected ModuleWindow()
        {
            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowState = WindowState.Normal;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            Display = null;
            Loaded += ModuleWindow_Loaded;
        }

        // Constructor w/ Display information
        // : for calibration, pattern generator, et cetra.
        protected ModuleWindow(Display display)
        {
            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Minimized;
            WindowStartupLocation = WindowStartupLocation.Manual;

            Display = display;
            Loaded += ModuleWindow_Loaded;            
        }

        protected virtual void ModuleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Maximize if Display information is specified...
            if (Display is not null)
            {
                Left = Display.Rect.left;
                Top = Display.Rect.top;
                WindowState = WindowState.Maximized;
            }
            // Otherwise...
            else
            {
                ;
            }
        }

        protected static bool IsAvailable(string monitor, Type type)
        {
            if (type == typeof(ToolkitPage))
                return ModuleAvailableAsToolkit;
            else
                return ModuleAvailableAsToolkit is false
                    && (Monitors.Count is 0 || Monitors.Contains(monitor));
        }

        /// <summary>
        /// Print message for debugging; DEBUG-mode exclusive.
        /// </summary>
        private void DebugMessage(string msg)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"'{GetType().Name}.cs' {msg}");
#endif
        }
    }
}
