using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iron.IntelligentDispsingMachine.Models
{
    public class StorageMedModel:INotifyPropertyChanged
    {
        private bool _Selected = false;

        public bool Selected
        {
            get { return _Selected; }
            set {
                _Selected = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Selected")); }
        }
        public string MedOnlyCode { get; set; }
        public string MedName { get; set; }
        
        public string MedUnit { get; set; }

        public string MedPack { get; set; }
        public string MedPos { get; set; }
        private int? _MedNotAMT;
        public int? MedNowAMT {
            get { return _MedNotAMT; }
            set { _MedNotAMT = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MedNowAMT"));
            }
                }
        public string MedPYCode { get; set; }
        public string MedFactory { get; set; }
        public DateTime? MedValidTime { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
