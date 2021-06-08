using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;
using WindowsInput;
using System;

namespace Personal_GUI.Views
{
    /// <summary>
    /// Interaction logic for AutoTyper.xaml
    /// </summary>
    public partial class AutoTyper : UserControl
    {
        public AutoTyper()
        {
            InitializeComponent();
        }


        string text;
        int start_delay;
        int amount;
        int delay;
        bool enter;

        private void Start_auto_typer_click(object sender, RoutedEventArgs e)
        {
            text = Text_to_type_box.Text;
            start_delay = Convert.ToInt32(Start_delay_text.Text);
            amount = Convert.ToInt32(Amount_text.Text);
            delay = Convert.ToInt32(Delay_text.Text);
            enter = (bool)AutoTyper_press_enter_checkbox.IsChecked;

            if(text == "")
            {
                MessageBox.Show("Cannot have no input text");
            }
            else
            {
                Thread Auto_typer_thread = new Thread(() => Auto_typer_program(text, start_delay, amount, delay, enter));
                Auto_typer_thread.Start();
            }

        }


        private void Auto_typer_program(string _thing_to_write, int _start_delay, int _times_to_write, int _delay_between_write, bool press_enter)
        {
            InputSimulator write = new InputSimulator();

            while (!System.IO.File.Exists($@"C:\Users\{Environment.UserName}\AppData\Roaming\Personal_GUI\Auto_typer\kill_all_autotyper"))
            {
                for (int delay = _start_delay; delay > 0; delay--)
                {
                    this.Dispatcher.Invoke(() => { Start_time.Content = delay.ToString(); });
                    write.Keyboard.Sleep(1000);
                }
                this.Dispatcher.Invoke(() => { Start_time.Content = "0"; });

                for (int times = _times_to_write; times > 0; times--)
                {
                    write.Keyboard.TextEntry(_thing_to_write);
                    if(press_enter)
                    {
                        write.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
                    }
                    this.Dispatcher.Invoke(() => { time_through.Content = times.ToString(); });
                    Thread.Sleep(_delay_between_write);
                }
                this.Dispatcher.Invoke(() => { time_through.Content = "0"; });
                break;
            }
            try
            {
                System.IO.File.Delete($@"C:\Users\{Environment.UserName}\AppData\Roaming\Personal_GUI\Auto_typer\kill_all_autotyper");
            }
            catch
            {

            }
        }

        private void Kill_all_autotyper(object sender, RoutedEventArgs e)
        {
            System.IO.File.Create($@"C:\Users\{Environment.UserName}\AppData\Roaming\Personal_GUI\Auto_typer\kill_all_autotyper");
        }
    }
}
