using Iron.IntelligentDispsingMachine.CPLC;
using Iron.IntelligentDispsingMachine.DataAccess;
using log4net;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iron.IntelligentDispsingMachine.Common
{
    /// <summary>
    /// 全局变量
    /// </summary>
    public class GlobalValue
    {
        public static string DbName = "";
        public static string DbIp = "";
        public static string UserId = "";
        public static string Pwd = "";
        public static string MachineID = "";
        public static string ConfirmCode = "";
        public static string ServoPortCom = "";
        public static string DPortCom = "";
        public static string ArkLayer = "";
        public static string MedInFlag = "";
        public static string MotorAutoSpeed = "";
        public static string MotorHandSpeed = "";
        public static string MotorZeroSpeed = "";
        public static string MedOutMode = "";
        public static string ScanCom = "";

        public static string SqlConn = "";
        public static DeltaPLC DeltaPLC;
        public static SerialPort LedSerialPort;
        public static SerialPort ScanPort;
        public static LocalDataAccess LocalDataAccess;
        public static Dictionary<string, object> WindDic=new Dictionary<string, object>();
        public static ILog Loger;
    }
}
