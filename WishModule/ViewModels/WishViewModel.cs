using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;

namespace Wish.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    public class WishViewModel : ViewModelBase
    {
        private bool _powershellChecked;
        private bool _cmdChecked = true;
        private bool _vsChecked;
        private DelegateCommand _powershellSelectedCommand;
        private DelegateCommand _cmdSelectedCommand;
        private DelegateCommand _vsSelectedCommand;
        private string _title;

        public bool PowershellChecked
        {
            get
            {
                return _powershellChecked;
            }
            set
            {
                _powershellChecked = value;
                NotifyPropertyChanged("PowershellChecked");
            }
        }

        public bool CmdChecked
        {
            get
            {
                return _cmdChecked;
            }
            set
            {
                _cmdChecked = value;
                NotifyPropertyChanged("CmdChecked");
            }
        }

        public bool VsChecked
        {
            get
            {
                return _vsChecked;
            }
            set
            {
                _vsChecked = value;
                NotifyPropertyChanged("VsChecked");
            }
        }

        private void DoPowershellSelected()
        {
            CmdChecked = false;
            VsChecked = false;
            PowershellChecked = true;
            //_wishModel.SetRunner(new Powershell(), Title);
            //Execute();
        }

        public ICommand PowershellSelected
        {
            get 
            {
                return _powershellSelectedCommand ?? (_powershellSelectedCommand = new DelegateCommand(DoPowershellSelected));
            }
        }

        private void DoCmdSelected()
        {
            CmdChecked = true;
            VsChecked = false;
            PowershellChecked = false;
        }

        public ICommand CmdSelected
        {
            get
            {
                return _cmdSelectedCommand ?? (_cmdSelectedCommand = new DelegateCommand(DoCmdSelected));
            }
        }

        private void DoVsSelected()
        {
            CmdChecked = false;
            VsChecked = true;
            PowershellChecked = false;
        }

        public ICommand VsSelected
        {
            get
            {
                return _vsSelectedCommand ?? (_vsSelectedCommand = new DelegateCommand(DoVsSelected));
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                NotifyPropertyChanged("Title");
            }
        }
    }
}
