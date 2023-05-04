using System;

namespace Iron.IntelligentDispsingMachine.Models
{
    public class MenuModel
    {
        public bool IsSelected { get; set; }
        public int Key { get; set; }
        public string MenuHeader { get; set; }
        public string TargetView { get; set; }
        public string MenuIcon { get; set; }
    }
}
