using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

namespace TestovaciPriklad1
{
    public partial class Login : Form
    {
        /// <summary>
        /// Meno uzivatela
        /// </summary>
        private string meno;
        /// <summary>
        /// Heslo uzivatela
        /// </summary>
        private string heslo;

        public Login()
        {
            InitializeComponent();
            this.meno = ConfigurationManager.AppSettings["Login"] ;
            if (this.meno != "admin") MessageBox.Show("Neprebehlo nacitanie dat");
            this.heslo = System.Configuration.ConfigurationManager.AppSettings["password"];
            //this.meno = "admin";
            //this.heslo = "heslo";
            //

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            if (loginTextBox.Text.ToString().Equals(this.meno))
            {
                if (passwordTextBox.Text.ToString().Equals(this.heslo))
                {
                    this.Close();
                }
                else MessageBox.Show("Je zadane zle heslo");
            }
            else MessageBox.Show("Je zadane zle meno");
            //MessageBox.Show("Prihlasenie sa nepodarilo");
        }
    }
}
