namespace WinForms
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.AxTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.AyTextBox = new System.Windows.Forms.TextBox();
            this.BxTextBox = new System.Windows.Forms.TextBox();
            this.ByTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.DyTextBox = new System.Windows.Forms.TextBox();
            this.DxTextBox = new System.Windows.Forms.TextBox();
            this.CyTextBox = new System.Windows.Forms.TextBox();
            this.CxTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // AxTextBox
            // 
            this.AxTextBox.Location = new System.Drawing.Point(31, 35);
            this.AxTextBox.MaxLength = 3;
            this.AxTextBox.Name = "AxTextBox";
            this.AxTextBox.Size = new System.Drawing.Size(58, 20);
            this.AxTextBox.TabIndex = 0;
            this.AxTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AxTextBox_KeyPress);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ByTextBox);
            this.groupBox1.Controls.Add(this.BxTextBox);
            this.groupBox1.Controls.Add(this.AyTextBox);
            this.groupBox1.Controls.Add(this.AxTextBox);
            this.groupBox1.Location = new System.Drawing.Point(155, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 118);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Odcinek 1";
            // 
            // AyTextBox
            // 
            this.AyTextBox.Location = new System.Drawing.Point(139, 35);
            this.AyTextBox.MaxLength = 3;
            this.AyTextBox.Name = "AyTextBox";
            this.AyTextBox.Size = new System.Drawing.Size(58, 20);
            this.AyTextBox.TabIndex = 1;
            this.AyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AyTextBox_KeyPress);
            // 
            // BxTextBox
            // 
            this.BxTextBox.Location = new System.Drawing.Point(31, 61);
            this.BxTextBox.MaxLength = 3;
            this.BxTextBox.Name = "BxTextBox";
            this.BxTextBox.Size = new System.Drawing.Size(58, 20);
            this.BxTextBox.TabIndex = 2;
            this.BxTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.BxTextBox_KeyPress);
            // 
            // ByTextBox
            // 
            this.ByTextBox.Location = new System.Drawing.Point(139, 61);
            this.ByTextBox.MaxLength = 3;
            this.ByTextBox.Name = "ByTextBox";
            this.ByTextBox.Size = new System.Drawing.Size(58, 20);
            this.ByTextBox.TabIndex = 3;
            this.ByTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ByTextBox_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Ax";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(114, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Ay";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Bx";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(114, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "By";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.DyTextBox);
            this.groupBox2.Controls.Add(this.DxTextBox);
            this.groupBox2.Controls.Add(this.CyTextBox);
            this.groupBox2.Controls.Add(this.CxTextBox);
            this.groupBox2.Location = new System.Drawing.Point(411, 28);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(250, 118);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Odcinek 2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(114, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Dy";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Dx";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(114, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(19, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "Cy";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Cx";
            // 
            // DyTextBox
            // 
            this.DyTextBox.Location = new System.Drawing.Point(139, 61);
            this.DyTextBox.MaxLength = 3;
            this.DyTextBox.Name = "DyTextBox";
            this.DyTextBox.Size = new System.Drawing.Size(58, 20);
            this.DyTextBox.TabIndex = 3;
            this.DyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DyTextBox_KeyPress);
            // 
            // DxTextBox
            // 
            this.DxTextBox.Location = new System.Drawing.Point(31, 61);
            this.DxTextBox.MaxLength = 3;
            this.DxTextBox.Name = "DxTextBox";
            this.DxTextBox.Size = new System.Drawing.Size(58, 20);
            this.DxTextBox.TabIndex = 2;
            this.DxTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DxTextBox_KeyPress);
            // 
            // CyTextBox
            // 
            this.CyTextBox.Location = new System.Drawing.Point(139, 35);
            this.CyTextBox.MaxLength = 3;
            this.CyTextBox.Name = "CyTextBox";
            this.CyTextBox.Size = new System.Drawing.Size(58, 20);
            this.CyTextBox.TabIndex = 1;
            this.CyTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CyTextBox_KeyPress);
            // 
            // CxTextBox
            // 
            this.CxTextBox.Location = new System.Drawing.Point(31, 35);
            this.CxTextBox.MaxLength = 3;
            this.CxTextBox.Name = "CxTextBox";
            this.CxTextBox.Size = new System.Drawing.Size(58, 20);
            this.CxTextBox.TabIndex = 0;
            this.CxTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CxTextBox_KeyPress);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.button1.Location = new System.Drawing.Point(369, 152);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(82, 27);
            this.button1.TabIndex = 9;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 189);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(815, 660);
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 861);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox AxTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox ByTextBox;
        private System.Windows.Forms.TextBox BxTextBox;
        private System.Windows.Forms.TextBox AyTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox DyTextBox;
        private System.Windows.Forms.TextBox DxTextBox;
        private System.Windows.Forms.TextBox CyTextBox;
        private System.Windows.Forms.TextBox CxTextBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

