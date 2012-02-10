using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using Wish.Commands.Runner;

namespace Wish.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    public class WishViewModel : ViewModelBase
    {
        private bool _powershellChecked = true;
        private bool _cmdChecked;
        private bool _vsChecked;
        private DelegateCommand _powershellSelectedCommand;
        private DelegateCommand _cmdSelectedCommand;
        private DelegateCommand _vsSelectedCommand;
        private string _title;
        public WishModel Model { get; set; }


        [ImportingConstructor]
        public WishViewModel(WishModel model)
        {
            Model = model;
        }

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

        public void DoPowershellSelected()
        {
            CmdChecked = false;
            VsChecked = false;
            PowershellChecked = true;
            Model.SetRunner(new Powershell(), Title);
            //Execute();
        }

        public ICommand PowershellSelected
        {
            get 
            {
                return _powershellSelectedCommand ?? (_powershellSelectedCommand = new DelegateCommand(DoPowershellSelected));
            }
        }

        public void DoCmdSelected()
        {
            CmdChecked = true;
            VsChecked = false;
            PowershellChecked = false;
            Model.SetRunner(new Cmd(), Title);
        }

        public ICommand CmdSelected
        {
            get
            {
                return _cmdSelectedCommand ?? (_cmdSelectedCommand = new DelegateCommand(DoCmdSelected));
            }
        }

        public void DoVsSelected()
        {
            CmdChecked = false;
            VsChecked = true;
            PowershellChecked = false;
            //Model.SetRunner(new Vs(), Title);
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
