using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Broos_Gerrit_PRG282_prj
{
    public partial class MainMenu : Form
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            Settings settingsObject = new Settings();
            settingsObject.Show();
            this.Hide();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            frmSim formObject = new frmSim();
            formObject.Show();
            this.Hide();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

            //Method 1. center at initilization
            this.StartPosition = FormStartPosition.CenterScreen;

            //Method 2. The manual way
            this.StartPosition = FormStartPosition.Manual;
            this.Top = (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2;
            this.Left = (Screen.PrimaryScreen.Bounds.Width - this.Width) / 2;

            pictureBox1.Hide();
            pictureBox2.Hide();
            pictureBox3.Hide();

        }       

        private void btnPlay_MouseHover(object sender, EventArgs e)
        {
            pictureBox1.Show();            
        }

        private void btnPlay_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Hide();
        }

        private void btnSettings_MouseHover(object sender, EventArgs e)
        {
            pictureBox2.Show();
        }

        private void btnSettings_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Hide();
        }

        private void btnExit_MouseHover(object sender, EventArgs e)
        {
            pictureBox3.Show();
        }

        private void btnExit_MouseLeave(object sender, EventArgs e)
        {
            pictureBox3.Hide();
        }


    }
}
