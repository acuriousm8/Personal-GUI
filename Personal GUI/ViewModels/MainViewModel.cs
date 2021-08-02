using Personal_GUI.Commands;
using Personal_GUI.Stores;
using System.Windows.Input;

namespace Personal_GUI.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly NavigationStore _navigationStore;

        public ICommand NavigateHomeCommand { get; }
        public ViewModelBase CurrentViewModel => _navigationStore.CurrentViewModel;

        public MainViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(new Services.NavigationService<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore)));
        }

        private void OnCurrentViewModelChanged()
        {
            onPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
