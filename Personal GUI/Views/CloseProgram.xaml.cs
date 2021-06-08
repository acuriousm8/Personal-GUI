using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Linq;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using Personal_GUI.ViewModels;

namespace Personal_GUI.Views
{
    /// <summary>
    /// Interaction logic for CloseProgram.xaml
    /// </summary>
    public partial class CloseProgram : UserControl
    {
        public CloseProgram()
        {
            InitializeComponent();
            All_process_list.Items.Add("Loading Processes...");

            var viewModel = new CloseProgramViewModel();
            
            DataContext = viewModel;

            //get_running_procs();
        }


        void get_running_procs()
        {
            foreach (Process proc in Process.GetProcesses())
            {
                
            }
        }


    }
}
