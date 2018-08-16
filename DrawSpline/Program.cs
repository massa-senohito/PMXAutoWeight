using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawSpline
{
  public static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      SplineForm mainForm = new SplineForm();
      var Points = new List<Point>();
      Points.Add(new Point(14, 60));
      Points.Add(new Point(19, 10));
      mainForm.SetPoints(Points);
      Application.Run(mainForm);
    }

    public static SplineForm SplineStarter(Action a)
    {
      var spl = new SplineForm(a);
      spl.Show();
      return spl;
    }

    public static void ShowMessage(string m)
    {
      MessageBox.Show(m);
    }
  }
}
