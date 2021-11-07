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

namespace QviKD.Modules.QvPattern
{
    /// <summary>
    /// Interaction logic for MainModule.xaml
    /// </summary>
    public partial class MainModule : UserControl, IModuleWindow
    {
        public MainModule()
        {
            InitializeComponent();
        }

        private static HashSet<string> Monitors { get; set; } = new();
        public static bool IsAvailable(Types.EDID monitor)
        {
            Monitors.UnionWith(new HashSet<string>
            {
                "C27F390"
            });
            return Monitors.Count is 0 || Monitors.Contains(monitor.DisplayName);
        }
    }
}