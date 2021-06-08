using System;
using System.Windows.Input;
using System.Threading;
using WindowsInput;

namespace Personal_GUI.Commands
{
    class WriteKeyboardCommand : CommandBase
    {
        private readonly string _thing_to_write;
        private readonly int _times_to_write;
        private readonly int _delay_between_write;
        private readonly int _start_delay;

        public WriteKeyboardCommand(string thing_to_write, int times_to_write = 1, int delay_between_write = 50, int start_delay = 4)
        {
            _thing_to_write = thing_to_write;
            _times_to_write = times_to_write;
            _delay_between_write = delay_between_write;
            _start_delay = start_delay;
        }

        public override void Execute(object parameter)
        {
            InputSimulator write = new InputSimulator();

            write.Keyboard.Sleep(_start_delay);

            for (int times = _times_to_write; times > 0; times--)
            {
                write.Keyboard.TextEntry(_thing_to_write);
                write.Keyboard.Sleep(_delay_between_write);
            }

        }


        public static void Send(Key key)
        {
            if (Keyboard.PrimaryDevice != null)
            {
                if (Keyboard.PrimaryDevice.ActiveSource != null)
                {
                    var e1 = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Down) { RoutedEvent = Keyboard.KeyDownEvent };
                    InputManager.Current.ProcessInput(e1);
                }
            }
        }

    }
}
