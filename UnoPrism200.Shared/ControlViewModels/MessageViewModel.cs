using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace UnoPrism200.ControlViewModels
{
    public class MessageViewModel : BindableBase, IDialogAware
    {
        public ICommand CloseDialogCommand { get; set; }

        private string _messge;

        public string Message
        {
            get { return _messge; }
            set { SetProperty(ref _messge ,value); }
        }

        public MessageViewModel()
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

        private void RaiseRequestClose(DialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

        private string _title = "Notification";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Message = parameters.GetValue<string>("message");
        }
    }
}
