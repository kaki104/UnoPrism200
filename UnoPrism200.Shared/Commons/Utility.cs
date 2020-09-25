using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using UnoPrism200.Infrastructure.Interfaces;
using Windows.UI.Popups;

namespace UnoPrism200.Commons
{
    public class Utility : IUtility
    {
        private readonly IDialogService _dialogService;

        public Utility(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }

        public void ShowMessage(string message,
                IDialogParameters parameters = null,
                Action<IDialogResult> callback = null,
                string name = "MessageControl")
        {
            parameters = MakeParameters(message, parameters);
            _dialogService.ShowDialog(name, parameters, callback);
        }

        private static IDialogParameters MakeParameters(string message, IDialogParameters parameters)
        {
            if (parameters == null)
            {
                parameters = new DialogParameters
                {
                    {"message", message }
                };
            }
            else if (parameters.ContainsKey("message") == false)
            {
                parameters.Add("message", message);
            }

            return parameters;
        }

        public void ShowConfirm(string message,
            IDialogParameters parameters = null,
            Action<IDialogResult> callback = null,
            string name = "ConfirmControl")
        {
            parameters = MakeParameters(message, parameters);
            _dialogService.ShowDialog(name, parameters, callback);
        }

        public async void ShowMessageSimple(string message,
            Action<IDialogResult> callback = null)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
            callback?.Invoke(new DialogResult(ButtonResult.OK));
        }

        public async void ShowConfirmSimple(string message,
            Action<IDialogResult> callback = null)
        {
            var dialog = new MessageDialog(message);
            dialog.Commands.Add(new UICommand("OK"));
            dialog.Commands.Add(new UICommand("Cancel"));
            dialog.DefaultCommandIndex = 1;
            dialog.CancelCommandIndex = 1;
            var result = await dialog.ShowAsync();
            DialogResult dialogResult;
            if (result.Label == "OK")
                dialogResult = new DialogResult(ButtonResult.OK);
            else
                dialogResult = new DialogResult(ButtonResult.Cancel);
            callback?.Invoke(dialogResult);
        }
    }
}
