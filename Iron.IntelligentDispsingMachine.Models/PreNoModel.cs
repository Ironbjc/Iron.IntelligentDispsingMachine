using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iron.IntelligentDispsingMachine.Models
{
    public class PreNoModel:INotifyPropertyChanged
    {
        private string _PreNo;
        public string PreNo {
            get{ return _PreNo; }
            set{
                _PreNo = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PreNo"));
            }
        }//处方号

        private bool _IsCurrentInPositon;

        public bool IsCurrentInPositon
        {
            get { return _IsCurrentInPositon; }
            set { _IsCurrentInPositon = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsCurrentInPositon"));
            }
        }

        public string Name { get; set; } //患者姓名
        public string MedPos { get; set; }//位置
        public string MedName { get; set; }//药品名称
        public bool OutFlag { get; set; } //是否出药
        public float MedOutAMT { get; set; } //出药数量
        public string MedOnlyCode { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
