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
    /// DrugOutView.xaml 的交互逻辑
    /// </summary>
    public partial class DrugOutView : UserControl
    {


        //public int MyProperty
        //{
        //    get { return (int)GetValue(MyPropertyProperty); }
        //    set { SetValue(MyPropertyProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty MyPropertyProperty =
        //    DependencyProperty.Register("MyProperty", typeof(int), typeof(DrugOutView), new PropertyMetadata(0));


        public DrugOutView()
        {
            InitializeComponent();
            this.Unloaded += DrugOutView_Unloaded;
        }

        private void DrugOutView_Unloaded(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.Cleanup<DrugOutViewModel>();
        }

       
    }
}
