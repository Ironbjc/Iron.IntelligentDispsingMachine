using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iron.IntelligentDispsingMachine.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SystemDebugViewModel>();
            SimpleIoc.Default.Register<LedSetViewModel>();
            SimpleIoc.Default.Register<DrugOutViewModel>();
            SimpleIoc.Default.Register<DrugInViewModel>();
            SimpleIoc.Default.Register<DrugCheckViewModel>();
            SimpleIoc.Default.Register<DrugMaintainViewModel>();
         
        }
        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();
        public SystemDebugViewModel SystemDebugViewModel => ServiceLocator.Current.GetInstance<SystemDebugViewModel>();
        public LedSetViewModel LedSetViewModel => ServiceLocator.Current.GetInstance<LedSetViewModel>();
        public DrugOutViewModel DrugOutViewModel => ServiceLocator.Current.GetInstance<DrugOutViewModel>();
        public DrugInViewModel DrugInViewModel => ServiceLocator.Current.GetInstance<DrugInViewModel>();
        public DrugCheckViewModel DrugCheckViewModel => ServiceLocator.Current.GetInstance<DrugCheckViewModel>();

        public DrugMaintainViewModel DrugMaintainViewModel => ServiceLocator.Current.GetInstance<DrugMaintainViewModel>();
        public static void Cleanup<T>()where T:ViewModelBase
        {
            ServiceLocator.Current.GetInstance<T>().Cleanup();
            SimpleIoc.Default.Unregister<T>();
            SimpleIoc.Default.Register<T>();
        }
    }
}
