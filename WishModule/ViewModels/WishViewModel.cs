using System;
using System.ComponentModel.Composition;
using System.Windows.Input;
using Microsoft.Practices.Prism.Commands;
using ICSharpCode.AvalonEdit.Highlighting;
using System.Xml;
using Wish.Commands.Runner;
using Wish.Views;

namespace Wish.ViewModels
{
    [Export, PartCreationPolicy(CreationPolicy.Shared)]
    public class WishViewModel : ViewModelBase
    {
        private bool _powershellChecked = true;
        private bool _cmdChecked;
        private bool _vsChecked;
        private bool _darkChecked;
        private bool _zenburnChecked = true;
        private bool _lightChecked;
        private DelegateCommand _powershellSelectedCommand;
        private DelegateCommand _cmdSelectedCommand;
        private DelegateCommand _vsSelectedCommand;
        private DelegateCommand _darkSelectedCommand;
        private DelegateCommand _zenburnSelectedCommand;
        private DelegateCommand _lightSelectedCommand;
        private string _title;
        public WishModel Model { get; set; }

        private string _background;
        public string Background 
        {
            get { return _background; }
            set 
            {
                _background = value;
                NotifyPropertyChanged("Background");
            }
        }

        private string _foreground;
        public string Foreground 
        {
            get { return _foreground; }
            set 
            {
                _foreground = value;
                NotifyPropertyChanged("Foreground");
            }
        }

        [ImportingConstructor]
        public WishViewModel(WishModel model)
        {
            Model = model;
            // need to change initial bg and fg based on theme
            SetSyntaxHighlighting("Wish.Views.Zenburn.xshd");
            Background = "#3f3f3f";
            Foreground = "#d7d7c8";
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

        public bool DarkChecked
        {
            get
            {
                return _darkChecked;
            }
            set
            {
                _darkChecked = value;
                NotifyPropertyChanged("DarkChecked");
            }
        }

        public bool ZenburnChecked
        {
            get
            {
                return _zenburnChecked;
            }
            set
            {
                _zenburnChecked = value;
                NotifyPropertyChanged("ZenburnChecked");
            }
        }

        public bool LightChecked
        {
            get
            {
                return _lightChecked;
            }
            set
            {
                _lightChecked = value;
                NotifyPropertyChanged("LightChecked");
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

        public ICommand DarkSelected
        {
            get
            {
                return _darkSelectedCommand ?? (_darkSelectedCommand = new DelegateCommand(DoDarkSelected));
            }
        }

        private void DoDarkSelected()
        {
            DarkChecked = true;
            ZenburnChecked = false;
            LightChecked = false;
            Background = "black";
            Foreground = "#d7d7c8";
            SetSyntaxHighlighting("Wish.Views.Dark.xshd");
        }

        public ICommand ZenburnSelected
        {
            get
            {
                return _zenburnSelectedCommand ?? (_zenburnSelectedCommand = new DelegateCommand(DoZenburnSelected));
            }
        }

        private void DoZenburnSelected()
        {
            DarkChecked = false;
            ZenburnChecked = true;
            LightChecked = false;
            Background = "#3f3f3f";
            Foreground = "#d7d7c8";
            SetSyntaxHighlighting("Wish.Views.Zenburn.xshd");
        }

        public ICommand LightSelected
        {
            get
            {
                return _lightSelectedCommand ?? (_lightSelectedCommand = new DelegateCommand(DoLightSelected));
            }
        }

        private void DoLightSelected()
        {
            DarkChecked = false;
            ZenburnChecked = false;
            LightChecked = true;
            Background = "white";
            Foreground = "black";
            SetSyntaxHighlighting("Wish.Views.Light.xshd");
        }

        public void SetSyntaxHighlighting(string xshdFile)
        {
            IHighlightingDefinition customHighlighting;
            var type = typeof(WishView);
            using (var s = type.Assembly.GetManifestResourceStream(xshdFile))
            {
                if (s == null)
                {
                    throw new InvalidOperationException("Could not find embedded resource");
                }
                using (XmlReader reader = new XmlTextReader(s))
                {
                    customHighlighting = ICSharpCode.AvalonEdit.Highlighting.Xshd.
                        HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            HighlightingManager.Instance.RegisterHighlighting("Custom Highlighting", null, customHighlighting);
            Theme = customHighlighting;
        }

        private IHighlightingDefinition _theme;
        public IHighlightingDefinition Theme
        {
            get { return _theme; }
            set
            {
                _theme = value;
                NotifyPropertyChanged("Theme");
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
