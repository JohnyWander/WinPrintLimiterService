namespace WinPrintLimiter
{
    partial class XPSreprintForm
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
            printersCombo = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            used = new Label();
            max = new Label();
            label3 = new Label();
            PrintStart = new Button();
            cancel = new Button();
            SuspendLayout();
            // 
            // printersCombo
            // 
            printersCombo.FormattingEnabled = true;
            printersCombo.Location = new Point(12, 31);
            printersCombo.Name = "printersCombo";
            printersCombo.Size = new Size(252, 23);
            printersCombo.TabIndex = 0;
            printersCombo.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 13);
            label1.Name = "label1";
            label1.Size = new Size(187, 15);
            label1.TabIndex = 1;
            label1.Text = "Wybierz drukarkę / Choose printer";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 77);
            label2.Name = "label2";
            label2.Size = new Size(318, 15);
            label2.TabIndex = 2;
            label2.Text = "Użyty dzienny limit stron / Used daily limit of pages printed";
            // 
            // used
            // 
            used.AutoSize = true;
            used.Location = new Point(16, 96);
            used.Name = "used";
            used.Size = new Size(38, 15);
            used.TabIndex = 3;
            used.Text = "label3";
            // 
            // max
            // 
            max.AutoSize = true;
            max.Location = new Point(60, 96);
            max.Name = "max";
            max.Size = new Size(38, 15);
            max.TabIndex = 4;
            max.Text = "label3";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(51, 96);
            label3.Name = "label3";
            label3.Size = new Size(12, 15);
            label3.TabIndex = 5;
            label3.Text = "/";
            // 
            // PrintStart
            // 
            PrintStart.Location = new Point(12, 122);
            PrintStart.Name = "PrintStart";
            PrintStart.Size = new Size(161, 53);
            PrintStart.TabIndex = 6;
            PrintStart.Text = "Drukuj / Print";
            PrintStart.UseVisualStyleBackColor = true;
            PrintStart.Click += PrintStart_Click;
            // 
            // cancel
            // 
            cancel.Location = new Point(198, 122);
            cancel.Name = "cancel";
            cancel.Size = new Size(132, 53);
            cancel.TabIndex = 7;
            cancel.Text = "Anuluj / Cancel";
            cancel.UseVisualStyleBackColor = true;
            cancel.Click += cancel_Click;
            // 
            // XPSreprintForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(362, 187);
            ControlBox = false;
            Controls.Add(cancel);
            Controls.Add(PrintStart);
            Controls.Add(label3);
            Controls.Add(max);
            Controls.Add(used);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(printersCombo);
            Name = "XPSreprintForm";
            Text = "Print menu";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        public ComboBox printersCombo;
        private Label label2;
        private Label label3;
        public Label used;
        public Label max;
        private Button PrintStart;
        private Button cancel;
    }
}