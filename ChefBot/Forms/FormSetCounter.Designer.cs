
namespace Forms
{
    partial class FormSetCounter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetCounter));
            this.lbDigitalMeter1 = new LBSoft.IndustrialCtrls.Meters.LBDigitalMeter();
            this.labelHeaderName = new System.Windows.Forms.Label();
            this.buttonSecondDown = new System.Windows.Forms.Button();
            this.buttonSecondUp = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnOk = new System.Windows.Forms.Button();
            this.lbDigitalMeter2 = new LBSoft.IndustrialCtrls.Meters.LBDigitalMeter();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbDigitalMeter1
            // 
            this.lbDigitalMeter1.BackColor = System.Drawing.Color.Black;
            this.lbDigitalMeter1.ForeColor = System.Drawing.Color.MistyRose;
            this.lbDigitalMeter1.Format = "00";
            this.lbDigitalMeter1.Location = new System.Drawing.Point(67, 81);
            this.lbDigitalMeter1.Margin = new System.Windows.Forms.Padding(0);
            this.lbDigitalMeter1.Name = "lbDigitalMeter1";
            this.lbDigitalMeter1.Renderer = null;
            this.lbDigitalMeter1.Signed = false;
            this.lbDigitalMeter1.Size = new System.Drawing.Size(200, 200);
            this.lbDigitalMeter1.TabIndex = 519;
            this.lbDigitalMeter1.Value = 0D;
            // 
            // labelHeaderName
            // 
            this.labelHeaderName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelHeaderName.Location = new System.Drawing.Point(0, 0);
            this.labelHeaderName.Name = "labelHeaderName";
            this.labelHeaderName.Size = new System.Drawing.Size(552, 69);
            this.labelHeaderName.TabIndex = 530;
            this.labelHeaderName.Text = "label3";
            this.labelHeaderName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSecondDown
            // 
            this.buttonSecondDown.Image = ((System.Drawing.Image)(resources.GetObject("buttonSecondDown.Image")));
            this.buttonSecondDown.Location = new System.Drawing.Point(488, 181);
            this.buttonSecondDown.Name = "buttonSecondDown";
            this.buttonSecondDown.Size = new System.Drawing.Size(50, 100);
            this.buttonSecondDown.TabIndex = 525;
            this.buttonSecondDown.UseVisualStyleBackColor = true;
            this.buttonSecondDown.Click += new System.EventHandler(this.buttonSecondDown_Click);
            // 
            // buttonSecondUp
            // 
            this.buttonSecondUp.Image = ((System.Drawing.Image)(resources.GetObject("buttonSecondUp.Image")));
            this.buttonSecondUp.Location = new System.Drawing.Point(488, 81);
            this.buttonSecondUp.Name = "buttonSecondUp";
            this.buttonSecondUp.Size = new System.Drawing.Size(50, 100);
            this.buttonSecondUp.TabIndex = 524;
            this.buttonSecondUp.UseVisualStyleBackColor = true;
            this.buttonSecondUp.Click += new System.EventHandler(this.buttonSecondUp_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("BtnCancel.Image")));
            this.BtnCancel.Location = new System.Drawing.Point(209, 302);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(133, 60);
            this.BtnCancel.TabIndex = 523;
            this.BtnCancel.Text = "취소";
            this.BtnCancel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnOk
            // 
            this.BtnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnOk.Image = ((System.Drawing.Image)(resources.GetObject("BtnOk.Image")));
            this.BtnOk.Location = new System.Drawing.Point(361, 302);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(133, 60);
            this.BtnOk.TabIndex = 522;
            this.BtnOk.Text = "저장";
            this.BtnOk.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // lbDigitalMeter2
            // 
            this.lbDigitalMeter2.BackColor = System.Drawing.Color.Black;
            this.lbDigitalMeter2.ForeColor = System.Drawing.Color.White;
            this.lbDigitalMeter2.Format = "00";
            this.lbDigitalMeter2.Location = new System.Drawing.Point(285, 81);
            this.lbDigitalMeter2.Margin = new System.Windows.Forms.Padding(0);
            this.lbDigitalMeter2.Name = "lbDigitalMeter2";
            this.lbDigitalMeter2.Renderer = null;
            this.lbDigitalMeter2.Signed = false;
            this.lbDigitalMeter2.Size = new System.Drawing.Size(200, 200);
            this.lbDigitalMeter2.TabIndex = 521;
            this.lbDigitalMeter2.Value = 0D;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(267, 81);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(18, 200);
            this.tableLayoutPanel1.TabIndex = 520;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 200);
            this.label2.TabIndex = 529;
            this.label2.Text = "현\r\n재\r\n값";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Image = ((System.Drawing.Image)(resources.GetObject("button1.Image")));
            this.button1.Location = new System.Drawing.Point(57, 302);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(133, 60);
            this.button1.TabIndex = 531;
            this.button1.Text = "횟수\r\n초기화";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormSetCounter
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(245)))), ((int)(((byte)(251)))));
            this.ClientSize = new System.Drawing.Size(552, 380);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbDigitalMeter1);
            this.Controls.Add(this.labelHeaderName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonSecondDown);
            this.Controls.Add(this.buttonSecondUp);
            this.Controls.Add(this.BtnCancel);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.lbDigitalMeter2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormSetCounter";
            this.Text = "FormSetCounter";
            this.Load += new System.EventHandler(this.FormSetCounter_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private LBSoft.IndustrialCtrls.Meters.LBDigitalMeter lbDigitalMeter1;
        private System.Windows.Forms.Label labelHeaderName;
        private System.Windows.Forms.Button buttonSecondDown;
        private System.Windows.Forms.Button buttonSecondUp;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnOk;
        private LBSoft.IndustrialCtrls.Meters.LBDigitalMeter lbDigitalMeter2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}