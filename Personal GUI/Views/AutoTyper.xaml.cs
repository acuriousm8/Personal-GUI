using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Threading;
using WindowsInput;
using System.Windows.Media;
using System;
using System.Threading.Tasks;

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
        bool running_ = false;
        private void Start_auto_typer_click(object sender, RoutedEventArgs e)
        {
            text = Text_to_type_box.Text;
            start_delay = Convert.ToInt32(Start_delay_text.Text);
            amount = Convert.ToInt32(Amount_text.Text);
            delay = Convert.ToInt32(Delay_text.Text);
            enter = (bool)AutoTyper_press_enter_checkbox.IsChecked;
            Thread Auto_typer_thread = new Thread(() => Auto_typer_program(text, start_delay, amount, delay, enter));

            if (!running_)
            {
                if(text == "")
                {
                    MessageBox.Show("Cannot have no input text");
                }
                else
                {
                    running_ = true;
                    Auto_typer_thread.Start();
                    start_button_label.Content = "Stop";
                }
            }
            else
            {
                start_button_label.Content = "Start";
                running_ = false;
            }

        }


        private void Auto_typer_program(string _thing_to_write, int _start_delay, int _times_to_write, int _delay_between_write, bool press_enter)
        {
            InputSimulator write = new InputSimulator();

            int __start_delay = _start_delay;
            int __times_to_write = _times_to_write;
            int __delay_between_write = _delay_between_write;
            
            while (running_)
            {
                for (int delay = __start_delay; delay > -1; delay--)
                {
                    if (running_)
                    {
                        this.Dispatcher.Invoke(() => { Start_time.Content = delay.ToString(); });
                        write.Keyboard.Sleep(1000);
                    }
                }
                this.Dispatcher.Invoke(() => { Start_time.Content = "0"; });

                for (int times = __times_to_write; times > 0; times--)
                {
                    if(running_)
                    {
                        write.Keyboard.TextEntry(_thing_to_write);
                        if(press_enter)
                        {
                            write.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.RETURN);
                        }
                        this.Dispatcher.Invoke(() => { time_through.Content = times.ToString(); });
                        write.Keyboard.Sleep(__delay_between_write);
                    }
                }
                running_ = false;

            }
            this.Dispatcher.Invoke(() => { time_through.Content = "0"; });
            this.Dispatcher.Invoke(() => { start_button_label.Content = "Start"; });
        }

    }
}
