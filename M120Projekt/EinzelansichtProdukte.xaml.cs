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
        private const string textProdukt = "Produkterfassung";
        private const string textAnleitung = "Anleitung";
        private const string textKontakt = "Kontakt";

        private Fenster fenster;

        MainWindow window1;
        UserControl anleitung;
        UserControl kontakt;

        public EinzelansichtProdukte()
        {
            fenster = Fenster.Leer;
            InitializeComponent();
            System.Windows.Threading.DispatcherTimer dt = new System.Windows.Threading.DispatcherTimer();
            // Configure timer with interval of 1 second
            dt.Tick += new EventHandler(timer_tick);
            dt.Interval = new TimeSpan(0, 0, 1);
            // Start timer
            dt.Start();
        }

        private void btnErfassen_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate MainWindow
            window1 = new MainWindow();
            // Place UserControl
            Platzhalter.Children.Add(window1);
            fenster = Fenster.Produkte;
        }

        private void timer_tick(object sender, EventArgs e)
        {
            // Manage states
            checkStates();
        }

        public void checkStates()
        {
            switch (fenster)
            {
                case Fenster.Leer:
                    btnZurueck.IsEnabled = false;
                    btnProdukte.IsEnabled = true;
                    btnAnleitung.IsEnabled = true;
                    btnKontakt.IsEnabled = true;
                    lblTitel.Content = textWelcome;
                    Platzhalter.Children.Clear();
                    break;
                case Fenster.Produkte:
                    btnZurueck.IsEnabled = true;
                    btnProdukte.IsEnabled = false;
                    btnAnleitung.IsEnabled = false;
                    btnKontakt.IsEnabled = false;
                    lblTitel.Content = textProdukt;
                    window1 = null;
                    break;
                case Fenster.Anleitung:
                    btnZurueck.IsEnabled = true;
                    btnProdukte.IsEnabled = false;
                    btnAnleitung.IsEnabled = false;
                    btnKontakt.IsEnabled = false;
                    lblTitel.Content = textAnleitung;
                    window1 = null;
                    break;
                case Fenster.Kontakt:
                    btnZurueck.IsEnabled = true;
                    btnProdukte.IsEnabled = false;
                    btnAnleitung.IsEnabled = false;
                    btnKontakt.IsEnabled = false;
                    lblTitel.Content = textKontakt;
                    window1 = null;
                    break;
            }
        }

        enum Fenster
        {
            Leer,
            Produkte,
            Anleitung,
            Kontakt
        }

        private void btnAnleitung_Click(object sender, RoutedEventArgs e)
        {
            fenster = Fenster.Anleitung;
            anleitung = new UserControl();
            Platzhalter.Children.Add(anleitung);

        }

        private void btnKontakt_Click(object sender, RoutedEventArgs e)
        {
            fenster = Fenster.Kontakt;
            kontakt = new UserControl();
            Platzhalter.Children.Add(kontakt);
        }

        private void btnZurueck_Click(object sender, RoutedEventArgs e)
        {
            fenster = Fenster.Leer;
        }
    }
}
