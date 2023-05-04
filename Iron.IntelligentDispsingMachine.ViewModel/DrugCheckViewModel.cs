using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Iron.IntelligentDispsingMachine.Common;
using Iron.IntelligentDispsingMachine.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using static Iron.IntelligentDispsingMachine.CPLC.DeltaPLC;

namespace Iron.IntelligentDispsingMachine.ViewModel
{
    public class DrugCheckViewModel:ViewModelBase
    {
        //private bool _IsPopopOpen;

        //public bool IsPopopOpen
        //{
        //    get { return _IsPopopOpen; }
        //    set { Set(ref _IsPopopOpen, value); }
        //}
        private Visibility _ProgressVisibility = Visibility.Hidden;

        public Visibility ProgressVisibility
        {
            get { return _ProgressVisibility; }
            set { Set(ref _ProgressVisibility, value); }
        }
        private bool _GridLoading = false;

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
        private StorageMedModel _CurrentSelectStorage;

        public StorageMedModel CurrentSelectStorage
        {
            get { return _CurrentSelectStorage; }
            set {
                Set(ref _CurrentSelectStorage, value);
                foreach (var item in cabinetModels)
                {
                    foreach (var med in item.StorageCoreModels)
                    {
                        med.Med.Selected = false;
                    }
                }
                if (value != null)
                {
                    foreach (var item in cabinetModels)
                    {
                        var result = item.StorageCoreModels.Where(p => p.MedPos == this._CurrentSelectStorage.MedPos).FirstOrDefault();
                        if (result != null)
                            result.Med.Selected = true;
                    }
                }

            }
        }

        private List<StorageMedModel> _StorageMedList=new List<StorageMedModel>();

        public List<StorageMedModel> StorageMedList
        {
            get { return _StorageMedList; }
            set { Set(ref _StorageMedList, value); }
        }
        private string _PYCode;

