using Sample;
using SlimDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AttEditorWin
{
  public static class Program
  {
    /// <summary>
    /// アプリケーションのメイン エントリ ポイントです。
    /// </summary>
    [STAThread]
    static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      InvMat();
      var att =
        Enumerable.Range(1, 100).Select(i => new VertAttribute(i, i, i)).ToList();
      Application.Run(new Form1(new SortableBindingList<VertAttribute>(att)));

    }
            static void InvMat()
        {
            var v = new Vector3(3, -4, 5);
            var r = new Quaternion(new Vector3(0, 0, 1), 45);
            //r = Quaternion.Identity;
            var m = Matrix.Transformation
                (v,Quaternion.Identity, new Vector3(1,1,1)
                , Vector3.Zero, r, v);
           // m.M11 = 0;
            m.Decompose(out Vector3 vector1, out Quaternion quaternion1, out Vector3 trans1);
            m.Invert();
            m.Decompose(out Vector3 vector, out Quaternion quaternion, out Vector3 trans);
            m.Invert();
        }

    public static Form1 RunForm(List<VertAttribute> atts)
    {
      var f = new Form1(new SortableBindingList<VertAttribute>(atts));
      f.Show();
      return f;

    }
  }
}
