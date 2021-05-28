using Personal_GUI.Commands;
using Personal_GUI.Stores;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Personal_GUI.ViewModels
{
    public class WelcomeScreenViewModel : ViewModelBase
    {
        public ICommand NavigateHomeCommand { get; }

        public WelcomeScreenViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateCommand<WelcomeScreenViewModel>(navigationStore, () => new WelcomeScreenViewModel());
        }

        public WelcomeScreenViewModel()
        {
        }
    }
}
