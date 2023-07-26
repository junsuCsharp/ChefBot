
namespace Forms
{
    partial class UcCooker
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

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelIndexName = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelCurrTime = new System.Windows.Forms.Label();
            this.labelSetTime = new System.Windows.Forms.Label();
            this.labelState = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lbLedState = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelIndexName
            // 
            this.labelIndexName.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelIndexName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelIndexName.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelIndexName.ForeColor = System.Drawing.Color.White;
            this.labelIndexName.Location = new System.Drawing.Point(3, 0);
            this.labelIndexName.Name = "labelIndexName";
            this.labelIndexName.Size = new System.Drawing.Size(193, 76);
            this.labelIndexName.TabIndex = 0;
            this.labelIndexName.Text = "indexName";
            this.labelIndexName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.labelCurrTime, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelSetTime, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelState, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelIndexName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(199, 329);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // labelCurrTime
            // 
            this.labelCurrTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCurrTime.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelCurrTime.Location = new System.Drawing.Point(3, 228);
            this.labelCurrTime.Name = "labelCurrTime";
            this.labelCurrTime.Size = new System.Drawing.Size(193, 76);
            this.labelCurrTime.TabIndex = 2;
            this.labelCurrTime.Text = "etcState";
            this.labelCurrTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSetTime
            // 
            this.labelSetTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSetTime.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelSetTime.Location = new System.Drawing.Point(3, 152);
            this.labelSetTime.Name = "labelSetTime";
            this.labelSetTime.Size = new System.Drawing.Size(193, 76);
            this.labelSetTime.TabIndex = 1;
            this.labelSetTime.Text = "Set mm:ss";
            this.labelSetTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelSetTime.Click += new System.EventHandler(this.labelSetTime_Click);
            // 
            // labelState
            // 
            this.labelState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelState.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelState.Location = new System.Drawing.Point(3, 76);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(193, 76);
            this.labelState.TabIndex = 0;
            this.labelState.Text = "cookingState";
            this.labelState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.lbLedState, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 307);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(193, 19);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // lbLedState
            // 
            this.lbLedState.BackColor = System.Drawing.Color.Transparent;
            this.lbLedState.BlinkInterval = 500;
            this.lbLedState.Label = "Led";
            this.lbLedState.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Top;
            this.lbLedState.LedColor = System.Drawing.Color.Red;
            this.lbLedState.LedSize = new System.Drawing.SizeF(1960F, 10F);
            this.lbLedState.Location = new System.Drawing.Point(3, 3);
            this.lbLedState.Name = "lbLedState";
            this.lbLedState.Renderer = null;
            this.lbLedState.Size = new System.Drawing.Size(187, 13);
            this.lbLedState.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
            this.lbLedState.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLedState.TabIndex = 0;
            // 
            // UcCooker
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UcCooker";
            this.Size = new System.Drawing.Size(199, 329);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelIndexName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelCurrTime;
        private System.Windows.Forms.Label labelSetTime;
        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLedState;
    }
}
