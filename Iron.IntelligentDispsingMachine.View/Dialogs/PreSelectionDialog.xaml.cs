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
using System.Windows.Shapes;

namespace Iron.IntelligentDispsingMachine.View.Dialogs
{
    /// <summary>
    /// PreSelectionDialog.xaml 的交互逻辑
    /// </summary>
    public partial class PreSelectionDialog : Window
    {
        public PreSelectionDialog()
        {
            InitializeComponent();
            this.DataContext = new PreSelectionDialogViewModel();
        }
    }
}
