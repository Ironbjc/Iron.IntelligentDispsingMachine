using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Iron.IntelligentDispsingMachine.Common;
using Iron.IntelligentDispsingMachine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Iron.IntelligentDispsingMachine.ViewModel
{
    public class UpdateMedCountDialogViewModel:ViewModelBase
    {
        private StorageMedModel _CurrentMed;

        public StorageMedModel CurrentMed
        {
            get { return _CurrentMed; }
            set { Set(ref _CurrentMed, value); }
        }

        public UpdateMedCountDialogViewModel(object obj)
        {
            var result = obj as StorageMedModel;
            this.CurrentMed =new StorageMedModel()
            {
                MedName=result.MedName,
                MedFactory=result.MedFactory,
                MedNowAMT=result.MedNowAMT,
                MedPos=result.MedPos
            };
        }
        public ICommand UpdateCommand
        {
            get => new RelayCommand<object>(arg =>
            {
                try
                {
                    GlobalValue.LocalDataAccess.UpdateStoreTableCheck(this.CurrentMed.MedPos, this.CurrentMed.MedNowAMT);
                    var result=   arg as Window;
                    result.DialogResult = true;
                    result.Close();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }
        public ICommand CancelCommand
        {
            get => new RelayCommand<object>(arg =>
            {
                var result = arg as Window;
                result.DialogResult = false;
                result.Close();
            });
        }
    }
}
