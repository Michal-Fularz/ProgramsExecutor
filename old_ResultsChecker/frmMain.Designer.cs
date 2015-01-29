namespace SiSW_SprProjektu
{
    partial class frmMain
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
            this.lblChoosenPath = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.Main_MenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemChooseDir = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tbPath = new System.Windows.Forms.TextBox();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.rtbMain = new System.Windows.Forms.RichTextBox();
            this.btnChooseDirectory = new System.Windows.Forms.Button();
            this.btnCalculateResults = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblChoosenPath
            // 
            this.lblChoosenPath.AutoSize = true;
            this.lblChoosenPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblChoosenPath.Location = new System.Drawing.Point(12, 35);
            this.lblChoosenPath.Name = "lblChoosenPath";
            this.lblChoosenPath.Size = new System.Drawing.Size(105, 15);
            this.lblChoosenPath.TabIndex = 1;
            this.lblChoosenPath.Text = "Wybrana ścieżka: ";
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Main_MenuItem,
            this.menuItemAbout});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(671, 24);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip";
            // 
            // Main_MenuItem
            // 
            this.Main_MenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemChooseDir});
            this.Main_MenuItem.Name = "Main_MenuItem";
            this.Main_MenuItem.Size = new System.Drawing.Size(50, 20);
            this.Main_MenuItem.Text = "Menu";
            // 
            // menuItemChooseDir
            // 
            this.menuItemChooseDir.Name = "menuItemChooseDir";
            this.menuItemChooseDir.Size = new System.Drawing.Size(158, 22);
            this.menuItemChooseDir.Text = "Wybór katalogu";
            this.menuItemChooseDir.Click += new System.EventHandler(this.menuItemChooseDir_Click);
            // 
            // menuItemAbout
            // 
            this.menuItemAbout.Name = "menuItemAbout";
            this.menuItemAbout.Size = new System.Drawing.Size(86, 20);
            this.menuItemAbout.Text = "O programie";
            this.menuItemAbout.Click += new System.EventHandler(this.miAbout_Click);
            // 
            // tbPath
            // 
            this.tbPath.Location = new System.Drawing.Point(113, 32);
            this.tbPath.Name = "tbPath";
            this.tbPath.Size = new System.Drawing.Size(358, 20);
            this.tbPath.TabIndex = 3;
            // 
            // btnEdit
            // 
            this.btnEdit.AccessibleDescription = "cosik";
            this.btnEdit.Location = new System.Drawing.Point(477, 59);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(78, 24);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "Edytuj";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(588, 29);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(78, 24);
            this.btnOpen.TabIndex = 5;
            this.btnOpen.Text = "Otwórz";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // rtbMain
            // 
            this.rtbMain.DetectUrls = false;
            this.rtbMain.Location = new System.Drawing.Point(12, 88);
            this.rtbMain.Name = "rtbMain";
            this.rtbMain.Size = new System.Drawing.Size(570, 398);
            this.rtbMain.TabIndex = 6;
            this.rtbMain.Text = "";
            // 
            // btnChooseDirectory
            // 
            this.btnChooseDirectory.Location = new System.Drawing.Point(477, 29);
            this.btnChooseDirectory.Name = "btnChooseDirectory";
            this.btnChooseDirectory.Size = new System.Drawing.Size(105, 24);
            this.btnChooseDirectory.TabIndex = 9;
            this.btnChooseDirectory.Text = "Wybierz folder";
            this.btnChooseDirectory.UseVisualStyleBackColor = true;
            this.btnChooseDirectory.Click += new System.EventHandler(this.btnChooseDirectory_Click);
            // 
            // btnCalculateResults
            // 
            this.btnCalculateResults.Location = new System.Drawing.Point(561, 58);
            this.btnCalculateResults.Name = "btnCalculateResults";
            this.btnCalculateResults.Size = new System.Drawing.Size(105, 24);
            this.btnCalculateResults.TabIndex = 10;
            this.btnCalculateResults.Text = "Oblicz wyniki";
            this.btnCalculateResults.UseVisualStyleBackColor = true;
            this.btnCalculateResults.Click += new System.EventHandler(this.btnCalculateResults_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(159, 59);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 498);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCalculateResults);
            this.Controls.Add(this.btnChooseDirectory);
            this.Controls.Add(this.rtbMain);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.lblChoosenPath);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "frmMain";
            this.Text = "SiSW Project Checker";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblChoosenPath;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem Main_MenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemChooseDir;
        private System.Windows.Forms.ToolStripMenuItem menuItemAbout;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.RichTextBox rtbMain;
        private System.Windows.Forms.Button btnChooseDirectory;
        private System.Windows.Forms.Button btnCalculateResults;
        private System.Windows.Forms.Button button1;
    }
}

