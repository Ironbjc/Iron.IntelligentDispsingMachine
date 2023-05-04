using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Iron.IntelligentDispsingMachine.Common;
using Iron.IntelligentDispsingMachine.Entities;
using Iron.IntelligentDispsingMachine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Iron.IntelligentDispsingMachine.ViewModel
{
    public class DrugMaintainViewModel:ViewModelBase
    {
        public override void Cleanup()
        {
            base.Cleanup();
            Messenger.Default.Unregister<string>(this, "ScanPortMessage");
        }
        public DrugMaintainViewModel()
        {
            Messenger.Default.Register<string>(this, "ScanPortMessage", s =>
            {
                if (s.Length < 18)
                {
                   
                    this.BarCode = s;
                }
                else
                {
                    this.SupervisoryCode = s;
                }
            });
        }
        private List<DrugDetail> _DrugDetailList=new List<DrugDetail>();

        public List<DrugDetail> DrugDetailList
        {
            get { return _DrugDetailList; }
            set { Set(ref _DrugDetailList, value); }
        }
        private DrugDetail _CurrentSelectDrug;

        public DrugDetail CurrentSelectDrug
        {
            get { return _CurrentSelectDrug; }
            set { Set(ref _CurrentSelectDrug, value); }
        }


        private string _PYCode;

		public string PYCode
		{
			get { return _PYCode; }
			set { 
                Set(ref _PYCode, value);
                try
                {
                    if (!string.IsNullOrEmpty(this._PYCode))
                    {
                        var result = GlobalValue.LocalDataAccess.GetDrugDetailByPYCode(_PYCode);
                        if (result != null && result.Count() > 0)
                            this.DrugDetailList = result;
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
		private string _BarCode="";

		public string BarCode
		{
			get { return _BarCode; }
			set { Set(ref _BarCode, value);
                if (!string.IsNullOrEmpty(_BarCode))
                {
                    var result = GlobalValue.LocalDataAccess.GetDrugInfosByBarCode(_BarCode);
                    if (result != null && result.Count() > 0)
                        this.DrugDetailList = result;
                }

            }
		}
        /// <summary>
        /// 监管码
        /// </summary>

        private string _SupervisoryCode="";

        public string SupervisoryCode
        {
            get { return _SupervisoryCode; }
            set
            {
                Set(ref _SupervisoryCode, value);
                //根据监管码查询
            }
        }

        #region Command
        public ICommand SaveCode
        {
            get => new RelayCommand<object>(arg =>
            {
                if (string.IsNullOrEmpty(this.BarCode) && string.IsNullOrEmpty(this.SupervisoryCode))
                {
                    MessageBox.Show("请扫条形码或监管码");
                    return;
                }
                GlobalValue.LocalDataAccess.UpdateDrugCode(this.CurrentSelectDrug.MedOnlyCode, this.CurrentSelectDrug.MedName,this.BarCode, this.SupervisoryCode);

            });
        }
        public ICommand DeleteCode
        {
            get => new RelayCommand<object>(arg =>
            {
                if (this.CurrentSelectDrug != null)
                {
                    if (string.IsNullOrEmpty(this.CurrentSelectDrug.MedBarCode) && string.IsNullOrEmpty(this.CurrentSelectDrug.MedMonitorCode))
                    {
                        MessageBox.Show("该药品没有绑定条形码或监管码");
                        return;
                    }
                    else
                    {
                        GlobalValue.LocalDataAccess.DeleteBarCode(this.CurrentSelectDrug.MedBarCode, this.CurrentSelectDrug.MedMonitorCode);
                    }
                }
                else
                {
                    MessageBox.Show("请选择要删除的药品");
                    return;
                }
            });
        }
        #endregion
    }
}
