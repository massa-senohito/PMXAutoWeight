using Sample;
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
      var att =
        Enumerable.Range(1, 100).Select(i => new VertAttribute(i, i, i)).ToList();
      Application.Run(new Form1(new SortableBindingList<VertAttribute>(att)));
    }
    public static Form1 RunForm(List<VertAttribute> atts)
    {
      var f = new Form1(new SortableBindingList<VertAttribute>(atts));
      f.Show();
      return f;

    }
  }
}
