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
        public ICommand NavigateHueLightsProgramCommand { get; }
        public ICommand NavigateAudioOutputProgramCommand { get; }
        public ICommand NavigateRokuRemoteCommand { get; }

        public HomeViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(new Services.NavigationService<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore)));
            NavigateAutoTyperCommand = new NavigateCommand<AutoTyperViewModel>(new Services.NavigationService<AutoTyperViewModel>(navigationStore, () => new AutoTyperViewModel(navigationStore)));
            NavigateCloseProgramCommand = new NavigateCommand<CloseProgramViewModel>(new Services.NavigationService<CloseProgramViewModel>(navigationStore, () => new CloseProgramViewModel(navigationStore)));
            NavigateHueLightsProgramCommand = new NavigateCommand<HueLightsViewModel>(new Services.NavigationService<HueLightsViewModel>(navigationStore, () => new HueLightsViewModel(navigationStore)));
            NavigateAudioOutputProgramCommand = new NavigateCommand<AudioSwitcherViewModel>(new Services.NavigationService<AudioSwitcherViewModel>(navigationStore, () => new AudioSwitcherViewModel(navigationStore)));
            NavigateRokuRemoteCommand = new NavigateCommand<RokuControllerViewModel>(new Services.NavigationService<RokuControllerViewModel>(navigationStore, () => new RokuControllerViewModel(navigationStore)));
        }

    }
}
