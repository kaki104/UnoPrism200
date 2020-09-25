using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnoPrism200.Infrastructure.Interfaces
{
    public interface IUtility
    {
        void ShowConfirm(string message, IDialogParameters parameters = null, Action<IDialogResult> callback = null, string name = "ConfirmControl");
        
        void ShowMessage(string message, IDialogParameters parameters = null, Action<IDialogResult> callback = null, string name = "MessageControl");

        void ShowMessageSimple(string message, Action<IDialogResult> callback = null);

        void ShowConfirmSimple(string message, Action<IDialogResult> callback = null);
    }
}
