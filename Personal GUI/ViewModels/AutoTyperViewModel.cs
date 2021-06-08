using Personal_GUI.Commands;
using Personal_GUI.Stores;
using System.Windows.Input;

namespace Personal_GUI.ViewModels
{
    public class AutoTyperViewModel : ViewModelBase
    {
        private AutoTyperStore properties = new AutoTyperStore();

        public ICommand NavigateHomeCommand { get; }
        public ICommand WriteKeyboardCommand { get; }




        public AutoTyperViewModel(NavigationStore navigationStore)
        {
            NavigateHomeCommand = new NavigateCommand<HomeViewModel>(navigationStore, () => new HomeViewModel(navigationStore));
            //WriteKeyboardCommand = new WriteKeyboardCommand(properties.Text_to_type, ); // got too lazy to impliment ICommand properly, just doing it based on the AutoTyper.xaml.cs for now. Couldn't figure out how to pass the values of the text boxes/sliders into the Icommand (5/31/2021)
            // https://stackoverflow.com/questions/32563111/get-user-input-from-a-textbox-in-a-wpf-application looks like it may have something

        }

    }
}
