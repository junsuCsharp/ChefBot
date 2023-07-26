using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChefBot
{
    public partial class FormTutorial : Form
    {
        public FormTutorial()
        {
            InitializeComponent();
        }

        public FormTutorial(Point pt)
        {
            InitializeComponent();
            this.TopMost = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Location = pt;
        }

        private void FormTutorial_Load(object sender, EventArgs e)
        {
         
            //this.BackColor = Color.Black;
            //this.Location = this.ParentForm.Location;
        }

        public void SetTutorial(double opt, ETryal eTryal)
        {
            this.Opacity = opt;

            switch (eTryal)
            {
                default:
                    break;

                case ETryal.Automode:
                    break;

                case ETryal.Cobot_Ready:
                    break;

                case ETryal.Cobot_Start:
                    break;

                case ETryal.Cobot_Abort:
                    break;


            }
        }

        public enum ETryal
        { 
            Automode,  Cobot_Ready, Cobot_Start, Cobot_Abort
        }

        private void FormTutorial_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
