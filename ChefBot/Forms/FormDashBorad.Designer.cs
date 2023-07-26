
namespace Forms
{
    partial class FormDashBorad
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
            this.metroTile1 = new MetroFramework.Controls.MetroTile();
            this.labelYear = new System.Windows.Forms.Label();
            this.metroTile2 = new MetroFramework.Controls.MetroTile();
            this.labelMonth = new System.Windows.Forms.Label();
            this.metroTile3 = new MetroFramework.Controls.MetroTile();
            this.labelWeeks = new System.Windows.Forms.Label();
            this.metroTile4 = new MetroFramework.Controls.MetroTile();
            this.labelDays = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.metroToggle1 = new MetroFramework.Controls.MetroToggle();
            this.metroToggle2 = new MetroFramework.Controls.MetroToggle();
            this.metroToggle3 = new MetroFramework.Controls.MetroToggle();
            this.metroComboBoxSel = new MetroFramework.Controls.MetroComboBox();
            this.winChartViewer1 = new ChartDirector.WinChartViewer();
            this.winChartViewer2 = new ChartDirector.WinChartViewer();
            this.buttonTest = new System.Windows.Forms.Button();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
            this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
            this.metroTile1.SuspendLayout();
            this.metroTile2.SuspendLayout();
            this.metroTile3.SuspendLayout();
            this.metroTile4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer2)).BeginInit();
            this.SuspendLayout();
            // 
            // metroTile1
            // 
            this.metroTile1.Controls.Add(this.labelYear);
            this.metroTile1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTile1.Location = new System.Drawing.Point(7, 4);
            this.metroTile1.Margin = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.metroTile1.Name = "metroTile1";
            this.metroTile1.Size = new System.Drawing.Size(237, 112);
            this.metroTile1.TabIndex = 0;
            this.metroTile1.Text = "연간 생산량";
            // 
            // labelYear
            // 
            this.labelYear.BackColor = System.Drawing.Color.Transparent;
            this.labelYear.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelYear.Font = new System.Drawing.Font("굴림", 40F);
            this.labelYear.Location = new System.Drawing.Point(0, 0);
            this.labelYear.Name = "labelYear";
            this.labelYear.Size = new System.Drawing.Size(237, 83);
            this.labelYear.TabIndex = 0;
            this.labelYear.Text = "99,999";
            this.labelYear.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // metroTile2
            // 
            this.metroTile2.Controls.Add(this.labelMonth);
            this.metroTile2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTile2.Location = new System.Drawing.Point(254, 4);
            this.metroTile2.Margin = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.metroTile2.Name = "metroTile2";
            this.metroTile2.Size = new System.Drawing.Size(237, 112);
            this.metroTile2.TabIndex = 1;
            this.metroTile2.Text = "월간 생산량";
            // 
            // labelMonth
            // 
            this.labelMonth.BackColor = System.Drawing.Color.Transparent;
            this.labelMonth.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelMonth.Font = new System.Drawing.Font("굴림", 40F);
            this.labelMonth.Location = new System.Drawing.Point(0, 0);
            this.labelMonth.Name = "labelMonth";
            this.labelMonth.Size = new System.Drawing.Size(237, 83);
            this.labelMonth.TabIndex = 0;
            this.labelMonth.Text = "99,999";
            this.labelMonth.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // metroTile3
            // 
            this.metroTile3.Controls.Add(this.labelWeeks);
            this.metroTile3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTile3.Location = new System.Drawing.Point(501, 4);
            this.metroTile3.Margin = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.metroTile3.Name = "metroTile3";
            this.metroTile3.Size = new System.Drawing.Size(237, 112);
            this.metroTile3.TabIndex = 2;
            this.metroTile3.Text = "주간 생산량";
            // 
            // labelWeeks
            // 
            this.labelWeeks.BackColor = System.Drawing.Color.Transparent;
            this.labelWeeks.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelWeeks.Font = new System.Drawing.Font("굴림", 40F);
            this.labelWeeks.Location = new System.Drawing.Point(0, 0);
            this.labelWeeks.Name = "labelWeeks";
            this.labelWeeks.Size = new System.Drawing.Size(237, 83);
            this.labelWeeks.TabIndex = 0;
            this.labelWeeks.Text = "99,999";
            this.labelWeeks.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // metroTile4
            // 
            this.metroTile4.Controls.Add(this.labelDays);
            this.metroTile4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.metroTile4.Location = new System.Drawing.Point(748, 4);
            this.metroTile4.Margin = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.metroTile4.Name = "metroTile4";
            this.metroTile4.Size = new System.Drawing.Size(237, 112);
            this.metroTile4.TabIndex = 3;
            this.metroTile4.Text = "하루 생산량";
            // 
            // labelDays
            // 
            this.labelDays.BackColor = System.Drawing.Color.Transparent;
            this.labelDays.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDays.Font = new System.Drawing.Font("굴림", 40F);
            this.labelDays.Location = new System.Drawing.Point(0, 0);
            this.labelDays.Name = "labelDays";
            this.labelDays.Size = new System.Drawing.Size(237, 83);
            this.labelDays.TabIndex = 0;
            this.labelDays.Text = "99,999";
            this.labelDays.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.metroTile1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.metroTile4, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.metroTile2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.metroTile3, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(23, 13);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(992, 120);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // metroToggle1
            // 
            this.metroToggle1.AutoSize = true;
            this.metroToggle1.BackColor = System.Drawing.Color.White;
            this.metroToggle1.Checked = true;
            this.metroToggle1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.metroToggle1.Location = new System.Drawing.Point(529, 204);
            this.metroToggle1.Name = "metroToggle1";
            this.metroToggle1.Size = new System.Drawing.Size(80, 16);
            this.metroToggle1.TabIndex = 5;
            this.metroToggle1.Text = "On";
            this.metroToggle1.UseVisualStyleBackColor = false;
            this.metroToggle1.CheckedChanged += new System.EventHandler(this.IsEnable);
            // 
            // metroToggle2
            // 
            this.metroToggle2.AutoSize = true;
            this.metroToggle2.Checked = true;
            this.metroToggle2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.metroToggle2.Location = new System.Drawing.Point(646, 204);
            this.metroToggle2.Name = "metroToggle2";
            this.metroToggle2.Size = new System.Drawing.Size(80, 16);
            this.metroToggle2.TabIndex = 6;
            this.metroToggle2.Text = "On";
            this.metroToggle2.UseVisualStyleBackColor = true;
            this.metroToggle2.CheckedChanged += new System.EventHandler(this.IsEnable);
            // 
            // metroToggle3
            // 
            this.metroToggle3.AutoSize = true;
            this.metroToggle3.Checked = true;
            this.metroToggle3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.metroToggle3.Location = new System.Drawing.Point(763, 204);
            this.metroToggle3.Name = "metroToggle3";
            this.metroToggle3.Size = new System.Drawing.Size(80, 16);
            this.metroToggle3.TabIndex = 7;
            this.metroToggle3.Text = "On";
            this.metroToggle3.UseVisualStyleBackColor = true;
            this.metroToggle3.CheckedChanged += new System.EventHandler(this.IsEnable);
            // 
            // metroComboBoxSel
            // 
            this.metroComboBoxSel.FormattingEnabled = true;
            this.metroComboBoxSel.ItemHeight = 23;
            this.metroComboBoxSel.Items.AddRange(new object[] {
            "Month",
            "Week"});
            this.metroComboBoxSel.Location = new System.Drawing.Point(893, 191);
            this.metroComboBoxSel.Name = "metroComboBoxSel";
            this.metroComboBoxSel.Size = new System.Drawing.Size(121, 29);
            this.metroComboBoxSel.TabIndex = 8;
            this.metroComboBoxSel.SelectedIndexChanged += new System.EventHandler(this.metroComboBoxSel_SelectedIndexChanged);
            // 
            // winChartViewer1
            // 
            this.winChartViewer1.Location = new System.Drawing.Point(23, 180);
            this.winChartViewer1.Name = "winChartViewer1";
            this.winChartViewer1.Size = new System.Drawing.Size(491, 446);
            this.winChartViewer1.TabIndex = 9;
            this.winChartViewer1.TabStop = false;
            // 
            // winChartViewer2
            // 
            this.winChartViewer2.Location = new System.Drawing.Point(520, 236);
            this.winChartViewer2.Name = "winChartViewer2";
            this.winChartViewer2.Size = new System.Drawing.Size(495, 390);
            this.winChartViewer2.TabIndex = 10;
            this.winChartViewer2.TabStop = false;
            // 
            // buttonTest
            // 
            this.buttonTest.Location = new System.Drawing.Point(893, 138);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(122, 39);
            this.buttonTest.TabIndex = 11;
            this.buttonTest.Text = "Init";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(23, 158);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(101, 19);
            this.metroLabel1.TabIndex = 12;
            this.metroLabel1.Text = "생산 비율 통계";
            // 
            // metroLabel2
            // 
            this.metroLabel2.AutoSize = true;
            this.metroLabel2.Location = new System.Drawing.Point(524, 158);
            this.metroLabel2.Name = "metroLabel2";
            this.metroLabel2.Size = new System.Drawing.Size(51, 19);
            this.metroLabel2.TabIndex = 13;
            this.metroLabel2.Text = "생산량";
            // 
            // metroLabel3
            // 
            this.metroLabel3.AutoSize = true;
            this.metroLabel3.Location = new System.Drawing.Point(529, 182);
            this.metroLabel3.Name = "metroLabel3";
            this.metroLabel3.Size = new System.Drawing.Size(22, 19);
            this.metroLabel3.TabIndex = 14;
            this.metroLabel3.Text = "#1";
            // 
            // metroLabel4
            // 
            this.metroLabel4.AutoSize = true;
            this.metroLabel4.Location = new System.Drawing.Point(646, 182);
            this.metroLabel4.Name = "metroLabel4";
            this.metroLabel4.Size = new System.Drawing.Size(24, 19);
            this.metroLabel4.TabIndex = 15;
            this.metroLabel4.Text = "#2";
            // 
            // metroLabel5
            // 
            this.metroLabel5.AutoSize = true;
            this.metroLabel5.Location = new System.Drawing.Point(763, 182);
            this.metroLabel5.Name = "metroLabel5";
            this.metroLabel5.Size = new System.Drawing.Size(24, 19);
            this.metroLabel5.TabIndex = 16;
            this.metroLabel5.Text = "#3";
            // 
            // FormDashBorad
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1126, 688);
            this.ControlBox = false;
            this.Controls.Add(this.metroLabel5);
            this.Controls.Add(this.metroLabel4);
            this.Controls.Add(this.metroLabel3);
            this.Controls.Add(this.metroLabel2);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.buttonTest);
            this.Controls.Add(this.winChartViewer2);
            this.Controls.Add(this.winChartViewer1);
            this.Controls.Add(this.metroComboBoxSel);
            this.Controls.Add(this.metroToggle3);
            this.Controls.Add(this.metroToggle2);
            this.Controls.Add(this.metroToggle1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("굴림", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FormDashBorad";
            this.Padding = new System.Windows.Forms.Padding(21, 60, 21, 19);
            this.Text = "DashBorad";
            this.Load += new System.EventHandler(this.FormDashBorad_Load);
            this.metroTile1.ResumeLayout(false);
            this.metroTile2.ResumeLayout(false);
            this.metroTile3.ResumeLayout(false);
            this.metroTile4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.winChartViewer2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroTile metroTile1;
        private MetroFramework.Controls.MetroTile metroTile2;
        private MetroFramework.Controls.MetroTile metroTile3;
        private MetroFramework.Controls.MetroTile metroTile4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private MetroFramework.Controls.MetroToggle metroToggle1;
        private MetroFramework.Controls.MetroToggle metroToggle2;
        private MetroFramework.Controls.MetroToggle metroToggle3;
        private MetroFramework.Controls.MetroComboBox metroComboBoxSel;
        private ChartDirector.WinChartViewer winChartViewer1;
        private ChartDirector.WinChartViewer winChartViewer2;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Label labelYear;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroLabel metroLabel2;
        private System.Windows.Forms.Label labelMonth;
        private System.Windows.Forms.Label labelWeeks;
        private System.Windows.Forms.Label labelDays;
        private MetroFramework.Controls.MetroLabel metroLabel3;
        private MetroFramework.Controls.MetroLabel metroLabel4;
        private MetroFramework.Controls.MetroLabel metroLabel5;
    }
}