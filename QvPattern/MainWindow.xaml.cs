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
        /// <summary>
        /// Index for the list of patterns.
        /// </summary>
        private int PatternIndex = 0;

        /// <summary>
        /// List of patterns available in the module.
        /// </summary>
        private readonly List<string> PatternList = new()
        {
            "GradientWhite256",
            "GradientRGBW256",
        };

        public MainWindow() : base(new HashSet<string>()
        {
            // Insert list of EDID.DisplayName strings here...
            

        }) => DebugMessage(string.Format("Available by {0}: {1}", typeof(MainWindow).Namespace, Monitors.Count > 0 ? string.Join(", ", Monitors) : "All"));
        public MainWindow(Display display) : base(display)
        {
            InitializeComponent();
        }

        private void ModuleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            QvPatternContent.Source = new Uri($"Patterns/{PatternList[PatternIndex]}.xaml", UriKind.Relative);
        }
        private void ModuleWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                if (++PatternIndex > PatternList.Count - 1)
                    PatternIndex = 0;
            }
            else if (e.Key == Key.Left)
            {
                if (--PatternIndex < 0)
                    PatternIndex = PatternList.Count - 1;
            }
            QvPatternContent.Source = new Uri($"Patterns/{PatternList[PatternIndex]}.xaml", UriKind.Relative);
        }

        private void QvPatternContent_LoadCompleted(object sender, NavigationEventArgs e)
        {
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

    internal enum DEPTH : uint
    {
        BIT8  = 8,
        BIT10 = 10,
        BIT12 = 12,
        BIT14 = 14,
    }
}