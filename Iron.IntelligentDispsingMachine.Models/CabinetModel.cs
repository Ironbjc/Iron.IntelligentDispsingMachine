using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iron.IntelligentDispsingMachine.Models
{
    public class CabinetModel: INotifyPropertyChanged
    {
        /// <summary>
        /// 1,2,3,4共4列，1.2左电机控制，3.4右电机控制
        /// </summary>
        public int Line { get; set; }//列ID


        public List<StorageCoreModel> StorageCoreModels { get; set; }
        public List<LedModel> ledModels { get; set; }//每一列的灯

        public List<StorageModel> storageModels { get; set; } //每一列的存储位置

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
