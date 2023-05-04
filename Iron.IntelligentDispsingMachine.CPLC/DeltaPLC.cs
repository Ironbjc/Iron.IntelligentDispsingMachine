using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Iron.IntelligentDispsingMachine.CPLC
{
    public class DeltaPLC
    {
        #region 变量

        delegate void DelegateClose(int conn_num);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr LoadLibrary(string dllPath);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern bool FreeLibrary(IntPtr hDll);

        // Data Access
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int RequestData(int comm_type, int conn_num, int slave_addr, int func_code, byte[] sendbuf, int sendlen);
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int ResponseData(int comm_type, int conn_num, ref int slave_addr, ref int func_code, byte[] recvbuf);

        // Serial Communication
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int OpenModbusSerial(int conn_num, int baud_rate, int data_len, char parity, int stop_bits, int modbus_mode);
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern void CloseSerial(int conn_num);
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int GetLastSerialErr();
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern void ResetSerialErr();

        // Socket Communication
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int OpenModbusTCPSocket(int conn_num, int ipaddr, int port);
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern void CloseSocket(int conn_num);
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int GetLastSocketErr();
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern void ResetSocketErr();
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int ReadSelect(int conn_num, int millisecs);

        // MODBUS Address Calculation
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int DevToAddrW(string series, string device, int qty);

        // Wrapped MODBUS Funcion : 0x01
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int ReadCoilsW(int comm_type, int conn_num, int slave_addr, int dev_addr, int qty, UInt32[] data_r, StringBuilder req, StringBuilder res);

        // Wrapped MODBUS Funcion : 0x02
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int ReadInputsW(int comm_type, int conn_num, int slave_addr, int dev_addr, int qty, UInt32[] data_r, StringBuilder req, StringBuilder res);

        // Wrapped MODBUS Funcion : 0x03
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int ReadHoldRegsW(int comm_type, int conn_num, int slave_addr, int dev_addr, int qty, UInt32[] data_r, StringBuilder req, StringBuilder res);
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int ReadHoldRegs32W(int comm_type, int conn_num, int slave_addr, int dev_addr, int qty, UInt32[] data_r, StringBuilder req, StringBuilder res);

        // Wrapped MODBUS Funcion : 0x04
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int ReadInputRegsW(int comm_type, int conn_num, int slave_addr, int dev_addr, int qty, UInt32[] data_r, StringBuilder req, StringBuilder res);

        // Wrapped MODBUS Funcion : 0x05		   
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int WriteSingleCoilW(int comm_type, int conn_num, int slave_addr, int dev_addr, UInt32 data_w, StringBuilder req, StringBuilder res);

        // Wrapped MODBUS Funcion : 0x06
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int WriteSingleRegW(int comm_type, int conn_num, int slave_addr, int dev_addr, UInt32 data_w, StringBuilder req, StringBuilder res);
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int WriteSingleReg32W(int comm_type, int conn_num, int slave_addr, int dev_addr, UInt32 data_w, StringBuilder req, StringBuilder res);

        // Wrapped MODBUS Funcion : 0x0F
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int WriteMultiCoilsW(int comm_type, int conn_num, int slave_addr, int dev_addr, int qty, UInt32[] data_w, StringBuilder req, StringBuilder res);

        // Wrapped MODBUS Funcion : 0x10
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int WriteMultiRegsW(int comm_type, int conn_num, int slave_addr, int dev_addr, int qty, UInt32[] data_w, StringBuilder req, StringBuilder res);
        [DllImport("DMT.dll", CharSet = CharSet.Auto)]
        static extern int WriteMultiRegs32W(int comm_type, int conn_num, int slave_addr, int dev_addr, int qty, UInt32[] data_w, StringBuilder req, StringBuilder res);


        readonly int conn_num = 0;
        readonly int baud_rate = 9600;
        readonly int data_len = 7;
        readonly char parity = 'E';
        readonly int stop_bits = 1;

        readonly int modbus_mode = 1; // 1:ASCII , 2:RTU
        int status = 0;
        readonly int comm_type = 0; // 0:RS-232 , 1:Ethernet

        DelegateClose CloseModbus;

        #endregion

        #region 常用枚举
        public enum ServoType
        {
            LeftServo,
            RightServo
        }

        public enum JogType
        {
            Up,
            Down,
            Stop
        }
        #endregion

        #region 构造函数
        public DeltaPLC(string comid)
        {
            CloseModbus = new DelegateClose(CloseSerial);
            conn_num = Convert.ToInt32(comid.Replace("COM", ""));
            CloseModbus = CloseSerial;
        }
        #endregion

        /// <summary>
        /// 建立连接PLC
        /// </summary>
        /// <returns></returns>
        public bool ConnectPLC()
        {
            try
            {
                status = OpenModbusSerial(conn_num, baud_rate, data_len, parity, stop_bits, modbus_mode);
                return status != -1;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 断开连接PLC
        /// </summary>
        public void DisConnectPLC()
        {
            CloseModbus(conn_num);
        }


        #region 调配机动作
        /// <summary>
        /// 伺服初始化
        /// </summary>
        public void ServoInit(ServoType servo)
        {
            string addr = servo == ServoType.LeftServo ? PLCAddress.addr_LeftServoInit : PLCAddress.addr_RightServoInit;
            // SendCmd(addr, 1);
            SendCmd(addr, 1);
        }

        /// <summary>
        /// 伺服初始化完成
        /// </summary>
        /// <param name="servo"></param>
        /// <returns></returns>
        public int ServoInitFinish(ServoType servo)
        {
            string addr = servo == ServoType.LeftServo ? PLCAddress.addr_LeftServoInit : PLCAddress.addr_RightServoInit;
            int rt = ReadCmd(addr);
            return rt;
        }

        /// <summary>
        /// 伺服手动JOG
        /// </summary>
        /// <param name="servo"></param>
        public void ServoJog(ServoType servo, JogType dir)
        {
            string addr = servo == ServoType.LeftServo ? PLCAddress.addr_LeftServoMove : PLCAddress.addr_RightServoMove;
            int val = 0;
            switch (dir)
            {
                case JogType.Up:
                    val = 1;
                    break;
                case JogType.Down:
                    val = 2;
                    break;
                case JogType.Stop:
                    val = 0;
                    break;
                default:
                    break;
            }
            SendCmd(addr, val);
        }

        /// <summary>
        /// 伺服定位
        /// </summary>
        /// <param name="servo">伺服</param>
        /// <param name="posno">位置</param>
        public void ServoPositionRun(ServoType servo, int posno)
        {
            string addr = servo == ServoType.LeftServo ? PLCAddress.addr_LeftServoPositionRun : PLCAddress.addr_RightServoPositionRun;
            SendCmd(addr, posno);
        }

        /// <summary>
        /// 伺服定位完成
        /// </summary>
        /// <param name="servo">伺服</param>
        /// <param name="posno">位置</param>
        public int ServoPositionRunFinish(ServoType servo)
        {
            string addr = servo == ServoType.LeftServo ? PLCAddress.addr_LeftServoPositionRun : PLCAddress.addr_RightServoPositionRun;
            return ReadCmd(addr);
        }

        /// <summary>
        /// 原点返回速度
        /// </summary>
        /// <param name="speed">速度</param>
        public void ServoBackZeroSpeed(int speed)
        {
            SendCmd(PLCAddress.addr_ServoBackZeroSpeed, speed);
        }

        /// <summary>
        /// 伺服自动速度
        /// </summary>
        /// <param name="speed">速度</param>
        public void ServoAutoSpeed(int speed)
        {
            SendCmd(PLCAddress.addr_ServoAutoSpeed, speed);
        }

        /// <summary>
        /// 手动JOG速度 
        /// </summary>
        /// <param name="speed">速度</param>
        public void ServoJogSpeed(int speed)
        {
            SendCmd(PLCAddress.addr_ServoJogSpeed, speed);
        }

        /// <summary>
        /// 伺服运行位置设定
        /// </summary>
        /// <param name="servo">伺服</param>
        /// <param name="posno">位置</param>
        public void ServoPositionSet(ServoType servo, int posno)
        {
            string addr = servo == ServoType.LeftServo ? PLCAddress.addr_LeftServoPositionSet : PLCAddress.addr_RightServoPositionSet;
            SendCmd(addr, posno);
        }

        /// <summary>
        /// 伺服故障清除
        /// </summary>
        public void ServoAlarmReset()
        {
            SendCmd(PLCAddress.addr_ServoAlarmReset, 1);
        }

        /// <summary>
        /// 获取伺服报警
        /// </summary>
        /// <returns></returns>
        public int GetServoAlarm()
        {
            return ReadCmd(PLCAddress.addr_GetServoAlarm);
        }
        #endregion
        private static readonly object Lock_SendAndRecive = new object();
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="_val">写入值</param>
        /// <param name="_addr">通信地址</param>
        public void SendCmd(string _addr, int _val)
        {
            lock (Lock_SendAndRecive)
            {
                try
                {
                    StringBuilder req = new StringBuilder(1024);
                    StringBuilder res = new StringBuilder(1024);
                    UInt32[] data_to_dev = new UInt32[1];
                    data_to_dev[0] = Convert.ToUInt32(_val);
                    string strProduct = "DVP";
                    string strDev = _addr;
                    int slave_addr = 0;
                    int dev_qty = 1;
                    int addr = DevToAddrW(strProduct, strDev, dev_qty);
                    int ret = WriteSingleRegW(comm_type, conn_num, slave_addr, addr, data_to_dev[0], req, res);
                    if (ret == -1)
                    {
                        WriteLog("发送失败,地址：" + _addr + ",写入值：" + _val.ToString());
                    }
                }
                catch (Exception ex)
                {
                    WriteLog("发送失败,地址：" + _addr + ",写入值：" + _val.ToString() + "错误信息：" + ex.Message.ToString());
                }
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="_addr">通信地址</param>
        public int ReadCmd(string _addr)
        {
            lock (Lock_SendAndRecive)
            {
                try
                {
                    StringBuilder req = new StringBuilder(1024);
                    StringBuilder res = new StringBuilder(1024);
                    UInt32[] data_from_dev = new UInt32[1];
                    data_from_dev[0] = 0;
                    string strProduct = "DVP";
                    string strDev = _addr;
                    int slave_addr = 0;
                    int dev_qty = 1;
                    int addr = DevToAddrW(strProduct, strDev, dev_qty);
                    int ret = ReadHoldRegsW(comm_type, conn_num, slave_addr, addr, dev_qty, data_from_dev, req, res);
                    if (ret == -1)
                    {
                        WriteLog("读取失败,地址：" + _addr);
                        return 0;
                    }
                    else
                    {
                        return Convert.ToInt32(data_from_dev[0]);
                    }
                }
                catch (Exception ex)
                {
                    WriteLog("读取失败地址：" + _addr + "错误信息：" + ex.Message.ToString());
                    return 0;
                }
            }
        }

        #region 方法
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="Msg">消息内容</param>
        /// <remarks></remarks>
        public static void WriteLog(string Msg)
        {
            try
            {
                string varAppPath = "";
                varAppPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "PLClog\\info";
                string strDate = "";
                strDate = Convert.ToString(System.DateTime.Now.ToString("yyyyMMdd"));
                //文件名
                string strFile = "";
                strFile = varAppPath + "\\" + strDate + ".log";
                WriteFile(varAppPath, strFile, Msg);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 将消息写入文件
        /// </summary>
        /// <param name="varAppPath">文件目录</param>
        /// <param name="strFile">文件名</param>
        /// <param name="Msg">消息内容</param>
        /// <remarks></remarks>
        private static void WriteFile(string varAppPath, string strFile, string Msg)
        {
            Directory.CreateDirectory(varAppPath);
            string head;
            head = System.DateTime.Now.ToString() + ":" + System.DateTime.Now.Minute.ToString();
            head = head + ":" + System.DateTime.Now.Second.ToString() + ":" + System.DateTime.Now.Millisecond.ToString();
            Msg = head + "\n" + Msg + "\n";
            StreamWriter SW = default(StreamWriter);
            SW = new StreamWriter(strFile, true);
            SW.WriteLine(Msg);
            SW.Flush();
            SW.Close();
        }
        #endregion
    }
}
