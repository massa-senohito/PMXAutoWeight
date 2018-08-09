using Sample;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AttEditorWin
{
  //public class 
  public partial class Form1 : Form
  {
    public Action<IEnumerable< VertAttribute>> OnClickedCol;
    public Form1(SortableBindingList<VertAttribute> vert)
    {
      InitializeComponent();

      dataGridView1.DataSource = vert;
      SortByDist();
    }
    public void SetAttr(List<VertAttribute> vert)
    {
      dataGridView1.DataSource = new SortableBindingList<VertAttribute>(vert);
    }
    public void SortByWeight()
    {
      dataGridView1.Sort(dataGridView1.Columns[2],ListSortDirection.Ascending);
    }
    public void SortByDist()
    {
      dataGridView1.Sort(dataGridView1.Columns[1],ListSortDirection.Ascending);
    }

    public IEnumerable<VertAttribute> SelectedVert ()
    {
      foreach (var item in dataGridView1.SelectedRows)
      {
         var i = (DataGridViewRow)item;
        yield return (VertAttribute)i.DataBoundItem;
      }
    }

    private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      //var a = (VertAttribute)dataGridView1.Rows[e.RowIndex].DataBoundItem;
      var s = SelectedVert();
      OnClickedCol?.Invoke(s);
      //
    }

    private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
      var a = (VertAttribute)dataGridView1.Rows[e.RowIndex].DataBoundItem;
      MessageBox.Show(a.ID + ":" + a.Weight);
    }
    //even
  }
  public class VertAttribute
  {
    public int ID { get; set; }
    public double Dist { get; set; }
    public  float Weight { get; set; }
    public VertAttribute(int id , double dist,float weight)
    {
      ID = id;
      Dist = dist;
      Weight = weight;
    }
  }
}
