namespace PmdWeightAuto
{
  partial class AttributeEditor
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
      this.listView1 = new System.Windows.Forms.ListView();
      this.SuspendLayout();
      // 
      // listView1
      // 
      this.listView1.Location = new System.Drawing.Point(13, 13);
      this.listView1.Name = "listView1";
      this.listView1.Size = new System.Drawing.Size(259, 236);
      this.listView1.TabIndex = 0;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = System.Windows.Forms.View.Details;
      // 
      // AttributeEditor
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 261);
      this.Controls.Add(this.listView1);
      this.Name = "AttributeEditor";
      this.Text = "AttributeEditor";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView listView1;
  }
}