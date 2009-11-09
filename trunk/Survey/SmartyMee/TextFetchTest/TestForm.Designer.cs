namespace TextFetchTest
{
    partial class TestForm
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
            this.stringBox = new System.Windows.Forms.TextBox();
            this.positionBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // stringBox
            // 
            this.stringBox.Location = new System.Drawing.Point(32, 45);
            this.stringBox.Name = "stringBox";
            this.stringBox.Size = new System.Drawing.Size(100, 21);
            this.stringBox.TabIndex = 0;
            // 
            // positionBox
            // 
            this.positionBox.Location = new System.Drawing.Point(32, 109);
            this.positionBox.Name = "positionBox";
            this.positionBox.Size = new System.Drawing.Size(100, 21);
            this.positionBox.TabIndex = 1;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 369);
            this.Controls.Add(this.positionBox);
            this.Controls.Add(this.stringBox);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox stringBox;
        private System.Windows.Forms.TextBox positionBox;
    }
}