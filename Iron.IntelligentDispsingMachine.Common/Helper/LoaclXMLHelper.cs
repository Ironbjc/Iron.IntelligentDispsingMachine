using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Iron.IntelligentDispsingMachine.Common.Helper
{
    public class LoaclXMLHelper
    {
        public static readonly string path = AppDomain.CurrentDomain.BaseDirectory + "Config\\ApplicationConfig.xml";
        //根据名称，获得节点的Value植
        //"D:\IronProject\Iron.RapidDrugDelivery\Iron.RapidDrugDelivery\bin\Debug\net5.0-windows\Assets\Config\ApplicationConfig.xml"
        // D:\IronProject\Iron.RapidDrugDelivery\Iron.RapidDrugDelivery\bin\Debug\net5.0-windows\Assets\Config\ApplicationConfig.xml
        public static XmlConfig GetConfig()
        {
            try
            {
                //反射获取对象所有字段，再去配置文件夹做映射
                XmlConfig constConfig = new XmlConfig();
                Type t = typeof(XmlConfig);
                var result = t.GetFields();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                //XmlNodeList xmlNodeList=
                XmlNode xn1 = xmlDoc.SelectSingleNode("Config");
                XmlNode xn2 = xn1.SelectSingleNode("Connstr");
                foreach (var item in result)
                {
                    XmlNode xn3 = xn2.SelectSingleNode(item.Name);
                    Type fieldType = item.FieldType;
                    var nodeResult = xn3.InnerText;
                    var value = Convert.ChangeType(nodeResult, fieldType);
                    item.SetValue(constConfig, value);
                }
                //XmlNodeList xmlNodeList = xn.SelectNodes("Connstr");
                return constConfig;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 写入配置文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Description"></param>
        /// <param name="value"></param>
        public static void WriteConfig(string node, string Description, string value)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path);
                XmlNode root = xmlDoc.SelectSingleNode("Config");
                XmlNode List = root.SelectSingleNode(node);
                XmlNode xxNode = List.SelectSingleNode(Description);
                xxNode.InnerText = value;
                xmlDoc.Save(path);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
