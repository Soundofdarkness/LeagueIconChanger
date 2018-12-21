using LeagueIconChanger.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LeagueIconChanger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WebClient wc = new WebClient();
        private LeagueClient leagueClient = new LeagueClient();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Initialized(object sender, EventArgs e)
        {
            foreach(int iconId in Icons.icons)
            {
                string url = Icons.GetIconUrl(iconId);
                AddImage(url);
            }

            if (!leagueClient.LeagueRunning())
            {
                ApplyButton.IsEnabled = false;
                StatLabel.Content = "League is not running";
                StatLabel.Foreground = Brushes.Red;
            }
            else
            {
                StatLabel.Content = "League is Running";
                StatLabel.Foreground = Brushes.Green;
                await Task.Run(() =>
                {
                    leagueClient.getLeagueApi();
                });
            }
        }

        public void AddImage(string url)
        {
            Console.WriteLine(url);
            IconList.Items.Add(new BitmapImage(new Uri(url)));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.Show();
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if(IconList.SelectedItem == null)
            {
                MessageBox.Show("You didn't select an icon.", "Failed to apply icon", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

                var code = leagueClient.applyIcon(((BitmapImage)IconList.SelectedItem).UriSource.ToString());
                if (code == HttpStatusCode.Created)
                {
                    MessageBox.Show("Successfully applied icon.", "Finished", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                else
                {
                    Console.WriteLine(code.ToString());
                    MessageBox.Show("Failed to apply icon. HTTP: " + code.ToString(), "Failed to apply icon.", MessageBoxButton.OK, MessageBoxImage.Error);
                }
        }
    }
}
