using Personal_GUI.Commands;
using Personal_GUI.Stores;
using System.Windows.Input;

namespace Personal_GUI.ViewModels
{
    public class AutoTyperViewModel : ViewModelBase
    {

        public ICommand NavigateHomeCommand { get; }

        public AutoTyperViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore));
        }

    }
}
