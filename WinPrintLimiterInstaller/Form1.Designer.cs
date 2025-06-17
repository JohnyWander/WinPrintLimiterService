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
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(InstallButton);
            Controls.Add(BrowseButton);
            Controls.Add(label1);
            Controls.Add(InstallDirectoryBox);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox InstallDirectoryBox;
        private Label label1;
        private Button BrowseButton;
        private Button InstallButton;
    }
}