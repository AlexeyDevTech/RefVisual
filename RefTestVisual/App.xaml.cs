using System.Configuration;
using System.Data;
using System.Windows;

namespace RefTestVisual
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            
        }
        /// <summary>
        /// Application Entry Point.
        /// </summary>
        [System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "8.0.2.0")]
        public static void Main()
        {
            RefTestVisual.App app = new RefTestVisual.App();
            app.InitializeComponent();
            app.Run();
        }
    }

}
