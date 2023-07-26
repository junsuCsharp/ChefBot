
namespace Forms
{
    partial class FormErrorBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormErrorBox));
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonAlarmClear = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonGripperOpen = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lbLedSwRightPause = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.lbLedSwLeftPause = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioButton1
            // 
            this.radioButton1.Enabled = false;
            this.radioButton1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.radioButton1.FlatAppearance.CheckedBackColor = System.Drawing.Color.White;
            this.radioButton1.Location = new System.Drawing.Point(68, 394);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(120, 50);
            this.radioButton1.TabIndex = 12;
            this.radioButton1.Text = "Warning :";
            this.radioButton1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.Enabled = false;
            this.radioButton2.Location = new System.Drawing.Point(68, 444);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(120, 50);
            this.radioButton2.TabIndex = 13;
            this.radioButton2.Text = "Error :";
            this.radioButton2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.Enabled = false;
            this.radioButton3.Location = new System.Drawing.Point(68, 494);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(120, 50);
            this.radioButton3.TabIndex = 14;
            this.radioButton3.Text = "Fatal :";
            this.radioButton3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(194, 394);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(293, 50);
            this.label1.TabIndex = 19;
            this.label1.Text = "경고 메세지 입니다.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(194, 444);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(293, 50);
            this.label2.TabIndex = 20;
            this.label2.Text = "알람 메세지 입니다.";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(194, 494);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(293, 50);
            this.label3.TabIndex = 21;
            this.label3.Text = "심각한 오류 메세지 입니다.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::ChefBot.Properties.Resources.FormError_Fatal_32_32;
            this.pictureBox3.Location = new System.Drawing.Point(12, 494);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(50, 50);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 24;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ChefBot.Properties.Resources.FormError_Error_32_32;
            this.pictureBox2.Location = new System.Drawing.Point(12, 444);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(50, 50);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 23;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 394);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(50, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 22;
            this.pictureBox1.TabStop = false;
            // 
            // buttonAlarmClear
            // 
            this.buttonAlarmClear.Image = global::ChefBot.Properties.Resources.FormError_AlarmClear_50_50;
            this.buttonAlarmClear.Location = new System.Drawing.Point(792, 496);
            this.buttonAlarmClear.Name = "buttonAlarmClear";
            this.buttonAlarmClear.Size = new System.Drawing.Size(180, 100);
            this.buttonAlarmClear.TabIndex = 15;
            this.buttonAlarmClear.Text = "알람 해제";
            this.buttonAlarmClear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonAlarmClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonAlarmClear.UseVisualStyleBackColor = true;
            this.buttonAlarmClear.Click += new System.EventHandler(this.buttonAlarmClear_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(960, 293);
            this.dataGridView1.TabIndex = 25;
            // 
            // buttonGripperOpen
            // 
            this.buttonGripperOpen.Font = new System.Drawing.Font("굴림", 12F);
            this.buttonGripperOpen.Location = new System.Drawing.Point(493, 394);
            this.buttonGripperOpen.Name = "buttonGripperOpen";
            this.buttonGripperOpen.Size = new System.Drawing.Size(228, 60);
            this.buttonGripperOpen.TabIndex = 530;
            this.buttonGripperOpen.Text = "그리퍼 열림";
            this.buttonGripperOpen.UseVisualStyleBackColor = true;
            this.buttonGripperOpen.Click += new System.EventHandler(this.buttonGripperOpen_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lbLedSwRightPause
            // 
            this.lbLedSwRightPause.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLedSwRightPause.BackColor = System.Drawing.Color.Transparent;
            this.lbLedSwRightPause.BlinkInterval = 500;
            this.lbLedSwRightPause.Label = "오른쪽 일시정지";
            this.lbLedSwRightPause.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Top;
            this.lbLedSwRightPause.LedColor = System.Drawing.Color.Blue;
            this.lbLedSwRightPause.LedSize = new System.Drawing.SizeF(80F, 80F);
            this.lbLedSwRightPause.Location = new System.Drawing.Point(607, 453);
            this.lbLedSwRightPause.Name = "lbLedSwRightPause";
            this.lbLedSwRightPause.Renderer = null;
            this.lbLedSwRightPause.Size = new System.Drawing.Size(143, 143);
            this.lbLedSwRightPause.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.On;
            this.lbLedSwRightPause.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLedSwRightPause.TabIndex = 532;
            // 
            // lbLedSwLeftPause
            // 
            this.lbLedSwLeftPause.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLedSwLeftPause.BackColor = System.Drawing.Color.Transparent;
            this.lbLedSwLeftPause.BlinkInterval = 500;
            this.lbLedSwLeftPause.Label = "왼쪽 일시정지";
            this.lbLedSwLeftPause.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Top;
            this.lbLedSwLeftPause.LedColor = System.Drawing.Color.Blue;
            this.lbLedSwLeftPause.LedSize = new System.Drawing.SizeF(80F, 80F);
            this.lbLedSwLeftPause.Location = new System.Drawing.Point(464, 453);
            this.lbLedSwLeftPause.Name = "lbLedSwLeftPause";
            this.lbLedSwLeftPause.Renderer = null;
            this.lbLedSwLeftPause.Size = new System.Drawing.Size(143, 143);
            this.lbLedSwLeftPause.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.On;
            this.lbLedSwLeftPause.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLedSwLeftPause.TabIndex = 531;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButton4);
            this.panel1.Location = new System.Drawing.Point(744, 394);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(228, 60);
            this.panel1.TabIndex = 533;
            // 
            // radioButton4
            // 
            this.radioButton4.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radioButton4.Font = new System.Drawing.Font("굴림", 12F);
            this.radioButton4.Image = global::ChefBot.Properties.Resources.icons8_robot_arm_50;
            this.radioButton4.Location = new System.Drawing.Point(0, 0);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(228, 60);
            this.radioButton4.TabIndex = 7;
            this.radioButton4.Text = "  코봇 복구 모드";
            this.radioButton4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radioButton4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("굴림", 20F);
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(175, 308);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(797, 34);
            this.label4.TabIndex = 20;
            this.label4.Text = "*** 주의 ***            그리퍼가 닫힘 상태일 경우 ***";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("굴림", 20F);
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(12, 342);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(960, 42);
            this.label5.TabIndex = 534;
            this.label5.Text = "로봇 복구모드만 동작 됩니다. 그리퍼 열림 후 알람 해제 바랍니다.";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormErrorBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(984, 599);
            this.ControlBox = false;
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lbLedSwRightPause);
            this.Controls.Add(this.lbLedSwLeftPause);
            this.Controls.Add(this.buttonGripperOpen);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAlarmClear);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "FormErrorBox";
            this.Text = "Alarm Meassage";
            this.Load += new System.EventHandler(this.FormErrorBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Button buttonAlarmClear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonGripperOpen;
        private System.Windows.Forms.Timer timer1;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLedSwRightPause;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLedSwLeftPause;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton radioButton4;
    }
}