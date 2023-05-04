using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Iron.IntelligentDispsingMachine.Common;
using Iron.IntelligentDispsingMachine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using static Iron.IntelligentDispsingMachine.CPLC.DeltaPLC;

namespace Iron.IntelligentDispsingMachine.ViewModel
{
    public class DrugInViewModel:ViewModelBase
    {
        public override void Cleanup()
        {
            base.Cleanup();
            // LeftMotoTimer.Stop();
            //RightMotoTimer.Stop();
            Messenger.Default.Unregister<string>(this, "ScanPortMessage");
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
        public List<CabinetModel> cabinetModels { get; set; } = new List<CabinetModel>();
        public DrugInViewModel()
		{
            InitCabinetModels();
            Messenger.Default.Register<string>(this, "ScanPortMessage", s =>
            {
                if (s.Length < 18)
                {
                    this.BarCode = s;
                }
                else
                {
                    this.SupervisoryCode = s;
                }
            });
			//LeftMotoTimer = new DispatcherTimer();
			//LeftMotoTimer.Tick += LeftMotoTimer_Tick;
			//LeftMotoTimer.Interval = new TimeSpan(1000);
			//RightMotoTimer = new DispatcherTimer();
			//RightMotoTimer.Tick += RightMotoTimer_Tick;
			//RightMotoTimer.Interval = new TimeSpan(1000);  
        }
        #region 按钮使能
        /// <summary>
        /// 运行按钮
        /// </summary>
        private bool _BtnRunEnable=true;

        public bool BtnRunEnable
        {
            get { return _BtnRunEnable; }
            set { Set(ref _BtnRunEnable, value); }
        }
        private bool _BtnMedInEnable=true; 

        public bool BtnMedInEnable
        {
            get { return _BtnMedInEnable; }
            set { Set(ref _BtnMedInEnable, value); }
        }

        #endregion
        private void RightMotoTimer_Tick(object? sender, EventArgs e)
		{
            int val_right = GlobalValue.DeltaPLC.ServoPositionRunFinish(ServoType.RightServo);
            if (val_right == 5)
            {
                isRunning = false;
                RightMotoTimer.IsEnabled = false;
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
                RightMotoTimer.IsEnabled = false;
                RightLedShow();
                isRunning = false;
                
            }
        }

        private void RightLedShow()
        {
            
        }

  //      private void LeftMotoTimer_Tick(object? sender, EventArgs e)
		//{
  //          int val_left = GlobalValue.DeltaPLC.ServoPositionRunFinish(ServoType.LeftServo);
  //          if (val_left == 5)
  //          {
  //              LeftMotoTimer.IsEnabled = false;
  //              int val = GlobalValue.DeltaPLC.GetServoAlarm();
  //              string alarmcode = Convert.ToString(val, 2).PadLeft(8, '0');
  //              string msg = "";
  //              if (alarmcode.Substring(7, 1) == "1")
  //              {
  //                  msg = "光幕报警";
  //              }
  //              else if (alarmcode.Substring(6, 1) == "1")
  //              {
  //                  msg = "急停报警";
  //              }
  //              else if (alarmcode.Substring(5, 1) == "1")
  //              {
  //                  msg = "左侧上极限报警";
  //              }
  //              else if (alarmcode.Substring(4, 1) == "1")
  //              {
  //                  msg = "左侧下极限报警";
  //              }
  //              else if (alarmcode.Substring(3, 1) == "1")
  //              {
  //                  msg = "右侧上极限报警";
  //              }
  //              else if (alarmcode.Substring(2, 1) == "1")
  //              {
  //                  msg = "右侧下极限报警";
  //              }
  //              else if (alarmcode.Substring(1, 1) == "1")
  //              {
  //                  msg = "左侧驱动器报警";
  //              }
  //              else if (alarmcode.Substring(0, 1) == "1")
  //              {
  //                  msg = "右侧驱动器报警";
  //              }
  //              MessageBox.Show("左电机运行完成异常-" + msg);
  //          }
  //          else if (val_left == 4) //完成
  //          {
  //              //左侧亮灯
  //              LeftMotoTimer.IsEnabled = false;
  //              LeftLedShow();
  //              isRunning = false;                
  //          }
  //      }

		private void LeftLedShow()
		{
			
		}
        /// <summary>
        /// 扫的条码
        /// </summary>
		private string _BarCode;

		public string BarCode
		{
			get { return _BarCode; }
			set {
				Set(ref _BarCode, value);
				if (!isRunning)
				{
					try
					{
						var result = GlobalValue.LocalDataAccess.GetMedInStoreByBarCode(_BarCode);
						if (result != null)
						{
                            //this.StorageMedList.Clear();
							this.StorageMedList = result.Select(p => new StorageMedModel()
							{
								MedName = p.MedName,
								MedPos = p.MedPos,
								MedUnit = p.MedUnit,
								MedNowAMT = p.MedNowAMT,
								MedFactory = p.MedFactory,
								MedValidTime = p.MedValidTime,
                                MedOnlyCode= p.MedOnlyCode,

							}).ToList();
							if (StorageMedList.Count > 0)
							{
                                // this.CurrentSelectStorage = null;
                                Application.Current.Dispatcher.Invoke(() =>
                                {
                                    this.CurrentSelectStorage = this.StorageMedList[0];
                                });
								
                                isRunning = true;
                                //开始运行
                                string s = this.CurrentSelectStorage.MedPos;
                                DoMedInTask(s);
                     
                               
							}
							else
							{
								MessageBox.Show("未找到该药品的信息");
								return;
							}
						}
					}
					catch(Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
				}
			}
		}

        /// <summary>
        /// 监管码
        /// </summary>

        private string _SupervisoryCode;

        public string SupervisoryCode
        {
            get { return _SupervisoryCode; }
            set {
                Set(ref _SupervisoryCode, value);
                //根据监管码查询
            }
        }

        /// <summary>
        /// 拼音码
        /// </summary>
        private string _PYCode;

        public string PYCode
        {
            get { return _PYCode; }
            set { 
                Set(ref _PYCode, value);
               var result=  GlobalValue.LocalDataAccess.GetMedInStoreByPYCode(_PYCode,GlobalValue.MachineID);
                if(result != null)
                {
                    StorageMedList = result.Select(p => new StorageMedModel()
                    {
                        MedOnlyCode = p.MedOnlyCode,
                        MedName = p.MedName,
                        MedPos = p.MedPos,
                        MedUnit = p.MedUnit,
                        MedNowAMT = p.MedNowAMT,
                        MedFactory = p.MedFactory,
                        MedValidTime = p.MedValidTime,
                    }).ToList();
                }
            }
        }
        private string _MedInCount;

        public string MedInCount
        {
            get { return _MedInCount; }
            set { Set(ref _MedInCount, value); }
        }

        private List<StorageMedModel> _StorageMedList;

		public List<StorageMedModel> StorageMedList
        {
			get { return _StorageMedList; }
			set { Set(ref _StorageMedList, value); }
		}
		private StorageMedModel _CurrentSelectStorage;

		public StorageMedModel CurrentSelectStorage
        {
			get { return _CurrentSelectStorage; }
			set {

				Set(ref _CurrentSelectStorage, value);
				//开始动作
			}
		}
        private Visibility _ProgressVisibility=Visibility.Hidden;

        public Visibility ProgressVisibility
        {
            get { return _ProgressVisibility; }
            set { Set(ref _ProgressVisibility, value); }
        }
        private bool _GridLoading=false;

        public bool GridLoading
        {
            get { return _GridLoading; }
            set { Set(ref _GridLoading, value); }
        }


        private string _SaveFailedMessage;

        public string SaveFailedMessage
        {
            get { return _SaveFailedMessage; }
            set { Set(ref _SaveFailedMessage, value); }
        }

        private string _ProgressMessage="电机正在运行";

        public string ProgressMessage
        {
            get { return _ProgressMessage; }
            set { Set(ref _ProgressMessage, value); }
        }

        bool isRunning = false;
		DispatcherTimer LeftMotoTimer;
		DispatcherTimer RightMotoTimer;



        private void DoMedInTask(string MedPosition)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressVisibility = Visibility.Visible;
                GridLoading = true;
            });
           
