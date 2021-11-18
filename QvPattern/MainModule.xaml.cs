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
    /// Interaction logic for MainModule.xaml
    /// </summary>
    public partial class MainModule : ModuleControl
    {
        public MainModule() : base() { }
        public MainModule(Display display) : base(display)
        {
            InitializeComponent();
        }

        /// <summary>
        /// A set of monitors the module is available; leave empty for every monitor.
        /// </summary>
        protected override HashSet<string> Monitors => new()
        {
            // Insert list of EDID.DisplayName strings here...
            
        };

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
    }
}