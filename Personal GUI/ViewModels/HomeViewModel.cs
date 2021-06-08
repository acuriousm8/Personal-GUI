using Personal_GUI.Commands;
using Personal_GUI.Stores;
using System.Windows.Input;

namespace Personal_GUI.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {

        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateAutoTyperCommand { get; }
        public ICommand NavigateCloseProgramCommand { get; }

        public HomeViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore));
            NavigateAutoTyperCommand = new NavigateCommand<AutoTyperViewModel>(navigationStore, () => new AutoTyperViewModel(navigationStore));
            NavigateCloseProgramCommand = new NavigateCommand<CloseProgramViewModel>(navigationStore, () => new CloseProgramViewModel(navigationStore));
        }

    }
}
