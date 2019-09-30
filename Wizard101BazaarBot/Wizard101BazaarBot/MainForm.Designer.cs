namespace Wizard101BazaarBot
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
            this.availableBox = new System.Windows.Forms.ListBox();
            this.selectedBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // availableBox
            // 
            this.availableBox.FormattingEnabled = true;
            this.availableBox.Items.AddRange(new object[] {
            "dsad",
            "dasda",
            "dasdad"});
            this.availableBox.Location = new System.Drawing.Point(364, 62);
            this.availableBox.Name = "availableBox";
            this.availableBox.Size = new System.Drawing.Size(120, 95);
            this.availableBox.TabIndex = 0;
            this.availableBox.SelectedIndexChanged += new System.EventHandler(this.AvailableBox_SelectedIndexChanged);
            // 
            // selectedBox
            // 
            this.selectedBox.FormattingEnabled = true;
            this.selectedBox.Items.AddRange(new object[] {
            "dsad",
            "dasda",
            "dasdad"});
            this.selectedBox.Location = new System.Drawing.Point(579, 62);
            this.selectedBox.Name = "selectedBox";
            this.selectedBox.Size = new System.Drawing.Size(120, 95);
            this.selectedBox.TabIndex = 1;
            this.selectedBox.SelectedIndexChanged += new System.EventHandler(this.SelectedBox_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 446);
            this.Controls.Add(this.selectedBox);
            this.Controls.Add(this.availableBox);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox availableBox;
        private System.Windows.Forms.ListBox selectedBox;
    }
}

