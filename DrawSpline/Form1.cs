using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DrawSpline
{
  public static class Util{
    public static void SetX(this Point p,int x)
    {
      p.X = x;
    }    public static void SetY(this Point p,int y)
    {
      p.Y = y;
    }
}
  public partial class Form1 : Form
  {
    const float cpSize = 15;
    List<Point> Points;
    Point point1 = new Point(50, 300);
    Point point2 = new Point(150, 150);
    Point point3 = new Point(300, 400);
    Point point4 = new Point(450, 150);
    Point point5 = new Point(566, 400);
    Point point6 = new Point(616, 450);
    Pen pen = new Pen(Color.Black, 1);
    Brush brush = new SolidBrush(Color.FromArgb(160, 255, 0, 0));
    int moveIndex = 0;
    bool moveFlag = false;

    public Form1()
    {
      InitializeComponent();
      Text = "DrawBezier";
      Points = new List<Point> { point1, point2, point3, point4, point5, point6};
      InitPointLocationLabel();
      SetStyle(
ControlStyles.DoubleBuffer |
ControlStyles.UserPaint |
ControlStyles.AllPaintingInWmPaint, true
);
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
      Graphics g = e.Graphics;
      g.DrawBeziers(pen, Points.ToArray());
      //g.DrawLine(pen, Points[0], Points[1]);
      //g.DrawLine(pen, Points[3], Points[2]);
      for (int i = 0; i < Points.Count / 2; i+=2)
      {
        g.DrawLine(pen, Points[i], Points[i + 1]);
      }
      for (int i = 0; i < Points.Count; i++)
      {
        e.Graphics.FillRectangle(brush, Points[i].X - cpSize / 2, Points[i].Y - cpSize / 2, cpSize, cpSize);
      }
    }

    private void Form1_MouseDown(object sender, MouseEventArgs e)
    {
      for (int i = 0; i < Points.Count; i++)
      {
        if (e.X >= Points[i].X - cpSize / 2 && e.X < Points[i].X + cpSize / 2)
        {
          if (e.Y >= Points[i].Y - cpSize / 2 && e.Y < Points[i].Y + cpSize / 2)
          {
            moveIndex = i;
            moveFlag = true;
          }
        }
      }
      if(!moveFlag)
      {
        Points.Add(e.Location);
        Points.Add(new Point(e.Location.X + 50, e.Location.Y + 50));
      }

    }

    private void Form1_MouseUp(object sender, MouseEventArgs e)
    {
      moveFlag = false;
    }

    private void Form1_MouseMove(object sender, MouseEventArgs e)
    {
      if (moveFlag)
      {
        if (e.X < 1000)
        {
          Points[moveIndex].SetX( e.X );
        }
        else
        {
          Points[moveIndex].SetX( 1000 );
        }

        Points[moveIndex].SetY( e.Y);

        Control cx = Controls[moveIndex + "X"];
        cx.Text = Points[moveIndex].X.ToString();
        Control cy = Controls[moveIndex + "Y"];
        cy.Text = Points[moveIndex].Y.ToString();

        Invalidate();
      }
    }
    private void InitPointLocationLabel()
    {
      for (int i = 0; i < Points.Count; i++)
      {
        Label l = new Label();
        l.Location = new Point(1000, 20 * i + 20);
        l.Size = new Size(64, 20);
        l.Text = "Point" + i;
        Controls.Add(l);
        Label l2 = new Label();
        l2.Name = i + "X";
        l2.Text = Points[i].X.ToString();
        l2.Location = new Point(1064, 20 * i + 20);
        l2.Size = new Size(64, 20);
        Controls.Add(l2);
        Label l3 = new Label();
        l3.Name = i + "Y";
        l3.Text = Points[i].X.ToString();
        l3.Location = new Point(1128, 20 * i + 20);
        l3.Size = new Size(64, 20);
        Controls.Add(l3);

      }

    }

    private void pictureBox1_Click(object sender, EventArgs e)
    {

    }
  }
}
