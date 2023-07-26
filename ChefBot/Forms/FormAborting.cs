using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Forms
{
    public partial class FormAborting : Form
    {
        public FormAborting()
        {
            InitializeComponent();

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
              $"{System.Reflection.MethodBase.GetCurrentMethod().Name}");
        }

        private void FormAborting_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.TopMost= true;

            this.Size = new Size(1280, 720);
            //this.Size = new Size(1920, 1080);
            //this.Location = new Point(0, 0);

            //label1.Font = new Font(Fonts.FontLibrary.Families[0], 100);

            //this.Location = new Point(0, 1000);
            //this.Size = new Size(1920, 1080);

            this.Size = new Size(1280, 720);
            this.Location = new Point(320, 3);
            this.BtnOk.Location = new Point(this.Size.Width - BtnOk.Size.Width , this.Size.Height - BtnOk.Size.Height);

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
               $"{System.Reflection.MethodBase.GetCurrentMethod().Name}");
        }

        public void meassage(string msg, int level)
        { 
            //label1.Text = msg;

            switch(level)             
            {
                case 0:
                    this.BackColor = Color.Moccasin;

                    pictureBox1.Image = global::ChefBot.Properties.Resources.Warning_of_industrial_robot_;
                    break;

                case 1:
                    this.BackColor = Color.Red;
                    pictureBox1.Image = global::ChefBot.Properties.Resources.laserscnner_error_ei;
                    break;
            }
            //label1.Font = new Font(Fonts.FontLibrary.Families[0], 100);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private string msg = null;
    }
}
