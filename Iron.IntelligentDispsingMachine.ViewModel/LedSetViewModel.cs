using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Iron.IntelligentDispsingMachine.Common;
using Iron.IntelligentDispsingMachine.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Iron.IntelligentDispsingMachine.ViewModel
{
    public class LedSetViewModel : ViewModelBase
    {
        public ObservableCollection<CabinetModel> CabineModels { get; set; } = new ObservableCollection<CabinetModel>();
        public LedSetViewModel()
        {
            for (int i = 1; i < 5; i++)
            {
                CabineModels.Add(new CabinetModel()
                {
                    Line = i,
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

        #region 
        private int _PCBIdIndex = -1;

        public int PCBIdIndex
        {
            get { return _PCBIdIndex; }
            set { Set(ref _PCBIdIndex, value); }
        }
        private bool _FirstCheck;

        public bool FirstCheck
        {
            get { return _FirstCheck; }
            set { Set(ref _FirstCheck, value); }
        }
        private bool _SecondCheck;

        public bool SecondCheck
        {
            get { return _SecondCheck; }
            set { Set(ref _SecondCheck, value); }
        }
        private bool _ThirdCheck;

        public bool ThirdCheck
        {
            get { return _ThirdCheck; }
            set { Set(ref _ThirdCheck, value); }
        }
        #endregion

        #region Command
        /// <summary>
        ///选择需要打开的灯
        /// </summary>
        public RelayCommand<object> OpenLedByChoose
        {
            get => new RelayCommand<object>(arg =>
            {
                //先关闭所有的灯
                foreach (var item in CabineModels)
                {
                    foreach (var led in item.ledModels)
                    {
                        led.IsLight = false;
                    }
                }
                if (this.PCBIdIndex == -1)
                {
                    MessageBox.Show("请选中电路板ID");
                    return;
                }
                if (!FirstCheck && !SecondCheck && !ThirdCheck)
                {
                    MessageBox.Show("请选择灯ID");
                    return;
                }
                try
                {
                    #region 拼接字节数组               
                    var index = this.PCBIdIndex + 1;
                    var ledResult = (FirstCheck == true ? 1 : 0) * 1 + (SecondCheck == true ? 1 : 0) * 2 + (ThirdCheck == true ? 1 : 0) * 4;
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
                    var Cab = CabineModels.FirstOrDefault(i => i.Line == (this.PCBIdIndex) / 3 + 1);
                    foreach (var item in Cab.ledModels)
                    {
                        if (item.X == this.PCBIdIndex + 1)
                        {
                            if (this.FirstCheck && item.Y == 1)
                                item.IsLight = true;
                            if (this.SecondCheck && item.Y == 2)
                                item.IsLight = true;
                            if (this.ThirdCheck && item.Y == 3)
                                item.IsLight = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

        public RelayCommand<object> OpenAllLed
        {
            get => new RelayCommand<object>(arg =>
            {
                try
                {
                    foreach (var item in CabineModels)
                    {
                        foreach (var led in item.ledModels)
                        {
                            led.IsLight = false;
                        }
                    }
                    #region 全部亮灯
                    byte[] sendStr = new byte[7];
                    sendStr[0] = 0xFF;
                    sendStr[1] = 0xFF;
                    sendStr[2] = 0x00;
                    sendStr[3] = 0x00;
                    sendStr[4] = 0x05;
                    sendStr[5] = 0x03;
                    sendStr[6] = 0xC0;
                    GlobalValue.LedSerialPort.DiscardOutBuffer();
                    GlobalValue.LedSerialPort.DiscardInBuffer();
                    GlobalValue.LedSerialPort.Write(sendStr, 0, 7);
                    foreach (var item in CabineModels)
                    {
                        foreach (var led in item.ledModels)
                        {
                            led.IsLight = true;
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }

        public RelayCommand<object> LightOffAllLed
        {
            get => new RelayCommand<object>(arg =>
            {
                #region 全部灭灯
                byte[] sendStr11 = new byte[7];
                sendStr11[0] = 0xFF;
                sendStr11[1] = 0xFF;
                sendStr11[2] = 0x00;
                sendStr11[3] = 0x00;
                sendStr11[4] = 0x04;
                sendStr11[5] = 0xC3;
                sendStr11[6] = 0x01;
                GlobalValue.LedSerialPort.DiscardOutBuffer();
                GlobalValue.LedSerialPort.DiscardInBuffer();
                GlobalValue.LedSerialPort.Write(sendStr11, 0, 7);
                foreach (var item in CabineModels)
                {
                    foreach (var led in item.ledModels)
                    {
                        led.IsLight = false;
                    }
                }
                #endregion
            });
        }
        public RelayCommand<object> SetID
        {
            get => new RelayCommand<object>(arg =>
            {
                #region 烧制ID
                #region 效验码验证定义
                byte[] CRC = new byte[2];
                ushort CRCFull = 0xFFFF;
                byte CRCHigh = 0xFF, CRCLow = 0xFF;
                char CRCLSB;
                #endregion

                int ID = this.PCBIdIndex + 1; ;
                if (ID == 0)
                {
                    MessageBox.Show("请选择ID");
                    return;
                }

                byte[] sendStr = new byte[8];
                sendStr[0] = 0x00;
                sendStr[1] = Convert.ToByte(ID);
                sendStr[2] = 0x00;
                sendStr[3] = 0x00;
                sendStr[4] = 0x06;
                sendStr[5] = 0xC8;
                #region 效验码验证求值
                for (int i = 0; i < (sendStr.Length - 2); i++)
                {
                    CRCFull = (ushort)(CRCFull ^ sendStr[i]);

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
                CRC[1] = CRCHigh = (byte)((CRCFull >> 8) & 0xFF);
                CRC[0] = CRCLow = (byte)(CRCFull & 0xFF);
                #endregion
                sendStr[6] = CRC[1];
                sendStr[7] = CRC[0];
                GlobalValue.LedSerialPort.DiscardOutBuffer();
                GlobalValue.LedSerialPort.DiscardInBuffer();
                GlobalValue.LedSerialPort.Write(sendStr, 0, 8);
     
                #endregion
            });
        }
        #endregion
    }
}
