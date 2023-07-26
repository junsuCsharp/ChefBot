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
    public partial class FormPause : Form
    {
        public FormPause()
        {
            InitializeComponent();

            devJace.Program.LogSave(NLog.LogManager.GetCurrentClassLogger(), devJace.Program.ELogLevel.Info,
                $"{System.Reflection.MethodBase.GetCurrentMethod().Name}");
        }

        private void FormPause_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.StartPosition = FormStartPosition.CenterScreen;

        }

        public void SwitchState(int left, int center, int right)
        {
            if (left == 1)
            {
                label1.BackColor = Color.Blue;
                label1.ForeColor = Color.Wheat;
            }
            else
            {
                label1.BackColor = Color.Transparent;
                label1.ForeColor = Color.Black;
            }

            if (center == 1)
            {
                label2.BackColor = Color.Red;
                label2.ForeColor = Color.Wheat;
            }
            else
            {
                label2.BackColor = Color.Transparent;
                label2.ForeColor = Color.Black;
            }

            if (right == 1)
            {
                label3.BackColor = Color.Blue;
                label3.ForeColor = Color.Wheat;
            }
            else
            {
                label3.BackColor = Color.Transparent;
                label3.ForeColor = Color.Black;
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
