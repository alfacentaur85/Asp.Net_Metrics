using System.Windows;

namespace MetricsManagerClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CpuChart.ColumnSeriesValues[0].Values.Add(48d);
        }
    }
}

