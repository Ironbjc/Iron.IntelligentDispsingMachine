using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iron.IntelligentDispsingMachine.Common
{
    public class ActionManager
    {
        public ActionManager()
        {

        }
        static Dictionary<string, Delegate> actionMap = new Dictionary<string, Delegate>();
        public static void Register<T>(string key, Delegate d)
        {
            if (!actionMap.ContainsKey(key))
                actionMap.Add(key, d);
        }
        public static void UnRegister(string key)
        {
            if (actionMap.ContainsKey(key))
                actionMap.Remove(key);
        }
        public static void Execute<T>(string key, T data)
        {
            if (actionMap.ContainsKey(key))
                actionMap[key].DynamicInvoke(data);
        }
        public static bool ExecuteAndResult<T>(string key, T data)
        {
            if (actionMap.ContainsKey(key))
            {
                var action = (actionMap[key] as Func<T, bool>);
                if (action == null)
                    return false;
                return action.Invoke(data);
            }
            return false;
        }
    }
}
