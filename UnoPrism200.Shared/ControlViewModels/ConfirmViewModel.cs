using Microsoft.Toolkit.Uwp.UI;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using UnoPrism200.Bases;

namespace UnoPrism200.ControlViewModels
{
    public class ConfirmViewModel : DialogViewModelBase
    {
        public ICommand CloseDialogCommand { get; set; }

        private string _messge;

        public string Message
        {
            get { return _messge; }
            set { SetProperty(ref _messge, value); }
        }

        protected override void Init()
        {
            CloseDialogCommand = new DelegateCommand<string>(OnCloseDialog);
        }

        private void OnCloseDialog(string obj)
        {
            ButtonResult result = ButtonResult.None;
            if (obj?.ToLower() == "true")
                result = ButtonResult.OK;
            else if (obj?.ToLower() == "false")
                result = ButtonResult.Cancel;
            RaiseRequestClose(new DialogResult(result));
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            parameters.TryGetValue("title", out string title);
            Title = title ?? "Confirmation";
            Message = parameters.GetValue<string>("message");
        }
    }
}
