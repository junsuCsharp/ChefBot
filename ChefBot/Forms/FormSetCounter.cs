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
    public partial class FormSetCounter : Form
    {
        public FormSetCounter()
        {
            InitializeComponent();
        }

        public FormSetCounter(int prevCount, int setCount, string header)
        {
            InitializeComponent();

            int fontSize = 12;
            this.labelHeaderName.Font = new Font(Fonts.FontLibrary.Families[0], fontSize, FontStyle.Bold);
            this.labelHeaderName.BackColor = Color.Ivory;            
            this.label2.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);

            //tsCurrentCount = prevCount;
            //tsCurrSetCount = setCount;

            lbDigitalMeter1.Value = prevCount;
            lbDigitalMeter2.Value = setCount;

            this.labelHeaderName.Text = header;
            this.StartPosition = FormStartPosition.CenterScreen;


        }

        public int tsCurrentCount = 0;
        public int tsCurrSetCount = 0;

        private void FormSetCounter_Load(object sender, EventArgs e)
        {

        }

        private void buttonSecondUp_Click(object sender, EventArgs e)
        {
            if (lbDigitalMeter2.Value < 99)
            {
                lbDigitalMeter2.Value++;
            }
            else if (lbDigitalMeter2.Value == 99)
            {
                lbDigitalMeter2.Value = 0;
            }
        }

        private void buttonSecondDown_Click(object sender, EventArgs e)
        {
            if (lbDigitalMeter2.Value > 0)
            {
                lbDigitalMeter2.Value--;
            }
            else if (lbDigitalMeter2.Value == 0)
            {
                lbDigitalMeter2.Value = 99;
            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            tsCurrentCount = (int)lbDigitalMeter1.Value;
            tsCurrSetCount = (int)lbDigitalMeter2.Value;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            lbDigitalMeter1.Value = 0;
        }
    }
}
