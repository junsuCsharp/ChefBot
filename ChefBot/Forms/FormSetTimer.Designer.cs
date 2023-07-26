
namespace Forms
{
    partial class FormSetTimer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetTimer));
            this.lbDigitalMeter1 = new LBSoft.IndustrialCtrls.Meters.LBDigitalMeter();
            this.lbLed1 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.lbLed2 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lbDigitalMeter2 = new LBSoft.IndustrialCtrls.Meters.LBDigitalMeter();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelHeaderName = new System.Windows.Forms.Label();
            this.buttonMinuateDown = new System.Windows.Forms.Button();
            this.buttonMinuateUp = new System.Windows.Forms.Button();
            this.buttonSecondDown = new System.Windows.Forms.Button();
            this.buttonSecondUp = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnOk = new System.Windows.Forms.Button();
            this.buttonChikenExist = new System.Windows.Forms.Button();
            this.buttonChikenDelete = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbDigitalMeter1
            // 
            this.lbDigitalMeter1.BackColor = System.Drawing.Color.Black;
            this.lbDigitalMeter1.ForeColor = System.Drawing.Color.MistyRose;
            this.lbDigitalMeter1.Format = "00";
            this.lbDigitalMeter1.Location = new System.Drawing.Point(67, 69);
            this.lbDigitalMeter1.Margin = new System.Windows.Forms.Padding(0);
            this.lbDigitalMeter1.Name = "lbDigitalMeter1";
            this.lbDigitalMeter1.Renderer = null;
            this.lbDigitalMeter1.Signed = false;
            this.lbDigitalMeter1.Size = new System.Drawing.Size(200, 200);
            this.lbDigitalMeter1.TabIndex = 0;
            this.lbDigitalMeter1.Value = 0D;
            // 
            // lbLed1
            // 
            this.lbLed1.BackColor = System.Drawing.Color.Transparent;
            this.lbLed1.BlinkInterval = 500;
            this.lbLed1.Label = "";
            this.lbLed1.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Top;
            this.lbLed1.LedColor = System.Drawing.Color.Gainsboro;
            this.lbLed1.LedSize = new System.Drawing.SizeF(12F, 12F);
            this.lbLed1.Location = new System.Drawing.Point(3, 43);
            this.lbLed1.Name = "lbLed1";
            this.lbLed1.Renderer = null;
            this.lbLed1.Size = new System.Drawing.Size(12, 32);
            this.lbLed1.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
            this.lbLed1.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLed1.TabIndex = 1;
            // 
            // lbLed2
            // 
            this.lbLed2.BackColor = System.Drawing.Color.Transparent;
            this.lbLed2.BlinkInterval = 500;
            this.lbLed2.Label = "";
            this.lbLed2.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Bottom;
            this.lbLed2.LedColor = System.Drawing.Color.Gainsboro;
            this.lbLed2.LedSize = new System.Drawing.SizeF(12F, 12F);
            this.lbLed2.Location = new System.Drawing.Point(3, 123);
            this.lbLed2.Name = "lbLed2";
            this.lbLed2.Renderer = null;
            this.lbLed2.Size = new System.Drawing.Size(12, 32);
            this.lbLed2.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Blink;
            this.lbLed2.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLed2.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lbLed1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbLed2, 0, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(267, 69);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(18, 200);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // lbDigitalMeter2
            // 
            this.lbDigitalMeter2.BackColor = System.Drawing.Color.Black;
            this.lbDigitalMeter2.ForeColor = System.Drawing.Color.White;
            this.lbDigitalMeter2.Format = "00";
            this.lbDigitalMeter2.Location = new System.Drawing.Point(285, 69);
            this.lbDigitalMeter2.Margin = new System.Windows.Forms.Padding(0);
            this.lbDigitalMeter2.Name = "lbDigitalMeter2";
            this.lbDigitalMeter2.Renderer = null;
            this.lbDigitalMeter2.Signed = false;
            this.lbDigitalMeter2.Size = new System.Drawing.Size(200, 200);
            this.lbDigitalMeter2.TabIndex = 4;
            this.lbDigitalMeter2.Value = 0D;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 272);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 50);
            this.label1.TabIndex = 516;
            this.label1.Text = "분";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(488, 272);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 50);
            this.label2.TabIndex = 517;
            this.label2.Text = "초";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelHeaderName
            // 
            this.labelHeaderName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHeaderName.Location = new System.Drawing.Point(0, 0);
            this.labelHeaderName.Name = "labelHeaderName";
            this.labelHeaderName.Size = new System.Drawing.Size(552, 69);
            this.labelHeaderName.TabIndex = 518;
            this.labelHeaderName.Text = "label3";
            this.labelHeaderName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonMinuateDown
            // 
            this.buttonMinuateDown.Image = ((System.Drawing.Image)(resources.GetObject("buttonMinuateDown.Image")));
            this.buttonMinuateDown.Location = new System.Drawing.Point(14, 169);
            this.buttonMinuateDown.Name = "buttonMinuateDown";
            this.buttonMinuateDown.Size = new System.Drawing.Size(50, 100);
            this.buttonMinuateDown.TabIndex = 515;
            this.buttonMinuateDown.UseVisualStyleBackColor = true;
            this.buttonMinuateDown.Click += new System.EventHandler(this.seltedButton);
            // 
            // buttonMinuateUp
            // 
            this.buttonMinuateUp.Image = ((System.Drawing.Image)(resources.GetObject("buttonMinuateUp.Image")));
            this.buttonMinuateUp.Location = new System.Drawing.Point(14, 69);
            this.buttonMinuateUp.Name = "buttonMinuateUp";
            this.buttonMinuateUp.Size = new System.Drawing.Size(50, 100);
            this.buttonMinuateUp.TabIndex = 514;
            this.buttonMinuateUp.UseVisualStyleBackColor = true;
            this.buttonMinuateUp.Click += new System.EventHandler(this.seltedButton);
            // 
            // buttonSecondDown
            // 
            this.buttonSecondDown.Image = ((System.Drawing.Image)(resources.GetObject("buttonSecondDown.Image")));
            this.buttonSecondDown.Location = new System.Drawing.Point(488, 169);
            this.buttonSecondDown.Name = "buttonSecondDown";
            this.buttonSecondDown.Size = new System.Drawing.Size(50, 100);
            this.buttonSecondDown.TabIndex = 513;
            this.buttonSecondDown.UseVisualStyleBackColor = true;
            this.buttonSecondDown.Click += new System.EventHandler(this.seltedButton);
            // 
            // buttonSecondUp
            // 
            this.buttonSecondUp.Image = ((System.Drawing.Image)(resources.GetObject("buttonSecondUp.Image")));
            this.buttonSecondUp.Location = new System.Drawing.Point(488, 69);
            this.buttonSecondUp.Name = "buttonSecondUp";
            this.buttonSecondUp.Size = new System.Drawing.Size(50, 100);
            this.buttonSecondUp.TabIndex = 512;
            this.buttonSecondUp.UseVisualStyleBackColor = true;
            this.buttonSecondUp.Click += new System.EventHandler(this.seltedButton);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("BtnCancel.Image")));
            this.BtnCancel.Location = new System.Drawing.Point(134, 297);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(133, 60);
            this.BtnCancel.TabIndex = 93;
            this.BtnCancel.Text = "취소";
            this.BtnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.seltedButton);
            // 
            // BtnOk
            // 
            this.BtnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnOk.Image = ((System.Drawing.Image)(resources.GetObject("BtnOk.Image")));
            this.BtnOk.Location = new System.Drawing.Point(285, 297);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(133, 60);
            this.BtnOk.TabIndex = 92;
            this.BtnOk.Text = "저장";
            this.BtnOk.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.seltedButton);
            // 
            // buttonChikenExist
            // 
            this.buttonChikenExist.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonChikenExist.Image = global::ChefBot.Properties.Resources.Exit_red_full_32;
            this.buttonChikenExist.Location = new System.Drawing.Point(285, 404);
            this.buttonChikenExist.Name = "buttonChikenExist";
            this.buttonChikenExist.Size = new System.Drawing.Size(253, 60);
            this.buttonChikenExist.TabIndex = 519;
            this.buttonChikenExist.Text = "강제 배출";
            this.buttonChikenExist.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonChikenExist.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonChikenExist.UseVisualStyleBackColor = true;
            this.buttonChikenExist.Click += new System.EventHandler(this.buttonChikenExist_Click);
            // 
            // buttonChikenDelete
            // 
            this.buttonChikenDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonChikenDelete.Image = global::ChefBot.Properties.Resources.Exit_black_full_32;
            this.buttonChikenDelete.Location = new System.Drawing.Point(14, 404);
            this.buttonChikenDelete.Name = "buttonChikenDelete";
            this.buttonChikenDelete.Size = new System.Drawing.Size(253, 60);
            this.buttonChikenDelete.TabIndex = 520;
            this.buttonChikenDelete.Text = "조리 내용 삭제";
            this.buttonChikenDelete.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonChikenDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonChikenDelete.UseVisualStyleBackColor = true;
            this.buttonChikenDelete.Click += new System.EventHandler(this.buttonChikenDelete_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Image = global::ChefBot.Properties.Resources.Manual_Driving_32;
            this.button1.Location = new System.Drawing.Point(14, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(114, 60);
            this.button1.TabIndex = 521;
            this.button1.Text = "조리 시작";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormSetTimer
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(245)))), ((int)(((byte)(251)))));
            this.ClientSize = new System.Drawing.Size(552, 476);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonChikenDelete);
            this.Controls.Add(this.buttonChikenExist);
            this.Controls.Add(this.labelHeaderName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonMinuateDown);
            this.Controls.Add(this.buttonMinuateUp);
            this.Controls.Add(this.buttonSecondDown);
            this.Controls.Add(this.buttonSecondUp);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.lbDigitalMeter2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.lbDigitalMeter1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSetTimer";
            this.Text = "FormSetTimer";
            this.Load += new System.EventHandler(this.FormSetTimer_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private LBSoft.IndustrialCtrls.Meters.LBDigitalMeter lbDigitalMeter1;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLed1;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLed2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private LBSoft.IndustrialCtrls.Meters.LBDigitalMeter lbDigitalMeter2;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Button buttonSecondUp;
        private System.Windows.Forms.Button buttonSecondDown;
        private System.Windows.Forms.Button buttonMinuateDown;
        private System.Windows.Forms.Button buttonMinuateUp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelHeaderName;
        private System.Windows.Forms.Button buttonChikenExist;
        private System.Windows.Forms.Button buttonChikenDelete;
        private System.Windows.Forms.Button button1;
    }
}