            #region 先发送给电机
            //   StorageCoreModel storageCoreModel = new StorageCoreModel(MedPosition);

            StorageModel storageModel = StorageModel.GetStorageByPositon(MedPosition);
            GlobalValue.DeltaPLC.ServoPositionRun(storageModel.motorPositon.direction == Direction.Left ? ServoType.LeftServo : ServoType.RightServo, Convert.ToInt32(storageModel.motorPositon.rowPosition));

            bool Finish = false;
            bool IsAlarm = false;
            int i = 0;
            int k = 0;
            #region 测试
            //do
            //{
            //    switch (storageModel.motorPositon.direction)
            //    {
            //        case Direction.Left:
            //            if (i == 10)
            //            {
            //                Finish = true;
            //                Application.Current.Dispatcher.Invoke(() =>
            //                {
            //                    ProgressVisibility = Visibility.Hidden;
            //                    GridLoading = false;
            //                });

            //            }

            //            i++;

            //            break;
            //        case Direction.Right:
            //            if (i == 10)
            //            {
            //                Finish = true;
            //                Application.Current.Dispatcher.Invoke(() =>
            //                {
            //                    ProgressVisibility = Visibility.Hidden;
            //                    GridLoading = false;
            //                });


            //            }

            //            i++;
            //            break;
            //        default:
            //            break;
            //    }
            //    Thread.Sleep(500);
            //}
            //while (!Finish);
            #endregion
            #region 实际运行
            isRunning = true;
            BtnMedInEnable = false;
            BtnRunEnable = false;
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


                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                var obj = GlobalValue.WindDic["DrugOutView"];

