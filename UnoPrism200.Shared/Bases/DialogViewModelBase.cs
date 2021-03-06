﻿using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnoPrism200.Bases
{
    public abstract class DialogViewModelBase : BindableBase, IDialogAware
    {

        #region Title
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        #endregion

        public DialogViewModelBase()
        {
            Init();
        }

        protected virtual void Init()
        {
        }

        public event Action<IDialogResult> RequestClose;

        protected void RaiseRequestClose(DialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

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
