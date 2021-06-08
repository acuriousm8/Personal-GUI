using Personal_GUI.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace Personal_GUI.ViewModels
{
    public class CloseProgramViewModel : ViewModelBase 
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<string> running_Programs { get; private set; }

        public CloseProgramViewModel(NavigationStore navigationStore)
        {
        }

        public CloseProgramViewModel()
        {
            running_Programs = new ObservableCollection<string>();
        }

        public void AddTitleIfUnique(string title)
        {
            if (!running_Programs.Contains(title))
            {
                running_Programs.Add(title);
            }
        }
    }
}
