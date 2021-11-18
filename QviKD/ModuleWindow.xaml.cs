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
using System.Windows.Shapes;
using QviKD.Types;

namespace QviKD
{
    /// <summary>
    /// Interaction logic for ModuleWindow.xaml
    /// </summary>
    public partial class ModuleWindow : Window
    {
        private readonly Display Display;
        private readonly object ContentControl;

        public ModuleWindow(Display display, Module module)
        {
            Display = display;
            ContentControl = Activator.CreateInstance(module.Type, display) as Modules.ModuleControl;

            InitializeComponent();
        }

        private void ModuleWindow_Initialized(object sender, EventArgs e)
        {
            ModuleWindowContent.Content = ContentControl;
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

    namespace Modules
    {
        /// <summary>
        /// An abstract class for MainModule.xaml use for creating a module control.
        /// </summary>
        public abstract class ModuleControl : UserControl
        {
            protected ModuleControl() { }
            protected ModuleControl(Display display) => Display = display;

            protected Display Display { get; }

            /// <summary>
            /// A set of monitors the module is available; leave empty for every monitor.
            /// </summary>
            protected abstract HashSet<string> Monitors { get; }
            
            /// <summary>
            /// Identifies whether the module is available for the monitor.
            /// </summary>
            public bool IsAvailable(Display display) => Monitors.Count is 0 || Monitors.Contains(display.EDID.DisplayName);

            /// <summary>
            /// Close the module window.
            /// </summary>
            protected void Close() => Window.GetWindow(this).Close();
        }
    }
}
