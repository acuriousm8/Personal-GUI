using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void close_program_button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void maximize_program_button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void minimize_program_button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
