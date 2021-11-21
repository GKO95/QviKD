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
        private readonly List<string> PatternList = new();

        public MainWindow() : base(new HashSet<string>()
        {
            // Insert list of EDID.DisplayName strings here...
            

        }) => DebugMessage(string.Format("Available by {0}: {1}", typeof(MainWindow).Namespace, Monitors.Count > 0 ? string.Join(", ", Monitors) : "All"));
        public MainWindow(Display display) : base(display)
        {
            InitializeComponent();

            // Enumerate pages included in the QvPattern assembly and only extract patterns that is of Page type.
            foreach (System.Reflection.TypeInfo type in GetType().Assembly.DefinedTypes)
            {
                if (type.BaseType == typeof(Page))
                {
                    PatternList.Add(type.Name);
                }
            }
        }

        private void ModuleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            QvPatternContent.Source = new Uri($"Patterns/{PatternList[PatternIndex]}.xaml", UriKind.Relative);
        }
        private void ModuleWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Right:
                    if (PatternIndex < PatternList.Count - 1) PatternIndex++;
                    else PatternIndex = 0;
                    e.Handled = true;
                    break;

                case Key.Left:
                    if (PatternIndex > 0) PatternIndex--;
                    else PatternIndex = PatternList.Count - 1;
                    e.Handled = true;
                    break;

                default:
                    return;
            }
            ModuleWindow_Loaded(sender, e);
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