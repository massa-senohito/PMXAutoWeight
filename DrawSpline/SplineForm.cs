using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawSpline
{
  public partial class SplineForm : Form
  {
    //http://blog.livedoor.jp/tkarasuma/archives/1035308583.html
    public SplineForm(Action updateClicked = null)
    {
      InitializeComponent();
      SetStyle(
        ControlStyles.DoubleBuffer |
        ControlStyles.UserPaint |
        ControlStyles.AllPaintingInWmPaint, true
      );

      OnUpdateClicked = updateClicked;

      textBox1.TextChanged += textBox_TextChanged;
      textBox2.TextChanged += textBox_TextChanged;
      textBox3.TextChanged += textBox_TextChanged;
      textBox4.TextChanged += textBox_TextChanged;
      textBox5.TextChanged += textBox_TextChanged;
      textBox6.TextChanged += textBox_TextChanged;
      textBox7.TextChanged += textBox_TextChanged;
      textBox8.TextChanged += textBox_TextChanged;
      textBox1.Text = cxs[0].ToString();
      textBox2.Text = cys[0].ToString();
      textBox3.Text = cxs[1].ToString();
      textBox4.Text = cys[1].ToString();
      textBox5.Text = cxs[2].ToString();
      textBox6.Text = cys[2].ToString();
      textBox7.Text = cxs[3].ToString();
      textBox8.Text = cys[3].ToString();

      textBoxes = new List<TextBox>
      {
        textBox1,
        textBox2,
        textBox3,
        textBox4,
        textBox5,
        textBox6,
        textBox7,
        textBox8,
      };
      XScaleBox.TextChanged += ScaleBox_TextChanged;
      YScaleBox.TextChanged += ScaleBox_TextChanged;
      XOffsetBox.TextChanged += OffsetBox_TextChanged;
      YOffsetBox.TextChanged += OffsetBox_TextChanged;
      XScaleBox.Text = "1.0";
      YScaleBox.Text = "1.0";
      XOffsetBox.Text = "30";
      YOffsetBox.Text = "80";
      MouseWheel += SplineForm_MouseWheel;
    }

    private void SplineForm_MouseWheel(object sender, MouseEventArgs e)
    {
      bool CtrlPress  = (ModifierKeys & Keys.Control) != 0;
      bool ShiftPress = (ModifierKeys & Keys.Shift) != 0;
      bool xMove = CtrlPress || (!CtrlPress && !ShiftPress);
      bool yMove = ShiftPress || (!CtrlPress && !ShiftPress);
      if(e.Delta > 0)
      {
        if (xMove)
        {
          xScale += 0.2f;
        }
        if (yMove)
        {
          yScale += 0.2f;
        }
      }
      if(e.Delta < 0)
      {
        if (xMove)
        {
          xScale -= 0.2f;
        }
        if (yMove)
        {
          yScale -= 0.2f;
        }
      }
      XScaleBox.Text = xScale.ToString();
      YScaleBox.Text = yScale.ToString();
      Invalidate();
    }

    private void OffsetBox_TextChanged(object sender, EventArgs e)
    {
      var sentBox = (TextBox)sender;
      try
      {
        if (sentBox.Name[0] == 'X')
        {
          xOffset = float.Parse(sentBox.Text);
        }
        else
        {
          yOffset = float.Parse(sentBox.Text);
        }
      }
      catch(Exception ex) { }

      Invalidate();
    }

    private void ScaleBox_TextChanged(object sender, EventArgs e)
    {
      var sentBox = (TextBox)sender;
      try
      {
        if (sentBox.Name[0] == 'X')
        {
          xScale = float.Parse(sentBox.Text);
        }
        else
        {
          yScale = float.Parse(sentBox.Text);
        }
      }
      catch(Exception ex) { }

      Invalidate();
    }

    private void textBox_TextChanged(object sender, EventArgs e)
    {
      var box = (TextBox)sender;
      var changedIndex = int.Parse( box.Name.Substring(7))-1;
      bool isYAxis = changedIndex % 2 == 1;
      try
      {
        if (isYAxis)
        {
          cys[changedIndex / 2] = double.Parse(box.Text);
        }
        else
        {
          cxs[changedIndex / 2] = double.Parse(box.Text);
        }
      }
      catch(Exception ex) { }

        Invalidate();
    }

    Action OnUpdateClicked;
    List<TextBox> textBoxes ;
    const float cpSize = 15;
    double[] cxs = new double[] { 150.0 , 200.0  , 400.0 , 600.0 };
    double[] cys = new double[] { 150.0 , 300.0  , 300.0 , 150.0  };
    public double[] xs, ys;
    int moveIndex = 0;
    bool moveFlag = false;
    Brush brush = new SolidBrush(Color.FromArgb(160, 255, 0, 0));
    Brush blkbrush = new SolidBrush(Color.FromArgb(255, 0, 0, 0));
    float xOffset = 30;
    float yOffset = 80;
    float xScale = 1;
    float yScale =  1;
    public BSpline Spline;

    private int YTrans(int y)
    {
      double v = ((y) * yScale + yOffset) ;
      return Height - (int)( v );
    }

    private int InvYTrans(int y)
    {
      double v = Height - ( y + yOffset) ;
      return (int)((v )*(1.0 / yScale));
    }

    private Point TranslatePos(Point p)
    {
      Point temp = p;
      temp.X = (int)(temp.X  * xScale + xOffset);
      temp.Y = YTrans(temp.Y);
      return temp;
    }

    private Point DrawPosToDataPos(Point p)
    {
      Point temp = p;
      temp.X = (int)((temp.X - xOffset) *(1.0f / xScale));
      temp.Y = InvYTrans(temp.Y);
      return temp;
    }

    private void SplineForm_Paint(object sender, PaintEventArgs e)
    {
      // BSplineクラスの生成
      Spline = new BSpline(cxs, cys);
      var ts = Enumerable.Range(0, 100).Select(t => t / 100.0).ToArray();
      Spline.Interpolate(ts, out xs, out ys);
      Graphics g = e.Graphics;
      for (int i = 0; i < xs.Length; i += 2)
      {
        Point p  = new Point((int)xs[i]     , (int)ys[i]);
        Point pp = new Point((int)xs[i + 1] , (int)ys[i + 1]);
        g.DrawLine(pen, TranslatePos( p ),TranslatePos( pp ) );
      }
      g.DrawLine(pen, TranslatePos(new Point(0, 0)), TranslatePos(new Point(100, 0) ));
      g.DrawLine(pen, TranslatePos(new Point(0, 0)), TranslatePos(new Point(0, 100) ));
      for (int i = 0; i < cxs.Length; i++)
      {
        float x = (float)cxs[i];
        float y = (float)cys[i];
        Point drawPoint = TranslatePos(new Point((int)x, (int)y));
        var cp = cpSize / 2;
        e.Graphics.FillRectangle(brush, drawPoint.X - cp, drawPoint.Y - cp, cpSize, cpSize);
        e.Graphics.DrawString(x + " : " + y, Font, blkbrush, drawPoint.X, drawPoint.Y - 30);

      }
    }

    private void BeginMovePoint(MouseEventArgs e)
    {
      for (int i = 0; i < cxs.Length; i++)
      {
        Point drawPoint = TranslatePos(new Point((int)cxs[i], (int)cys[i]));
        //int ex = e.X - xOffset;
        if (drawPoint.X >= e.X - cpSize / 2 && drawPoint.X < e.X + cpSize / 2)
        {
          //float y = Height - ((float)cys[i] - cpSize / 2);
          float y = drawPoint.Y - cpSize / 2;
          if (e.Y >= y && e.Y < y + cpSize)
          {
            moveIndex = i;
            moveFlag = true;
          }
        }
      }
    }
    Point PrevPos;
    private void SplineForm_MouseDown(object sender, MouseEventArgs e)
    {
      if(e.Button == MouseButtons.Left)
      {
        BeginMovePoint(e);
      }
    }

    private void Form1_MouseUp(object sender, MouseEventArgs e)
    {
      moveFlag = false;
    }

    private void TickMovePoint(MouseEventArgs e)
    {
      if (moveFlag)
      {
        var point = DrawPosToDataPos(new Point(e.X, e.Y));
        var x = point.X ;
        var y = point.Y;
        cxs[moveIndex] = x;
        cys[moveIndex] = y;

        UpdateTextBox(x, y, moveIndex);
        Invalidate();
      }
    }

    private void Form1_MouseMove(object sender, MouseEventArgs e)
    {

      if(e.Button == MouseButtons.Left)
      {
        TickMovePoint(e);
      }

      if(e.Button == MouseButtons.Middle)
      {
        var xDelta = (e.Location.X - PrevPos.X);
        var yDelta = (e.Location.Y - PrevPos.Y);
        xOffset += xDelta;
        yOffset -= yDelta;
        XOffsetBox.Text = xOffset.ToString();
        YOffsetBox.Text = yOffset.ToString();
        Invalidate();

      }
      PrevPos = e.Location;
    }

    private void UpdateTextBox(double x, double y, int i)
    {
      textBoxes[i * 2].Text = x.ToString();
      textBoxes[i * 2 + 1].Text = y.ToString();
    }

    Pen pen = new Pen(Color.Black, 1);

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      DialogResult result = saveFileDialog1.ShowDialog();
      if (result != DialogResult.Cancel)
      {
        List<string> lines = new List<string>();
        for (int i = 0; i < cxs.Length; i++)
        {
          lines.Add(cxs[i] + "," + cys[i]);
        }
        lines.Add("--splineEnd--");
        lines.Add(XOffsetBox.Text + "," + XScaleBox.Text);
        lines.Add(YOffsetBox.Text + "," + YScaleBox.Text);

        File.WriteAllLines(saveFileDialog1.FileName, lines.ToArray());
      }
    }

    private void loadToolStripMenuItem_Click(object sender, EventArgs e)
    {

      DialogResult result = openFileDialog1.ShowDialog();
      if (result != DialogResult.Cancel)
      {
        string[] lines = File.ReadAllLines(openFileDialog1.FileName);
        int i = 0;
        while(lines[i] != "--splineEnd--")
        {
          string[] xy = lines[i].Split(',');
          double x = double.Parse(xy[0]);
          double y = double.Parse(xy[1]);
          cxs[i] = x;
          cys[i] = y;
          UpdateTextBox(x, y, i);
          i++;
        }
        string[] xofSc = lines[i+1].Split(',');
        string[] yofSc = lines[i+2].Split(',');

        XOffsetBox.Text = xofSc[0];
        YOffsetBox.Text = yofSc[0];
        XScaleBox.Text = xofSc[1];
        YScaleBox.Text = yofSc[1];
        Invalidate();
      }
    }

    private void SplineForm_Resize(object sender, EventArgs e)
    {
      Invalidate();
    }


    private void updateToolStripMenuItem_Click(object sender, EventArgs e)
    {
      OnUpdateClicked?.Invoke();
    }


  }
}
