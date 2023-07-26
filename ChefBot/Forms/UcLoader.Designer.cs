
namespace Forms
{
    partial class UcLoader
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
            this.labelState = new System.Windows.Forms.Label();
            this.labelIndexName = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelSetIO = new System.Windows.Forms.Label();
            this.labelEventName = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lbLedSw1 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.lbLedSw2 = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.lbLedState = new LBSoft.IndustrialCtrls.Leds.LBLed();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelState
            // 
            this.labelState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelState.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelState.Location = new System.Drawing.Point(3, 76);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(193, 76);
            this.labelState.TabIndex = 0;
            this.labelState.Text = "None";
            this.labelState.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.tableLayoutPanel1.Controls.Add(this.labelSetIO, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.labelEventName, 0, 2);
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
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // labelSetIO
            // 
            this.labelSetIO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSetIO.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelSetIO.Location = new System.Drawing.Point(3, 228);
            this.labelSetIO.Name = "labelSetIO";
            this.labelSetIO.Size = new System.Drawing.Size(193, 76);
            this.labelSetIO.TabIndex = 2;
            this.labelSetIO.Text = "IN/OUT";
            this.labelSetIO.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelSetIO.Click += new System.EventHandler(this.labelSetIO_Click);
            // 
            // labelEventName
            // 
            this.labelEventName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelEventName.Font = new System.Drawing.Font("굴림", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.labelEventName.Location = new System.Drawing.Point(3, 152);
            this.labelEventName.Name = "labelEventName";
            this.labelEventName.Size = new System.Drawing.Size(193, 76);
            this.labelEventName.TabIndex = 1;
            this.labelEventName.Text = "mm:ss";
            this.labelEventName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.lbLedSw1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lbLedSw2, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.lbLedState, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 307);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(193, 19);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // lbLedSw1
            // 
            this.lbLedSw1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLedSw1.BackColor = System.Drawing.Color.Transparent;
            this.lbLedSw1.BlinkInterval = 500;
            this.lbLedSw1.Label = "Led";
            this.lbLedSw1.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Top;
            this.lbLedSw1.LedColor = System.Drawing.Color.Black;
            this.lbLedSw1.LedSize = new System.Drawing.SizeF(10F, 10F);
            this.lbLedSw1.Location = new System.Drawing.Point(3, 3);
            this.lbLedSw1.Name = "lbLedSw1";
            this.lbLedSw1.Renderer = null;
            this.lbLedSw1.Size = new System.Drawing.Size(58, 13);
            this.lbLedSw1.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
            this.lbLedSw1.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLedSw1.TabIndex = 5;
            // 
            // lbLedSw2
            // 
            this.lbLedSw2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLedSw2.BackColor = System.Drawing.Color.Transparent;
            this.lbLedSw2.BlinkInterval = 500;
            this.lbLedSw2.Label = "Led";
            this.lbLedSw2.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Top;
            this.lbLedSw2.LedColor = System.Drawing.Color.Black;
            this.lbLedSw2.LedSize = new System.Drawing.SizeF(10F, 10F);
            this.lbLedSw2.Location = new System.Drawing.Point(131, 3);
            this.lbLedSw2.Name = "lbLedSw2";
            this.lbLedSw2.Renderer = null;
            this.lbLedSw2.Size = new System.Drawing.Size(59, 13);
            this.lbLedSw2.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
            this.lbLedSw2.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLedSw2.TabIndex = 6;
            // 
            // lbLedState
            // 
            this.lbLedState.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbLedState.BackColor = System.Drawing.Color.Transparent;
            this.lbLedState.BlinkInterval = 500;
            this.lbLedState.Label = "Led";
            this.lbLedState.LabelPosition = LBSoft.IndustrialCtrls.Leds.LBLed.LedLabelPosition.Top;
            this.lbLedState.LedColor = System.Drawing.Color.Black;
            this.lbLedState.LedSize = new System.Drawing.SizeF(10F, 10F);
            this.lbLedState.Location = new System.Drawing.Point(67, 3);
            this.lbLedState.Name = "lbLedState";
            this.lbLedState.Renderer = null;
            this.lbLedState.Size = new System.Drawing.Size(58, 13);
            this.lbLedState.State = LBSoft.IndustrialCtrls.Leds.LBLed.LedState.Off;
            this.lbLedState.Style = LBSoft.IndustrialCtrls.Leds.LBLed.LedStyle.Circular;
            this.lbLedState.TabIndex = 4;
            // 
            // UcLoader
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UcLoader";
            this.Size = new System.Drawing.Size(199, 329);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelState;
        private System.Windows.Forms.Label labelIndexName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelSetIO;
        private System.Windows.Forms.Label labelEventName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLedSw1;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLedSw2;
        private LBSoft.IndustrialCtrls.Leds.LBLed lbLedState;
    }
}
