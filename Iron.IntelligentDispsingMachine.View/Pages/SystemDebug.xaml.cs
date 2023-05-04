using Iron.IntelligentDispsingMachine.ViewModel;
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

namespace Iron.IntelligentDispsingMachine.View.Pages
{
    /// <summary>
    /// SystemDebug.xaml 的交互逻辑
    /// </summary>
    public partial class SystemDebug : UserControl
    {
        public SystemDebug()
        {
            InitializeComponent();
            this.Unloaded += SystemDebug_Unloaded;
        }

        private void SystemDebug_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.Cleanup<SystemDebugViewModel>();
        }

        
    }
}
