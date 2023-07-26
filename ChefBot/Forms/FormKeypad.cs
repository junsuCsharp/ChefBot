using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Common
{
    public partial class FormKeypad : Form
    {
        private string strInputNo;
        private string strPrevNo;

        public FormKeypad(string strNumber)
        {
            InitializeComponent();
            strPrevNo = strNumber;
            this.KeyPreview = true;
        }

        private void BtnNoClick(object sender, EventArgs e)
        {
            Button BtnNo = sender as Button;

            switch(BtnNo.Text)
            {
                case "0":
                    strInputNo += "0"; 
                    break;
                case "1":
                    strInputNo += "1";
                    break;
                case "2":
                    strInputNo += "2";
                    break;
                case "3":
                    strInputNo += "3";
                    break;
                case "4":
                    strInputNo += "4";
                    break;
                case "5":
                    strInputNo += "5";
                    break;
                case "6":
                    strInputNo += "6";
                    break;
                case "7":
                    strInputNo += "7";
                    break;
                case "8":
                    strInputNo += "8";
                    break;
                case "9":
                    strInputNo += "9";
                    break;
                case "-":
                    strInputNo += "-";
                    break;
            }

            LabelInputNo.Text = strInputNo;

        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            int nLength = 0;

            if(strInputNo == string.Empty || strInputNo == null)
            {
                return;
            }

            nLength = strInputNo.Length;

            if(nLength > 0)
            {
                strInputNo = strInputNo.Substring(0, nLength - 1);
            }

            LabelInputNo.Text = strInputNo;

        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            strInputNo = string.Empty;
            LabelInputNo.Text = strInputNo;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void BtnPM_Click(object sender, EventArgs e)
        {
            if (strInputNo == string.Empty || strInputNo == null)
            {
                return;
            }

            string strSigh = string.Empty;

            strSigh = strInputNo.Substring(0, 1);

           if(strSigh != "-")
           {
                strInputNo = "-" + strInputNo;
           }
           else
           {
               strInputNo = strInputNo.Substring(1);
           }

            LabelInputNo.Text = strInputNo;
        }

        private void BtnCom_Click(object sender, EventArgs e)
        {
            if (strInputNo == string.Empty || strInputNo == null)
            {
                return;
            }

            strInputNo += ".";
            LabelInputNo.Text = strInputNo;
        }

        private void FormKeypad_Shown(object sender, EventArgs e)
        {
            LabelPrevNo.Text = strPrevNo;
        }

        public string UserInputNo
        {
            get
            {
                return strInputNo;
            }

            set
            {
                strInputNo = value;
            }
        }

        private void FormKeypad_KeyDown(object sender, KeyEventArgs e)
        {
            string x = Keys.KeyCode.ToString();
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.D1:
                    BtnNoClick(BtnNo1, null);
                    break;
                case Keys.NumPad1:
                    BtnNoClick(BtnNo1, null);
                    break;
                case Keys.D2:
                    BtnNoClick(BtnNo2, null);
                    break;
                case Keys.NumPad2:
                    BtnNoClick(BtnNo2, null);
                    break;
                case Keys.D3:
                    BtnNoClick(BtnNo3, null);
                    break;
                case Keys.NumPad3:
                    BtnNoClick(BtnNo3, null);
                    break;
                case Keys.D4:
                    BtnNoClick(BtnNo4, null);
                    break;
                case Keys.NumPad4:
                    BtnNoClick(BtnNo4, null);
                    break;
                case Keys.D5:
                    BtnNoClick(BtnNo5, null);
                    break;
                case Keys.NumPad5:
                    BtnNoClick(BtnNo5, null);
                    break;
                case Keys.D6:
                    BtnNoClick(BtnNo6, null);
                    break;
                case Keys.NumPad6:
                    BtnNoClick(BtnNo6, null);
                    break;
                case Keys.D7:
                    BtnNoClick(BtnNo7, null);
                    break;
                case Keys.NumPad7:
                    BtnNoClick(BtnNo7, null);
                    break;
                case Keys.D8:
                    BtnNoClick(BtnNo8, null);
                    break;
                case Keys.NumPad8:
                    BtnNoClick(BtnNo8, null);
                    break;
                case Keys.D9:
                    BtnNoClick(BtnNo9, null);
                    break;
                case Keys.NumPad9:
                    BtnNoClick(BtnNo9, null);
                    break;
                case Keys.D0:
                    BtnNoClick(BtnNo0, null);
                    break;
                case Keys.NumPad0:
                    BtnNoClick(BtnNo0, null);
                    break;
                case Keys.Back:
                    BtnBack_Click(null, null);
                    break;
                case Keys.Clear:
                    BtnClear_Click(null, null);
                    break;
                case Keys.Enter:
                    BtnOK_Click(null, null);
                    break;
                case Keys.Decimal:
                    BtnCom_Click(null, null);
                    break;
                case Keys.OemPeriod:
                    BtnCom_Click(null, null);
                    break;
                case Keys.OemMinus:
                    BtnPM_Click(null, null);
                    break;
            }
        }
    }
}
