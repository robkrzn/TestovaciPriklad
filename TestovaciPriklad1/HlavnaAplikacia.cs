using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace TestovaciPriklad1
{
    /// <summary>
    /// Okno s aplikáciou na obsluhu databáze
    /// </summary>
    public partial class HlavnaAplikacia : Form
    {
        /// <summary>
        /// List ktorý tvorí databázu kníh
        /// </summary>
        private List<Book> library = new List<Book>();
        private string file;
        /// <summary>
        /// Konštruktor okna s hlavnou aplikacou
        /// </summary>
        public HlavnaAplikacia()
        {
            InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1250");
            this.file = System.Configuration.ConfigurationManager.AppSettings["file"];

            //Login login = new Login();
            //login.ShowDialog();

            nacitajDatabazu();
        }
        /// <summary>
        /// Metóda pre načítanie databáze z XML súboru.
        /// Po načítani sa automaticky prevedú aj do tabuľky.
        /// </summary>
        /// <returns>Navrati uspesnost operacie</returns>
        private bool nacitajDatabazu() {
            dataGridView.Rows.Clear();
            dataGridView.Refresh();
            library.Clear();
            try
            {
                DataSet dataset = new DataSet();
                dataset.ReadXml(this.file);
                for (int i = 0; i < dataset.Tables[0].Rows.Count; i++)
                {
                    Book kniha = new Book(Int16.Parse(dataset.Tables[0].Rows[i][3].ToString()), dataset.Tables[0].Rows[i][0].ToString(), dataset.Tables[0].Rows[i][1].ToString(),
                        new Borrowed(dataset.Tables[1].Rows[i][0].ToString(), dataset.Tables[1].Rows[i][1].ToString(), dataset.Tables[1].Rows[i][2].ToString()));
                    library.Add(kniha);
                }
                vypisDatabaze();
                return true;
            }
            catch (IOException ex) {
                MessageBox.Show("Chyba pri nacitani databaze " + ex.ToString());
                return false;
            }
        }
        /// <summary>
        /// Pomocná metóda pre vypis databáze do tabulky.
        /// Podľa zaklinutých ChceckBoxov sa zobrazia príslušné dáta v tabulke.
        /// </summary>
        private void vypisDatabaze() {
            for (int i = 0; i < library.Count; i++)
            {
                if (freeBookCheckBox.Checked == true)//vypis bez požičanych
                {
                    if (library[i].borrowed.FirstName == "")
                        dataGridView.Rows.Add(library[i].id, library[i].Name, library[i].Author, library[i].borrowed.FirstName, library[i].borrowed.LastName, library[i].borrowed.From);
                }
                else if (borrowedBookCheckBox.Checked == true)//vypis s požičanimi
                {
                    if (library[i].borrowed.FirstName != "")
                        dataGridView.Rows.Add(library[i].id, library[i].Name, library[i].Author, library[i].borrowed.FirstName, library[i].borrowed.LastName, library[i].borrowed.From);
                }
                else//vypis všetkych
                {
                    dataGridView.Rows.Add(library[i].id, library[i].Name, library[i].Author, library[i].borrowed.FirstName, library[i].borrowed.LastName, library[i].borrowed.From);
                }
            }
        }
        /// <summary>
        /// Metóda na uloženie databáze do XML súboru
        /// </summary>
        /// <returns>Úspešnosť zápisu</returns>
        private bool ulozDatabazu(){
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = Encoding.GetEncoding("windows-1250");
            try
            {
                using (XmlWriter xw = XmlWriter.Create(this.file, settings))
                {
                    xw.WriteStartDocument();
                    xw.WriteStartElement("Library");
                    foreach (Book b in library)
                    {
                        xw.WriteStartElement("Book");
                        xw.WriteAttributeString("id", b.id.ToString());
                        xw.WriteElementString("Name", b.Name);
                        xw.WriteElementString("Author", b.Author);
                        xw.WriteStartElement("Borrowed");
                        xw.WriteElementString("FirstName", b.borrowed.FirstName);
                        xw.WriteElementString("LastName", b.borrowed.LastName);
                        xw.WriteElementString("From", b.borrowed.From);
                        xw.WriteEndElement();
                        xw.WriteEndElement();
                    }
                    xw.WriteEndElement();
                    xw.WriteEndDocument();
                    xw.Flush();

                    return true;
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Chyba pri ukladani databaze "+ex.ToString());
                return false;
            }
        }

        private void HlavnaAplikacia_Load(object sender, EventArgs e)
        {
            
        }
        /// <summary>
        /// Oblužná metóda stlačenia tlačidla pridať.
        /// Po stlačení sa uložia vyplnené záznamy do databáze.
        /// Databáza sa následne uloží a opäť načíta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addButton_Click(object sender, EventArgs e)
        {
            if (bookTextBox.Text != "" && authorTextBox.Text != "")
            {
                if (fromDateTimePicker.Value < DateTime.Now)
                {
                    //overenie či sa kniha už nachadza v zozname
                    bool zhoda = false;
                    for (int i = 0; i < library.Count; i++) {
                        if (library[i].Name.ToString().Equals(bookTextBox.Text)) zhoda = true;
                    }
                    if (!zhoda)
                    {
                        if (nameTextBox.Text == "")
                            library.Add(new Book(zistiNajvysieID() + 1, bookTextBox.Text, authorTextBox.Text, new Borrowed("", "", "")));
                        else
                            library.Add(new Book(zistiNajvysieID() + 1, bookTextBox.Text, authorTextBox.Text, new Borrowed(nameTextBox.Text, lastNameTextBox.Text, fromDateTimePicker.Value.ToString("d.M.yyyy"))));
                        ulozDatabazu();
                        nacitajDatabazu();
                    }
                    else MessageBox.Show("Kniha sa už v zozname nachádza.");
                    
                }
                else MessageBox.Show("Datum je z búcnosti");
            }
            else MessageBox.Show("Nieje zadany autor alebo dielo");
        }
        /// <summary>
        /// Obslužná metóda tláčidla vymazať.
        /// Vybraný objekt sa po stlačení vymaže.
        /// Pred vymazaním sa zobrazí dopytovacie sa okno pre overenie rozhodnutia.
        /// Po vymazaní sa nová databáza uloží a opäť načíta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeButton_Click(object sender, EventArgs e)
        {
            if (idTextBox.ToString() != "")
            {
                int pom = 0;
                for(int i =0; i < library.Count; i++) {
                    if (library[i].id == int.Parse(idTextBox.Text)) pom = i;
                }
                DialogResult dialogResult = MessageBox.Show("Naozaj cheš odstrániť záznam: " + library[pom].Name + " od  "+library[pom].Author
                + " ?", "Odstrániť záznam", MessageBoxButtons.YesNo); 
                if (dialogResult == DialogResult.Yes)
                {
                    library.RemoveAt(pom);
                    ulozDatabazu();
                    nacitajDatabazu();
                    MessageBox.Show("Záznam bol vymazaný"); 
                } 
            }
            else
            {
                MessageBox.Show("Nevybral si žiadny záznam");
            }
        }
        /// <summary>
        /// Obslužná metóda tláčidla upraviť.
        /// Upravené dáta sa uložia do databáze.
        /// Po uložení sa zobrazí informatívne okno o dokončení operácie.
        /// Databáza sa následne uloží a opätovne načíta.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void editButton_Click(object sender, EventArgs e)
        {
            if (idTextBox.ToString() != "") {
                int pom = 0;
                for (int i = 0; i < library.Count; i++)
                {
                    if (library[i].id == int.Parse(idTextBox.Text)) pom = i;
                }
                if (nameTextBox.Text == "")
                    library[pom]=(new Book(int.Parse(idTextBox.Text), bookTextBox.Text, authorTextBox.Text, new Borrowed("", "", "")));
                else
                    library[pom]=(new Book(int.Parse(idTextBox.Text), bookTextBox.Text, authorTextBox.Text, new Borrowed(nameTextBox.Text, lastNameTextBox.Text, fromDateTimePicker.Value.ToString("d.M.yyyy"))));
                ulozDatabazu();
                nacitajDatabazu();
                MessageBox.Show("Záznam bol upravený");
            }
            else
                MessageBox.Show("Nevybral si žiadny záznam");
        }
        /// <summary>
        /// Metóda pre zistenie súčasného najväčšieho ID.
        /// </summary>
        /// <returns></returns>
        private int zistiNajvysieID() {
            int id=0;
            foreach (Book b in library) {
                if(b.id>id)
                id = b.id;
            }
            return id;
        }
        /// <summary>
        /// CheckBox pre zobrazenie záznamov s knihami ktoré sú k dispozícii v knižnici.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void freeBookCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (freeBookCheckBox.Checked == true) {
                borrowedBookCheckBox.Checked = false;
            }
            nacitajDatabazu();

        }
        /// <summary>
        /// ChceckBox pre zobrazenie záznamov s knihami ktoré sú požičáné.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void borrowedBookCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (borrowedBookCheckBox.Checked == true)
            {
                freeBookCheckBox.Checked = false;
            }
            nacitajDatabazu();
        }
        /// <summary>
        /// Po zakliknutí objektu v tabulke sa zobrazia dáta do TextBoxov pod tabulkou
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            idTextBox.Text = dataGridView.SelectedRows[0].Cells[0].Value.ToString();
            bookTextBox.Text = dataGridView.SelectedRows[0].Cells[1].Value.ToString();
            authorTextBox.Text = dataGridView.SelectedRows[0].Cells[2].Value.ToString();
            nameTextBox.Text = dataGridView.SelectedRows[0].Cells[3].Value.ToString();
            lastNameTextBox.Text = dataGridView.SelectedRows[0].Cells[4].Value.ToString();
            if (dataGridView.SelectedRows[0].Cells[5].Value.ToString() != "")
                fromDateTimePicker.Value = DateTime.ParseExact(dataGridView.SelectedRows[0].Cells[5].Value.ToString(), "d.M.yyyy", null);
            else fromDateTimePicker.Value = DateTime.Today;
        }
        /// <summary>
        /// Tláčidlo pre rýchlo vrátenie knihy.
        /// Po stlačení tlačidla sa kniha označí ako vrátená.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void returnButton_Click(object sender, EventArgs e)
        {
            if (nameTextBox.Text != "" || lastNameTextBox.Text != "")
            {
                DialogResult dialogResult = MessageBox.Show("Prajete si knihu " + bookTextBox.Text + " od " + authorTextBox.Text
                + " označiť ako vrátenú ?", "Vrátiť knihu", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes) {
                    int pom = 0;
                    for (int i = 0; i < library.Count; i++)
                    {
                        if (library[i].id == int.Parse(idTextBox.Text)) pom = i;
                    }
                    library[pom] = (new Book(int.Parse(idTextBox.Text), bookTextBox.Text, authorTextBox.Text, new Borrowed("", "", "")));
                    ulozDatabazu();
                    nacitajDatabazu();
                }
            }
            else {
                MessageBox.Show("Zvolenú knihu nieje možné vrátiť.");
            }
        }
    }
}
