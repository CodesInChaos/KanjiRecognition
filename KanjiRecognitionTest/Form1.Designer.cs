namespace KanjiRecognitionTest
{
	partial class Form1
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.match = new System.Windows.Forms.Button();
			this.clear = new System.Windows.Forms.Button();
			this.kanjiImage = new System.Windows.Forms.Panel();
			this.matchQuality = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.matchQuality);
			this.panel1.Controls.Add(this.match);
			this.panel1.Controls.Add(this.clear);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(0, 214);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(284, 48);
			this.panel1.TabIndex = 1;
			// 
			// match
			// 
			this.match.Location = new System.Drawing.Point(93, 13);
			this.match.Name = "match";
			this.match.Size = new System.Drawing.Size(75, 23);
			this.match.TabIndex = 1;
			this.match.Text = "Match";
			this.match.UseVisualStyleBackColor = true;
			this.match.Click += new System.EventHandler(this.match_Click);
			// 
			// clear
			// 
			this.clear.Location = new System.Drawing.Point(12, 13);
			this.clear.Name = "clear";
			this.clear.Size = new System.Drawing.Size(75, 23);
			this.clear.TabIndex = 0;
			this.clear.Text = "Clear";
			this.clear.UseVisualStyleBackColor = true;
			this.clear.Click += new System.EventHandler(this.clear_Click);
			// 
			// kanjiImage
			// 
			this.kanjiImage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.kanjiImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.kanjiImage.Location = new System.Drawing.Point(0, 0);
			this.kanjiImage.Name = "kanjiImage";
			this.kanjiImage.Size = new System.Drawing.Size(284, 214);
			this.kanjiImage.TabIndex = 2;
			this.kanjiImage.Paint += new System.Windows.Forms.PaintEventHandler(this.kanjiImage_Paint);
			this.kanjiImage.MouseMove += new System.Windows.Forms.MouseEventHandler(this.kanjiImage_MouseMove);
			// 
			// matchQuality
			// 
			this.matchQuality.AutoSize = true;
			this.matchQuality.Location = new System.Drawing.Point(183, 18);
			this.matchQuality.Name = "matchQuality";
			this.matchQuality.Size = new System.Drawing.Size(35, 13);
			this.matchQuality.TabIndex = 2;
			this.matchQuality.Text = "label1";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Controls.Add(this.kanjiImage);
			this.Controls.Add(this.panel1);
			this.Name = "Form1";
			this.Text = "Form1";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button clear;
		private System.Windows.Forms.Panel kanjiImage;
		private System.Windows.Forms.Button match;
		private System.Windows.Forms.Label matchQuality;
	}
}

