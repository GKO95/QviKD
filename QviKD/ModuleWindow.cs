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
    public class ModuleWindow : Window
    {
        static ModuleWindow() => DefaultStyleKeyProperty.OverrideMetadata(typeof(ModuleWindow), new FrameworkPropertyMetadata(typeof(ModuleWindow)));

        /// <summary>
        /// A set of monitors the module is available; leave empty for every monitor.
        /// </summary>
        protected readonly IReadOnlySet<string> Monitors;
        protected readonly Display Display;

        public ModuleWindow(HashSet<string> monitors) => Monitors = monitors;
        public ModuleWindow(Display display)
        {
            Display = display;

            Initialized += ModuleWindow_Initialized;
            Loaded += ModuleWindow_Loaded;
            Closed += ModuleWindow_Closed;
        }

        public override void OnApplyTemplate()
        {
            (GetTemplateChild("ModuleWindowCaptionButtonClose") as Button).Click += ModuleWindowCaptionButtonClose_Click;
            (GetTemplateChild("ModuleWindowCaptionButtonMinimize") as Button).Click += ModuleWindowCaptionButtonMinimize_Click;
        }

        /// <summary>
        /// Identifies whether the module is available for the monitor.
        /// </summary>
        public bool IsAvailable(Display display) => Monitors.Count is 0 || Monitors.Contains(display.EDID.DisplayName);

        private void ModuleWindow_Initialized(object sender, EventArgs e)
        {
            Display.InUse = true;
        }
        private void ModuleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Left = Display.Rect.left;
            Top = Display.Rect.top;
            WindowState = WindowState.Maximized;
        }
        private void ModuleWindow_Closed(object sender, EventArgs e)
        {
            Display.InUse = false;
        }

        private void ModuleWindowCaptionButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ModuleWindowCaptionButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
