using GalaSoft.MvvmLight.Messaging;
using Iron.IntelligentDispsingMachine.Common;
using Iron.IntelligentDispsingMachine.View.Dialogs;
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

namespace Iron.IntelligentDispsingMachine.View.Windows
{
    /// <summary>
    /// MainView.xaml 的交互逻辑
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            Messenger.Default.Register<string>(this, "LedSetView", s =>
            {
                LedSetView LedSetView = new LedSetView();
                LedSetView.Owner = this;
                LedSetView.ShowDialog();
            });
            ActionManager.Register<object>("UpdateMedCount", new Func<object, bool>(ShowUpdateMedCountWin));
            ActionManager.Register<object>("PreCommand", new Func<object, bool>(ShowPreSelect));
        }
        private bool ShowUpdateMedCountWin(object obj)
        {
            var Dialog = new UpdateMedCountDialog() { Owner = this, DataContext = obj };
            return Dialog.ShowDialog() == true;
        }
        private bool ShowPreSelect(object? obj)
        {
            var Dialog = new PreSelectionDialog() { Owner = this };
            return Dialog.ShowDialog() == true;           
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if(this.WindowState ==WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
