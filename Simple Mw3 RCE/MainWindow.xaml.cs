using MahApps.Metro.Controls;

namespace Simple_Mw3_RCE
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Resources = Simple_Mw3_RCE.Resources.Language.SetLanguageDictionary();
        }
    }
}
