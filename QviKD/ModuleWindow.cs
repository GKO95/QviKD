using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
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

        /// <summary>
        /// A display that the module is going to handle.
        /// </summary>
        protected readonly Display Display;

        /// <summary>
        /// Popup control from ModuleWindow.
        /// </summary>
        public Controls.Notification Notification = null;

        /// <summary>
        /// Constructor for identifying the module availability.
        /// </summary>
        public ModuleWindow(HashSet<string> monitors) => Monitors = monitors;

        /// <summary>
        /// Constructor for processing on-display module task.
        /// </summary>
        public ModuleWindow(Display display)
        {
            Display = display;

            Initialized += ModuleWindow_Initialized;
            Loaded += ModuleWindow_Loaded;
            Closed += ModuleWindow_Closed;
        }

        public override void OnApplyTemplate()
        {
            // The method is automatically called once the template is applied from Generic.xaml, assigning events to controls.
            if (GetTemplateChild("ModuleWindowCaptionButtonClose") is Button btnClose) btnClose.Click += ModuleWindowCaptionButtonClose_Click;
            if (GetTemplateChild("ModuleWindowCaptionButtonMinimize") is Button btnMinimize) btnMinimize.Click += ModuleWindowCaptionButtonMinimize_Click;

            // Assign the Popup control to the field via element name within the template.
            Notification = GetTemplateChild("ModuleWindowNotification") as Controls.Notification;
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
            if (Notification is not null)
            {
                Notification.Hide();
            }
        }

        private void ModuleWindowCaptionButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ModuleWindowCaptionButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Print message for debugging; DEBUG-mode exclusive.
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        protected void DebugMessage(string msg)
        {
            System.Diagnostics.Debug.WriteLine($"'{GetType().Name}.cs' {msg}");
        }
    }
}
