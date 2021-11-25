using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
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
        public ModuleNotification Notification = null;

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
            Notification = new(GetTemplateChild("ModuleWindowPopup") as System.Windows.Controls.Primitives.Popup, this);
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

            Notification.VerticalOffset = 10;
            Notification.HorizontalOffset = 10;
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

    public class ModuleNotification
    {
        private readonly ModuleWindow Window;
        private readonly System.Windows.Controls.Primitives.Popup Popup;
        private readonly DispatcherTimer dispatcherTimer;

        internal ModuleNotification(System.Windows.Controls.Primitives.Popup popup, ModuleWindow wnd)
        {
            Window = wnd;
            Popup = popup;

            dispatcherTimer = new();
            dispatcherTimer.Tick += ModuleNotification_Tick;
        }

        /// <summary>
        /// Alert notification through a popup for a given TimeSpan duration.
        /// </summary>
        protected void Show(string message, TimeSpan timeSpan)
        {
            Window.Tag = message;

            Popup.PopupAnimation = System.Windows.Controls.Primitives.PopupAnimation.Slide;
            Popup.IsOpen = true;

            dispatcherTimer.Interval = timeSpan;
            dispatcherTimer.Start();
        }

        /// <summary>
        /// Alert notification through a popup for a given seconds duration.
        /// </summary>
        protected void Show(string message, int second) => Show(message, new TimeSpan(0, 0, second));

        /// <summary>
        /// Alert notification through a popup.
        /// </summary>
        public void Show(string message) => Show(message, 2);

        /// <summary>
        /// Hide notification.
        /// </summary>
        public void Hide()
        {
            Popup.PopupAnimation = System.Windows.Controls.Primitives.PopupAnimation.Fade;
            Popup.IsOpen = false;

            dispatcherTimer.Stop();
        }

        private void ModuleNotification_Tick(object sender, EventArgs e) => Hide();

        /// <summary>
        /// Vertical offset positioning from the bottom respect to the bottom-right corner.
        /// </summary>
        public double VerticalOffset { get => Popup.VerticalOffset; set => Popup.VerticalOffset -= value; }

        /// <summary>
        /// Horizontal offset positioning from the right respect to the bottom-right corner.
        /// </summary>
        public double HorizontalOffset { get => Popup.HorizontalOffset; set => Popup.HorizontalOffset -= value; }
    }
}
