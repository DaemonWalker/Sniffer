namespace Sniffer.UI
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
            if (disposing && (components != null))
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
            this.drpDevices = new System.Windows.Forms.ComboBox();
            this.dgvIP = new System.Windows.Forms.DataGridView();
            this.FromIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ToIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkToBase64 = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.dgvPort = new System.Windows.Forms.DataGridView();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProcName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtContent = new System.Windows.Forms.RichTextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.drpEncoding = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIP)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPort)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // drpDevices
            // 
            this.drpDevices.Font = new System.Drawing.Font("宋体", 12F);
            this.drpDevices.FormattingEnabled = true;
            this.drpDevices.Location = new System.Drawing.Point(91, 12);
            this.drpDevices.Name = "drpDevices";
            this.drpDevices.Size = new System.Drawing.Size(561, 24);
            this.drpDevices.TabIndex = 0;
            this.drpDevices.SelectedIndexChanged += new System.EventHandler(this.drpDevices_SelectedIndexChanged);
            // 
            // dgvIP
            // 
            this.dgvIP.AllowUserToAddRows = false;
            this.dgvIP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIP.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FromIP,
            this.ToIP});
            this.dgvIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvIP.Location = new System.Drawing.Point(0, 0);
            this.dgvIP.Name = "dgvIP";
            this.dgvIP.ReadOnly = true;
            this.dgvIP.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvIP.Size = new System.Drawing.Size(217, 398);
            this.dgvIP.TabIndex = 1;
            this.dgvIP.SelectionChanged += new System.EventHandler(this.dgvIP_SelectionChanged);
            // 
            // FromIP
            // 
            this.FromIP.DataPropertyName = "FromIP";
            this.FromIP.Frozen = true;
            this.FromIP.HeaderText = "FromIP";
            this.FromIP.Name = "FromIP";
            this.FromIP.ReadOnly = true;
            // 
            // ToIP
            // 
            this.ToIP.DataPropertyName = "ToIP";
            this.ToIP.HeaderText = "ToIP";
            this.ToIP.Name = "ToIP";
            this.ToIP.ReadOnly = true;
            // 
            // chkToBase64
            // 
            this.chkToBase64.AutoSize = true;
            this.chkToBase64.Font = new System.Drawing.Font("宋体", 12F);
            this.chkToBase64.Location = new System.Drawing.Point(3, 3);
            this.chkToBase64.Name = "chkToBase64";
            this.chkToBase64.Size = new System.Drawing.Size(123, 20);
            this.chkToBase64.TabIndex = 2;
            this.chkToBase64.Text = "转化为Base64";
            this.chkToBase64.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.drpDevices);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 52);
            this.panel1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F);
            this.label2.Location = new System.Drawing.Point(662, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "label2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F);
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "设备选择";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 52);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dgvIP);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(800, 398);
            this.splitContainer1.SplitterDistance = 217;
            this.splitContainer1.TabIndex = 4;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.dgvPort);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.txtContent);
            this.splitContainer2.Panel2.Controls.Add(this.panel3);
            this.splitContainer2.Panel2.Controls.Add(this.panel2);
            this.splitContainer2.Size = new System.Drawing.Size(579, 398);
            this.splitContainer2.SplitterDistance = 275;
            this.splitContainer2.TabIndex = 3;
            // 
            // dgvPort
            // 
            this.dgvPort.AllowUserToAddRows = false;
            this.dgvPort.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPort.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column3,
            this.Column4,
            this.ProcName});
            this.dgvPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPort.Location = new System.Drawing.Point(0, 0);
            this.dgvPort.Name = "dgvPort";
            this.dgvPort.ReadOnly = true;
            this.dgvPort.RowTemplate.Height = 23;
            this.dgvPort.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPort.Size = new System.Drawing.Size(275, 398);
            this.dgvPort.TabIndex = 0;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "FromPort";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "ToPort";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            // 
            // ProcName
            // 
            this.ProcName.DataPropertyName = "ProcName";
            this.ProcName.HeaderText = "ProcName";
            this.ProcName.Name = "ProcName";
            this.ProcName.ReadOnly = true;
            // 
            // txtContent
            // 
            this.txtContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtContent.Location = new System.Drawing.Point(0, 28);
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(300, 340);
            this.txtContent.TabIndex = 6;
            this.txtContent.Text = "";
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 368);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(300, 30);
            this.panel3.TabIndex = 5;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkToBase64);
            this.panel2.Controls.Add(this.drpEncoding);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(300, 28);
            this.panel2.TabIndex = 4;
            // 
            // drpEncoding
            // 
            this.drpEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drpEncoding.FormattingEnabled = true;
            this.drpEncoding.Items.AddRange(new object[] {
            "UTF-8",
            "GB2312",
            "GBK",
            "Unicode"});
            this.drpEncoding.Location = new System.Drawing.Point(165, 3);
            this.drpEncoding.Name = "drpEncoding";
            this.drpEncoding.Size = new System.Drawing.Size(129, 20);
            this.drpEncoding.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvIP)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPort)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox drpDevices;
        private System.Windows.Forms.DataGridView dgvIP;
        private System.Windows.Forms.CheckBox chkToBase64;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ComboBox drpEncoding;
        private System.Windows.Forms.DataGridView dgvPort;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn FromIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn ToIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProcName;
        private System.Windows.Forms.RichTextBox txtContent;
        private System.Windows.Forms.Panel panel3;
    }
}