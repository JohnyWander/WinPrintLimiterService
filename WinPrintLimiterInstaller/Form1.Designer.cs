namespace WinPrintLimiterInstaller
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            InstallDirectoryBox = new TextBox();
            label1 = new Label();
            BrowseButton = new Button();
            InstallButton = new Button();
            groupBox1 = new GroupBox();
            radio_xps = new RadioButton();
            radio_inflight = new RadioButton();
            pName = new TextBox();
            label2 = new Label();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // InstallDirectoryBox
            // 
            InstallDirectoryBox.Location = new Point(58, 29);
            InstallDirectoryBox.Name = "InstallDirectoryBox";
            InstallDirectoryBox.Size = new Size(323, 23);
            InstallDirectoryBox.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(58, 11);
            label1.Name = "label1";
            label1.Size = new Size(88, 15);
            label1.TabIndex = 1;
            label1.Text = "install directory";
            // 
            // BrowseButton
            // 
            BrowseButton.Location = new Point(387, 29);
            BrowseButton.Name = "BrowseButton";
            BrowseButton.Size = new Size(75, 23);
            BrowseButton.TabIndex = 2;
            BrowseButton.Text = "Browse";
            BrowseButton.UseVisualStyleBackColor = true;
            BrowseButton.Click += BrowseButton_click;
            // 
            // InstallButton
            // 
            InstallButton.Location = new Point(620, 381);
            InstallButton.Name = "InstallButton";
            InstallButton.Size = new Size(152, 42);
            InstallButton.TabIndex = 3;
            InstallButton.Text = "Install";
            InstallButton.UseVisualStyleBackColor = true;
            InstallButton.Click += InstallButton_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(radio_xps);
            groupBox1.Controls.Add(radio_inflight);
            groupBox1.Location = new Point(58, 91);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(629, 100);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "groupBox1";
            // 
            // radio_xps
            // 
            radio_xps.AutoSize = true;
            radio_xps.Location = new Point(6, 47);
            radio_xps.Name = "radio_xps";
            radio_xps.Size = new Size(143, 19);
            radio_xps.TabIndex = 1;
            radio_xps.TabStop = true;
            radio_xps.Text = "XPS printer redirection";
            radio_xps.UseVisualStyleBackColor = true;
            radio_xps.CheckedChanged += radio_xps_CheckedChanged;
            // 
            // radio_inflight
            // 
            radio_inflight.AutoSize = true;
            radio_inflight.Location = new Point(6, 22);
            radio_inflight.Name = "radio_inflight";
            radio_inflight.Size = new Size(370, 19);
            radio_inflight.TabIndex = 0;
            radio_inflight.TabStop = true;
            radio_inflight.Text = "In Flight pages calculation(may not work for some printer drivers)";
            radio_inflight.UseVisualStyleBackColor = true;
            radio_inflight.CheckedChanged += radio_inflight_CheckedChanged;
            // 
            // pName
            // 
            pName.Location = new Point(59, 218);
            pName.Name = "pName";
            pName.Size = new Size(260, 23);
            pName.TabIndex = 5;
            pName.Text = "WPLprinter";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(325, 221);
            label2.Name = "label2";
            label2.Size = new Size(96, 15);
            label2.TabIndex = 6;
            label2.Text = "xps printer name";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label2);
            Controls.Add(pName);
            Controls.Add(groupBox1);
            Controls.Add(InstallButton);
            Controls.Add(BrowseButton);
            Controls.Add(label1);
            Controls.Add(InstallDirectoryBox);
            Name = "Form1";
            Text = "Form1";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox InstallDirectoryBox;
        private Label label1;
        private Button BrowseButton;
        private Button InstallButton;
        private GroupBox groupBox1;
        private RadioButton radio_xps;
        private RadioButton radio_inflight;
        private TextBox pName;
        private Label label2;
    }
}