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
            this.components = new System.ComponentModel.Container();
            this.stringBox = new System.Windows.Forms.TextBox();
            this.positionBox = new System.Windows.Forms.TextBox();
            this.anotherBox = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.textBox1 = new System.Windows.Forms.TextBox();
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
            // anotherBox
            // 
            this.anotherBox.Location = new System.Drawing.Point(191, 44);
            this.anotherBox.Name = "anotherBox";
            this.anotherBox.Size = new System.Drawing.Size(100, 21);
            this.anotherBox.TabIndex = 2;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 5000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(46, 168);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(235, 21);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "we are here to test the fetcher!";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(418, 369);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.anotherBox);
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
        private System.Windows.Forms.TextBox anotherBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox textBox1;
    }
}