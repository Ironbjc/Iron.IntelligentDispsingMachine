using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Iron.IntelligentDispsingMachine.Common;
using Iron.IntelligentDispsingMachine.Entities;
using Iron.IntelligentDispsingMachine.Models;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Threading;
using static Iron.IntelligentDispsingMachine.CPLC.DeltaPLC;

namespace Iron.IntelligentDispsingMachine.ViewModel
{
    public class DrugOutViewModel : ViewModelBase
    {
        private string _RunFailedMessage;

        public string RunFailedMessage
        {
            get { return _RunFailedMessage; }
            set { Set(ref _RunFailedMessage, value); }
        }


        #region 按钮使能
        /// <summary>
        /// 确认出药和选择处方的按钮使能
        /// </summary>
        private bool _BtnEnable = true;

        public bool BtnEnable
        {
            get { return _BtnEnable; }
            set { Set(ref _BtnEnable, value); }
        }
       /// <summary>
       /// 继续运行按钮的使能
       /// </summary>

        private bool _ContinueRunBtnEnable = true;

        public bool ContinueRunBtnEnable
        {
            get { return _ContinueRunBtnEnable; }
            set { Set(ref _ContinueRunBtnEnable, value); }
        }

        #endregion

        public override void Cleanup()
        {
            if (GlobalValue.MedOutMode == "0")
            {
                Messenger.Default.Unregister<string>(this, "ScanPortMessage");
                cts.Cancel();
            }
            else
            {
                cts.Cancel();
                // RightDispatcherTimer.Stop();
                // LeftDispatcherTimer.Stop();
            }
            //DoLeftOutMedTask.Wait();
            //DoRightOutMedTask.Wait();
            base.Cleanup();
            //this.CurrentLeftSelectPresN = null;
            //this.CurrentRightSelectPresN = null;
            //RightDispatcherTimer = null;
            //LeftDispatcherTimer = null;
        }
        public List<CabinetModel> cabinetModels { get; set; } = new List<CabinetModel>();
        #region 左侧和右侧的处方列表
        private ObservableCollection<PreNoModel> _PreNoListLeft = new ObservableCollection<PreNoModel>();

        public ObservableCollection<PreNoModel> PreNoListLeft
        {
            get { return _PreNoListLeft; }
            set { Set(ref _PreNoListLeft, value); }
        }

        private ObservableCollection<PreNoModel> _PreNoListRight = new ObservableCollection<PreNoModel>();

        public ObservableCollection<PreNoModel> PreNoListRight
        {
            get { return _PreNoListRight; }
            set { Set(ref _PreNoListRight, value); }
        }
        #endregion


