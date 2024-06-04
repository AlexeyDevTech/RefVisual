using System.Windows;

namespace RefTestVisual
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }

        private void grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (DataContext != null)
                (DataContext as MainWindowViewModel).Rect = e.NewSize;
        }
    }
}