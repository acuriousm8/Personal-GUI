using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace Personal_GUI.Views
{
    /// <summary>
    /// Interaction logic for AudioSwitcher.xaml
    /// </summary>
    public partial class AudioSwitcher : UserControl
    {
        CoreAudioController controller;// = new CoreAudioController();
        List<string> device_list = new List<string>();
        List<string> favorites_list = new List<string>();
        string List_backlog_path = $@"C:\Users\{Environment.UserName}\AppData\Roaming\Personal_GUI\Audio_Switcher\list_backlog";
        string favorties_list_path = $@"C:\Users\{Environment.UserName}\AppData\Roaming\Personal_GUI\Audio_Switcher\favorites_list";

        public AudioSwitcher()
        {
            InitializeComponent();
            AudioDeviceList.Items.Add("Loading device list");
            if (System.IO.File.Exists(List_backlog_path))
            {
                AudioDeviceList.Items.Clear();
                AudioDeviceList.Items.Add("Loading Stored List");
                foreach(string line in System.IO.File.ReadAllLines(List_backlog_path))
                {
                    ListViewItem item = new ListViewItem();
                    item.Foreground = System.Windows.Media.Brushes.White;
                    item.Content = line;
                    AudioDeviceList.Items.Add(item);
                    device_list.Add(line);
                }
            }

            if(File.Exists(favorties_list_path))
            {
                set_favorites();
            }
            else
            {
                Favorites_stack_panel.Visibility = Visibility.Hidden;
            }
            new Thread(() =>
            {
                controller = new CoreAudioController();
                get_audio_devices();
            }).Start();
        }

        void get_audio_devices()
        {
            IEnumerable<CoreAudioDevice> devices = controller.GetPlaybackDevices();
            List<string> local_device_list = new List<string>();

            this.Dispatcher.Invoke(() =>
            {
                AudioDeviceList.Items.Clear();
                foreach (CoreAudioDevice d in devices)
                {
                    ListViewItem item = new ListViewItem();
                    item.Foreground = System.Windows.Media.Brushes.White;
                    item.Content = d.FullName;
                    local_device_list.Add(d.FullName);
                    AudioDeviceList.Items.Add(item);
                }
            });

            if(local_device_list != device_list)
            {
                device_list = local_device_list;

                try
                {
                    if (File.Exists(List_backlog_path))
                    {
                        System.IO.File.Delete(List_backlog_path);
                    }

                    using (TextWriter tw = new StreamWriter(List_backlog_path))
                    {
                        foreach (string item in device_list)
                        {
                            tw.WriteLine(item);
                        }
                    }
                }
                finally
                {
                }
            } 
        }

        void set_active(string item_name)
        {
            foreach (CoreAudioDevice device in controller.GetPlaybackDevices())
            {
                if (device.FullName == item_name)
                {
                    controller.DefaultPlaybackDevice = device;
                }
            }
        }

        void set_favorites()
        {
            favorites_list.Clear();
            this.Dispatcher.Invoke(() =>
            {
                string[] lines = File.ReadAllLines(favorties_list_path);
                if (lines.Count() == 0)
                {
                    favorite_button_1.Visibility = Visibility.Collapsed;
                    favorite_button_2.Visibility = Visibility.Collapsed;
                    favorite_button_3.Visibility = Visibility.Collapsed;
                }
                else
                {
                    int favorite_count = 1;
                    foreach (string line in File.ReadAllLines(favorties_list_path))
                    {
                        switch (favorite_count)
                        {
                            case 1:
                                Favorites_1_button_text.Text = line;
                                favorite_button_1.Visibility = Visibility.Visible;
                                favorite_button_2.Visibility = Visibility.Collapsed;
                                favorite_button_3.Visibility = Visibility.Collapsed;
                                break;

                            case 2:
                                Favorites_2_button_text.Text = line;
                                favorite_button_2.Visibility = Visibility.Visible;
                                favorite_button_3.Visibility = Visibility.Collapsed;
                                break;

                            case 3:
                                Favorites_3_button_text.Text = line;
                                favorite_button_3.Visibility = Visibility.Visible;
                                break;
                        }
                        favorites_list.Add(line);
                        favorite_count++;
                    }
                }
            });
        }

        private void Refresh_list_button(object sender, RoutedEventArgs e)
        {
            new Thread(() => { get_audio_devices(); }).Start();
        }

        private void Set_active_button(object sender, RoutedEventArgs e)
        {
            if (AudioDeviceList.SelectedItems.Count == 1)
            {
                string item_name = "";
                foreach(ListViewItem item in AudioDeviceList.SelectedItems)
                {
                    item_name = item.Content.ToString();
                }

                set_active(item_name);
            }
            else
            {
                MessageBox.Show("Please select one output");
            }

        }

        private void Add_Favorites_click(object sender, RoutedEventArgs e)
        {
            foreach (ListViewItem item in AudioDeviceList.SelectedItems)
            {
                if (!favorites_list.Contains(item.Content.ToString()))
                {
                    favorites_list.Add(item.Content.ToString());
                }
                else
                {
                    favorites_list.Remove(item.Content.ToString());
                }
            }

            try
            {
                if (File.Exists(favorties_list_path))
                {
                    System.IO.File.Delete(favorties_list_path);
                }

                using (TextWriter tw = new StreamWriter(favorties_list_path))
                {
                    foreach (string item in favorites_list)
                    {
                        tw.WriteLine(item);
                    }
                }
            }
            finally
            {
            }
            set_favorites();
        }

        private void Favorites_1_button_click(object sender, RoutedEventArgs e)
        {
            set_active(favorites_list[0]);
        }
        private void Favorites_2_button_click(object sender, RoutedEventArgs e)
        {
            set_active(favorites_list[1]);
        }
        private void Favorites_3_button_click(object sender, RoutedEventArgs e)
        {
            set_active(favorites_list[2]);
        }

        private void Favorites_button_1_favorite(object sender, RoutedEventArgs e)
        {
            favorites_list.Remove(Favorites_1_button_text.Text);
            try
            {
                if (File.Exists(favorties_list_path))
                {
                    System.IO.File.Delete(favorties_list_path);
                }

                using (TextWriter tw = new StreamWriter(favorties_list_path))
                {
                    foreach (string item in favorites_list)
                    {
                        tw.WriteLine(item);
                    }
                }
            }
            finally
            {
            }
            set_favorites();
        }
        private void Favorites_button_2_favorite(object sender, RoutedEventArgs e)
        {
            favorites_list.Remove(Favorites_2_button_text.Text);
            try
            {
                if (File.Exists(favorties_list_path))
                {
                    System.IO.File.Delete(favorties_list_path);
                }

                using (TextWriter tw = new StreamWriter(favorties_list_path))
                {
                    foreach (string item in favorites_list)
                    {
                        tw.WriteLine(item);
                    }
                }
            }
            finally
            {
            }
            set_favorites();
        }
        private void Favorites_button_3_favorite(object sender, RoutedEventArgs e)
        {
            favorites_list.Remove(Favorites_3_button_text.Text);
            try
            {
                if (File.Exists(favorties_list_path))
                {
                    System.IO.File.Delete(favorties_list_path);
                }

                using (TextWriter tw = new StreamWriter(favorties_list_path))
                {
                    foreach (string item in favorites_list)
                    {
                        tw.WriteLine(item);
                    }
                }
            }
            finally
            {
            }
            set_favorites();
        }
    }
}