                                VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedNormal", true);
                                SaveFailedMessage = msg;
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
                            });

                            // CanUpdateLeft = true;
                        }
                        break;
                    case Direction.Right:
                        int val_right = GlobalValue.DeltaPLC.ServoPositionRunFinish(ServoType.RightServo);
                        if (val_right == 5)
                        {
                            IsAlarm = true;
                            // LeftDispatcherTimer.IsEnabled = false;
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


                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                var obj = GlobalValue.WindDic["DrugOutView"];

                                VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedNormal", true);
                                SaveFailedMessage = msg;
                                VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedShow", true);
                                ProgressVisibility = Visibility.Hidden;
                                GridLoading = false;
                            });
                            IsAlarm = true;
                            Finish = true;

                        }
                        else if (val_right == 4) //完成
                        {
                            //左侧亮灯
                            Finish = true;
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                ProgressVisibility = Visibility.Hidden;
                                GridLoading = false;
                            });

                            //CanUpdateLeft = true;
                        }
                        break;
                }
                Thread.Sleep(500);
                k++;
                if (k == 10)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var obj = GlobalValue.WindDic["DrugInView"];

                        VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedNormal", true);
                        SaveFailedMessage = "电机运行超时";
                        VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedShow", true);
                        ProgressVisibility = Visibility.Hidden;
                        GridLoading = false;
                    });
                    IsAlarm = true;
                    Finish = true;
                }
            }
            while (!Finish);
            isRunning = false;
         
            #endregion
            if (IsAlarm)
            {

                return;
            }
            BtnRunEnable = true;
            BtnMedInEnable = true;
            //点亮灯
            LightUp(this.CurrentSelectStorage.MedPos);
       
            #endregion
        }
        private void ShowDataGridView(string MedPosList)
        {
           if(this.StorageMedList!=null&&this.StorageMedList.Count()>0)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.StorageMedList.Where(p => p.MedPos == MedPosList).FirstOrDefault();
                });
            }
        }
        private void LightUp(string medpos)
        {

            StorageCoreModel storageCoreModel = new StorageCoreModel(medpos);
            // SendByteTOPCB(storageCoreModel.Led.X, storageCoreModel.Led.Y);
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
        }
        private void LightOff(string medpos)
        {
            StorageCoreModel storageCoreModel = new StorageCoreModel(medpos);
           // SendByteTOPCB(storageCoreModel.Led.X, storageCoreModel.Led.Y);
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
        }


        /// <summary>
        /// 关闭所有的灯
        /// </summary>
        private void LightOffAll()
        {
            //SendByteTOPCBOff();
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
        /// 发送串口数据关闭所有的灯
        /// </summary>
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

        //private void DoMedOutTask(string MedPos)
        //{
        //	try
        //	{
        //		isRunning = true;
        //		//根据位置盘点是左电机还是右电机
        //		StorageModel storageModel = StorageModel.GetStorageByPositon(MedPos);
        //		if (storageModel.motorPositon.direction == Direction.Left)//如果是左电机
        //		{
        //			//开启监控左电机的线程
        //			LeftMotoTimer.IsEnabled = true;
        //			GlobalValue.DeltaPLC.ServoPositionRun(ServoType.LeftServo, Convert.ToInt32(storageModel.motorPositon.rowPosition));
        //		}
        //	}
        //	catch(Exception ex)
        //	{
        //		throw new Exception(ex.Message);
        //	}
        //      }
        #region Command
        /// <summary>
        /// 允许电机
        /// </summary>
        public RelayCommand<object> RunMoto
        {
            get => new RelayCommand<object>(arg =>
            {
                try
                {
                    LightOffAll();
                    if (this.CurrentSelectStorage == null)
                    {
                        WPFMessageBox.ShowDialog("请选择药品上药", true);
                        //  MessageBox.Show("请选择药品上药");
                        return;
                    }
                    if (!isRunning)
                    {
                        Task.Run(() =>
                        {
                            DoMedInTask(this.CurrentSelectStorage.MedPos);
                        });

                    }
                }
                catch (Exception EX)
                {
                    MessageBox.Show(EX.Message);
                }
            });
        }
       
        public RelayCommand<object> AddDrugs
        {
            get => new RelayCommand<object>(obj =>
            {
               
                VisualStateManager.GoToElementState(obj as UserControl, "NormalSuccess", true);
                VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedNormal", true);
                try
                {
                    if (this.CurrentSelectStorage == null)
                    {
                        MessageBox.Show("请选择药品，或者扫码上药");
                        return;
                    }
                    if (string.IsNullOrWhiteSpace(this.MedInCount))
                    {
                        MessageBox.Show("请输入要添加的药品数量");
                        return;
                    }
                    GlobalValue.LocalDataAccess.UpdateStoreTableAMTAdd(int.Parse(this.MedInCount), this.CurrentSelectStorage.MedOnlyCode, this.CurrentSelectStorage.MedName, this.CurrentSelectStorage.MedPos, GlobalValue.MachineID);
                    LightOff(this.CurrentSelectStorage.MedPos);
                    VisualStateManager.GoToElementState(obj as UserControl, "SaveSuccess", true);
                }
                catch(Exception ex)
                {
                    SaveFailedMessage = ex.Message;
                    //提示保存失败，包括异常信息
                    VisualStateManager.GoToElementState(obj as UserControl, "SaveFailedShow", true);
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
                BtnRunEnable = true;
                BtnMedInEnable = true;
                VisualStateManager.GoToElementState(arg as UserControl, "SaveFailedClose", true);
            });
        }
        #endregion
    }
}
