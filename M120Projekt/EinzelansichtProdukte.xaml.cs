using System;
using System.CodeDom;
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
using System.Windows.Shapes;

namespace M120Projekt
{
    /// <summary>
    /// Interaktionslogik für EinzelansichtProdukte.xaml
    /// </summary>
    public partial class EinzelansichtProdukte : Window
    {
        // Predefined text for banner label
        private const string textWelcome = "Willkommen";
        private const string textEntry = "Produkterfassung";
        public EinzelansichtProdukte()
        {
            InitializeComponent();
            // Change banner text
            lblTitel.Content = textWelcome;
        }

        private void lblErfassen_Click(object sender, RoutedEventArgs e)
        {
            // Hide StackPanel
            stckNeu.Visibility = Visibility.Hidden;
            // Instantiate MainWindow
            MainWindow window1 = new MainWindow();
            // Place UserControl
            Platzhalter.Children.Add(window1);
            // Change banner text
            lblTitel.Content = textEntry;
        }
    }
}
