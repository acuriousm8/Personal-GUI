using System;
using System.Windows;
using System.Windows.Input;

namespace Personal_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int click_count;
        bool cleanup_program = false;

        public MainWindow()
        {
            InitializeComponent();
            create_main_program_files();
        }

        private void create_main_program_files()
        {
            string[] sub_folder = { "Auto_typer", "Hue", "Auto_application_close", "Audio_Switcher"};
            System.IO.Directory.CreateDirectory($@"C:\Users\{Environment.UserName}\AppData\Roaming\Personal_GUI");
            foreach (string folder in sub_folder)
            {
                System.IO.Directory.CreateDirectory($@"C:\Users\{Environment.UserName}\AppData\Roaming\Personal_GUI\{folder}");
            }
        }

        private void close_program_button_Click(object sender, RoutedEventArgs e)
        {
            if(cleanup_program)
            {
                System.IO.Directory.Delete($@"C:\Users\{Environment.UserName}\AppData\Roaming\");
            }
            System.Windows.Application.Current.Shutdown();
        }

        private void maximize_program_button_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void minimize_program_button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            click_count = 0;
            click_count = click_count + e.ClickCount;

            if (click_count == 2)
            {
                if (this.WindowState == WindowState.Normal)
                {
                    this.WindowState = WindowState.Maximized;
                }
                else
                {
                    this.WindowState = WindowState.Normal;
                }
                click_count = 0;
            }
            else
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                    DragMove();
                }
                else
                {
                    DragMove();
                }
            }
        }
    }
}
