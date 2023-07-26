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
    public partial class FormMessageBox : Form
    {
        public FormMessageBox(string strText)
        {
            InitializeComponent();

            LabelMsg.Text = strText;
        }

        public FormMessageBox(EButtons btn,  string strText)
        {
            InitializeComponent();

            LabelMsg.Text = strText;

            if (btn == EButtons.None)
            {
                BtnCancel.Visible = false;
            }
        }

        public FormMessageBox(EButtons btn, string strText, int SecTimer)
        {
            InitializeComponent();

            LabelMsg.Text = strText;
            strTextBuffer = strText;
            if (btn == EButtons.None)
            {
                BtnCancel.Visible = false;
            }
            //timeSpan = new TimeSpan(0,0, SecTimer);
            iCounter = SecTimer;
            closeTimer = new Timer();
            closeTimer.Interval = 1000;
            closeTimer.Tick += CloseTimer_Tick;
            closeTimer.Start();
        }

        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            LabelMsg.Text = $"{strTextBuffer}\r\n {iCounter--}초뒤 메세지 박스 종료 됩니다.";
            if (iCounter < 0)
            {
                closeTimer.Stop();
                this.Close();
            }
        }

        string strTextBuffer;
        Timer closeTimer;
        //TimeSpan timeSpan;
        int iCounter = 0;

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public enum EButtons
        {
            Normal, None
        }
    }
}
