using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Personal_GUI.Views
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
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

        private void home_view_clicked(object sender, RoutedEventArgs e)
        {

        }
    }
}
