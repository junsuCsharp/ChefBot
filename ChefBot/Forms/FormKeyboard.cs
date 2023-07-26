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
    public partial class FormKeyboard : Form
    {
        private string strInputChar;

        public FormKeyboard(string PrevChar)
        {
            InitializeComponent();
            
            FormKeyboard Keyboard = this;
            Keyboard.Text = "Keyboard : " + PrevChar;
            
            this.KeyPreview = true;
        }

        public string UserInputChar
        {
            get
            {
                return strInputChar;
            }

            set
            {
                strInputChar = value;
            }
        }

        private void BtnCharClick(object sender, EventArgs e)
        {
            Button BtnChar = sender as Button;

            strInputChar += BtnChar.Text;

            LabelInputChar.Text = strInputChar;
        }

        private void BtnSpace_Click(object sender, EventArgs e)
        {
            strInputChar = strInputChar + " ";
            LabelInputChar.Text = strInputChar;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            int nLength = 0;

            if (strInputChar == string.Empty || strInputChar == null)
            {
                return;
            }

            nLength = strInputChar.Length;

            if (nLength > 0)
            {
                strInputChar = strInputChar.Substring(0, nLength - 1);
            }

            LabelInputChar.Text = strInputChar;
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

        private void BtnClear_Click(object sender, EventArgs e)
        {
            strInputChar = string.Empty;
            LabelInputChar.Text = strInputChar;
        }

        private void FormKeyboard_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.D1:
                    BtnCharClick(BtnChar1, null);
                    break;
                    case Keys.D2:
                        BtnCharClick(BtnChar2, null);
                        break;
                    case Keys.D3:
                        BtnCharClick(BtnChar3, null);
                        break;
                    case Keys.D4:
                        BtnCharClick(BtnChar4, null);
                        break;
                    case Keys.D5:
                        BtnCharClick(BtnChar5, null);
                        break;
                    case Keys.D6:
                        BtnCharClick(BtnChar6, null);
                        break;
                    case Keys.D7:
                        BtnCharClick(BtnChar7, null);
                        break;
                    case Keys.D8:
                        BtnCharClick(BtnChar8, null);
                        break;
                    case Keys.D9:
                        BtnCharClick(BtnChar9, null);
                        break;
                    case Keys.D0:
                        BtnCharClick(BtnChar0, null);
                        break;
                    case Keys.Back:
                        BtnBack_Click(BtnBack, null);
                        break;
                    case Keys.Clear:
                        BtnClear_Click(BtnClear, null);
                        break;
                    case Keys.Enter:
                        BtnOK_Click(null, null);
                        break;
                    case Keys.Q:
                        BtnCharClick(BtnCharQ, null);
                        break;
                    case Keys.W:
                        BtnCharClick(BtnCharW, null);
                        break;
                    case Keys.E:
                        BtnCharClick(BtnCharE, null);
                        break;
                    case Keys.R:
                        BtnCharClick(BtnCharR, null);
                        break;
                    case Keys.T:
                        BtnCharClick(BtnCharT, null);
                        break;
                    case Keys.Y:
                        BtnCharClick(BtnCharY, null);
                        break;
                    case Keys.U:
                        BtnCharClick(BtnCharU, null);
                        break;
                    case Keys.I:
                        BtnCharClick(BtnCharI, null);
                        break;
                    case Keys.O:
                        BtnCharClick(BtnCharO, null);
                        break;
                    case Keys.P:
                        BtnCharClick(BtnCharP, null);
                        break;
                    case Keys.A:
                        BtnCharClick(BtnCharA, null);
                        break;
                    case Keys.S:
                        BtnCharClick(BtnCharS, null);
                        break;
                    case Keys.D:
                        BtnCharClick(BtnCharD, null);
                        break;
                    case Keys.F:
                        BtnCharClick(BtnCharF, null);
                        break;
                    case Keys.G:
                        BtnCharClick(BtnCharG, null);
                        break;
                    case Keys.H:
                        BtnCharClick(BtnCharH, null);
                        break;
                    case Keys.J:
                        BtnCharClick(BtnCharJ, null);
                        break;
                    case Keys.K:
                        BtnCharClick(BtnCharK, null);
                        break;
                    case Keys.L:
                        BtnCharClick(BtnCharL, null);
                        break;
                    case Keys.Z:
                        BtnCharClick(BtnCharZ, null);
                        break;
                    case Keys.X:
                        BtnCharClick(BtnCharX, null);
                        break;
                    case Keys.C:
                        BtnCharClick(BtnCharC, null);
                        break;
                    case Keys.V:
                        BtnCharClick(BtnCharV, null);
                        break;
                    case Keys.B:
                        BtnCharClick(BtnCharB, null);
                        break;
                    case Keys.N:
                        BtnCharClick(BtnCharN, null);
                        break;
                    case Keys.M:
                        BtnCharClick(BtnCharM, null);
                        break;
                    case Keys.Shift:
                    BtnCharClick(BtnCharM, null);
                    break;
                    case Keys.OemPeriod:
                        BtnCharClick(BtnColon, null);
                        break;
                    case Keys.Subtract:
                    BtnCharClick(BtnHyphen, null);
                        break;
                    //case Keys._:
                //        BtnCharClick(BtnUnderScore, null);
                //        break;
                    case Keys.Divide:
                    BtnCharClick(BtnSlash, null);
                        break;
                    case Keys.Cancel:
                        BtnCancel_Click(null, null);
                        break;
                    case Keys.Space:
                        BtnSpace_Click(null, null);
                        break;
            }
        }

        private void FormKeyboard_Load(object sender, EventArgs e)
        {

        }
    }
}


