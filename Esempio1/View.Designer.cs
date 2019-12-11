namespace Esempio1
{
    partial class View
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(View));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton_setDB = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_viewCustomerSeries = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton_ARIMA = new System.Windows.Forms.ToolStripButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.readDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.label_Costumer = new System.Windows.Forms.Label();
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.entityCommand1 = new System.Data.Entity.Core.EntityClient.EntityCommand();
            this.toolStripButton_Optimize = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton_setDB,
            this.toolStripButton_viewCustomerSeries,
            this.toolStripButton_ARIMA,
            this.toolStripButton_Optimize});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(725, 35);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton_setDB
            // 
            this.toolStripButton_setDB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_setDB.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_setDB.Image")));
            this.toolStripButton_setDB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_setDB.Name = "toolStripButton_setDB";
            this.toolStripButton_setDB.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.toolStripButton_setDB.Size = new System.Drawing.Size(32, 32);
            this.toolStripButton_setDB.Text = "setDB";
            this.toolStripButton_setDB.ToolTipText = "Set Your DB File";
            this.toolStripButton_setDB.Click += new System.EventHandler(this.toolStripButton_SetDB_Click);
            // 
            // toolStripButton_viewCustomerSeries
            // 
            this.toolStripButton_viewCustomerSeries.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_viewCustomerSeries.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_viewCustomerSeries.Image")));
            this.toolStripButton_viewCustomerSeries.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_viewCustomerSeries.Name = "toolStripButton_viewCustomerSeries";
            this.toolStripButton_viewCustomerSeries.Size = new System.Drawing.Size(32, 32);
            this.toolStripButton_viewCustomerSeries.Text = "load100";
            this.toolStripButton_viewCustomerSeries.Click += new System.EventHandler(this.toolStripButton_CustomersOrdersChart_Click);
            // 
            // toolStripButton_ARIMA
            // 
            this.toolStripButton_ARIMA.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_ARIMA.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_ARIMA.Image")));
            this.toolStripButton_ARIMA.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_ARIMA.Name = "toolStripButton_ARIMA";
            this.toolStripButton_ARIMA.Size = new System.Drawing.Size(32, 32);
            this.toolStripButton_ARIMA.Text = "toolStripButton_ARIMA";
            this.toolStripButton_ARIMA.Click += new System.EventHandler(this.toolStripButton_ARIMA_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(3, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(725, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.readDatabaseToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(37, 22);
            this.toolStripMenuItem1.Text = "File";
            // 
            // readDatabaseToolStripMenuItem
            // 
            this.readDatabaseToolStripMenuItem.Name = "readDatabaseToolStripMenuItem";
            this.readDatabaseToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.readDatabaseToolStripMenuItem.Text = "Read database";
            this.readDatabaseToolStripMenuItem.Click += new System.EventHandler(this.readDatabaseToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 59);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtCustomer);
            this.splitContainer1.Panel1.Controls.Add(this.label_Costumer);
            this.splitContainer1.Panel1.Controls.Add(this.txtConsole);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.splitContainer1_Panel2_Paint);
            this.splitContainer1.Size = new System.Drawing.Size(725, 330);
            this.splitContainer1.SplitterDistance = 241;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 2;
            // 
            // txtCustomer
            // 
            this.txtCustomer.Location = new System.Drawing.Point(68, 16);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(138, 20);
            this.txtCustomer.TabIndex = 3;
            this.txtCustomer.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label_Costumer
            // 
            this.label_Costumer.AutoSize = true;
            this.label_Costumer.Location = new System.Drawing.Point(11, 18);
            this.label_Costumer.Name = "label_Costumer";
            this.label_Costumer.Size = new System.Drawing.Size(51, 13);
            this.label_Costumer.TabIndex = 1;
            this.label_Costumer.Text = "Costumer";
            // 
            // txtConsole
            // 
            this.txtConsole.Location = new System.Drawing.Point(2, 48);
            this.txtConsole.Margin = new System.Windows.Forms.Padding(2);
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtConsole.Size = new System.Drawing.Size(241, 280);
            this.txtConsole.TabIndex = 0;
            this.txtConsole.WordWrap = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(482, 330);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(61, 4);
            // 
            // entityCommand1
            // 
            this.entityCommand1.CommandTimeout = 0;
            this.entityCommand1.CommandTree = null;
            this.entityCommand1.Connection = null;
            this.entityCommand1.EnablePlanCaching = true;
            this.entityCommand1.Transaction = null;
            // 
            // toolStripButton_Optimize
            // 
            this.toolStripButton_Optimize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton_Optimize.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton_Optimize.Image")));
            this.toolStripButton_Optimize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton_Optimize.Name = "toolStripButton_Optimize";
            this.toolStripButton_Optimize.Size = new System.Drawing.Size(32, 32);
            this.toolStripButton_Optimize.Text = "Optimize";
            this.toolStripButton_Optimize.Click += new System.EventHandler(this.toolStripButton_Optimize_Click);
            // 
            // View
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 389);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "View";
            this.Text = "View";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton_setDB;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem readDatabaseToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtConsole;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        //private System.Windows.Forms.TextBox txtNewCustomer;
        private System.Windows.Forms.TextBox txtCustomer;
        //private System.Windows.Forms.Label label_newCustomer;
        private System.Windows.Forms.Label label_Costumer;
        //private System.Windows.Forms.ToolStripButton toolStripButton_createCustomer;
        //private System.Windows.Forms.ToolStripButton toolStripButton_deleteCustomer;
        //private System.Windows.Forms.ToolStripButton toolStripButton_updateCustomer;
        private System.Data.Entity.Core.EntityClient.EntityCommand entityCommand1;
        private System.Windows.Forms.ToolStripButton toolStripButton_viewCustomerSeries;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripButton toolStripButton_ARIMA;
        private System.Windows.Forms.ToolStripButton toolStripButton_Optimize;
    }
}

