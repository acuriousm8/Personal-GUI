using Personal_GUI.Stores;
using Personal_GUI.ViewModels;
using System;

namespace Personal_GUI.Commands
{
    public class NavigateCommand<TViewModel> : CommandBase
        where TViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<TViewModel> _createViewModel;

        public NavigateCommand(NavigationStore navigationStore, Func<TViewModel> CreateViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = CreateViewModel;
        }

        public override void Execute(object parameter)
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
