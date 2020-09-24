using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnoPrism200.Shared.Commons
{
    public class Utility
    {
        public void ShowMessage(string message, 
                IDialogParameters parameters = null, 
                Action<IDialogResult> callback = null,
                string name = "MessageControl")
        {

        }

        public IDialogResult ShowConfirm(string message,
            IDialogParameters parameters = null)
        {
            return new DialogResult(ButtonResult.None);
        }
        
    }
}
