using Personal_GUI.Stores;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;

namespace Personal_GUI.ViewModels
{
    public class AudioSwitcherViewModel : ViewModelBase
    {
        public ICommand favorites_list { get; set; }
        public AudioSwitcherViewModel(NavigationStore navigationStore)
        {


        }

        private void test()
        {
            MessageBox.Show("test");
        }
    }

}
