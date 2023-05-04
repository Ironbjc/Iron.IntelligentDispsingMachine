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

namespace Iron.IntelligentDispsingMachine.Common
{
    public class MessageResult
    {
        /// <summary>
        /// 结果，Yes为true，No为false
        /// </summary>
        public bool IsYes { get; set; }
    }
    public class MessageBoxEventArgs : EventArgs
    {
        /// <summary>
        /// 结果，Yes为true，No为false
        /// </summary>
        public MessageResult Result { get; set; }
    }
    /// <summary>
    /// WPFMessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class WPFMessageBox : Window
    {
        public event EventHandler<MessageBoxEventArgs> Result;
        public string Context
        {
            get { return TB_Context.Text; }
            set { TB_Context.Text = value; }
        }
        public Visibility V1
        {
            get { return btnSure.Visibility; }
            set { btnSure.Visibility = value; }
        }
        public Visibility V2
        {
            get { return btnYes.Visibility; }
            set { btnYes.Visibility = value; }
        }
        public Visibility V3
        {
            get { return btnNo.Visibility; }
            set { btnNo.Visibility = value; }
        }
        bool _isLegal = false;
        public static void ShowDialog(string context,bool result)
        {
            MessageResult r = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                var mb = new WPFMessageBox();
                mb.Context = context;

                if(result)
                {
                    mb.V1 = Visibility.Visible;
                    mb.V2 = Visibility.Hidden;
                    mb.V3 = Visibility.Hidden;
                }
                mb.ShowDialog();

            });
          
        }
        public static void Show(string context, EventHandler<MessageBoxEventArgs> result)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var mb = new WPFMessageBox();
                mb.Context = context;
                mb.Result += result;
                mb.Show();
            });
           
        }
        public static MessageResult ShowDialog(string context)
        {
            MessageResult r = null;
            Application.Current.Dispatcher.Invoke(() =>
            {
                var mb = new WPFMessageBox();
                mb.Context = context;
               
                mb.Result += (s, e) =>
                {
                    r = e.Result;
                };
                mb.ShowDialog();
                
            });
            return r;
        }
        private void No_Button_Click(object sender, RoutedEventArgs e)
        {
            _isLegal = true;
            Close();
            Result?.Invoke(this, new MessageBoxEventArgs() { Result = new MessageResult() { IsYes = false } });
        }
        private void Yes_Button_Click(object sender, RoutedEventArgs e)
        {
            _isLegal = true;
            Close();
            Result?.Invoke(this, new MessageBoxEventArgs() { Result = new MessageResult() { IsYes = true } });
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !_isLegal;
        }
        public WPFMessageBox()
        {
            InitializeComponent();
        }

        private void btnSure_Click(object sender, RoutedEventArgs e)
        {
            _isLegal = true;
            Close();
            Result?.Invoke(this, new MessageBoxEventArgs() { Result = new MessageResult() { IsYes = true } });
        }
    }
}
