﻿using Microsoft.Toolkit.Uwp.UI;
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
    public class MessageViewModel : DialogViewModelBase
    {
        public ICommand CloseDialogCommand { get; set; }

        private string _messge;

        public string Message
        {
            get { return _messge; }
            set { SetProperty(ref _messge ,value); }
        }

        public MessageViewModel()
            : base()
        {
            if (DesignTimeHelpers.IsRunningInApplicationRuntimeMode)
            {
                return;
            }
            Message = "The Uno Platform is a Universal Windows Platform Bridge that allows UWP-based code (C# and XAML) to run on iOS, Android, and WebAssembly. It provides the full API definitions of the UWP Windows 10 2004 (19041), and the implementation of parts of the UWP API, such as Windows.UI.Xaml, to enable UWP applications to run on these platforms.";
        }

        protected override void Init()
        {
            CloseDialogCommand = new DelegateCommand<string>(OnCloseDialog);
        }

        private void OnCloseDialog(string obj)
        {
            RaiseRequestClose(new DialogResult(ButtonResult.OK));
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            parameters.TryGetValue("title", out string title);
            Title = title ?? "Notification";
            Message = parameters.GetValue<string>("message");
        }
    }
}
