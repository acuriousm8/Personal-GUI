using Personal_GUI.Services;
using Personal_GUI.Stores;
using Personal_GUI.ViewModels;
using System;

namespace Personal_GUI.Commands
{
    public class NavigateCommand<TViewModel> : CommandBase
        where TViewModel : ViewModelBase
    {
        private readonly NavigationService<TViewModel> _navigationStore;

        public NavigateCommand(NavigationService<TViewModel> navigationStore)
        {
            _navigationStore = navigationStore;
        }

        public override void Execute(object parameter)
        {
            _navigationStore.Navigate();
        }
    }
}
