using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Iron.IntelligentDispsingMachine.Common;
using Iron.IntelligentDispsingMachine.Common.Helper;
using Iron.IntelligentDispsingMachine.DataAccess;
using Iron.IntelligentDispsingMachine.Models;
using log4net.Config;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static Iron.IntelligentDispsingMachine.CPLC.DeltaPLC;
using System.Windows.Input;
using System.Threading;

namespace Iron.IntelligentDispsingMachine.ViewModel
{
    public class MainViewModel:ViewModelBase
    {
       // public static DrugInViewModel drugInViewModel { get; set; }
        private object _viewContent;

        public object ViewContent
        {
            get { return _viewContent; }
            set { Set(ref _viewContent, value); }
        }
        private Visibility _InitBtmVis=Visibility.Hidden;

        public Visibility InitBtmVis
        {
            get { return _InitBtmVis; }
            set { 
                Set(ref _InitBtmVis, value);
            }
        }

        private Visibility _ProgressVis = Visibility.Hidden;

        public Visibility ProgressVis
        {
            get { return _ProgressVis ; }
            set { Set(ref _ProgressVis, value); }
        }

        private bool _InitBtnEnable=true;

        public bool InitBtnEnable
        {
            get { return _InitBtnEnable; }
            set { Set(ref _InitBtnEnable, value); }
        }

        public RelayCommand<object> SwitchPageCommand { get; set; }
        public MainViewModel()
        {
            try
            {
                #region 加载配置文件
                LoadXML();
                #endregion
                #region 加载串口

                #endregion
                #region 加载页面按钮
                Menus = new List<MenuModel>();
                //Menus.Add(new MenuModel()
                //{
                //   // IsSelected = false,
                //    MenuHeader = "报表",
                //    //&#xe62d;
                //    MenuIcon = "\xe62d",
                //    TargetView = "ReportView"
                //});
                Menus.Add(new MenuModel()
                {
                    MenuHeader = "出药",
                    //&#xe601;
                    MenuIcon = "\xe601",
                    TargetView = "DrugOutView"
                });
                Menus.Add(new MenuModel()
                {
                    MenuHeader = "上药",
                    //&#xe606;
                    MenuIcon = "\xe606",
                    TargetView = "DrugInView"
                });
                Menus.Add(new MenuModel()
                {
                    MenuHeader = "盘点",
                    MenuIcon = "\xec4c",
                    TargetView = "DrugCheckView"
                });
                Menus.Add(new MenuModel()
                {

                    MenuHeader = "维护",
                    //&#xe67f;
                    MenuIcon = "\xe67f",
                    TargetView = "DrugMaintainView"
                });
                Menus.Add(new MenuModel()
                {
                    MenuHeader = "调试",
                    //&#xe650;
                    MenuIcon = "\xe650",
                    TargetView = "SystemDebug"
                });
                #endregion

               
                #region 初始化变量
                GlobalValue.LocalDataAccess = new LocalDataAccess(GlobalValue.SqlConn);
                GlobalValue.DeltaPLC = new CPLC.DeltaPLC(GlobalValue.ServoPortCom);
                ConnectPLC();
                GlobalValue.LedSerialPort = new SerialPort(GlobalValue.DPortCom, 19200, Parity.None, 8, StopBits.One);
                GlobalValue.ScanPort = new SerialPort(GlobalValue.ScanCom, 9600, Parity.None, 8, StopBits.One);
                GlobalValue.ScanPort.DataReceived += ScanPort_DataReceived;
                GlobalValue.ScanPort.Open();
                // GlobalValue.LedSerialPort.DataReceived += LedSerialPort_DataReceived;
                GlobalValue.LedSerialPort.Open();
               // ConnectPLC();
              
                //if (!GlobalValue.DeltaPLC.ConnectPLC())
                //{
                //    MessageBox.Show("PLC连接失败，请检查配置");
                //    return;
                //}
                //else
                //{
                //    //如果没有初始化完成
                //    if (GlobalValue.DeltaPLC.ServoInitFinish(ServoType.LeftServo) != 2 || GlobalValue.DeltaPLC.ServoInitFinish(ServoType.RightServo) != 2)
                //    {
                //        MessageBox.Show("PLC未初始化完成");
                //        InitBtmVis = Visibility.Visible;
                //    }
                //    else
                //    {
                //        SwitchPageCommand = new RelayCommand<object>(ShowPage);
                //        InitBtmVis = Visibility.Hidden;
                //    }
                //}

                #endregion

            }
            catch(Exception ex)
            {
                GlobalValue.Loger.Error(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        private void ConnectPLC()
        {
            if (!GlobalValue.DeltaPLC.ConnectPLC())
            {
                InitBtmVis = Visibility.Visible;
                #region 测试
                {
                    SwitchPageCommand = new RelayCommand<object>(ShowPage);
                    InitBtmVis = Visibility.Hidden;
                }
                #endregion
                throw new Exception("PLC连接失败，请检查配置");
            }
            else
            {
                //如果没有初始化完成
                if (GlobalValue.DeltaPLC.ServoInitFinish(ServoType.LeftServo) != 2 || GlobalValue.DeltaPLC.ServoInitFinish(ServoType.RightServo) != 2)
                {
                    InitBtmVis = Visibility.Visible;
                    throw new Exception("PLC未初始化完成");                    
                }
                else
                {
                    SwitchPageCommand = new RelayCommand<object>(ShowPage);
                    InitBtmVis = Visibility.Hidden;
                    //初始化速度
                    GlobalValue.DeltaPLC.ServoBackZeroSpeed(Convert.ToInt32(GlobalValue.MotorZeroSpeed));
                    GlobalValue.DeltaPLC.ServoAutoSpeed(Convert.ToInt32(GlobalValue.MotorAutoSpeed));
                    GlobalValue.DeltaPLC.ServoJogSpeed(Convert.ToInt32(GlobalValue.MotorHandSpeed));
                }
            }
         
        }
        private void ScanPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] ReceiveBuffer = new byte[GlobalValue.ScanPort.BytesToRead];
            GlobalValue.ScanPort.Read(ReceiveBuffer, 0, GlobalValue.ScanPort.BytesToRead);
            string str = System.Text.Encoding.ASCII.GetString(ReceiveBuffer);
            //if (drugInViewModel!=null)
            //{
            //    drugInViewModel.BarCode = str;
            //}
            var s= str.Replace("\'", "").Trim();
            Messenger.Default.Send<string>(s,"ScanPortMessage");
        }

        private void LedSerialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            byte[] bytes = new byte[GlobalValue.LedSerialPort.BytesToWrite];
            GlobalValue.LedSerialPort.Read(bytes, 0, bytes.Length);
        }

