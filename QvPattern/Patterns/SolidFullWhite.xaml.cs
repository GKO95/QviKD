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

namespace QviKD.Modules.QvPattern.Patterns
{
    /// <summary>
    /// Interaction logic for SolidFullWhite.xaml
    /// </summary>
    public partial class SolidFullWhite : Page
    {
        internal byte ucIndex = 255;
        public SolidFullWhite()
        {
            InitializeComponent();
            SolidFullWhiteContent.Background = new SolidColorBrush(Color.FromRgb(ucIndex, ucIndex, ucIndex));
        }
    }
}
