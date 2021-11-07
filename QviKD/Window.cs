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
            ModuleWindow_Constructor();
            Display = null;

            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowState = WindowState.Normal;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        // Constructor w/ Display information
        // : for calibration, pattern generator, et cetra.
        protected ModuleWindow(Display display)
        {
            ModuleWindow_Constructor();
            Display = display;

            WindowStyle = WindowStyle.None;
            WindowState = WindowState.Minimized;
            WindowStartupLocation = WindowStartupLocation.Manual;
        }

        private void ModuleWindow_Constructor()
        {
            Foreground = new SolidColorBrush(Colors.White);
            Background = new SolidColorBrush(Colors.Transparent);
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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        protected static bool IsAvailable(string monitor, Type type)
        {
            if (type == typeof(WidgetPage))
                return ModuleAvailableAsToolkit;
            else
                return ModuleAvailableAsToolkit is false
                    && (Monitors.Count is 0 || Monitors.Contains(monitor));
        }

        private void ModuleWindowCaptionButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Print message for debugging; DEBUG-mode exclusive.
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        private void DebugMessage(string msg)
        {
            System.Diagnostics.Debug.WriteLine($"'{GetType().Name}.cs' {msg}");
        }
    }

    public abstract class WidgetWindow : Window
    {
        static WidgetWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(WidgetWindow), new FrameworkPropertyMetadata(typeof(WidgetWindow)));
        }
    }
}
