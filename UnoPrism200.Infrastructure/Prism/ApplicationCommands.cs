using Prism.Commands;
using Prism.Mvvm;
using System.Linq;
using System.Windows.Input;
using UnoPrism200.Infrastructure.Interfaces;

namespace UnoPrism200.Infrastructure.Prism
{
    /// <summary>
    /// Application main command manager
    /// </summary>
    public class ApplicationCommands : BindableBase, IApplicationCommands
    {
        private bool _canFind;

        public bool CanFind
        {
            get => _canFind;
            set => SetProperty(ref _canFind, value);
        }

        public CompositeCommand FindCommand { get; }
            = new CompositeCommand();

        private bool _canCheck;

        public bool CanCheck
        {
            get => _canCheck;
            set => SetProperty(ref _canCheck, value);
        }

        public CompositeCommand CheckCommand { get; }
            = new CompositeCommand();

        private bool _canRefresh;

        public bool CanRefresh
        {
            get { return _canRefresh; }
            set { SetProperty(ref _canRefresh, value); }
        }

        public CompositeCommand RefreshCommand { get; }
            = new CompositeCommand();

        public ApplicationCommands()
        {
        }

        public void SetShellCommands(
            ICommand findCommand = null,
            ICommand checkCommand = null,
            ICommand refreshCommand = null)
        {
            FindCommand.RegisteredCommands
                .ToList()
                .ForEach(c => FindCommand.UnregisterCommand(c));
            if (findCommand != null)
            {
                FindCommand.RegisterCommand(findCommand);
            }
            CanFind = FindCommand.RegisteredCommands.Any();

            CheckCommand.RegisteredCommands
                .ToList()
                .ForEach(c => CheckCommand.UnregisterCommand(c));
            if (checkCommand != null)
            {
                CheckCommand.RegisterCommand(checkCommand);
            }
            CanCheck = CheckCommand.RegisteredCommands.Any();

            RefreshCommand.RegisteredCommands
                .ToList()
                .ForEach(c => RefreshCommand.UnregisterCommand(c));
            if (refreshCommand != null)
            {
                RefreshCommand.RegisterCommand(refreshCommand);
            }
            CanRefresh = RefreshCommand.RegisteredCommands.Any();
        }
    }
}
