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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModuleWindow
    {
        public MainWindow() : base(new HashSet<string>()
        {
            // Insert list of EDID.DisplayName strings here...
            

        }) => DebugMessage(string.Format("Available by {0}: {1}", typeof(MainWindow).Namespace, Monitors.Count > 0 ? string.Join(", ", Monitors) : "All"));
        public MainWindow(Display display) : base(display)
        {
            InitializeComponent();    
        }

        /// <summary>
        /// Print message for debugging; DEBUG-mode exclusive.
        /// </summary>
        [System.Diagnostics.Conditional("DEBUG")]
        private void DebugMessage(string msg)
        {
            System.Diagnostics.Debug.WriteLine($"'{GetType().Name}.cs' {msg}");
        }

        private void ModuleWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void QvPatternContent_LoadCompleted(object sender, NavigationEventArgs e)
        {
            // Allow the page to access the MainWindow object via its arbitrary data, namely "Tag" property.
            if (e.Content is Page page)
                page.Tag = this;

            // Disable Go Back navigation by removing recent entry history every time the frame navigates.
            if (QvPatternContent.CanGoBack)
            {
                JournalEntry entry = QvPatternContent.RemoveBackEntry();
                while (entry != null)
                {
                    entry = QvPatternContent.RemoveBackEntry();
                }
            }

        }

        private void ModuleWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}