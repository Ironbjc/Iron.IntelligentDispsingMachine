using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iron.IntelligentDispsingMachine.Entities
{
    public class StoreTable
    {
        public string MedOnlyCode { get; set; }
        public string MedName { get; set; }
        public string MedUnit { get; set; }
        public string MedPack { get; set; }
        public string MedPos { get; set; }
        public int MedNowAMT { get; set; }
        public string MedPYCode { get; set; }
        public string MedFactory { get; set; }
        public DateTime? MedValidTime { get; set; }
    }
}
