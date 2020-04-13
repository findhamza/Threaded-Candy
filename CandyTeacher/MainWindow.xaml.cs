using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CandyTeacher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CandyBowl candy;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void startButton_Click(object sender, RoutedEventArgs e)
        {
            this.candy = new CandyBowl(profCount.Value, candyCount.Value, multi.Value);
            await Task.Run(() => candy.GetCandy(outText));
            //candy.GetCandy(outText);
        }

        private async void stopButton_Click(object sender, RoutedEventArgs e)
        {
            if(candy!=null)
                await Task.Run(() => candy.StopCandy());
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            outText.Text = "";
        }
    }
}