using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iron.IntelligentDispsingMachine.Models
{
    public class LedModel: INotifyPropertyChanged
    {
        public int X { get; set; }
        public int Y { get; set; }
        private bool _IsLight;
        public bool IsLight { 
        get { return _IsLight; }
            set { _IsLight = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsLight"));
            }
        
        } //是否点亮

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
