﻿using System;
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
        }

        /// <summary>
        /// Identifies whether the module is available for the monitor.
        /// </summary>
        public bool IsAvailable(Display display) => Monitors.Count is 0 || Monitors.Contains(display.EDID.DisplayName);

        /// <summary>
        /// Alert notification through a popup.
        /// </summary>
        public void Notification(string message) => Tag = message;

        private void ModuleWindow_Initialized(object sender, EventArgs e)
        {
            Display.InUse = true;
        }
        private void ModuleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Left = Display.Rect.left;
            Top = Display.Rect.top;
            WindowState = WindowState.Maximized;

            ((System.Windows.Controls.Primitives.Popup)GetTemplateChild("ModuleWindowPopup")).VerticalOffset -= 10;
            ((System.Windows.Controls.Primitives.Popup)GetTemplateChild("ModuleWindowPopup")).HorizontalOffset -= 10;
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

        /// <summary>
        /// Print message for debugging; DEBUG-mode exclusive.
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        protected void DebugMessage(string msg)
        {
            System.Diagnostics.Debug.WriteLine($"'{GetType().Name}.cs' {msg}");
        }
    }

    /// <summary>
    /// Converter for binding data between <i>Display.InUse</i> and <i>Button.IsEnable</i> property.
    /// </summary>
    [ValueConversion(typeof(string), typeof(bool))]
    public class IsOpenPropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException($"The {targetType} of the binding target is incompatible with Boolean data type.");

            return (string)value != string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
