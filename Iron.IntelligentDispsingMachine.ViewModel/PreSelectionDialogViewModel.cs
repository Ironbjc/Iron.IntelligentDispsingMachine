using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Iron.IntelligentDispsingMachine.Common;
using Iron.IntelligentDispsingMachine.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Iron.IntelligentDispsingMachine.ViewModel
{
    public class PreSelectionDialogViewModel:ViewModelBase
    {
		private ObservableCollection<PreNoModel> _PreList;

		public ObservableCollection<PreNoModel> PreList
		{
			get { return _PreList; }
			set {
				Set(ref _PreList, value);
			}
		}
		private PreNoModel _CurrentPre;

		public PreNoModel CurrentPre
		{
			get { return _CurrentPre; }
			set { Set(ref _CurrentPre, value); }
		}

		public PreSelectionDialogViewModel()
		{
			try
			{
				var result = GlobalValue.LocalDataAccess.GetAllPreList(GlobalValue.MachineID);
				PreList = new ObservableCollection<PreNoModel>(result.ToArray().Select(p => new PreNoModel()
				{
					PreNo=p.PresNo,
					Name=p.PName
				}));
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		public ICommand CloseWin
		{
			get => new RelayCommand<object>(arg =>
			{
				var win = arg as Window;
				win.DialogResult = false;
				win.Close();
			});
		}
		public ICommand DeletePreCommand
		{
			get => new RelayCommand<object>(arg =>
			{
				if(this.CurrentPre==null)
				{
					WPFMessageBox.ShowDialog("请选择要删除的处方", true);
					return;
				}
			});
		}
		public ICommand SelectPreCommand
		{
			get => new RelayCommand<object>(arg =>
			{
                if (this.CurrentPre == null)
                {
                    WPFMessageBox.ShowDialog("请选择出药处方", true);
					return;
                }
			
                var win = arg as Window;
                win.DialogResult = false;
                win.Close();
                Messenger.Default.Send(this.CurrentPre, "PreSend");
            });
		}
    }
}
