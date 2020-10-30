using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace M120Projekt
{
    // User Control that appears after pressing start-button
    public partial class MainWindow : UserControl
    {
        // Enum of possible states
        enum Zustand
        {
            Leer,
            Neu,
            Veraendert,
            Validiert
        }

        // Current state
        private Zustand state;

        // Model for DB
        Produkt model = new Produkt();

        public MainWindow()
        {
            InitializeComponent();
            // Instantiate DispatcherTimer to repeat certain functions every few seconds
            System.Windows.Threading.DispatcherTimer dt = new System.Windows.Threading.DispatcherTimer();
            // Configure timer with interval of 1 second
            dt.Tick += new EventHandler(timer_tick);
            dt.Interval = new TimeSpan(0,0,1);
            // Start timer
            dt.Start();
        }

        // Timer repeating 
        private void timer_tick(object sender, EventArgs e)
        {
            // Manage states
            checkStates();
            // Set state to new if fields are empty
            if (string.IsNullOrWhiteSpace(txtName.Text) & string.IsNullOrWhiteSpace(txtPreis.Text))
            {
                state = Zustand.Neu;
            } 
            
            lblName.Content = state;
        }

        // Switch states and manage clickable buttons
        public void checkStates()
        {
            switch (state)
            {
                case Zustand.Leer:
                    btnSave.IsEnabled = false;
                    btnCancel.IsEnabled = false;
                    break;
                case Zustand.Neu:
                    btnSave.IsEnabled = false;
                    btnCancel.IsEnabled = false;
                    break;
                case Zustand.Veraendert:
                    btnSave.IsEnabled = false;
                    btnCancel.IsEnabled = true;
                    break;
                case Zustand.Validiert:
                    btnSave.IsEnabled = true;
                    btnCancel.IsEnabled = true;
                    break;
            }
        }

        // resets state
        // resets all input fields to default value
        public void ClearAllFields()
        {
            // reset button text
            txtSave.Text = "SAVE";
            // reset state
            state = Zustand.Neu;
            // reset text
            txtName.Text = txtPreis.Text = "";
            // reset availability
            cbErhaeltlich.IsChecked = true;
            // reset date
            dtDatum.SelectedDate = DateTime.Now;
            // reset color
            cboxFarbe.SelectedIndex = -1;
            // Disable delete button
            btnDelete.IsEnabled = false;
            // Set ID to 0
            model.ProduktId = 0;
        }

        // Cancel-ClickEvent
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Clean form when cancelled 
            ClearAllFields();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Clean form on load
            ClearAllFields();
            PopulateDataGrid();
        }

        // Save-ClickEvent
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Set name attribute, remove whitespace
            model.Name = txtName.Text.Trim();
            // Set delivery date
            model.Lieferdatum = dtDatum.SelectedDate;
            // Set availability state
            model.Verfuegbar = cbErhaeltlich.IsChecked;
            // Set price attribute, parse string to int and remove whitespace
            model.Preis = int.Parse(txtPreis.Text.Trim());
            // Set color attribute
            model.Farbe = cboxFarbe.Text;
            // Save changes to DB
            using (DBEntities db = new DBEntities())
            {
                if (model.ProduktId == 0)
                    db.Produkt.Add(model);
                else
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
            // Clean form
            ClearAllFields();
            PopulateDataGrid();
            // Show dialogue 
            MessageBox.Show("Entry saved!", "Confirmation", MessageBoxButton.OK);
        }

        // TextChanged-Event for txtName
        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Generate Regex
            Regex regex = new Regex(@"^([A-Z]{1})([A-Za-z]*)$");
            // Validate field
            validate(sender, regex, lblName);
        }

        // TextChanged-Event for txtPreis
        private void txtPreis_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Generate Regex
            Regex regex = new Regex(@"^[0-9]+$");
            // Validate field
            validate(sender, regex, lblPreis);
        }

        //
        // Validates object with according regex and displays associated label in red or green depending on result
        //
        private void validate(object sender, Regex regex, Label targetLabel)
        {
            // Collect input
            string input = (sender as TextBox).Text;
            // Match w/ regex
            Match match = regex.Match(input);

            // Match result
            if (!match.Success)
            {
                // Set label color to red
                targetLabel.Foreground = Brushes.Red;
                // Set state to changed
                state = Zustand.Veraendert;
            }
            else
            {
                // Set label color to green
                targetLabel.Foreground = Brushes.Green;
                // Make sure other fields are not empty
                if (!string.IsNullOrWhiteSpace(txtName.Text) && !string.IsNullOrWhiteSpace(txtPreis.Text))
                    // Set state to validated
                    state = Zustand.Validiert;
            }
        }

        private void PopulateDataGrid()
        {
            using (DBEntities db = new DBEntities())
            {
                dgvProdukte.ItemsSource = db.Produkt.ToList<Produkt>();
            }
        }

        private void dgvProdukte_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            editColumn();
        }

        private string GetCellinformation(int index)
        {
            return (dgvProdukte.SelectedCells[index].Column.GetCellContent(dgvProdukte.SelectedCells[index].Item) as TextBlock).Text;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                using (DBEntities db = new DBEntities())
                {
                    var entry = db.Entry(model);
                    if (entry.State == System.Data.Entity.EntityState.Detached)
                        db.Produkt.Attach(model);
                    db.Produkt.Remove(model);
                    db.SaveChanges();
                    PopulateDataGrid();
                    ClearAllFields();
                    MessageBox.Show("Entry deleted!");
                }
            }
        }

        private void dgvProdukte_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MessageBox.Show("Test");
                editColumn();
            }
        }

        private void editColumn()
        {
            var currentRowIndex = dgvProdukte.Items.IndexOf(dgvProdukte.CurrentItem);
            string produktId;

            Produkt selectedItem = dgvProdukte.SelectedItem as Produkt;

            if (selectedItem != null)
            {
                produktId = Convert.ToString(selectedItem.ProduktId);
            }
            else
            {
                produktId = "Nicht vorhanden";
            }

            // MAKE HEADER NOT CLICKABLE

            model.ProduktId = Convert.ToInt32(produktId);

            using (DBEntities db = new DBEntities())
            {
                model = db.Produkt.Where(x => x.ProduktId == model.ProduktId).FirstOrDefault();
                txtName.Text = model.Name.Trim();
                txtPreis.Text = Convert.ToString(model.Preis);
                cbErhaeltlich.IsChecked = model.Verfuegbar;
                cboxFarbe.Text = model.Farbe.Trim();
                dtDatum.SelectedDate = model.Lieferdatum;
            }
            txtSave.Text = "UPDATE";
            btnDelete.IsEnabled = true;
        }

        private void dgvProdukte_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                editColumn();
            }
        }
    }
}
