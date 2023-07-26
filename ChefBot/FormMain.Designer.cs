
namespace Project_Main
{
    partial class FormMain
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.radioButtonDarkMode = new System.Windows.Forms.RadioButton();
            this.radioButtonLightMode = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelCpuUsed = new System.Windows.Forms.Label();
            this.labelMemoryUsed = new System.Windows.Forms.Label();
            this.labelCpuAsage = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.metroPanelMain = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbLed2 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.lbLed6 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.labelDateTime = new System.Windows.Forms.Label();
            this.lbLed7 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.pictureBoxLogo = new System.Windows.Forms.PictureBox();
            this.lbLed3 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.lbLed5 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.lbLed1 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.lbLed4 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.radioButton9 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButtonDarkMode
            // 
            this.radioButtonDarkMode.BackColor = System.Drawing.Color.Transparent;
            this.radioButtonDarkMode.Location = new System.Drawing.Point(1631, 36);
            this.radioButtonDarkMode.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonDarkMode.Name = "radioButtonDarkMode";
            this.radioButtonDarkMode.Size = new System.Drawing.Size(117, 30);
            this.radioButtonDarkMode.TabIndex = 6;
            this.radioButtonDarkMode.TabStop = true;
            this.radioButtonDarkMode.Text = "Dark Mode";
            this.radioButtonDarkMode.UseVisualStyleBackColor = false;
            this.radioButtonDarkMode.CheckedChanged += new System.EventHandler(this.radioButtonDarkMode_CheckedChanged);
            // 
            // radioButtonLightMode
            // 
            this.radioButtonLightMode.BackColor = System.Drawing.Color.Transparent;
            this.radioButtonLightMode.Location = new System.Drawing.Point(1631, 6);
            this.radioButtonLightMode.Margin = new System.Windows.Forms.Padding(0);
            this.radioButtonLightMode.Name = "radioButtonLightMode";
            this.radioButtonLightMode.Size = new System.Drawing.Size(117, 30);
            this.radioButtonLightMode.TabIndex = 5;
            this.radioButtonLightMode.TabStop = true;
            this.radioButtonLightMode.Text = "Light Mode";
            this.radioButtonLightMode.UseVisualStyleBackColor = false;
            this.radioButtonLightMode.CheckedChanged += new System.EventHandler(this.radioButtonLightMode_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(56)))), ((int)(((byte)(100)))));
            this.panel1.Controls.Add(this.radioButtonLightMode);
            this.panel1.Controls.Add(this.radioButtonDarkMode);
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.radioButton9);
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Controls.Add(this.radioButton7);
            this.panel1.Controls.Add(this.radioButton6);
            this.panel1.Controls.Add(this.radioButton2);
            this.panel1.Controls.Add(this.radioButton8);
            this.panel1.Controls.Add(this.radioButton4);
            this.panel1.Controls.Add(this.radioButton3);
            this.panel1.Controls.Add(this.radioButton5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 967);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1918, 71);
            this.panel1.TabIndex = 14;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.labelCpuUsed, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelMemoryUsed, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelCpuAsage, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1379, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(271, 60);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // labelCpuUsed
            // 
            this.labelCpuUsed.BackColor = System.Drawing.Color.Transparent;
            this.labelCpuUsed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCpuUsed.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.labelCpuUsed.Location = new System.Drawing.Point(0, 40);
            this.labelCpuUsed.Margin = new System.Windows.Forms.Padding(0);
            this.labelCpuUsed.Name = "labelCpuUsed";
            this.labelCpuUsed.Size = new System.Drawing.Size(271, 20);
            this.labelCpuUsed.TabIndex = 6;
            this.labelCpuUsed.Text = "CPU";
            this.labelCpuUsed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMemoryUsed
            // 
            this.labelMemoryUsed.BackColor = System.Drawing.Color.Transparent;
            this.labelMemoryUsed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMemoryUsed.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.labelMemoryUsed.Location = new System.Drawing.Point(0, 20);
            this.labelMemoryUsed.Margin = new System.Windows.Forms.Padding(0);
            this.labelMemoryUsed.Name = "labelMemoryUsed";
            this.labelMemoryUsed.Size = new System.Drawing.Size(271, 20);
            this.labelMemoryUsed.TabIndex = 5;
            this.labelMemoryUsed.Text = "CPU";
            this.labelMemoryUsed.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelCpuAsage
            // 
            this.labelCpuAsage.BackColor = System.Drawing.Color.Transparent;
            this.labelCpuAsage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCpuAsage.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.labelCpuAsage.Location = new System.Drawing.Point(0, 0);
            this.labelCpuAsage.Margin = new System.Windows.Forms.Padding(0);
            this.labelCpuAsage.Name = "labelCpuAsage";
            this.labelCpuAsage.Size = new System.Drawing.Size(271, 20);
            this.labelCpuAsage.TabIndex = 4;
            this.labelCpuAsage.Text = "CPU";
            this.labelCpuAsage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBar1.Location = new System.Drawing.Point(0, 0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1918, 1);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 30;
            this.progressBar1.Value = 100;
            // 
            // metroPanelMain
            // 
            this.metroPanelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(245)))), ((int)(((byte)(251)))));
            this.metroPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroPanelMain.Location = new System.Drawing.Point(0, 80);
            this.metroPanelMain.Name = "metroPanelMain";
            this.metroPanelMain.Size = new System.Drawing.Size(1918, 887);
            this.metroPanelMain.TabIndex = 15;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(245)))), ((int)(((byte)(251)))));
            this.panel2.BackgroundImage = global::ChefBot.Properties.Resources.FormMain_Top;
            this.panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Controls.Add(this.lbLed2);
            this.panel2.Controls.Add(this.lbLed6);
            this.panel2.Controls.Add(this.labelDateTime);
            this.panel2.Controls.Add(this.lbLed7);
            this.panel2.Controls.Add(this.pictureBoxLogo);
            this.panel2.Controls.Add(this.lbLed3);
            this.panel2.Controls.Add(this.lbLed5);
            this.panel2.Controls.Add(this.lbLed1);
            this.panel2.Controls.Add(this.lbLed4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1918, 80);
            this.panel2.TabIndex = 16;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ChefBot.Properties.Resources.JDLOGO2;
            this.pictureBox1.Location = new System.Drawing.Point(167, 3);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(161, 74);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // lbLed2
            // 
            this.lbLed2.BackColor = System.Drawing.Color.Transparent;
            this.lbLed2.BlinkInterval = 500;
            this.lbLed2.Label = "PC";
            this.lbLed2.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Left;
            this.lbLed2.LedColor = System.Drawing.Color.Red;
            this.lbLed2.LedSize = new System.Drawing.SizeF(13F, 13F);
            this.lbLed2.Location = new System.Drawing.Point(1829, 53);
            this.lbLed2.Name = "lbLed2";
            this.lbLed2.Renderer = null;
            this.lbLed2.Size = new System.Drawing.Size(64, 19);
            this.lbLed2.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
            this.lbLed2.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLed2.TabIndex = 8;
            // 
            // lbLed6
            // 
            this.lbLed6.BackColor = System.Drawing.Color.Transparent;
            this.lbLed6.BlinkInterval = 500;
            this.lbLed6.Label = "EIO_(3)";
            this.lbLed6.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Left;
            this.lbLed6.LedColor = System.Drawing.Color.Red;
            this.lbLed6.LedSize = new System.Drawing.SizeF(13F, 13F);
            this.lbLed6.Location = new System.Drawing.Point(1673, 53);
            this.lbLed6.Name = "lbLed6";
            this.lbLed6.Renderer = null;
            this.lbLed6.Size = new System.Drawing.Size(64, 19);
            this.lbLed6.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
            this.lbLed6.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLed6.TabIndex = 13;
            // 
            // labelDateTime
            // 
            this.labelDateTime.BackColor = System.Drawing.Color.Transparent;
            this.labelDateTime.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.labelDateTime.Location = new System.Drawing.Point(1614, 3);
            this.labelDateTime.Name = "labelDateTime";
            this.labelDateTime.Size = new System.Drawing.Size(301, 47);
            this.labelDateTime.TabIndex = 4;
            this.labelDateTime.Text = "PM 04:34:00 2023-01-31";
            this.labelDateTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbLed7
            // 
            this.lbLed7.BackColor = System.Drawing.Color.Transparent;
            this.lbLed7.BlinkInterval = 500;
            this.lbLed7.Label = "EIO_(4)";
            this.lbLed7.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Left;
            this.lbLed7.LedColor = System.Drawing.Color.Red;
            this.lbLed7.LedSize = new System.Drawing.SizeF(13F, 13F);
            this.lbLed7.Location = new System.Drawing.Point(1751, 53);
            this.lbLed7.Name = "lbLed7";
            this.lbLed7.Renderer = null;
            this.lbLed7.Size = new System.Drawing.Size(64, 19);
            this.lbLed7.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
            this.lbLed7.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLed7.TabIndex = 12;
            // 
            // pictureBoxLogo
            // 
            this.pictureBoxLogo.Image = global::ChefBot.Properties.Resources.Doosan_Robotics_Partner_logo_lightblue;
            this.pictureBoxLogo.Location = new System.Drawing.Point(0, 3);
            this.pictureBoxLogo.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBoxLogo.Name = "pictureBoxLogo";
            this.pictureBoxLogo.Size = new System.Drawing.Size(161, 74);
            this.pictureBoxLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxLogo.TabIndex = 2;
            this.pictureBoxLogo.TabStop = false;
            // 
            // lbLed3
            // 
            this.lbLed3.BackColor = System.Drawing.Color.Transparent;
            this.lbLed3.BlinkInterval = 500;
            this.lbLed3.Label = "X Axis";
            this.lbLed3.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Left;
            this.lbLed3.LedColor = System.Drawing.Color.Red;
            this.lbLed3.LedSize = new System.Drawing.SizeF(13F, 13F);
            this.lbLed3.Location = new System.Drawing.Point(1439, 53);
            this.lbLed3.Name = "lbLed3";
            this.lbLed3.Renderer = null;
            this.lbLed3.Size = new System.Drawing.Size(64, 19);
            this.lbLed3.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
            this.lbLed3.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLed3.TabIndex = 9;
            // 
            // lbLed5
            // 
            this.lbLed5.BackColor = System.Drawing.Color.Transparent;
            this.lbLed5.BlinkInterval = 500;
            this.lbLed5.Label = "EIO_(2)";
            this.lbLed5.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Left;
            this.lbLed5.LedColor = System.Drawing.Color.Red;
            this.lbLed5.LedSize = new System.Drawing.SizeF(13F, 13F);
            this.lbLed5.Location = new System.Drawing.Point(1595, 53);
            this.lbLed5.Name = "lbLed5";
            this.lbLed5.Renderer = null;
            this.lbLed5.Size = new System.Drawing.Size(64, 19);
            this.lbLed5.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
            this.lbLed5.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLed5.TabIndex = 11;
            // 
            // lbLed1
            // 
            this.lbLed1.BackColor = System.Drawing.Color.Transparent;
            this.lbLed1.BlinkInterval = 500;
            this.lbLed1.Label = "Cobot   ";
            this.lbLed1.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Left;
            this.lbLed1.LedColor = System.Drawing.Color.Lime;
            this.lbLed1.LedSize = new System.Drawing.SizeF(13F, 13F);
            this.lbLed1.Location = new System.Drawing.Point(1361, 53);
            this.lbLed1.Name = "lbLed1";
            this.lbLed1.Renderer = null;
            this.lbLed1.Size = new System.Drawing.Size(64, 19);
            this.lbLed1.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
            this.lbLed1.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLed1.TabIndex = 7;
            // 
            // lbLed4
            // 
            this.lbLed4.BackColor = System.Drawing.Color.Transparent;
            this.lbLed4.BlinkInterval = 500;
            this.lbLed4.Label = "EIO_(1)";
            this.lbLed4.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Left;
            this.lbLed4.LedColor = System.Drawing.Color.Red;
            this.lbLed4.LedSize = new System.Drawing.SizeF(13F, 13F);
            this.lbLed4.Location = new System.Drawing.Point(1517, 53);
            this.lbLed4.Name = "lbLed4";
            this.lbLed4.Renderer = null;
            this.lbLed4.Size = new System.Drawing.Size(64, 19);
            this.lbLed4.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
            this.lbLed4.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLed4.TabIndex = 10;
            // 
            // radioButton9
            // 
            this.radioButton9.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(199)))), ((int)(((byte)(232)))));
            this.radioButton9.FlatAppearance.BorderSize = 0;
            this.radioButton9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton9.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton9.ForeColor = System.Drawing.Color.White;
            this.radioButton9.Image = global::ChefBot.Properties.Resources.debug_48;
            this.radioButton9.Location = new System.Drawing.Point(1208, 6);
            this.radioButton9.Name = "radioButton9";
            this.radioButton9.Size = new System.Drawing.Size(155, 60);
            this.radioButton9.TabIndex = 11;
            this.radioButton9.TabStop = true;
            this.radioButton9.Text = "디버그";
            this.radioButton9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radioButton9.UseVisualStyleBackColor = false;
            this.radioButton9.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            this.radioButton9.Click += new System.EventHandler(this.radioButton9_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(199)))), ((int)(((byte)(232)))));
            this.radioButton1.FlatAppearance.BorderSize = 0;
            this.radioButton1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton1.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton1.ForeColor = System.Drawing.Color.White;
            this.radioButton1.Image = global::ChefBot.Properties.Resources.Main_48;
            this.radioButton1.Location = new System.Drawing.Point(11, 6);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(155, 60);
            this.radioButton1.TabIndex = 3;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = " 메인";
            this.radioButton1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radioButton1.UseVisualStyleBackColor = false;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // radioButton7
            // 
            this.radioButton7.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(199)))), ((int)(((byte)(232)))));
            this.radioButton7.FlatAppearance.BorderSize = 0;
            this.radioButton7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton7.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton7.ForeColor = System.Drawing.Color.White;
            this.radioButton7.Image = global::ChefBot.Properties.Resources.Alarm_48;
            this.radioButton7.Location = new System.Drawing.Point(1037, 6);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(155, 60);
            this.radioButton7.TabIndex = 9;
            this.radioButton7.TabStop = true;
            this.radioButton7.Text = " 알람";
            this.radioButton7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radioButton7.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radioButton7.UseVisualStyleBackColor = false;
            this.radioButton7.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // radioButton6
            // 
            this.radioButton6.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(199)))), ((int)(((byte)(232)))));
            this.radioButton6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.radioButton6.FlatAppearance.BorderSize = 0;
            this.radioButton6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton6.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton6.ForeColor = System.Drawing.Color.White;
            this.radioButton6.Image = global::ChefBot.Properties.Resources.power_black_blue_32;
            this.radioButton6.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radioButton6.Location = new System.Drawing.Point(1751, 6);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(155, 60);
            this.radioButton6.TabIndex = 8;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "종료";
            this.radioButton6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton6.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radioButton6.UseVisualStyleBackColor = false;
            this.radioButton6.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(199)))), ((int)(((byte)(232)))));
            this.radioButton2.FlatAppearance.BorderSize = 0;
            this.radioButton2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton2.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton2.ForeColor = System.Drawing.Color.White;
            this.radioButton2.Image = global::ChefBot.Properties.Resources.Auto_Driving_32;
            this.radioButton2.Location = new System.Drawing.Point(182, 6);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(155, 60);
            this.radioButton2.TabIndex = 4;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = " 자동 운전";
            this.radioButton2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radioButton2.UseVisualStyleBackColor = false;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // radioButton8
            // 
            this.radioButton8.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(199)))), ((int)(((byte)(232)))));
            this.radioButton8.FlatAppearance.BorderSize = 0;
            this.radioButton8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton8.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton8.ForeColor = System.Drawing.Color.White;
            this.radioButton8.Image = global::ChefBot.Properties.Resources.information_48;
            this.radioButton8.Location = new System.Drawing.Point(866, 6);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(155, 60);
            this.radioButton8.TabIndex = 10;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = " 정보";
            this.radioButton8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radioButton8.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radioButton8.UseVisualStyleBackColor = false;
            this.radioButton8.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(199)))), ((int)(((byte)(232)))));
            this.radioButton4.FlatAppearance.BorderSize = 0;
            this.radioButton4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton4.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton4.ForeColor = System.Drawing.Color.White;
            this.radioButton4.Image = global::ChefBot.Properties.Resources.Option_32;
            this.radioButton4.Location = new System.Drawing.Point(524, 6);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(155, 60);
            this.radioButton4.TabIndex = 6;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = " 설정";
            this.radioButton4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.radioButton4.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radioButton4.UseVisualStyleBackColor = false;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(199)))), ((int)(((byte)(232)))));
            this.radioButton3.FlatAppearance.BorderSize = 0;
            this.radioButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton3.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton3.ForeColor = System.Drawing.Color.White;
            this.radioButton3.Image = global::ChefBot.Properties.Resources.Manual_Driving_32;
            this.radioButton3.Location = new System.Drawing.Point(353, 6);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(155, 60);
            this.radioButton3.TabIndex = 5;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = " 수동 운전";
            this.radioButton3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radioButton3.UseVisualStyleBackColor = false;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // radioButton5
            // 
            this.radioButton5.Appearance = System.Windows.Forms.Appearance.Button;
            this.radioButton5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(172)))), ((int)(((byte)(199)))), ((int)(((byte)(232)))));
            this.radioButton5.FlatAppearance.BorderSize = 0;
            this.radioButton5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.radioButton5.Font = new System.Drawing.Font("Arial Black", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton5.ForeColor = System.Drawing.Color.White;
            this.radioButton5.Image = global::ChefBot.Properties.Resources.Monitor_48;
            this.radioButton5.Location = new System.Drawing.Point(695, 6);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(155, 60);
            this.radioButton5.TabIndex = 7;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = " 모니터";
            this.radioButton5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radioButton5.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.radioButton5.UseVisualStyleBackColor = false;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton5_CheckedChanged);
            // 
            // FormMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1918, 1038);
            this.Controls.Add(this.metroPanelMain);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximumSize = new System.Drawing.Size(1918, 1038);
            this.Name = "FormMain";
            this.Text = "JD System";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.RadioButton radioButton9;
        private System.Windows.Forms.Label labelDateTime;
        private System.Windows.Forms.PictureBox pictureBoxLogo;
        private System.Windows.Forms.RadioButton radioButtonDarkMode;
        private System.Windows.Forms.RadioButton radioButtonLightMode;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLed5;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLed4;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLed3;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLed2;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLed1;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLed6;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLed7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel metroPanelMain;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelCpuAsage;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelCpuUsed;
        private System.Windows.Forms.Label labelMemoryUsed;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

