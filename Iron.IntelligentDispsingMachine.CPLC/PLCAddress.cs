using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iron.IntelligentDispsingMachine.CPLC
{
    public class PLCAddress
    {
        /// <summary>
        /// 左侧伺服初始化回原点 1：回原点 2：回原点完成 3：异常停止
        /// </summary>
        public const string addr_LeftServoInit = "D10";
        /// <summary>
        /// 右侧伺服初始化回原点 1：回原点 2：回原点完成 3：异常停止
        /// </summary>
        public const string addr_RightServoInit = "D11";
        /// <summary>
        /// 左侧伺服手动JOG 1:上升 2：下降 0：停止
        /// </summary>
        public const string addr_LeftServoMove = "D1";
        /// <summary>
        /// 右侧伺服手动JOG 1:上升 2：下降 0：停止
        /// </summary>
        public const string addr_RightServoMove = "D2";
        /// <summary>
        /// 左侧伺服自动位置运行 1、2、3运行位置 4：到位完成 5：异常
        /// </summary>
        public const string addr_LeftServoPositionRun = "D20";
        /// <summary>
        /// 右侧伺服自动位置运行 1、2、3运行位置 4：到位完成 5：异常
        /// </summary>
        public const string addr_RightServoPositionRun = "D21";
        /// <summary>
        /// 原点返回速度 
        /// </summary>
        public const string addr_ServoBackZeroSpeed = "D600";
        /// <summary>
        /// 手动JOG速度 
        /// </summary>
        public const string addr_ServoJogSpeed = "D604";
        /// <summary>
        /// 自动速度 
        /// </summary>
        public const string addr_ServoAutoSpeed = "D606";
        /// <summary>
        /// 左侧伺服位置设定 
        /// </summary>
        public const string addr_LeftServoPositionSet = "D3";
        /// <summary>
        /// 右侧伺服位置设定 
        /// </summary>
        public const string addr_RightServoPositionSet = "D4";
        /// <summary>
        /// 伺服故障清除
        /// </summary>
        public const string addr_ServoAlarmReset = "D60";
        /// <summary>
        /// 报警
        /// </summary>
        public const string addr_GetServoAlarm = "D50";
    }
}
