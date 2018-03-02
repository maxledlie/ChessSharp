namespace Chess_Sharp
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.whiteComboBox = new System.Windows.Forms.ComboBox();
            this.whiteLabel = new System.Windows.Forms.Label();
            this.blackLabel = new System.Windows.Forms.Label();
            this.blackComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Symbol", 30F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 67);
            this.label1.TabIndex = 0;
            this.label1.Text = "Chess#";
            // 
            // startButton
            // 
            this.startButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.startButton.Location = new System.Drawing.Point(113, 174);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 31);
            this.startButton.TabIndex = 1;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // whiteComboBox
            // 
            this.whiteComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.whiteComboBox.Items.AddRange(new object[] {
            "Human",
            "Chimp",
            "Computer"});
            this.whiteComboBox.Location = new System.Drawing.Point(67, 85);
            this.whiteComboBox.Name = "whiteComboBox";
            this.whiteComboBox.Size = new System.Drawing.Size(121, 24);
            this.whiteComboBox.TabIndex = 2;
            // 
            // whiteLabel
            // 
            this.whiteLabel.AutoSize = true;
            this.whiteLabel.Location = new System.Drawing.Point(17, 85);
            this.whiteLabel.Name = "whiteLabel";
            this.whiteLabel.Size = new System.Drawing.Size(44, 17);
            this.whiteLabel.TabIndex = 3;
            this.whiteLabel.Text = "White";
            // 
            // blackLabel
            // 
            this.blackLabel.AutoSize = true;
            this.blackLabel.Location = new System.Drawing.Point(19, 132);
            this.blackLabel.Name = "blackLabel";
            this.blackLabel.Size = new System.Drawing.Size(42, 17);
            this.blackLabel.TabIndex = 4;
            this.blackLabel.Text = "Black";
            // 
            // blackComboBox
            // 
            this.blackComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.blackComboBox.FormattingEnabled = true;
            this.blackComboBox.Items.AddRange(new object[] {
            "Human",
            "Chimp",
            "Computer"});
            this.blackComboBox.Location = new System.Drawing.Point(67, 129);
            this.blackComboBox.Name = "blackComboBox";
            this.blackComboBox.Size = new System.Drawing.Size(121, 24);
            this.blackComboBox.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(230, 231);
            this.Controls.Add(this.blackComboBox);
            this.Controls.Add(this.blackLabel);
            this.Controls.Add(this.whiteLabel);
            this.Controls.Add(this.whiteComboBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.ComboBox whiteComboBox;
        private System.Windows.Forms.Label whiteLabel;
        private System.Windows.Forms.Label blackLabel;
        private System.Windows.Forms.ComboBox blackComboBox;
    }
}