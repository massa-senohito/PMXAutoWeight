using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reactive.Linq;
using Reactive.Bindings;

namespace AttEditorWPF
{
  /// <summary>
  /// MainWindow.xaml の相互作用ロジック
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      // 適当なデータ100件生成する
    var data = new ObservableCollection<Person>(
        Enumerable.Range(1, 100).Select(i => new Person
        {
            Name = "田中　太郎" + i,
            Gender = i % 2 == 0 ? Gender.Men : Gender.Women,
            Age = 20 + i % 50,
            AuthMember = i % 5 == 0
        }));
    // DataGridに設定する
    this.dataGrid.ItemsSource = data;
      dataGrid.RowEditEnding += DataGrid_RowEditEnding;
    }

    private void DataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {


      //then check if the newly added row is duplicated
    }

    private void DGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
    {
      Console.WriteLine(e.Row.Item);
    }
  }

  internal class VertAttribute
  {
    public int ID { get; set; }
    public double Dist { get; set; }
    public ReactiveProperty< float> Weight { get; set; }
  }

  public enum Gender
    {
        None,
        Men,
        Women
    }
 
    // DataGridに表示するデータ
    public class Person
    {
        public string Name { get; set; }
        public Gender Gender { get; set; }
        public int Age { get; set; }
        public bool AuthMember { get; set; }
    public override string ToString()
    {
      return Name;
    }
  }

}
