namespace JenkinsHelp
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.textBox = new System.Windows.Forms.TextBox();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label = new System.Windows.Forms.Label();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.audioOutputSelector = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxBranch = new System.Windows.Forms.ComboBox();
            this.timerLabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.buttonSend = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.textBoxMsg = new System.Windows.Forms.TextBox();
            this.notifyIconMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(12, 50);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(486, 234);
            this.textBox.TabIndex = 0;
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.notifyIconMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Jenkins";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // notifyIconMenu
            // 
            this.notifyIconMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripMenuItem,
            this.hideToolStripMenuItem,
            this.closeToolStripMenuItem});
            this.notifyIconMenu.Name = "notifyIconMenu";
            this.notifyIconMenu.Size = new System.Drawing.Size(109, 70);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.showToolStripMenuItem.Text = "Show";
            this.showToolStripMenuItem.Click += new System.EventHandler(this.showToolStripMenuItem_Click);
            // 
            // hideToolStripMenuItem
            // 
            this.hideToolStripMenuItem.Name = "hideToolStripMenuItem";
            this.hideToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.hideToolStripMenuItem.Text = "Hide";
            this.hideToolStripMenuItem.Click += new System.EventHandler(this.hideToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Location = new System.Drawing.Point(15, 5);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(41, 12);
            this.label.TabIndex = 1;
            this.label.Text = "音量：";
            // 
            // trackBar
            // 
            this.trackBar.LargeChange = 10;
            this.trackBar.Location = new System.Drawing.Point(62, -1);
            this.trackBar.Maximum = 100;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(141, 45);
            this.trackBar.TabIndex = 3;
            this.trackBar.TickFrequency = 0;
            this.trackBar.Value = 100;
            this.trackBar.ValueChanged += new System.EventHandler(this.trackBar_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "输出：";
            // 
            // audioOutputSelector
            // 
            this.audioOutputSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.audioOutputSelector.FormattingEnabled = true;
            this.audioOutputSelector.Location = new System.Drawing.Point(62, 23);
            this.audioOutputSelector.Name = "audioOutputSelector";
            this.audioOutputSelector.Size = new System.Drawing.Size(223, 20);
            this.audioOutputSelector.TabIndex = 4;
            this.audioOutputSelector.SelectedIndexChanged += new System.EventHandler(this.audioOutputSelector_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(305, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Jenkins：";
            // 
            // comboBoxBranch
            // 
            this.comboBoxBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxBranch.FormattingEnabled = true;
            this.comboBoxBranch.Location = new System.Drawing.Point(360, 23);
            this.comboBoxBranch.Name = "comboBoxBranch";
            this.comboBoxBranch.Size = new System.Drawing.Size(118, 20);
            this.comboBoxBranch.TabIndex = 4;
            this.comboBoxBranch.SelectedIndexChanged += new System.EventHandler(this.comboBoxBranch_SelectedIndexChanged);
            // 
            // timerLabel
            // 
            this.timerLabel.AutoSize = true;
            this.timerLabel.Location = new System.Drawing.Point(255, 5);
            this.timerLabel.Name = "timerLabel";
            this.timerLabel.Size = new System.Drawing.Size(17, 12);
            this.timerLabel.TabIndex = 5;
            this.timerLabel.Text = "--";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.OnTick);
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(423, 308);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 23);
            this.buttonSend.TabIndex = 6;
            this.buttonSend.Text = "发送";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // textBoxMsg
            // 
            this.textBoxMsg.Location = new System.Drawing.Point(12, 294);
            this.textBoxMsg.Multiline = true;
            this.textBoxMsg.Name = "textBoxMsg";
            this.textBoxMsg.Size = new System.Drawing.Size(405, 49);
            this.textBoxMsg.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 355);
            this.Controls.Add(this.textBoxMsg);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.timerLabel);
            this.Controls.Add(this.comboBoxBranch);
            this.Controls.Add(this.audioOutputSelector);
            this.Controls.Add(this.textBox);
            this.Controls.Add(this.trackBar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.Text = "播报员";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.notifyIconMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip notifyIconMenu;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.Label label;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox audioOutputSelector;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxBranch;
        private System.Windows.Forms.Label timerLabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox textBoxMsg;
    }
}

