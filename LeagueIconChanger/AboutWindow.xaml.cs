using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace LeagueIconChanger
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            CreditImage.Source = new BitmapImage(new Uri("https://cdn.communitydragon.org/latest/profile-icon/67.jpg"));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/Soundofdarkness/LeagueIconChanger");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Process.Start("https://www.removeddit.com/r/leagueoflegends/comments/9x5t65/tutorial_how_to_get_chinese_chibi_icons/");
        }
    }
}
