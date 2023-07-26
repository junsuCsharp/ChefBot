using Common;
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
    public partial class FormSetTimer : Form
    {
        public FormSetTimer()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public FormSetTimer(int min, int sec, string header)
        {
            InitializeComponent();

            int fontSize = 12;
            this.labelHeaderName.Font = new Font(Fonts.FontLibrary.Families[0], fontSize, FontStyle.Bold);
            this.labelHeaderName.BackColor = Color.Ivory;
            this.label1.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.label2.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);

            fontSize = 10;
            this.buttonChikenExist.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            

            tsPrevSetTimeMin = min;
            tsPrevSetTimeSec = sec;

            this.labelHeaderName.Text = header;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.Size = new Size(552, 476);
        }

        public FormSetTimer(int min, int sec, string header, bool IsExist)
        {
            InitializeComponent();

            int fontSize = 12;
            this.labelHeaderName.Font = new Font(Fonts.FontLibrary.Families[0], fontSize, FontStyle.Bold);
            this.labelHeaderName.BackColor = Color.Ivory;
            this.label1.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);
            this.label2.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);

            fontSize = 10;
            this.buttonChikenExist.Font = new Font(Fonts.FontLibrary.Families[0], fontSize);


            tsPrevSetTimeMin = min;
            tsPrevSetTimeSec = sec;

            this.labelHeaderName.Text = header;
            this.StartPosition = FormStartPosition.CenterScreen;

            this.buttonChikenExist.Visible = IsExist;

            IsChikenExist = false;
            IsChikenDelete = false;

            //this.Size = new Size(552, 476);
            this.Size = new Size(552, 367);
        }

        //public TimeSpan tsCurrSetTime;
        //public TimeSpan tsPrevSetTime;
        public int tsCurrSetTimeMin = 0;
        public int tsCurrSetTimeSec = 0;
        public int tsPrevSetTimeMin = 0;
        public int tsPrevSetTimeSec = 0;
        public bool IsChikenExist = false;
        public bool IsChikenDelete = false;
        public bool IsChikenStart = false;


        public int tsBuffSetTimeMin = 0;
        public int tsBuffSetTimeSec = 0;

        private void FormSetTimer_Load(object sender, EventArgs e)
        {
            lbDigitalMeter1.Value = tsPrevSetTimeMin;
            lbDigitalMeter2.Value = tsPrevSetTimeSec;
            IsChikenExist = false;
            IsChikenDelete = false;
            IsChikenStart = false;
        }

        private void seltedButton(object sender, EventArgs e)
        {
            Button sel = sender as Button;
            switch (sel.Name)
            {
                case "buttonMinuateUp":
                    if (lbDigitalMeter1.Value < 20)
                    {
                        lbDigitalMeter1.Value++;
                    }
                    else if (lbDigitalMeter1.Value == 20)
                    {
                        lbDigitalMeter1.Value = 0;
                    }
                    tsBuffSetTimeMin = (int)lbDigitalMeter1.Value;
                    break;
                case "buttonMinuateDown":
                    if (lbDigitalMeter1.Value > 0)
                    {
                        lbDigitalMeter1.Value--;
                    }
                    else if (lbDigitalMeter1.Value == 0)
                    {
                        lbDigitalMeter1.Value = 20;
                    }
                    tsBuffSetTimeMin = (int)lbDigitalMeter1.Value;
                    break;
                case "buttonSecondUp":
                    if (lbDigitalMeter2.Value < 60)
                    {
                        lbDigitalMeter2.Value++;
                    }
                    else if (lbDigitalMeter2.Value == 60)
                    {
                        lbDigitalMeter2.Value = 0;
                    }
                    tsBuffSetTimeMin = (int)lbDigitalMeter2.Value;
                    break;
                case "buttonSecondDown":
                    if (lbDigitalMeter2.Value > 0)
                    {
                        lbDigitalMeter2.Value--;
                    }
                    else if (lbDigitalMeter2.Value == 0)
                    {
                        lbDigitalMeter2.Value = 60;
                    }
                    tsBuffSetTimeMin = (int)lbDigitalMeter2.Value;
                    break;
                case "BtnCancel":
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                    break;
                case "BtnOk":
                    tsCurrSetTimeMin = (int)lbDigitalMeter1.Value;
                    tsCurrSetTimeSec = (int)lbDigitalMeter2.Value;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    break;
            }
        }

        private void buttonChikenExist_Click(object sender, EventArgs e)
        {
            FormMessageBox msg = new FormMessageBox("강제 배출 진행 하시겠습니까?");
            if(msg.ShowDialog() == DialogResult.OK) 
            {
                this.DialogResult = DialogResult.OK;
                IsChikenExist = true;
                this.Close();
            }
        }

        private void buttonChikenDelete_Click(object sender, EventArgs e)
        {
            FormMessageBox msg = new FormMessageBox("조리 내용을 삭제 하시겠습니까?");
            if (msg.ShowDialog() == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
                IsChikenDelete = true;
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tsBuffSetTimeMin == 0 && tsBuffSetTimeSec == 0)
            {
                FormMessageBox msgt = new FormMessageBox(FormMessageBox.EButtons.None,  "타이머 설정 후 강제 조리 시작 바랍니다.");
                msgt.ShowDialog();
                return;
            }


            FormMessageBox msg = new FormMessageBox("강제 조리 시작 하시겠습니까?");
            if (msg.ShowDialog() == DialogResult.OK)
            {
                this.DialogResult = DialogResult.OK;
                IsChikenStart = true;
                this.Close();
            }
        }
    }
}
