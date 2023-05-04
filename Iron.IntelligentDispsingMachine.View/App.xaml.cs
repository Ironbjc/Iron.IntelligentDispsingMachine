using Iron.IntelligentDispsingMachine.Common;
using Iron.IntelligentDispsingMachine.View.Pages;
using Iron.IntelligentDispsingMachine.ViewModel;
using log4net.Config;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Iron.IntelligentDispsingMachine.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ///检查程序是否已经启动了
            System.Diagnostics.Process thisProc = System.Diagnostics.Process.GetCurrentProcess();
            if (System.Diagnostics.Process.GetProcessesByName(thisProc.ProcessName).Length > 1)
            {
                MessageBox.Show("程序已经启动了");
                Application.Current.Shutdown();
                return;
            }
            ///初始化Log4Net日志
            XmlConfigurator.Configure(new FileInfo(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config\\log4Net.config")));
            GlobalValue.Loger = LogManager.GetLogger(typeof(App));
            base.OnStartup(e);          
        }
    }
}
