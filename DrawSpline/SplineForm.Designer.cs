namespace DrawSpline
{
  partial class SplineForm
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
      this.textBox1 = new System.Windows.Forms.TextBox();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.textBox3 = new System.Windows.Forms.TextBox();
      this.textBox4 = new System.Windows.Forms.TextBox();
      this.textBox5 = new System.Windows.Forms.TextBox();
      this.textBox6 = new System.Windows.Forms.TextBox();
      this.textBox7 = new System.Windows.Forms.TextBox();
      this.textBox8 = new System.Windows.Forms.TextBox();
      this.menuStrip1 = new System.Windows.Forms.MenuStrip();
      this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
      this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
      this.XOffsetBox = new System.Windows.Forms.TextBox();
      this.XScaleBox = new System.Windows.Forms.TextBox();
      this.YScaleBox = new System.Windows.Forms.TextBox();
      this.YOffsetBox = new System.Windows.Forms.TextBox();
      this.menuStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // textBox1
      // 
      this.textBox1.Location = new System.Drawing.Point(70, 31);
      this.textBox1.Name = "textBox1";
      this.textBox1.Size = new System.Drawing.Size(100, 19);
      this.textBox1.TabIndex = 0;
      // 
      // textBox2
      // 
      this.textBox2.Location = new System.Drawing.Point(176, 31);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(100, 19);
      this.textBox2.TabIndex = 1;
      // 
      // textBox3
      // 
      this.textBox3.Location = new System.Drawing.Point(70, 56);
      this.textBox3.Name = "textBox3";
      this.textBox3.Size = new System.Drawing.Size(100, 19);
      this.textBox3.TabIndex = 3;
      // 
      // textBox4
      // 
      this.textBox4.Location = new System.Drawing.Point(176, 56);
      this.textBox4.Name = "textBox4";
      this.textBox4.Size = new System.Drawing.Size(100, 19);
      this.textBox4.TabIndex = 2;
      // 
      // textBox5
      // 
      this.textBox5.Location = new System.Drawing.Point(70, 81);
      this.textBox5.Name = "textBox5";
      this.textBox5.Size = new System.Drawing.Size(100, 19);
      this.textBox5.TabIndex = 7;
      // 
      // textBox6
      // 
      this.textBox6.Location = new System.Drawing.Point(176, 81);
      this.textBox6.Name = "textBox6";
      this.textBox6.Size = new System.Drawing.Size(100, 19);
      this.textBox6.TabIndex = 6;
      // 
      // textBox7
      // 
      this.textBox7.Location = new System.Drawing.Point(70, 106);
      this.textBox7.Name = "textBox7";
      this.textBox7.Size = new System.Drawing.Size(100, 19);
      this.textBox7.TabIndex = 5;
      // 
      // textBox8
      // 
      this.textBox8.Location = new System.Drawing.Point(176, 106);
      this.textBox8.Name = "textBox8";
      this.textBox8.Size = new System.Drawing.Size(100, 19);
      this.textBox8.TabIndex = 4;
      // 
      // menuStrip1
      // 
      this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.updateToolStripMenuItem});
      this.menuStrip1.Location = new System.Drawing.Point(0, 0);
      this.menuStrip1.Name = "menuStrip1";
      this.menuStrip1.Size = new System.Drawing.Size(833, 24);
      this.menuStrip1.TabIndex = 8;
      this.menuStrip1.Text = "menuStrip1";
      // 
      // saveToolStripMenuItem
      // 
      this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
      this.saveToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
      this.saveToolStripMenuItem.Text = "Save";
      this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
      // 
      // loadToolStripMenuItem
      // 
      this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
      this.loadToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
      this.loadToolStripMenuItem.Text = "Load";
      this.loadToolStripMenuItem.Click += new System.EventHandler(this.loadToolStripMenuItem_Click);
      // 
      // updateToolStripMenuItem
      // 
      this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
      this.updateToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
      this.updateToolStripMenuItem.Text = "Update";
      this.updateToolStripMenuItem.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
      // 
      // saveFileDialog1
      // 
      this.saveFileDialog1.DefaultExt = "csv";
      this.saveFileDialog1.Filter = "csv|*.csv";
      // 
      // openFileDialog1
      // 
      this.openFileDialog1.DefaultExt = "csv";
      this.openFileDialog1.FileName = "openFileDialog1";
      this.openFileDialog1.Filter = "csv|*.csv";
      // 
      // XOffsetBox
      // 
      this.XOffsetBox.Location = new System.Drawing.Point(70, 131);
      this.XOffsetBox.Name = "XOffsetBox";
      this.XOffsetBox.Size = new System.Drawing.Size(100, 19);
      this.XOffsetBox.TabIndex = 9;
      // 
      // XScaleBox
      // 
      this.XScaleBox.Location = new System.Drawing.Point(176, 131);
      this.XScaleBox.Name = "XScaleBox";
      this.XScaleBox.Size = new System.Drawing.Size(100, 19);
      this.XScaleBox.TabIndex = 10;
      // 
      // YScaleBox
      // 
      this.YScaleBox.Location = new System.Drawing.Point(176, 156);
      this.YScaleBox.Name = "YScaleBox";
      this.YScaleBox.Size = new System.Drawing.Size(100, 19);
      this.YScaleBox.TabIndex = 12;
      // 
      // YOffsetBox
      // 
      this.YOffsetBox.Location = new System.Drawing.Point(70, 156);
      this.YOffsetBox.Name = "YOffsetBox";
      this.YOffsetBox.Size = new System.Drawing.Size(100, 19);
      this.YOffsetBox.TabIndex = 11;
      // 
      // SplineForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(833, 559);
      this.Controls.Add(this.YScaleBox);
      this.Controls.Add(this.YOffsetBox);
      this.Controls.Add(this.XScaleBox);
      this.Controls.Add(this.XOffsetBox);
      this.Controls.Add(this.textBox5);
      this.Controls.Add(this.textBox6);
      this.Controls.Add(this.textBox7);
      this.Controls.Add(this.textBox8);
      this.Controls.Add(this.textBox3);
      this.Controls.Add(this.textBox4);
      this.Controls.Add(this.textBox2);
      this.Controls.Add(this.textBox1);
      this.Controls.Add(this.menuStrip1);
      this.MainMenuStrip = this.menuStrip1;
      this.Name = "SplineForm";
      this.Text = "SplineForm";
      this.Paint += new System.Windows.Forms.PaintEventHandler(this.SplineForm_Paint);
      this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SplineForm_MouseDown);
      this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
      this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
      this.Resize += new System.EventHandler(this.SplineForm_Resize);
      this.menuStrip1.ResumeLayout(false);
      this.menuStrip1.PerformLayout();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox textBox1;
    private System.Windows.Forms.TextBox textBox2;
    private System.Windows.Forms.TextBox textBox3;
    private System.Windows.Forms.TextBox textBox4;
    private System.Windows.Forms.TextBox textBox5;
    private System.Windows.Forms.TextBox textBox6;
    private System.Windows.Forms.TextBox textBox7;
    private System.Windows.Forms.TextBox textBox8;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
    private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    private System.Windows.Forms.OpenFileDialog openFileDialog1;
    private System.Windows.Forms.TextBox XOffsetBox;
    private System.Windows.Forms.TextBox XScaleBox;
    private System.Windows.Forms.TextBox YScaleBox;
    private System.Windows.Forms.TextBox YOffsetBox;
  }
}