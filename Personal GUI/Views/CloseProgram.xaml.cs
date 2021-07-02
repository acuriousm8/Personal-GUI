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
using Personal_GUI.Stores;

//sorting method
class GFG : IComparer<string>
{
    public int Compare(string x, string y)
    {

        if (x == null || y == null)
        {
            return 0;
        }

        // "CompareTo()" method
        return x.CompareTo(y);

    }
}

namespace Personal_GUI.Views
{
    /// <summary>
    /// Interaction logic for CloseProgram.xaml
    /// </summary>
    public partial class CloseProgram : UserControl
    {
        private readonly NavigationStore _store;
        List<string> procs_added = new List<string>();
        int sort = 1; //1: none 2: alphabetical
        GFG alpha = new GFG();


        public CloseProgram()
        {
            InitializeComponent();
            All_process_list.Items.Add("Loading Processes...");

            get_running_procs();
            //running_procs_reoccuring();
        }

        void running_procs_reoccuring() //un-used
        {
            var test = _store.CurrentViewModel;
            while (_store.CurrentViewModel == new CloseProgramViewModel(_store))
            {
                procs_added.Clear();
                foreach (Process proc in Process.GetProcesses())
                {
                    if (!procs_added.Contains(proc.ProcessName))
                    {
                        ListViewItem item = new ListViewItem();
                        item.Foreground = System.Windows.Media.Brushes.White;
                        item.Content = proc.ProcessName;
                        All_process_list.Items.Add(item);
                        procs_added.Add(proc.ProcessName);
                    }
                }
            }
        }

        void get_running_procs()
        {
            All_process_list.Items.Clear();
            procs_added.Clear();
            foreach (Process proc in Process.GetProcesses())
            {
                if (!procs_added.Contains(proc.ProcessName))
                {
                    procs_added.Add(proc.ProcessName);
                }
            }
            if (sort == 2)
            {
                procs_added.Sort(alpha);
            }
            foreach(string name in procs_added)
            {
                ListViewItem item = new ListViewItem();
                item.Foreground = System.Windows.Media.Brushes.White;
                item.Content = name;
                All_process_list.Items.Add(item);
            }
        }

        string kill(string program_name)
        {
            get_running_procs();
            try
            {
                Process[] p = Process.GetProcessesByName(program_name);
                if (p.Length > 0)
                {
                    foreach (Process proc in p)
                    {
                        proc.Kill();
                        proc.WaitForExit();
                    }
                    return "Closed program successfully";
                }
                else
                {
                    foreach (string item in procs_added)
                    {
                        if (item.ToLower().Contains(program_name.ToLower()))
                        {
                            p = Process.GetProcessesByName(item);
                            foreach (Process proc in p)
                            {
                                proc.Kill();
                                proc.WaitForExit();
                            }
                            return "Closed program successfully";
                        }
                    }
                    return "The program was not running";
                }
            }
            catch (System.ComponentModel.Win32Exception ex)
            {
                if (ex.Message == "Access is denied.")
                {
                    return "Couldn't close program";
                }
                else
                {
                    return ex.Message;
                }
            }
        }

        private void Alphabetical_sorting_button(object sender, RoutedEventArgs e)
        {
            sort = 2;
            get_running_procs();
        }

        private void Normal_sorting_button(object sender, RoutedEventArgs e)
        {
            sort = 1;
            get_running_procs();
        }

        private void Refresh_list_button(object sender, RoutedEventArgs e)
        {
            get_running_procs();
        }

        private void Clear_list_button(object sender, RoutedEventArgs e)
        {
            All_process_list.Items.Clear();
        }

        private void Kill_proc_button(object sender, RoutedEventArgs e)
        {
            if (program_close_text_box.Text == "")
            {
                try
                {
                    foreach (ListViewItem item in All_process_list.SelectedItems)
                    {
                        Status_label.Content = kill(item.Content.ToString());
                    }
                }
                catch
                { }
            }
            else
            {
                Status_label.Content = kill(program_close_text_box.Text);
                program_close_text_box.Text = "";
            }
            get_running_procs();
        }
    }
}
