using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Iron.IntelligentDispsingMachine.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using static Iron.IntelligentDispsingMachine.CPLC.DeltaPLC;

namespace Iron.IntelligentDispsingMachine.ViewModel
{
    public class SystemDebugViewModel : ViewModelBase
    {
        public override void Cleanup()
        {
            base.Cleanup();
            cts.Cancel();
        }
       
        Task taskLeftMontior; //左电机监控线程
        Task taskRightMontior;//右电机监控线程
        public SystemDebugViewModel()
        {
            taskLeftMontior = new Task(() =>
            {
                while (!cts.IsCancellationRequested)
                {
                    if (isLeftBacktoZero)
                    {
                        Task.Delay(500).Wait();
                        int val = GlobalValue.DeltaPLC.ServoInitFinish(ServoType.LeftServo);
                        if (val == 2)
                        {
                            this.LeftMotoMessage = "左侧电机回原点完成";
                            isLeftBacktoZero = false;
                        }
                        else if (val == 3)
                        {
                           this.LeftMotoMessage = "左侧电机回原点异常";
                            isLeftBacktoZero = false;
                        }
                        else if (val == 1)
                        {
                            this.LeftMotoMessage = "左侧电机正在回原点";
                        }
                    }
                }
            }, cts.Token);
            taskRightMontior = new Task(() =>
            {
                while (!cts.IsCancellationRequested)
                {
                    if (isRightBacktoZero)
                    {
                        Task.Delay(500).Wait();
                        int val = GlobalValue.DeltaPLC.ServoInitFinish(ServoType.RightServo);
                        if (val == 2)
                        {
                            this.LeftMotoMessage = "左侧电机回原点完成";
                            isRightBacktoZero = false;
                        }
                        else if (val == 3)
                        {
                            this.LeftMotoMessage = "左侧电机回原点异常";
                            isRightBacktoZero = false;
                        }
                        else if (val == 1)
                        {
                            this.LeftMotoMessage = "左侧电机正在回原点";
                        }
                    }
                }
            }, cts.Token);
            taskLeftMontior.Start();
            taskRightMontior.Start();
        }
        CancellationTokenSource cts = new CancellationTokenSource();
        bool isLeftBacktoZero = false;
        bool isRightBacktoZero = false;
        #region Command     
        public RelayCommand<object> LeftMotoBacktoZero
        {
            get => new RelayCommand<object>(arg =>
            {
                GlobalValue.DeltaPLC.ServoInit(ServoType.LeftServo);
                isLeftBacktoZero = true;
            });
        }
        /// <summary>
        /// 左键长按下功能
        /// </summary>
        public RelayCommand<object> KeeptheLeftButtonDown
        {
            get => new RelayCommand<object>(arg =>
            {
                string mode = arg.ToString();
                switch (mode)
                {
                    case "1":
                        GlobalValue.DeltaPLC.ServoJog(ServoType.LeftServo, JogType.Up);
                        break;
                    case "2":
                        GlobalValue.DeltaPLC.ServoJog(ServoType.LeftServo, JogType.Down);
                        break;
                    case "3":
                        GlobalValue.DeltaPLC.ServoJog(ServoType.RightServo, JogType.Up);
                        break;
                    case "4":
                        GlobalValue.DeltaPLC.ServoJog(ServoType.RightServo, JogType.Down);
                        break;
                    default:
                        break;
                }
            });
        }
        /// <summary>
        /// 左键松开功能
        /// </summary>
        public RelayCommand<object> LeftButtonUp
        {
            get => new RelayCommand<object>(arg =>
            {
                string mode = arg.ToString();
                switch (mode)
                {
                    case "1":
                        GlobalValue.DeltaPLC.ServoJog(ServoType.LeftServo, JogType.Stop);
                        break;
                    case "2":
                        GlobalValue.DeltaPLC.ServoJog(ServoType.LeftServo, JogType.Stop);
                        break;
                    case "3":
                        GlobalValue.DeltaPLC.ServoJog(ServoType.RightServo, JogType.Stop);
                        break;
                    case "4":
                        GlobalValue.DeltaPLC.ServoJog(ServoType.RightServo, JogType.Stop);
                        break;
                    default:
                        break;
                }
            });
        }
        /// <summary>
        /// 保存左电机层数的脉冲位置
        /// </summary>
        public RelayCommand<object> SaveLeftLocation
        {
            get => new RelayCommand<object>(arg =>
            {
                try
                {
                    if (this.CurrentLeftSelectLocation == -1)
                    {
                        MessageBox.Show("请选择左电机当前位置的层数");
                    }
                    GlobalValue.DeltaPLC.ServoPositionSet(ServoType.LeftServo, CurrentLeftSelectLocation + 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            });
        }
        /// <summary>
        /// 保存右电机层数的脉冲位置
        /// </summary>
        public RelayCommand<object> SaveRightLocation
        {
            get => new RelayCommand<object>(arg =>
            {
                try
                {
                    if (this.CurrentRightSelectLocation == -1)
                    {
                        MessageBox.Show("请选择左电机当前位置的层数");
                    }
                    GlobalValue.DeltaPLC.ServoPositionSet(ServoType.RightServo, CurrentRightSelectLocation + 1);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            });
        }

        /// <summary>
        /// 电机运行到指定位置
        /// </summary>
        public RelayCommand<object> MotoRuntoSpecified
        {
            get => new RelayCommand<object>(arg =>
            {
                try
                {
                    string result = arg.ToString();
                    var moto = result.Split(',')[0] == "1" ? ServoType.LeftServo : ServoType.RightServo;
                    var loc = int.Parse(result.Split(',')[1]);
                    GlobalValue.DeltaPLC.ServoPositionRun(moto, loc);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }
        public RelayCommand<object> GetCurrentAlarm
        {
            get => new RelayCommand<object>(arg =>
            {
                CurrentAlarm = "";
                int val = GlobalValue.DeltaPLC.GetServoAlarm();
                string alarmcode = Convert.ToString(val, 2).PadLeft(8, '0');
                if (alarmcode.Substring(7, 1) == "1")
                {
                    CurrentAlarm = "光幕报警";
                }
                else if (alarmcode.Substring(6, 1) == "1")
                {
                    CurrentAlarm = "急停报警";
                }
                else if (alarmcode.Substring(5, 1) == "1")
                {
                    CurrentAlarm = "左侧上极限报警";
                }
                else if (alarmcode.Substring(4, 1) == "1")
                {
                    CurrentAlarm = "左侧下极限报警";
                }
                else if (alarmcode.Substring(3, 1) == "1")
                {
                    CurrentAlarm = "右侧上极限报警";
                }
                else if (alarmcode.Substring(2, 1) == "1")
                {
                    CurrentAlarm = "右侧下极限报警";
                }
                else if (alarmcode.Substring(1, 1) == "1")
                {
                    CurrentAlarm = "左侧驱动器报警";
                }
                else if (alarmcode.Substring(0, 1) == "1")
                {
                    CurrentAlarm = "右侧驱动器报警";
                }
                else
                {
                    CurrentAlarm = "正常";
                }
            });
        }

        public RelayCommand<object> ClearAlarm
        {
            get => new RelayCommand<object>(arg =>
            {
                GlobalValue.DeltaPLC.ServoAlarmReset();
            });
        }
        public RelayCommand<object> OpenLedSetView
        {
            get => new RelayCommand<object>(arg =>
            {
                Messenger.Default.Send("LedSetView", "LedSetView");
            });
        }
        #endregion

        private int _CurrentLeftSelectLocation = -1;

        public int CurrentLeftSelectLocation
        {
            get { return _CurrentLeftSelectLocation; }
            set { Set(ref _CurrentLeftSelectLocation, value); }
        }
        private int _CurrentRightSelectLocation = -1;

        public int CurrentRightSelectLocation
        {
            get { return _CurrentRightSelectLocation; }
            set { Set(ref _CurrentRightSelectLocation, value); }
        }


        private string _CurrentAlarm;

        public string CurrentAlarm
        {
            get { return _CurrentAlarm; }
            set { Set(ref _CurrentAlarm, value); }
        }
        private string _LeftMotoMessage="左电机回原点";

        public string LeftMotoMessage
        {
            get { return _LeftMotoMessage; }
            set { Set(ref _LeftMotoMessage, value); }
        }
        private string _RightMotoMessage = "左电机回原点";

        public string RightMotoMessage 
        {
            get { return _RightMotoMessage; }
            set { Set(ref _RightMotoMessage, value); }
        }
    }
}
