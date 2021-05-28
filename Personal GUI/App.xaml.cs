using Personal_GUI.Stores;
using Personal_GUI.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Personal_GUI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            NavigationStore navigationStore = new NavigationStore();

            navigationStore.CurrentViewModel = new HomeViewModel(navigationStore);

            MainWindow = new MainWindow()
            { 
                DataContext = new MainViewModel(navigationStore) 
            };
            MainWindow.Show();

            base.OnStartup(e);
        }

    }
}