        private void ShowPage(object obj)
        {
            var model = obj as MenuModel;
            if (model != null)
            {
                if (ViewContent != null && ViewContent.GetType().Name == model.TargetView)
                    return;
                Type type = Assembly.Load("Iron.IntelligentDispsingMachine.View").GetType("Iron.IntelligentDispsingMachine.View.Pages." + model.TargetView)!;
                ViewContent = Activator.CreateInstance(type)!;
                ///这里是为了在ViewModel中拿到View用于动画处理，后面考虑在View的Load中将View传递到ViewModel中
                if(ViewContent != null)
                {
                    if(!GlobalValue.WindDic.ContainsKey(model.TargetView))
                    {
                        GlobalValue.WindDic.Add(model.TargetView, ViewContent);
                    }
                    else
                    {
                        GlobalValue.WindDic.Remove(model.TargetView);
                        GlobalValue.WindDic.Add(model.TargetView, ViewContent);
                    }
                }
            }
        }
        public ICommand InitSysCommand
        {
            get => new RelayCommand<object>(arg =>
            {
                InitBtnEnable = false;
                Task.Run(() =>
                {
                    if (GlobalValue.DeltaPLC != null)
                    {
                        GlobalValue.DeltaPLC.ServoInit(ServoType.LeftServo);
                        GlobalValue.DeltaPLC.ServoInit(ServoType.RightServo);
                        int count = 0;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ProgressVis = Visibility.Visible;
                        });
                        do
                        {

                            // ProgressVis = Visibility.Visible;
                            int val_left = GlobalValue.DeltaPLC.ServoInitFinish(ServoType.LeftServo);
                            int val_right = GlobalValue.DeltaPLC.ServoInitFinish(ServoType.RightServo);
                            if (val_left == 2 && val_right == 2)
                            {
                                SwitchPageCommand = new RelayCommand<object>(ShowPage);
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    ProgressVis = Visibility.Hidden;
                                    InitBtnEnable = true;
                                });
                                InitBtmVis = Visibility.Hidden;
                             
                                break;
                            }
                            else if (val_left == 3 || val_right == 3)
                            {
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    ProgressVis = Visibility.Hidden;
                                });
                                InitBtnEnable = true;
                                MessageBox.Show("初始化回原点异常", "系统提示");
                                return;
                            }
                            if (count > 150)
                            {
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    ProgressVis = Visibility.Hidden;
                                });
                                InitBtnEnable = true;
                                MessageBox.Show("初始化回原点检测完成超时", "系统提示");
                                return;
                            }
                            else
                            {
                                count++;
                                Thread.Sleep(200);
                            }
                        } while (true);

                    }
                });
               
            });
        }
        public List<MenuModel> Menus { get; set; }
        private void LoadXML()
        {
            XmlConfig config=LoaclXMLHelper.GetConfig();
            GlobalValue.SqlConn= String.Format($"initial catalog={config.DBName};user id={config.USERID};password={config.PWD};data source={config.DBIP};pooling=false");
            GlobalValue.MachineID = config.MachineID;
            GlobalValue.ConfirmCode = config.ConfirmCode;
            GlobalValue.ServoPortCom = config.ServoPortCom;
            GlobalValue.DPortCom = config.DPortCom;
            GlobalValue.ArkLayer = config.ArkLayer;
            GlobalValue.MedInFlag = config.MedINFlag;
            GlobalValue.MotorAutoSpeed = config.MotorAutoSpeed;
            GlobalValue.MotorHandSpeed = config.MotorHandSpeed;
            GlobalValue.MotorZeroSpeed = config.MotorZeroSpeed;
            GlobalValue.MedOutMode = config.MedOutMode;
            GlobalValue.ScanCom = config.ScanCom;
        }
    }
}
