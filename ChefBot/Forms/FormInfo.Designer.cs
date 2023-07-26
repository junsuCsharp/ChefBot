
namespace Forms
{
    partial class FormInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInfo));
            this.treeViewInfo = new System.Windows.Forms.TreeView();
            this.dataGridViewInfo = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBoxAdmin = new System.Windows.Forms.GroupBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBoxAdminPassWord = new System.Windows.Forms.TextBox();
            this.BtnOk = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lbPassWordMsg = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInfo)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBoxAdmin.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewInfo
            // 
            this.treeViewInfo.Location = new System.Drawing.Point(12, 12);
            this.treeViewInfo.Name = "treeViewInfo";
            this.treeViewInfo.Size = new System.Drawing.Size(435, 863);
            this.treeViewInfo.TabIndex = 5;
            // 
            // dataGridViewInfo
            // 
            this.dataGridViewInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewInfo.Location = new System.Drawing.Point(453, 12);
            this.dataGridViewInfo.Name = "dataGridViewInfo";
            this.dataGridViewInfo.RowHeadersWidth = 51;
            this.dataGridViewInfo.RowTemplate.Height = 23;
            this.dataGridViewInfo.Size = new System.Drawing.Size(530, 863);
            this.dataGridViewInfo.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBoxAdmin);
            this.panel1.Controls.Add(this.radioButton2);
            this.panel1.Controls.Add(this.radioButton1);
            this.panel1.Location = new System.Drawing.Point(989, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(917, 863);
            this.panel1.TabIndex = 7;
            // 
            // groupBoxAdmin
            // 
            this.groupBoxAdmin.Controls.Add(this.radioButton5);
            this.groupBoxAdmin.Controls.Add(this.radioButton4);
            this.groupBoxAdmin.Controls.Add(this.radioButton3);
            this.groupBoxAdmin.Controls.Add(this.checkBox2);
            this.groupBoxAdmin.Controls.Add(this.checkBox1);
            this.groupBoxAdmin.Controls.Add(this.textBoxAdminPassWord);
            this.groupBoxAdmin.Controls.Add(this.BtnOk);
            this.groupBoxAdmin.Controls.Add(this.label4);
            this.groupBoxAdmin.Controls.Add(this.lbPassWordMsg);
            this.groupBoxAdmin.Controls.Add(this.label3);
            this.groupBoxAdmin.Location = new System.Drawing.Point(93, 334);
            this.groupBoxAdmin.Name = "groupBoxAdmin";
            this.groupBoxAdmin.Size = new System.Drawing.Size(596, 504);
            this.groupBoxAdmin.TabIndex = 14;
            this.groupBoxAdmin.TabStop = false;
            // 
            // checkBox2
            // 
            this.checkBox2.Font = new System.Drawing.Font("굴림", 12F);
            this.checkBox2.Location = new System.Drawing.Point(21, 407);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(224, 84);
            this.checkBox2.TabIndex = 15;
            this.checkBox2.Text = "GPIO Check Mode";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.Font = new System.Drawing.Font("굴림", 12F);
            this.checkBox1.Location = new System.Drawing.Point(21, 317);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(224, 84);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "ROBOT Debug Mode";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // textBoxAdminPassWord
            // 
            this.textBoxAdminPassWord.Font = new System.Drawing.Font("굴림", 25F);
            this.textBoxAdminPassWord.Location = new System.Drawing.Point(130, 24);
            this.textBoxAdminPassWord.Name = "textBoxAdminPassWord";
            this.textBoxAdminPassWord.PasswordChar = '*';
            this.textBoxAdminPassWord.Size = new System.Drawing.Size(432, 46);
            this.textBoxAdminPassWord.TabIndex = 8;
            this.textBoxAdminPassWord.Click += new System.EventHandler(this.textBoxAdminPassWord_Click);
            this.textBoxAdminPassWord.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxAdminPassWord_KeyDown);
            // 
            // BtnOk
            // 
            this.BtnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnOk.Image = ((System.Drawing.Image)(resources.GetObject("BtnOk.Image")));
            this.BtnOk.Location = new System.Drawing.Point(429, 134);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(133, 65);
            this.BtnOk.TabIndex = 7;
            this.BtnOk.Text = "   OK";
            this.BtnOk.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.BtnOk.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnOk.UseVisualStyleBackColor = true;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(18, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 46);
            this.label4.TabIndex = 13;
            this.label4.Text = "메세지:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbPassWordMsg
            // 
            this.lbPassWordMsg.Location = new System.Drawing.Point(130, 72);
            this.lbPassWordMsg.Name = "lbPassWordMsg";
            this.lbPassWordMsg.Size = new System.Drawing.Size(432, 46);
            this.lbPassWordMsg.TabIndex = 10;
            this.lbPassWordMsg.Text = "label1";
            this.lbPassWordMsg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(18, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 46);
            this.label3.TabIndex = 12;
            this.label3.Text = "비밀번호:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Font = new System.Drawing.Font("굴림", 25F);
            this.radioButton2.Location = new System.Drawing.Point(77, 281);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(135, 38);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "관리자";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Font = new System.Drawing.Font("굴림", 25F);
            this.radioButton1.Location = new System.Drawing.Point(77, 65);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(135, 38);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "사용자";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.Checked = true;
            this.radioButton3.Location = new System.Drawing.Point(414, 317);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(158, 47);
            this.radioButton3.TabIndex = 16;
            this.radioButton3.Text = "ZIMMER";
            this.radioButton3.UseVisualStyleBackColor = true;
            this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton4
            // 
            this.radioButton4.Location = new System.Drawing.Point(414, 381);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(158, 47);
            this.radioButton4.TabIndex = 17;
            this.radioButton4.Text = "DH";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton5
            // 
            this.radioButton5.Location = new System.Drawing.Point(414, 445);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(158, 47);
            this.radioButton5.TabIndex = 18;
            this.radioButton5.Text = "On Robot";
            this.radioButton5.UseVisualStyleBackColor = true;
            this.radioButton5.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // FormInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(245)))), ((int)(((byte)(251)))));
            this.ClientSize = new System.Drawing.Size(1918, 887);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.dataGridViewInfo);
            this.Controls.Add(this.treeViewInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormInfo";
            this.Text = "FormInfo";
            this.Load += new System.EventHandler(this.FormInfo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewInfo)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBoxAdmin.ResumeLayout(false);
            this.groupBoxAdmin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewInfo;
        private System.Windows.Forms.DataGridView dataGridViewInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.TextBox textBoxAdminPassWord;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.GroupBox groupBoxAdmin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbPassWordMsg;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
    }
}