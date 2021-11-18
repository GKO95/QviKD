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

        protected readonly Display Display;

        public ModuleWindow() { }
        public ModuleWindow(Display display)
        {
            Display = display;
        }

        /// <summary>
        /// A set of monitors the module is available; leave empty for every monitor.
        /// </summary>
        protected HashSet<string> Monitors { get; }

        /// <summary>
        /// Identifies whether the module is available for the monitor.
        /// </summary>
        public bool IsAvailable(Display display) => Monitors.Count is 0 || Monitors.Contains(display.EDID.DisplayName);

    }
}
