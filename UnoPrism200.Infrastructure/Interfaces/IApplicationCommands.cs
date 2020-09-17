using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace UnoPrism200.Infrastructure.Interfaces
{
    public interface IApplicationCommands
    {
        bool CanCheck { get; set; }
        bool CanFind { get; set; }
        bool CanRefresh { get; set; }
        CompositeCommand CheckCommand { get; }
        CompositeCommand FindCommand { get; }
        CompositeCommand RefreshCommand { get; }

        void SetShellCommands(ICommand findCommand = null, ICommand checkCommand = null, ICommand refreshCommand = null);
    }
}
