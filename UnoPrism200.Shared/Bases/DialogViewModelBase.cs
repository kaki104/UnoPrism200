using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnoPrism200.Bases
{
    public abstract class DialogViewModelBase : BindableBase, IDialogAware
    {

        #region Title
        private string _title = "Notification";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        } 
        #endregion

        public event Action<IDialogResult> RequestClose;

        public virtual bool CanCloseDialog()
        {
            return true;
        }

        public virtual void OnDialogClosed()
        {
        }

        public virtual void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}
