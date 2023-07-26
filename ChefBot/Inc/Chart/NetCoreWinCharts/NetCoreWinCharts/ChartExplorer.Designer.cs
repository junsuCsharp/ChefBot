namespace CSharpChartExplorer
{
    partial class ChartExplorer
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChartExplorer));
			this.treeView = new System.Windows.Forms.TreeView();
			this.treeViewImageList = new System.Windows.Forms.ImageList(this.components);
			this.splitter = new System.Windows.Forms.Splitter();
			this.rightPanel = new System.Windows.Forms.Panel();
			this.chartViewer8 = new ChartDirector.WinChartViewer();
			this.chartViewer7 = new ChartDirector.WinChartViewer();
			this.chartViewer6 = new ChartDirector.WinChartViewer();
			this.chartViewer5 = new ChartDirector.WinChartViewer();
			this.chartViewer4 = new ChartDirector.WinChartViewer();
			this.chartViewer3 = new ChartDirector.WinChartViewer();
			this.chartViewer2 = new ChartDirector.WinChartViewer();
			this.line = new System.Windows.Forms.Label();
			this.title = new System.Windows.Forms.Label();
			this.chartViewer1 = new ChartDirector.WinChartViewer();
			this.toolStrip = new System.Windows.Forms.ToolStrip();
			this.backButton = new System.Windows.Forms.ToolStripButton();
			this.forwardButton = new System.Windows.Forms.ToolStripButton();
			this.previousButton = new System.Windows.Forms.ToolStripButton();
			this.nextButton = new System.Windows.Forms.ToolStripButton();
			this.viewCodeButton = new System.Windows.Forms.ToolStripButton();
			this.viewDocButton = new System.Windows.Forms.ToolStripButton();
			this.statusBar = new System.Windows.Forms.StatusStrip();
			this.statusBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.rightPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.chartViewer8)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chartViewer7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chartViewer6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chartViewer5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chartViewer4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chartViewer3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chartViewer2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chartViewer1)).BeginInit();
			this.toolStrip.SuspendLayout();
			this.statusBar.SuspendLayout();
			this.SuspendLayout();
			// 
			// treeView
			// 
			this.treeView.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeView.HotTracking = true;
			this.treeView.ImageIndex = 0;
			this.treeView.ImageList = this.treeViewImageList;
			this.treeView.ItemHeight = 16;
			this.treeView.Location = new System.Drawing.Point(0, 50);
			this.treeView.Name = "treeView";
			this.treeView.SelectedImageIndex = 0;
			this.treeView.Size = new System.Drawing.Size(230, 489);
			this.treeView.TabIndex = 5;
			this.treeView.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeCollapse);
			this.treeView.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView_BeforeExpand);
			this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
			// 
			// treeViewImageList
			// 
			this.treeViewImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeViewImageList.ImageStream")));
			this.treeViewImageList.TransparentColor = System.Drawing.Color.Transparent;
			this.treeViewImageList.Images.SetKeyName(0, "");
			this.treeViewImageList.Images.SetKeyName(1, "");
			this.treeViewImageList.Images.SetKeyName(2, "");
			this.treeViewImageList.Images.SetKeyName(3, "");
			this.treeViewImageList.Images.SetKeyName(4, "");
			this.treeViewImageList.Images.SetKeyName(5, "");
			this.treeViewImageList.Images.SetKeyName(6, "");
			this.treeViewImageList.Images.SetKeyName(7, "");
			this.treeViewImageList.Images.SetKeyName(8, "");
			this.treeViewImageList.Images.SetKeyName(9, "");
			this.treeViewImageList.Images.SetKeyName(10, "");
			this.treeViewImageList.Images.SetKeyName(11, "");
			this.treeViewImageList.Images.SetKeyName(12, "");
			this.treeViewImageList.Images.SetKeyName(13, "");
			this.treeViewImageList.Images.SetKeyName(14, "");
			this.treeViewImageList.Images.SetKeyName(15, "");
			this.treeViewImageList.Images.SetKeyName(16, "");
			this.treeViewImageList.Images.SetKeyName(17, "");
			this.treeViewImageList.Images.SetKeyName(18, "");
			this.treeViewImageList.Images.SetKeyName(19, "");
			this.treeViewImageList.Images.SetKeyName(20, "");
			this.treeViewImageList.Images.SetKeyName(21, "linearmetericon.png");
			this.treeViewImageList.Images.SetKeyName(22, "barmetericon.png");
			// 
			// splitter
			// 
			this.splitter.Location = new System.Drawing.Point(230, 50);
			this.splitter.Name = "splitter";
			this.splitter.Size = new System.Drawing.Size(3, 489);
			this.splitter.TabIndex = 6;
			this.splitter.TabStop = false;
			// 
			// rightPanel
			// 
			this.rightPanel.AutoScroll = true;
			this.rightPanel.BackColor = System.Drawing.SystemColors.Window;
			this.rightPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.rightPanel.Controls.Add(this.chartViewer8);
			this.rightPanel.Controls.Add(this.chartViewer7);
			this.rightPanel.Controls.Add(this.chartViewer6);
			this.rightPanel.Controls.Add(this.chartViewer5);
			this.rightPanel.Controls.Add(this.chartViewer4);
			this.rightPanel.Controls.Add(this.chartViewer3);
			this.rightPanel.Controls.Add(this.chartViewer2);
			this.rightPanel.Controls.Add(this.line);
			this.rightPanel.Controls.Add(this.title);
			this.rightPanel.Controls.Add(this.chartViewer1);
			this.rightPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rightPanel.Location = new System.Drawing.Point(233, 50);
			this.rightPanel.Name = "rightPanel";
			this.rightPanel.Size = new System.Drawing.Size(711, 489);
			this.rightPanel.TabIndex = 7;
			this.rightPanel.Layout += new System.Windows.Forms.LayoutEventHandler(this.rightPanel_Layout);
			// 
			// chartViewer8
			// 
			this.chartViewer8.HotSpotCursor = System.Windows.Forms.Cursors.Hand;
			this.chartViewer8.Location = new System.Drawing.Point(368, 162);
			this.chartViewer8.Name = "chartViewer8";
			this.chartViewer8.Size = new System.Drawing.Size(112, 98);
			this.chartViewer8.TabIndex = 9;
			this.chartViewer8.TabStop = false;
			this.chartViewer8.ClickHotSpot += new ChartDirector.WinHotSpotEventHandler(this.chartViewer_ClickHotSpot);
			// 
			// chartViewer7
			// 
			this.chartViewer7.HotSpotCursor = System.Windows.Forms.Cursors.Hand;
			this.chartViewer7.Location = new System.Drawing.Point(248, 162);
			this.chartViewer7.Name = "chartViewer7";
			this.chartViewer7.Size = new System.Drawing.Size(112, 98);
			this.chartViewer7.TabIndex = 8;
			this.chartViewer7.TabStop = false;
			this.chartViewer7.ClickHotSpot += new ChartDirector.WinHotSpotEventHandler(this.chartViewer_ClickHotSpot);
			// 
			// chartViewer6
			// 
			this.chartViewer6.HotSpotCursor = System.Windows.Forms.Cursors.Hand;
			this.chartViewer6.Location = new System.Drawing.Point(128, 162);
			this.chartViewer6.Name = "chartViewer6";
			this.chartViewer6.Size = new System.Drawing.Size(112, 98);
			this.chartViewer6.TabIndex = 7;
			this.chartViewer6.TabStop = false;
			this.chartViewer6.ClickHotSpot += new ChartDirector.WinHotSpotEventHandler(this.chartViewer_ClickHotSpot);
			// 
			// chartViewer5
			// 
			this.chartViewer5.HotSpotCursor = System.Windows.Forms.Cursors.Hand;
			this.chartViewer5.Location = new System.Drawing.Point(6, 162);
			this.chartViewer5.Name = "chartViewer5";
			this.chartViewer5.Size = new System.Drawing.Size(112, 98);
			this.chartViewer5.TabIndex = 6;
			this.chartViewer5.TabStop = false;
			this.chartViewer5.ClickHotSpot += new ChartDirector.WinHotSpotEventHandler(this.chartViewer_ClickHotSpot);
			// 
			// chartViewer4
			// 
			this.chartViewer4.HotSpotCursor = System.Windows.Forms.Cursors.Hand;
			this.chartViewer4.Location = new System.Drawing.Point(368, 56);
			this.chartViewer4.Name = "chartViewer4";
			this.chartViewer4.Size = new System.Drawing.Size(112, 98);
			this.chartViewer4.TabIndex = 5;
			this.chartViewer4.TabStop = false;
			this.chartViewer4.ClickHotSpot += new ChartDirector.WinHotSpotEventHandler(this.chartViewer_ClickHotSpot);
			// 
			// chartViewer3
			// 
			this.chartViewer3.HotSpotCursor = System.Windows.Forms.Cursors.Hand;
			this.chartViewer3.Location = new System.Drawing.Point(248, 56);
			this.chartViewer3.Name = "chartViewer3";
			this.chartViewer3.Size = new System.Drawing.Size(112, 98);
			this.chartViewer3.TabIndex = 4;
			this.chartViewer3.TabStop = false;
			this.chartViewer3.ClickHotSpot += new ChartDirector.WinHotSpotEventHandler(this.chartViewer_ClickHotSpot);
			// 
			// chartViewer2
			// 
			this.chartViewer2.HotSpotCursor = System.Windows.Forms.Cursors.Hand;
			this.chartViewer2.Location = new System.Drawing.Point(128, 56);
			this.chartViewer2.Name = "chartViewer2";
			this.chartViewer2.Size = new System.Drawing.Size(112, 98);
			this.chartViewer2.TabIndex = 3;
			this.chartViewer2.TabStop = false;
			this.chartViewer2.ClickHotSpot += new ChartDirector.WinHotSpotEventHandler(this.chartViewer_ClickHotSpot);
			// 
			// line
			// 
			this.line.BackColor = System.Drawing.Color.DarkBlue;
			this.line.Dock = System.Windows.Forms.DockStyle.Top;
			this.line.Location = new System.Drawing.Point(0, 29);
			this.line.Name = "line";
			this.line.Size = new System.Drawing.Size(707, 3);
			this.line.TabIndex = 2;
			// 
			// title
			// 
			this.title.AutoSize = true;
			this.title.Dock = System.Windows.Forms.DockStyle.Top;
			this.title.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.title.Location = new System.Drawing.Point(0, 0);
			this.title.Name = "title";
			this.title.Size = new System.Drawing.Size(494, 29);
			this.title.TabIndex = 1;
			this.title.Text = "Up to 8 charts in each demo module";
			// 
			// chartViewer1
			// 
			this.chartViewer1.HotSpotCursor = System.Windows.Forms.Cursors.Hand;
			this.chartViewer1.Location = new System.Drawing.Point(8, 56);
			this.chartViewer1.Name = "chartViewer1";
			this.chartViewer1.Size = new System.Drawing.Size(112, 98);
			this.chartViewer1.TabIndex = 0;
			this.chartViewer1.TabStop = false;
			this.chartViewer1.ClickHotSpot += new ChartDirector.WinHotSpotEventHandler(this.chartViewer_ClickHotSpot);
			// 
			// toolStrip
			// 
			this.toolStrip.AutoSize = false;
			this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.backButton,
            this.forwardButton,
            this.previousButton,
            this.nextButton,
            this.viewCodeButton,
            this.viewDocButton});
			this.toolStrip.Location = new System.Drawing.Point(0, 0);
			this.toolStrip.Name = "toolStrip";
			this.toolStrip.Size = new System.Drawing.Size(944, 50);
			this.toolStrip.TabIndex = 8;
			this.toolStrip.TabStop = true;
			// 
			// backButton
			// 
			this.backButton.AutoSize = false;
			this.backButton.AutoToolTip = false;
			this.backButton.Image = ((System.Drawing.Image)(resources.GetObject("backButton.Image")));
			this.backButton.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.backButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.backButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.backButton.Name = "backButton";
			this.backButton.Size = new System.Drawing.Size(60, 47);
			this.backButton.Text = "Back";
			this.backButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.backButton.Click += new System.EventHandler(this.backButton_Click);
			// 
			// forwardButton
			// 
			this.forwardButton.AutoSize = false;
			this.forwardButton.AutoToolTip = false;
			this.forwardButton.Image = ((System.Drawing.Image)(resources.GetObject("forwardButton.Image")));
			this.forwardButton.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.forwardButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.forwardButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.forwardButton.Name = "forwardButton";
			this.forwardButton.Size = new System.Drawing.Size(60, 47);
			this.forwardButton.Text = "Forward";
			this.forwardButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.forwardButton.Click += new System.EventHandler(this.forwardButton_Click);
			// 
			// previousButton
			// 
			this.previousButton.AutoSize = false;
			this.previousButton.AutoToolTip = false;
			this.previousButton.Image = ((System.Drawing.Image)(resources.GetObject("previousButton.Image")));
			this.previousButton.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.previousButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.previousButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.previousButton.Name = "previousButton";
			this.previousButton.Size = new System.Drawing.Size(60, 47);
			this.previousButton.Text = "Previous";
			this.previousButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.previousButton.Click += new System.EventHandler(this.previousButton_Click);
			// 
			// nextButton
			// 
			this.nextButton.AutoSize = false;
			this.nextButton.AutoToolTip = false;
			this.nextButton.Image = ((System.Drawing.Image)(resources.GetObject("nextButton.Image")));
			this.nextButton.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.nextButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.nextButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.nextButton.Name = "nextButton";
			this.nextButton.Size = new System.Drawing.Size(60, 47);
			this.nextButton.Text = "Next";
			this.nextButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
			// 
			// viewCodeButton
			// 
			this.viewCodeButton.AutoSize = false;
			this.viewCodeButton.AutoToolTip = false;
			this.viewCodeButton.Image = ((System.Drawing.Image)(resources.GetObject("viewCodeButton.Image")));
			this.viewCodeButton.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.viewCodeButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.viewCodeButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.viewCodeButton.Name = "viewCodeButton";
			this.viewCodeButton.Size = new System.Drawing.Size(70, 47);
			this.viewCodeButton.Text = "View Code";
			this.viewCodeButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.viewCodeButton.Click += new System.EventHandler(this.viewCodeButton_Click);
			// 
			// viewDocButton
			// 
			this.viewDocButton.AutoSize = false;
			this.viewDocButton.AutoToolTip = false;
			this.viewDocButton.Image = ((System.Drawing.Image)(resources.GetObject("viewDocButton.Image")));
			this.viewDocButton.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.viewDocButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
			this.viewDocButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.viewDocButton.Name = "viewDocButton";
			this.viewDocButton.Size = new System.Drawing.Size(70, 47);
			this.viewDocButton.Text = "View Doc";
			this.viewDocButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
			this.viewDocButton.Click += new System.EventHandler(this.viewDocButton_Click);
			// 
			// statusBar
			// 
			this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarLabel});
			this.statusBar.Location = new System.Drawing.Point(0, 539);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(944, 22);
			this.statusBar.TabIndex = 9;
			// 
			// statusBarLabel
			// 
			this.statusBarLabel.Name = "statusBarLabel";
			this.statusBarLabel.Size = new System.Drawing.Size(144, 17);
			this.statusBarLabel.Text = "Please select chart to view";
			// 
			// ChartExplorer
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.ClientSize = new System.Drawing.Size(944, 561);
			this.Controls.Add(this.rightPanel);
			this.Controls.Add(this.splitter);
			this.Controls.Add(this.treeView);
			this.Controls.Add(this.toolStrip);
			this.Controls.Add(this.statusBar);
			this.Name = "ChartExplorer";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ChartDirector Sample Charts";
			this.rightPanel.ResumeLayout(false);
			this.rightPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.chartViewer8)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartViewer7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartViewer6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartViewer5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartViewer4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartViewer3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartViewer2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartViewer1)).EndInit();
			this.toolStrip.ResumeLayout(false);
			this.toolStrip.PerformLayout();
			this.statusBar.ResumeLayout(false);
			this.statusBar.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
		private System.Windows.Forms.TreeView treeView;
		private System.Windows.Forms.ImageList treeViewImageList;
		private System.Windows.Forms.Splitter splitter;
		private System.Windows.Forms.Panel rightPanel;
		private System.Windows.Forms.Label title;
		private System.Windows.Forms.Label line;
		private ChartDirector.WinChartViewer chartViewer1;
		private ChartDirector.WinChartViewer chartViewer2;
		private ChartDirector.WinChartViewer chartViewer3;
		private ChartDirector.WinChartViewer chartViewer4;
		private ChartDirector.WinChartViewer chartViewer5;
		private ChartDirector.WinChartViewer chartViewer6;
		private ChartDirector.WinChartViewer chartViewer7;
		private ChartDirector.WinChartViewer chartViewer8;
		private System.Windows.Forms.ToolStrip toolStrip;
		private System.Windows.Forms.ToolStripButton backButton;
		private System.Windows.Forms.ToolStripButton forwardButton;
		private System.Windows.Forms.ToolStripButton previousButton;
		private System.Windows.Forms.ToolStripButton nextButton;
		private System.Windows.Forms.ToolStripButton viewCodeButton;
		private System.Windows.Forms.ToolStripButton viewDocButton;
		private System.Windows.Forms.StatusStrip statusBar;
		private System.Windows.Forms.ToolStripStatusLabel statusBarLabel;
	}
}