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

        public ModuleWindow()
        { 
        
        }

        public ModuleWindow(Display display)
        {
            WindowStartupLocation = WindowStartupLocation.Manual;
            Left = display.Rect.left;
            Top  = display.Rect.top;
            //Left = (display.Rect.right - display.Rect.left) / 2;
            //Top  = (display.Rect.bottom - display.Rect.top) / 2;

            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Minimized;
            Loaded += ModuleWindow_Loaded;            
        }

        protected virtual void ModuleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// Condition that defines which monitor should the module be available.
        /// </summary>
        public static bool IsValidMonitor(string monitor)
        {
            if (monitor is null)
            {
                throw new ArgumentNullException(nameof(monitor));
            }

            return false;
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
