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
            (DataContext as MainWindowViewModel).Resize((int)e.NewSize.Width, (int)e.NewSize.Height);
        }
    }



    
}