       // private static readonly object Lock_Task = new object();
        private string GetSign(int i, int j)
        {
            string result = string.Empty;
            switch (i)
            {
                case 1:
                    if (j >= 1 && j <= 9)
                        result = "Left-1";
                    else if (j >= 10 && j <= 18)
                        result = "Left-2";
                    else if (j >= 19 && j <= 27)
                        result = "Left-3";
                    break;
                case 2:
                    if (j >= 1 && j <= 9)
                        result = "Left-3";
                    else if (j >= 10 && j <= 18)
                        result = "Left-2";
                    else if (j >= 19 && j <= 27)
                        result = "Left-1";
                    break;
                case 3:
                    if (j >= 1 && j <= 9)
                        result = "Right-3";
                    else if (j >= 10 && j <= 18)
                        result = "Right-2";
                    else if (j >= 19 && j <= 27)
                        result = "Right-1";
                    break;
                case 4:
                    if (j >= 1 && j <= 9)
                        result = "Right-1";
                    else if (j >= 10 && j <= 18)
                        result = "Right-2";
                    else if (j >= 19 && j <= 27)
                        result = "Right-3";
                    break;
            }
            return result;
        }
        // public ObservableCollection<PreNoModel> PreNoListLeft { get; set; } = new ObservableCollection<PreNoModel>();
        // public ObservableCollection<PreNoModel> PreNoListRight { get; set; } = new ObservableCollection<PreNoModel>();
        public DrugOutViewModel()
        {
            try
            {
                Messenger.Default.Register<PreNoModel>(this, "PreSend", DoPreSend);
                #region 初始化cabinetModels
                InitCabinetModels();
                #endregion
                for (int i = 1; i < 5; i++)
                {
                    List<string> S1 = new List<string>();
                    MedPositonListModel model1 = new MedPositonListModel();
                    for (int j = 1; j < 10; j++)
                    {
                        //  S1 = new List<string>();
                        S1.Add(GlobalValue.MachineID + i.ToString("00") + j.ToString("00"));
                        if (string.IsNullOrEmpty(model1.MedPosSign))
                        {
                            model1.MedPosSign = this.GetSign(i, j);
                        }
                    }
                    model1.MedPosGroup = S1.ToArray();
                    this.MedPositonGroup.Add(model1);
                    List<string> S2 = new List<string>();
                    MedPositonListModel model2 = new MedPositonListModel();
                    for (int j = 10; j < 19; j++)
                    {
                        S2.Add(GlobalValue.MachineID + i.ToString("00") + j.ToString("00"));
                        if (string.IsNullOrEmpty(model2.MedPosSign))
                        {
                            model2.MedPosSign = this.GetSign(i, j);
                        }
                    }
                    model2.MedPosGroup = S2.ToArray();

                    this.MedPositonGroup.Add(model2);
                    List<string> S3 = new List<string>();
                    MedPositonListModel model3 = new MedPositonListModel();
                    for (int j = 19; j < 28; j++)
                    {
                        S3.Add(GlobalValue.MachineID + i.ToString("00") + j.ToString("00"));
                        if (string.IsNullOrEmpty(model3.MedPosSign))
                        {
                            model3.MedPosSign = this.GetSign(i, j);
                        }

                    }
                    model3.MedPosGroup = S3.ToArray();

                    this.MedPositonGroup.Add(model3);
                    //for(int j=1;j<9;j++)
                    //{

                    //    List<string> S1 = new List<string>();
                    //    S1.Add(GlobalValue.MachineID + i.ToString("00") + j.ToString("00"));
                    //    this.MedPositonGroup.Add(S1.ToArray());
                    //}
                }


                #region 支持自动出药和扫码出药

                Messenger.Default.Register<string>(this, "ScanPortMessage", DoMedOutByPreNo);
                UpdateDataSource();

                DoLeftOutMedTask = Task.Run(() =>
                {
                    while (!cts.IsCancellationRequested)
                    {
                        Task.Delay(500).Wait();
                        if (PreNoListLeft.Count() > 0 && CanDoTaskLeft)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                this.CurrentLeftSelectPresN = PreNoListLeft[0];
                            });
                            
                            CanDoTaskLeft = false;
                            CanUpdateLeft = false;
                            DoMedOut(this.CurrentLeftSelectPresN.MedPos);
                        }
                    }
                }, cts.Token);
                DoRightOutMedTask = Task.Run(() =>
                {
                    while (!cts.IsCancellationRequested)
                    {
                        Task.Delay(500).Wait();
                        if (PreNoListLeft.Count() == 0 && PreNoListRight.Count() > 0 && CanDoTaskRight)
                        {
                            Task.Delay(500).Wait();//延迟半秒钟，不能在左结束后立马执行
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                this.CurrentRightSelectPresN = PreNoListRight[0];
                                //CanDoTaskRight = false;
                                //CanUpdateRight = false;
                                //DoMedOut(this.CurrentRightSelectPresN.MedPos);
                                //RightDispatcherTimer.IsEnabled = true;

                            });
                            CanDoTaskRight = false;
                            CanUpdateRight = false;
                            DoMedOut(this.CurrentRightSelectPresN.MedPos);
                            // RightDispatcherTimer.IsEnabled = true;
                        }

                    }
                }, cts.Token);
                CanDoTaskLeft = true;
                CanDoTaskRight = true;

                #endregion
                #region 根据配置文件只支持一种模式
                //if (GlobalValue.MedOutMode == "0")
                //{
                //    Messenger.Default.Register<string>(this, "ScanPortMessage", DoMedOutByPreNo);
                //    CanDoTaskLeft = true;
                //    CanDoTaskRight = true;
                //}
                //else
                //{
                //    UpdateDataSource();

                //    DoLeftOutMedTask = Task.Run(() =>
                //    {
                //        while (!cts.IsCancellationRequested)
                //        {
                //            Task.Delay(500).Wait();
                //            if (PreNoListLeft.Count() > 0 && CanDoTaskLeft)
                //            {
                //                Application.Current.Dispatcher.Invoke(() =>
                //                {
                //                    this.CurrentLeftSelectPresN = PreNoListLeft[0];
                //                });

                //                CanDoTaskLeft = false;
                //                CanUpdateLeft = false;
                //                DoMedOut(this.CurrentLeftSelectPresN.MedPos);
                //            }
                //        }
                //    }, cts.Token);
                //    DoRightOutMedTask = Task.Run(() =>
                //    {
                //        while (!cts.IsCancellationRequested)
                //        {
                //            Task.Delay(500).Wait();
                //            if (PreNoListLeft.Count() == 0 && PreNoListRight.Count() > 0 && CanDoTaskRight)
                //            {
                //                Task.Delay(500).Wait();//延迟半秒钟，不能在左结束后立马执行
                //                Application.Current.Dispatcher.Invoke(() =>
                //                {
                //                    this.CurrentRightSelectPresN = PreNoListRight[0];
                //                    //CanDoTaskRight = false;
                //                    //CanUpdateRight = false;
                //                    //DoMedOut(this.CurrentRightSelectPresN.MedPos);
                //                    //RightDispatcherTimer.IsEnabled = true;

                //                });
                //                CanDoTaskRight = false;
                //                CanUpdateRight = false;
                //                DoMedOut(this.CurrentRightSelectPresN.MedPos);
                //                // RightDispatcherTimer.IsEnabled = true;
                //            }

                //        }
                //    }, cts.Token);
                //    CanDoTaskLeft = true;
                //    CanDoTaskRight = true;
                //}
                #endregion

            }
            catch (Exception ex)
            {
                GlobalValue.Loger.Error("出药界面加载失败:" + ex.Message);
            }
        }
        private void DoPreSend(PreNoModel preNoModel)
        {
            this.CurrentLeftSelectPresN = null;
            this.CurrentRightSelectPresN = null;
            var result = GlobalValue.LocalDataAccess.GetPreByNo(GlobalValue.MachineID, preNoModel.PreNo);
            if (result != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    PreNoListLeft = new ObservableCollection<PreNoModel>(result.Where(p => new string[] { "01", "02" }.Contains(p.MedPos.Substring(2, 2))).Select(p => new PreNoModel()
                    {
                        PreNo = p.PresNo,
                        MedPos = p.MedPos,
                        MedName = p.MedName,
                        Name = p.PName,
                        MedOutAMT = p.MedOutAMT,
                        OutFlag = p.OutFlag == "1" ? true : false
                    }));
                    if (PreNoListLeft != null && PreNoListLeft.Count > 0)
                    {
                        this.CurrentLeftSelectPresN = PreNoListLeft[0];
                    }

                    PreNoListRight = new ObservableCollection<PreNoModel>(result.Where(p => new string[] { "03", "04" }.Contains(p.MedPos.Substring(2, 2))).Select(p => new PreNoModel()
                    {
                        PreNo = p.PresNo,
                        MedPos = p.MedPos,
                        MedName = p.MedName,
                        Name = p.PName,
                        MedOutAMT = p.MedOutAMT,
                        OutFlag = p.OutFlag == "1" ? true : false
                    }));
                    if (PreNoListRight != null && PreNoListRight.Count > 0)
                    {
                        this.CurrentRightSelectPresN = PreNoListRight[0];
                    }
                });


            }
        }
        /// <summary>
        /// 查询新的第一个处方
        /// </summary>
        private void UpdateDataSource()
        {
            this.CurrentLeftSelectPresN = null;
            this.CurrentRightSelectPresN = null;
            var result = GlobalValue.LocalDataAccess.GetAllPreDrugList(GlobalValue.MachineID);
            if (result != null)
            {
                PreNoListLeft = new ObservableCollection<PreNoModel>(result.Where(p => new string[] { "01", "02" }.Contains(p.MedPos.Substring(2, 2))).Select(p => new PreNoModel()
                {
                    PreNo = p.PresNo,
                    MedPos = p.MedPos,
                    MedName = p.MedName,
                    Name = p.PName,
                    MedOutAMT = p.MedOutAMT,
                    OutFlag = p.OutFlag == "1" ? true : false
                }));

                PreNoListRight = new ObservableCollection<PreNoModel>(result.Where(p => new string[] { "03", "04" }.Contains(p.MedPos.Substring(2, 2))).Select(p => new PreNoModel()
                {
                    PreNo = p.PresNo,
                    MedPos = p.MedPos,
                    MedName = p.MedName,
                    Name = p.PName,
                    MedOutAMT = p.MedOutAMT,
                    OutFlag = p.OutFlag == "1" ? true : false
                }
         ));

            }
            else
            {
                PreNoListLeft = new ObservableCollection<PreNoModel>();
                PreNoListRight = new ObservableCollection<PreNoModel>();
            }

        }
        Task MedOutByPreNoTask;
        bool isRunning = false;
        /// <summary>
        /// 通过扫处方出药
        /// </summary>
        /// <param name="s"></param>
        private void DoMedOutByPreNo(string s)
        {
            if (!isRunning)
            {
                //先根据条码找到处方
                var PreResult = GlobalValue.LocalDataAccess.GetAllPreDrugListByBarCode(GlobalValue.MachineID, s);
                if (PreResult != null && PreResult.Count() > 0)
                {
                    isRunning = true;
                    var LeftPre = PreResult.Where(P => new string[] { "01", "02" }.Contains(P.MedPos.Substring(2, 2))).ToList();
                    var RightPre = PreResult.Where(P => new string[] { "03", "04" }.Contains(P.MedPos.Substring(2, 2))).ToList();
                    if (LeftPre != null)
                        this.PreNoListLeft = new ObservableCollection<PreNoModel>(LeftPre.Select(o => new PreNoModel()
                        {
                            PreNo = o.PresNo,
                            MedName = o.MedName,
                            MedOnlyCode = o.MedOnlyCode,
                            MedPos = o.MedPos,
                            MedOutAMT = o.MedOutAMT,
                            Name = o.PName
                        }).ToList());
                    if (RightPre != null)
                        this.PreNoListRight = new ObservableCollection<PreNoModel>(RightPre.Select(o => new PreNoModel()
                        {
                            PreNo = o.PresNo,
                            MedName = o.MedName,
                            MedOnlyCode = o.MedOnlyCode,
                            MedPos = o.MedPos,
                            MedOutAMT = o.MedOutAMT,
                            Name = o.PName
                        }).ToList());
                }
                MedOutByPreNoTask = Task.Run(() =>
                {
                    while (!cts.IsCancellationRequested)
                    {
                        Thread.Sleep(500);
                        if (PreNoListLeft != null && PreNoListLeft.Count() > 0 && CanDoTaskLeft)
                        {

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                this.CurrentLeftSelectPresN = PreNoListLeft[0];
                            });
                            CanDoTaskLeft = false;
                            CanUpdateLeft = false;
                            DoMedOut(this.PreNoListLeft[0].MedPos);
                        }
                        else
                        {
                            if (PreNoListLeft != null && PreNoListRight != null && PreNoListLeft.Count() == 0 && PreNoListRight.Count() > 0 && CanDoTaskRight)
                            {

                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    this.CurrentRightSelectPresN = PreNoListRight[0];
                                });
                                CanDoTaskRight = false;
                                CanUpdateRight = false;
                                DoMedOut(this.PreNoListRight[0].MedPos);
                            }
                        }
                    }
                }, cts.Token);
            }

        }
        private void RightDispatcherTimer_Tick(object? sender, EventArgs e)
        {
            int val_right = GlobalValue.DeltaPLC.ServoPositionRunFinish(ServoType.RightServo);
            if (val_right == 5)
            {
                RightDispatcherTimer.IsEnabled = false;
                int val = GlobalValue.DeltaPLC.GetServoAlarm();
                string alarmcode = Convert.ToString(val, 2).PadLeft(8, '0');
                string msg = "";
                if (alarmcode.Substring(7, 1) == "1")
                {
                    msg = "光幕报警";
                }
                else if (alarmcode.Substring(6, 1) == "1")
                {
                    msg = "急停报警";
                }
                else if (alarmcode.Substring(5, 1) == "1")
                {
                    msg = "左侧上极限报警";
                }
                else if (alarmcode.Substring(4, 1) == "1")
                {
                    msg = "左侧下极限报警";
                }
                else if (alarmcode.Substring(3, 1) == "1")
                {
                    msg = "右侧上极限报警";
                }
                else if (alarmcode.Substring(2, 1) == "1")
                {
                    msg = "右侧下极限报警";
                }
                else if (alarmcode.Substring(1, 1) == "1")
                {
                    msg = "左侧驱动器报警";
                }
                else if (alarmcode.Substring(0, 1) == "1")
                {
                    msg = "右侧驱动器报警";
                }
                MessageBox.Show("右电机运行完成异常-" + msg);

            }
            if (val_right == 4)
            {
                RightLedShow();
                CanUpdateRight = true;
                RightDispatcherTimer.IsEnabled = false;
            }
        }



        private void LeftDispatcherTimer_Tick(object? sender, EventArgs e)
        {
            int val_left = GlobalValue.DeltaPLC.ServoPositionRunFinish(ServoType.LeftServo);
            if (val_left == 5)
            {
                LeftDispatcherTimer.IsEnabled = false;
                int val = GlobalValue.DeltaPLC.GetServoAlarm();
                string alarmcode = Convert.ToString(val, 2).PadLeft(8, '0');
                string msg = "";
                if (alarmcode.Substring(7, 1) == "1")
                {
                    msg = "光幕报警";
                }
                else if (alarmcode.Substring(6, 1) == "1")
                {
                    msg = "急停报警";
                }
                else if (alarmcode.Substring(5, 1) == "1")
                {
                    msg = "左侧上极限报警";
                }
                else if (alarmcode.Substring(4, 1) == "1")
                {
                    msg = "左侧下极限报警";
                }
                else if (alarmcode.Substring(3, 1) == "1")
                {
                    msg = "右侧上极限报警";
                }
                else if (alarmcode.Substring(2, 1) == "1")
                {
                    msg = "右侧下极限报警";
                }
                else if (alarmcode.Substring(1, 1) == "1")
                {
                    msg = "左侧驱动器报警";
                }
                else if (alarmcode.Substring(0, 1) == "1")
                {
                    msg = "右侧驱动器报警";
                }
                MessageBox.Show("左电机运行完成异常-" + msg);
            }
            else if (val_left == 4) //完成
            {
                //左侧亮灯

                LeftLedShow();
                CanUpdateLeft = true;
                LeftDispatcherTimer.IsEnabled = false;
            }
        }
        #region 定义控制左电机和右电机动作的相关变量
        bool CanUpdateLeft;                    //左侧出药完成，可以更新
        bool CanUpdateRight;                   //右侧出药完成，可以更新
        Task DoLeftOutMedTask;                 //左侧线程一直监控是否有任务可以做，并执行
        Task DoRightOutMedTask;                //右侧线程一直监控是否有任务可以做，并执行
        bool CanDoTaskLeft;                    //左侧 是否可以运行
        bool CanDoTaskRight;                   //右侧 是否可以运行
        DispatcherTimer LeftDispatcherTimer;  //左侧监控电机是否完成的定时器
        DispatcherTimer RightDispatcherTimer;  //右侧监控电机是否完成的定时器
        #endregion

        CancellationTokenSource cts = new CancellationTokenSource();
        /// <summary>
        /// 左库位亮灯
        /// </summary>
        private void LeftLedShow()
        {

        }
        /// <summary>
        /// 右侧亮灯
        /// </summary>
        private void RightLedShow()
        {

        }
        private void DoMedOut(string MedPosition)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                BtnEnable = false;
                ContinueRunBtnEnable = false;
                ProgressVisibility = Visibility.Visible;
                GridLoading = true;
            });
            //ProgressVisibility = Visibility.Visible;
            //GridLoading = true;
            #region 先发送给电机
            //   StorageCoreModel storageCoreModel = new StorageCoreModel(MedPosition);

            StorageModel storageModel = StorageModel.GetStorageByPositon(MedPosition);
            GlobalValue.DeltaPLC.ServoPositionRun(storageModel.motorPositon.direction == Direction.Left ? ServoType.LeftServo : ServoType.RightServo, Convert.ToInt32(storageModel.motorPositon.rowPosition));

            bool Finish = false;
            bool IsAlarm = false;
            int k = 0;
            int i = 0;
            #region 模拟
            //do
            //{
            //    int num = new Random().Next(1, 100);

            //    switch (storageModel.motorPositon.direction)
            //    {
            //        case Direction.Left:
            //            if (k > 5 && num % 10 != 0)
            //            {
            //                Finish = true;
            //                Application.Current.Dispatcher.Invoke(() =>
            //                {
            //                    BtnEnable = true;
            //                    ProgressVisibility = Visibility.Hidden;
            //                    GridLoading = false;
            //                });

            //                CanUpdateLeft = true;
            //            }
            //            else if (k > 5 && num % 10 == 0)
            //            {
            //                Application.Current.Dispatcher.Invoke(() =>
            //                {
            //                    var obj = GlobalValue.WindDic["DrugOutView"];

            //                    VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedNormal", true);
            //                    RunFailedMessage = "光幕报警";
            //                    VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedShow", true);
            //                    BtnEnable = true;
            //                    ProgressVisibility = Visibility.Hidden;
            //                    GridLoading = false;
            //                });
            //                IsAlarm = true;
            //                Finish = true;
            //            }

            //            break;
            //        case Direction.Right:
            //            if (k > 7 && num % 5 != 0)
            //            {
            //                Finish = true;
            //                Application.Current.Dispatcher.Invoke(() =>
            //                {
            //                    BtnEnable = true;
            //                    ProgressVisibility = Visibility.Hidden;
            //                    GridLoading = false;
            //                });
            //                CanUpdateRight = true;
            //            }
            //            else if (k > 7 && num % 5 == 0)
            //            {
            //                Application.Current.Dispatcher.Invoke(() =>
            //                {
            //                    var obj = GlobalValue.WindDic["DrugOutView"];
            //                    GlobalValue.Loger.Error("光幕报警");
            //                    VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedNormal", true);
            //                    RunFailedMessage = "光幕报警";
            //                    VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedShow", true);
            //                    BtnEnable = true;
            //                    ProgressVisibility = Visibility.Hidden;
            //                    GridLoading = false;
            //                });
            //                IsAlarm = true;
            //                Finish = true;
            //            }
            //            //ProgressMessage = "电机正在运行";
            //            //  i++;
            //            break;
            //        default:
            //            break;
            //    }
            //    Thread.Sleep(500);
            //    k++;
            //    if (k == 10)
            //    {
            //        Application.Current.Dispatcher.Invoke(() =>
            //        {
            //            var obj = GlobalValue.WindDic["DrugOutView"];
            //            GlobalValue.Loger.Error("电机运行超时");
            //            VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedNormal", true);
            //            RunFailedMessage = "电机运行超时";
            //            VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedShow", true);
            //            BtnEnable = true;
            //            ProgressVisibility = Visibility.Hidden;
            //            GridLoading = false;
            //        });
            //        IsAlarm = true;
            //        Finish = true;
            //    }
            //}
            //while (!Finish);
            #endregion
            #region 实际运行

            do
            {
                switch (storageModel.motorPositon.direction)
                {
                    //如果是左侧的药品
                    case Direction.Left:
                        int val_left = GlobalValue.DeltaPLC.ServoPositionRunFinish(ServoType.LeftServo);
                        if (val_left == 5)
                        {
                            IsAlarm = true;
                            // LeftDispatcherTimer.IsEnabled = false;
                            int val = GlobalValue.DeltaPLC.GetServoAlarm();
                            var result = Convert.ToString(val, 2).PadLeft(16, '0');
                            string alarmcode = result.Substring(result.Length-8,8);
                            string alarmcode1 = result.Substring(0, 8);
                            string msg = "";
                            if (alarmcode.Substring(7, 1) == "1")
                            {
                                msg = "光幕报警";
                            }
                            else if (alarmcode.Substring(6, 1) == "1")
                            {
                                msg = "急停报警";
                            }
                            else if (alarmcode.Substring(5, 1) == "1")
                            {
                                msg = "左侧上极限报警";
                            }
                            else if (alarmcode.Substring(4, 1) == "1")
                            {
                                msg = "左侧下极限报警";
                            }
                            else if (alarmcode.Substring(3, 1) == "1")
                            {
                                msg = "右侧上极限报警";
                            }
                            else if (alarmcode.Substring(2, 1) == "1")
                            {
                                msg = "右侧下极限报警";
                            }
                            else if (alarmcode.Substring(1, 1) == "1")
                            {
                                msg = "左侧驱动器报警";
                            }
                            else if (alarmcode.Substring(0, 1) == "1")
                            {
                                msg = "右侧驱动器报警";
                            }
                            else if (alarmcode1.Substring(8, 1) == "1")
                            {
                                msg = "疑似光幕失效";
                            }

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                var obj = GlobalValue.WindDic["DrugOutView"];
                                VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedNormal", true);
                                RunFailedMessage = msg;
                                VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedShow", true);
                                ProgressVisibility = Visibility.Hidden;
                               
                                GridLoading = false;
                            });
                            //IsAlarm = true;
                            Finish = true;

                        }
                        else if (val_left == 4) //完成
                        {
                            //左侧亮灯
                            Finish = true;
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                ProgressVisibility = Visibility.Hidden;
                                GridLoading = false;
                                //完成之后设置按钮状态
                                ContinueRunBtnEnable = true;
                                BtnEnable = true;
                            });

                            CanUpdateLeft = true;
                        }
                        break;
                    case Direction.Right:
                        int val_right = GlobalValue.DeltaPLC.ServoPositionRunFinish(ServoType.RightServo);
                        if (val_right == 5)
                        {
                            IsAlarm = true;
                            // LeftDispatcherTimer.IsEnabled = false;
                            int val = GlobalValue.DeltaPLC.GetServoAlarm();
                            var result = Convert.ToString(val, 2).PadLeft(16, '0');
                            string alarmcode = result.Substring(result.Length - 8, 8);
                            string alarmcode1 = result.Substring(0, 8);
                            string msg = "";
                            if (alarmcode.Substring(7, 1) == "1")
                            {
                                msg = "光幕报警";
                            }
                            else if (alarmcode.Substring(6, 1) == "1")
                            {
                                msg = "急停报警";
                            }
                            else if (alarmcode.Substring(5, 1) == "1")
                            {
                                msg = "左侧上极限报警";
                            }
                            else if (alarmcode.Substring(4, 1) == "1")
                            {
                                msg = "左侧下极限报警";
                            }
                            else if (alarmcode.Substring(3, 1) == "1")
                            {
                                msg = "右侧上极限报警";
                            }
                            else if (alarmcode.Substring(2, 1) == "1")
                            {
                                msg = "右侧下极限报警";
                            }
                            else if (alarmcode.Substring(1, 1) == "1")
                            {
                                msg = "左侧驱动器报警";
                            }
                            else if (alarmcode.Substring(0, 1) == "1")
                            {
                                msg = "右侧驱动器报警";
                            }
                            else if (alarmcode1.Substring(8, 1) == "1")
                            {
                                msg = "疑似光幕失效";
                            }

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                var obj = GlobalValue.WindDic["DrugOutView"];

                                VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedNormal", true);
                                RunFailedMessage = msg;
                                VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedShow", true);
                                ProgressVisibility = Visibility.Hidden;
                                GridLoading = false;
                            });
                            //IsAlarm = true;
                            Finish = true;

                        }
                        else if (val_right == 4) //完成
                        {
                            //左侧亮灯
                            Finish = true;
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                ProgressVisibility = Visibility.Hidden;
                                //完成之后设置按钮状态
                                ContinueRunBtnEnable = true;
                                BtnEnable = true;
                                GridLoading = false;
                            });

                            CanUpdateLeft = true;
                        }
                        break;
                }
                Thread.Sleep(500);
                k++;
                if (k == 10)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var obj = GlobalValue.WindDic["DrugOutView"];
                        VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedNormal", true);
                        RunFailedMessage = "电机运行超时";
                        VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedShow", true);
                   
                        ProgressVisibility = Visibility.Hidden;
                        GridLoading = false;
                    });
                    IsAlarm = true;
                    Finish = true;
                }
            }
            while (!Finish);

            #endregion
            ///没有报警，则允许出药
            if (!IsAlarm)
            {
                CanbeRemoveMedList = GetCurrentMedPos(MedPosition);
                //点亮灯
                LightUp(CanbeRemoveMedList);
                //点亮DataGridView的行
                ShowDataGridView(CanbeRemoveMedList);
            }
            #endregion
        }

        /// <summary>
        /// 找到的药品显示颜色
        /// </summary>
        /// <param name="MedPosList"></param>
        private void ShowDataGridView(List<string> MedPosList)
        {
            foreach (var item in MedPosList)
            {
                var Lie = item.Substring(2, 2);
                if (new string[] { "01", "02" }.Contains(Lie))
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        this.PreNoListLeft.Where(p => p.MedPos == item).FirstOrDefault().IsCurrentInPositon = true;
                    });

                }
                else if (new string[] { "03", "04" }.Contains(Lie))
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        this.PreNoListRight.Where(p => p.MedPos == item).FirstOrDefault().IsCurrentInPositon = true;
                    });
                }
            }
        }
        private List<MedPositonListModel> MedPositonGroup = new List<MedPositonListModel>();
        /// <summary>
        /// 根据当前第一个药品，找到所有的药品
        /// </summary>
        /// <param name="medpos"></param>
        /// <returns></returns>
        public List<string> GetCurrentMedPos(string medpos)
        {
            List<string> medList = new List<string>();
            var Lie = medpos.Substring(2, 2);
            if (new string[] { "01", "02" }.Contains(Lie))
            {
                var result = this.MedPositonGroup.Where(p => p.MedPosGroup.Contains(medpos)).FirstOrDefault();

                if (result != null)
                {
                    medList.AddRange(PreNoListLeft.Where(p => result.MedPosGroup.Contains(p.MedPos)).Select(p => p.MedPos));
                    var Serach = this.MedPositonGroup.Where(p => p.MedPosSign == result.MedPosSign).ToList();
                    Serach.Remove(result);
                    var r = Serach.FirstOrDefault();
                    if (r != null)
                    {
                        var Pre = PreNoListLeft.Where(p => r.MedPosGroup.Contains(p.MedPos)).ToList();
                        medList.AddRange(Pre.Select(p => p.MedPos).ToArray());
                    }
                }
            }
            else if (new string[] { "03", "04" }.Contains(Lie))
            {
                var result = this.MedPositonGroup.Where(p => p.MedPosGroup.Contains(medpos)).FirstOrDefault();

                if (result != null)
                {
                    medList.AddRange(_PreNoListRight.Where(p => result.MedPosGroup.Contains(p.MedPos)).Select(p => p.MedPos));
                    var Serach = this.MedPositonGroup.Where(p => p.MedPosSign == result.MedPosSign).ToList();
                    Serach.Remove(result);
                    var r = Serach.FirstOrDefault();
                    if (r != null)
                    {
                        var Pre = _PreNoListRight.Where(p => r.MedPosGroup.Contains(p.MedPos)).ToList();
                        medList.AddRange(Pre.Select(p => p.MedPos).ToArray());
                    }
                }
            }
            // medList.Add(medpos);          

            return medList;
        }
        /// <summary>
        /// 根据位置点亮灯，如果在当前位置其他还有其他的药，则点亮其他位置的灯
        /// </summary>
        /// <param name="MedPositon"></param>
        private void LightUp(List<string> MedPosList)
        {
            foreach (var item in MedPosList)
            {
                StorageCoreModel storageCoreModel = new StorageCoreModel(item);
                //SendByteTOPCB(storageCoreModel.Led.X, storageCoreModel.Led.Y);
                foreach (var Cab in this.cabinetModels)
                {
                    var result = Cab.ledModels.Where(p => p.X == storageCoreModel.Led.X && p.Y == storageCoreModel.Led.Y).FirstOrDefault();
                    if (result != null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            result.IsLight = true;
                        });

                    }
                }
                // this.cabinetModels.ForEach(p => p.ledModels.Where(p => p.X == storageCoreModel.Led.X && p.Y == storageCoreModel.Led.Y).FirstOrDefault().IsLight=true);
            }
            //先亮这个位置的灯，再亮其他位置的灯

            //先根据当前位置，找到所有可以亮灯的药


        }

        private void LightOff(List<string> MedPosList)
        {
            // SendByteTOPCBOff();
            foreach (var item in MedPosList)
            {
                StorageCoreModel storageCoreModel = new StorageCoreModel(item);

                foreach (var Cab in this.cabinetModels)
                {
                    var result = Cab.ledModels.Where(p => p.X == storageCoreModel.Led.X && p.Y == storageCoreModel.Led.Y).FirstOrDefault();
                    if (result != null)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            result.IsLight = false;
                        });

                    }
                }
                // this.cabinetModels.ForEach(p => p.ledModels.Where(p => p.X == storageCoreModel.Led.X && p.Y == storageCoreModel.Led.Y).FirstOrDefault().IsLight=true);
            }
        }
        /// <summary>
        /// 关闭所有的灯
        /// </summary>
        private void LightOffAll()
        {
            // SendByteTOPCBOff();
            foreach (var item in this.cabinetModels)
            {
                foreach (var led in item.ledModels)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        led.IsLight = false;
                    });
                }
            }
        }
        /// <summary>
        /// 发送给串口亮灯
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        private void SendByteTOPCB(int X, int Y)
        {
            try
            {
                #region 拼接字节数组               
                var index = X;
                var ledResult = (Y == 1 ? 1 : 0) * 1 + (Y == 2 ? 1 : 0) * 2 + (Y == 3 ? 1 : 0) * 4;
                byte[] SendBuffer = new byte[9];
                SendBuffer[0] = 0x00;
                SendBuffer[1] = Convert.ToByte(index);
                SendBuffer[2] = 0x00;
                SendBuffer[3] = 0x00;
                SendBuffer[4] = 0x01;
                SendBuffer[5] = 0x01;
                SendBuffer[6] = (byte)ledResult;
                ushort CRCFull = 0xFFFF;
                char CRCLSB;
                byte[] CRC = new byte[2];
                for (int i = 0; i < (SendBuffer.Length - 2); i++)
                {
                    CRCFull = (ushort)(CRCFull ^ SendBuffer[i]);

                    for (int j = 0; j < 8; j++)
                    {
                        CRCLSB = (char)(CRCFull & 0x0001);
                        CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                        if (CRCLSB == 1)
                        {
                            CRCFull = (ushort)(CRCFull ^ 0xA001);
                        }

                    }
                }


                CRC[1] = (byte)((CRCFull >> 8) & 0xFF);
                CRC[0] = (byte)(CRCFull & 0xFF);


                SendBuffer[7] = CRC[1];
                SendBuffer[8] = CRC[0];
                #endregion
                GlobalValue.LedSerialPort.DiscardOutBuffer();
                GlobalValue.LedSerialPort.DiscardInBuffer();
                GlobalValue.LedSerialPort.Write(SendBuffer, 0, SendBuffer.Count());

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SendByteTOPCBOff()
        {
            #region 全部灭灯
            byte[] sendStr111 = new byte[7];
            sendStr111[0] = 0xFF;
            sendStr111[1] = 0xFF;
            sendStr111[2] = 0x00;
            sendStr111[3] = 0x00;
            sendStr111[4] = 0x04;
            sendStr111[5] = 0xC3;
            sendStr111[6] = 0x01;
            GlobalValue.LedSerialPort.DiscardOutBuffer();
            GlobalValue.LedSerialPort.DiscardInBuffer();
            GlobalValue.LedSerialPort.Write(sendStr111, 0, 7);
            #endregion
        }
        private void InitCabinetModels()
        {
            for (int i = 1; i < 5; i++)
            {
                this.cabinetModels.Add(new CabinetModel()
                {
                    Line = 1,
                    storageModels = GetListStorages(i),
                    ledModels = GetLedModels(i)
                });
            }
        }
        public List<LedModel> GetLedModels(int id)
        {
            List<LedModel> ledModels = new List<LedModel>();
            for (int i = 1; i < 4; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    ledModels.Add(new LedModel() { X = (id - 1) * 3 + j, Y = i, IsLight = false }); ;
                }
            }
            return ledModels;
        }
        private List<StorageModel> GetListStorages(int id)
        {
            List<StorageModel> storageModels = new List<StorageModel>();
            for (int i = 1; i < 28; i++)
            {
                storageModels.Add(new StorageModel()
                {
                    CabID = id,
                    PValue = i,
                });
            }
            return storageModels;
        }
        #region 属性
        private List<string> _CanbeRemoveMedList;

        public List<string> CanbeRemoveMedList
        {
            get { return _CanbeRemoveMedList; }
            set { Set(ref _CanbeRemoveMedList, value); }
        }

        private bool _GridLoading = false;

        public bool GridLoading
        {
            get { return _GridLoading; }
            set { Set(ref _GridLoading, value); }
        }

        private Visibility _ProgressVisibility = Visibility.Hidden;

        public Visibility ProgressVisibility
        {
            get { return _ProgressVisibility; }
            set
            {
                Set(ref _ProgressVisibility, value);

            }
        }
        private string _ProgressMessage = "电机正在运行";

        public string ProgressMessage
        {
            get { return _ProgressMessage; }
            set { Set(ref _ProgressMessage, value); }
        }



        private PreNoModel _CurrentLeftSelectPresN;

        public PreNoModel CurrentLeftSelectPresN
        {
            get { return _CurrentLeftSelectPresN; }
            set { Set(ref _CurrentLeftSelectPresN, value); }
        }

        private PreNoModel _CurrentRightSelectPresN;

        public PreNoModel CurrentRightSelectPresN
        {
            get { return _CurrentRightSelectPresN; }
            set { Set(ref _CurrentRightSelectPresN, value); }
        }

        #endregion
        #region Command
        /// <summary>
        /// 确认出药
        /// </summary>
        public RelayCommand<object> ConfirmMedOut
        {
            get => new RelayCommand<object>(arg =>
            {

                if (!CanUpdateLeft && !CanUpdateRight)
                {
                    WPFMessageBox.ShowDialog("电机未运行到位，无法取药", true);
                    return;
                }
                if (this.PreNoListLeft.Count() > 0 || this.PreNoListRight.Count() > 0)
                {
                    #region 左右侧确认更新
                    if (CanUpdateLeft)
                    {
                        if (CanbeRemoveMedList != null && CanbeRemoveMedList.Count > 0)
                        {
                            //如果左侧运行更新
                            foreach (var item in CanbeRemoveMedList)
                            {
                                var lie = item.Substring(2, 2);
                                if (new string[] { "01", "02" }.Contains(lie))
                                {
                                    var Pre = PreNoListLeft.Where(p => p.MedPos == item).FirstOrDefault();
                                    GlobalValue.LocalDataAccess.UpdateDoListOut("1", Pre.PreNo, Pre.MedPos, GlobalValue.MachineID);
                                    GlobalValue.LocalDataAccess.UpdateStoreTableAMT(Pre.MedOutAMT, Pre.MedOnlyCode, Pre.MedPos);
                                }
                            }
                        }
                        // GlobalValue.LocalDataAccess.UpdateDoListOut("1", CurrentLeftSelectPresN.PreNo, CurrentLeftSelectPresN.MedPos, GlobalValue.MachineID);
                        // GlobalValue.LocalDataAccess.UpdateStoreTableAMT(CurrentLeftSelectPresN.MedOutAMT, CurrentLeftSelectPresN.MedOnlyCode, CurrentLeftSelectPresN.MedPos);
                    }
                    if (CanUpdateRight)
                    {
                        if (CanbeRemoveMedList != null && CanbeRemoveMedList.Count > 0)
                        {
                            foreach (var item in CanbeRemoveMedList)
                            {
                                var lie = item.Substring(2, 2);
                                if (new string[] { "03", "04" }.Contains(lie))
                                {
                                    var Pre = PreNoListRight.Where(p => p.MedPos == item).FirstOrDefault();
                                    if (Pre != null)
                                    {
                                        GlobalValue.LocalDataAccess.UpdateDoListOut("1", Pre.PreNo, Pre.MedPos, GlobalValue.MachineID);
                                        GlobalValue.LocalDataAccess.UpdateStoreTableAMT(Pre.MedOutAMT, Pre.MedOnlyCode, Pre.MedPos);
                                    }
                                }
                            }
                        }
                        //GlobalValue.LocalDataAccess.UpdateDoListOut("1", CurrentRightSelectPresN.PreNo, CurrentRightSelectPresN.MedPos, GlobalValue.MachineID);
                        //GlobalValue.LocalDataAccess.UpdateStoreTableAMT(CurrentRightSelectPresN.MedOutAMT, CurrentRightSelectPresN.MedOnlyCode, CurrentRightSelectPresN.MedPos);

                    }
                    #endregion
                    //更新数据
                    //先查询这个处方是否还有未出的药
                    //临时变量
                    List<do_list_out> result = null;
                    if (this.CurrentLeftSelectPresN != null)
                    {
                        result = GlobalValue.LocalDataAccess.GetAllPreDrugListByBarCode(GlobalValue.MachineID, this.CurrentLeftSelectPresN.PreNo);
                    }
                    if (this.CurrentLeftSelectPresN == null && this.CurrentRightSelectPresN != null)
                    {
                        result = GlobalValue.LocalDataAccess.GetAllPreDrugListByBarCode(GlobalValue.MachineID, this.CurrentRightSelectPresN.PreNo);
                    }
                    if (result != null)
                    {
                        var TempLeft = result.Where(p => new string[] { "01", "02" }.Contains(p.MedPos.Substring(2, 2))).ToArray();
                        var TempRight = result.Where(p => new string[] { "03", "04" }.Contains(p.MedPos.Substring(2, 2))).ToArray();
                        if (TempLeft.Count() == 0 && TempRight.Count() == 0)//没有这个处方需要出药了
                        {
                            if (GlobalValue.MedOutMode != "0")
                            {
                                //查询新的处方
                                UpdateDataSource();
                            }
                            else
                            {
                                this.PreNoListLeft = null;
                                this.PreNoListRight = null;
                                this.CurrentLeftSelectPresN = null;
                                this.CurrentRightSelectPresN = null;
                            }
                        }
                        else //如果还有处方则按照之前的处方查询
                        {
                            this.CurrentLeftSelectPresN = null;
                            this.CurrentRightSelectPresN = null;
                            PreNoListLeft = new ObservableCollection<PreNoModel>(TempLeft.Select(p => new PreNoModel()
                            {
                                PreNo = p.PresNo,
                                MedPos = p.MedPos,
                                MedName = p.MedName,
                                Name = p.PName,
                                MedOutAMT = p.MedOutAMT,
                                OutFlag = p.OutFlag == "1" ? true : false
                            }));
                            PreNoListRight = new ObservableCollection<PreNoModel>(TempRight.Select(p => new PreNoModel()
                            {
                                PreNo = p.PresNo,
                                MedPos = p.MedPos,
                                MedName = p.MedName,
                                Name = p.PName,
                                MedOutAMT = p.MedOutAMT,
                                OutFlag = p.OutFlag == "1" ? true : false
                            }));
                        }
                    }
                    else
                    {
                        //查询新的处方
                        if (GlobalValue.MedOutMode != "0")
                            UpdateDataSource();
                        else
                        {

                            this.PreNoListLeft = null;
                            this.PreNoListRight = null;
                            this.CurrentLeftSelectPresN = null;
                            this.CurrentRightSelectPresN = null;
                            isRunning = false;
                        }
                    }
                    #region 重置可以继续运行的变量
                    if (CanUpdateLeft)
                    {
                        CanDoTaskLeft = true;
                        CanUpdateLeft = false;
                    }
                    if (CanUpdateRight)
                    {
                        CanDoTaskRight = true;
                        CanUpdateRight = false;
                    }
                    #endregion
                    LightOff(CanbeRemoveMedList);
                    CanbeRemoveMedList?.Clear();
                }


            });
        }
        /// <summary>
        /// 继续运行，必须清除PLC故障后，按钮才能点击
        /// </summary>
        public RelayCommand<object> ContinueRun
        {
            get => new RelayCommand<object>(arg =>
            {
                //GlobalValue.DeltaPLC.ServoAlarmReset();//清除故障
                LightOffAll();
                var obj = GlobalValue.WindDic["DrugOutView"];
                VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedClose", true);
                if (PreNoListLeft.Count() > 0)
                {
                    CanDoTaskLeft = true;
                }
                else if (PreNoListRight.Count() > 0)
                {
                    CanDoTaskRight = true;
                }
            });
        }
        /// <summary>
        /// 处方选择，选择完成后，点击继续运行按钮设备动作
        /// </summary>
        public RelayCommand<object> PreSelectCommand
        {
            get => new RelayCommand<object>(arg =>
            {
                CanDoTaskLeft = false;
                CanDoTaskRight = false;
                LightOffAll();
                if (ActionManager.ExecuteAndResult("PreCommand", "PreCommand"))
                {

                }

            });
        }
        /// <summary>
        /// 关闭电机运行报警
        /// </summary>
        public RelayCommand<object> CloseSaveFailedCommand
        {
            get => new RelayCommand<object>(arg =>
            {
                //清除伺服故障
                GlobalValue.DeltaPLC.ServoAlarmReset();
                ContinueRunBtnEnable = true;
                VisualStateManager.GoToElementState(arg as UserControl, "SaveFailedClose", true);
            });
        }
        #endregion
    }
}