        public string PYCode
        {
            get { return _PYCode; }

            set
            {
                Set(ref _PYCode, value);
                var result = GlobalValue.LocalDataAccess.GetMedInStoreByPYCode(_PYCode, GlobalValue.MachineID);
                if (result != null)
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
        private List<CabinetModel> _cabinetModels = new List<CabinetModel>();
        public List<CabinetModel> cabinetModels
        {

            get
            { return _cabinetModels; }
            set
            {
                Set(ref _cabinetModels, value);
            }
        }
        public List<string> MedPosList = new List<string>();

        public DrugCheckViewModel()
        {
            #region Init
            for (int i = 1; i < 5; i++)
            {
                for (int j = 1; j < 28; j++)
                {
                    MedPosList.Add($"{GlobalValue.MachineID}" + i.ToString("00") + j.ToString("00"));
                }
            }
            for (int i = 1; i < 5; i++)
            {
                cabinetModels.Add(new CabinetModel()
                {
                    Line = i,
                    StorageCoreModels = GetCodeModels(i)
                });

            }
            #endregion
        }
        private void Refresh()
        {
            //cabinetModels = new List<CabinetModel>();
            for (int i = 1; i < 5; i++)
            {
                for (int j = 1; j < 28; j++)
                {
                    MedPosList.Add($"{GlobalValue.MachineID}" + i.ToString("00") + j.ToString("00"));
                }
            }
            for (int i = 1; i < 5; i++)
            {
                cabinetModels.Add(new CabinetModel()
                {
                    Line = i,
                    StorageCoreModels = GetCodeModels(i)
                });

            }
        }
        private List<StorageCoreModel> GetCodeModels(int i)
        {
            try
            {
                List<StorageCoreModel> codeModels = new List<StorageCoreModel>();
                var result = this.MedPosList.Where(p => p.Substring(2, 2) == i.ToString("00")).ToList();
                foreach (var item in result)
                {
                    codeModels.Add(new StorageCoreModel(item));
                }
                return codeModels;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void DoMedInTask(string MedPosition)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
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
            //ProgressMessage = "电机正在运行";
            int k = 0;
            bool IsAlarm = false;
            #region 模拟
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
            {
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
                                    var obj = GlobalValue.WindDic["DrugCheckView"];

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
                                    var obj = GlobalValue.WindDic["DrugCheckView"];

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
                            var obj = GlobalValue.WindDic["DrugCheckView"];

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
            }
            #endregion
            if(IsAlarm)
            {
                return;
            }
            //点亮灯
            LightUp(this.CurrentSelectStorage.MedPos);
            //点亮DataGridView的行

            #endregion
        }
        private void MotoRun(ServoType servoType,int pos)
        {
            GlobalValue.DeltaPLC.ServoPositionRun(servoType,pos);

            bool Finish = false;
            //ProgressMessage = "电机正在运行";
            int k = 0;
            bool IsAlarm = false;
      
            #region 实际运行
            {
                do
                {
                    switch (servoType)
                    {
                        //如果是左侧的药品
                        case ServoType.LeftServo:
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
                                    var obj = GlobalValue.WindDic["DrugCheckView"];

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
                        case ServoType.RightServo:
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
                                    var obj = GlobalValue.WindDic["DrugCheckView"];

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
                            var obj = GlobalValue.WindDic["DrugCheckView"];

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
            }
            #endregion
            if (IsAlarm)
            {
                return;
            }
      
        }
        #region 灯的相关操作
        /// <summary>
        /// 亮灯
        /// </summary>
        /// <param name="medpos"></param>
        private void LightUp(string medpos)
        {

            StorageCoreModel storageCoreModel = new StorageCoreModel(medpos);
             SendByteTOPCB(storageCoreModel.Led.X, storageCoreModel.Led.Y);
         
        }
        /// <summary>
        /// 灭灯
        /// </summary>
        /// <param name="medpos"></param>
        private void LightOff(string medpos)
        {
            SendByteTOPCBOff();
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
        /// <summary>
        /// 关闭所有的灯
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
        #endregion
        #region Command
        public RelayCommand<object> SelectMed
        {
            get => new RelayCommand<object>(arg =>
            {
                if (arg != null)
                {
                    foreach (var item in cabinetModels)
                    {
                        foreach (var med in item.StorageCoreModels)
                        {
                            med.Med.Selected = false;
                        }
                    }
                    var result = arg as StorageMedModel;
                    result.Selected = true;
                    this.CurrentSelectStorage = result;
                }
                    
            });
        }

        public ICommand UpdateMedCount
        {
            get => new RelayCommand<object>(arg =>
            {
                if (this.CurrentSelectStorage == null)
                {
                    MessageBox.Show("请选中需要盘点的药品");
                    return;
                }
                //IsPopopOpen = true; 
                if (ActionManager.ExecuteAndResult("UpdateMedCount", new UpdateMedCountDialogViewModel(this.CurrentSelectStorage)))
                {
                    //添加成功
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        foreach (var item in cabinetModels)
                        {
                            var result = item.StorageCoreModels.Where(p => p.MedPos == this.CurrentSelectStorage.MedPos).FirstOrDefault();
                            if (result != null)
                            {
                                var r1 = new StorageCoreModel(this.CurrentSelectStorage.MedPos);
                                result.Med.MedNowAMT = r1.Med.MedNowAMT;
                            }
                        }
                    });
                     
                }
                else
                {
                    //取消添加,不做刷新动作
                }
            });
        }
        public ICommand RunMoto
        {
            get => new RelayCommand<object>(arg =>
            {
                if(this.CurrentSelectStorage==null)
                {
                    MessageBox.Show("请选择要盘点的药品");
                    return;
                }
                Task.Run(() =>
                {
                    DoMedInTask(this.CurrentSelectStorage.MedPos);
                });
            });
        }

        /// <summary>
        /// 关闭电机运行报警
        /// </summary>
        public ICommand CloseSaveFailedCommand
        {
            get => new RelayCommand<object>(arg =>
            {
                //清除伺服故障
                GlobalValue.DeltaPLC.ServoAlarmReset();
                //BtnRunEnable = true;
                //BtnMedInEnable = true;
                VisualStateManager.GoToElementState(arg as UserControl, "SaveFailedClose", true);
            });
        }
        public RelayCommand<object> MultiBtnCommand
        {
            get => new RelayCommand<object>(arg =>
            {
                var Index = arg.ToString();
                switch (Index)
                {
                    case "Left-1":
                        MotoRun(ServoType.LeftServo, 1);
                        break;
                    case "Left-2":
                        MotoRun(ServoType.LeftServo, 2);
                        break;
                    case "Left-3":
                        MotoRun(ServoType.LeftServo, 3);
                        break;
                    case "Right-1":
                        MotoRun(ServoType.RightServo, 1);
                        break;
                    case "Right-2":
                        MotoRun(ServoType.RightServo, 2);
                        break;
                    case "Right-3":
                        MotoRun(ServoType.RightServo, 3);
                        break;
                }
            });
        }
        #endregion
    }
}
