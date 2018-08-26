using AttEditorWin;
using PEPlugin;
using PEPlugin.Pmx;
using PEPlugin.SDX;
using SlimDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PmdWeightAuto
{

  public class DrawBoneFromDist : PEPluginClass
  {
    public override string Name
    {
      get
      {
        return "距離からウェイトをぬる";
      }
    }
    public override string Description
    {
      get
      {
        return
          "距離から";
      }
    }

    DrawSpline.SplineForm form;
    DrawSpline.SplineForm graph;
    SpaceControll space;

    public static int FindUsingBone(IPXBone b, IPXVertex v)
    {
      if (v.Bone1.Equals(b))
      {
        return 0;
      }

      if (v.Bone2 == null) return -1;
      if (v.Bone2.Equals(b))
      {
        return 1;
      }

      if (v.Bone3 == null) return -1;
      if (v.Bone3.Equals(b))
      {
        return 2;
      }

      if (v.Bone4 == null) return -1;
      if (v.Bone4.Equals(b))
      {
        return 3;
      }
      return -1;
    }
    public static float BoneWeight(IPXBone b, IPXVertex v)
    {
      int usingBone = FindUsingBone(b, v);
          if (usingBone == 0)
          {
            return v.Weight1;
          }
          if (usingBone == 1)
          {
            return v.Weight2;
          }
          if (usingBone == 2)
          {
            return v.Weight3;
          }
          if (usingBone == 3)
          {
            return v.Weight4;
          }
      return 0;
    }

    // ID,頂点距離とウェイト
    private void OnUpdate()
    {
      var indices = View.GetSelectedBoneIndices();
      var maxDist = form.xs.Last() / 100.0f;
      for (int i = 0; i < indices.Length; i++)
      {
        var selBone = Pmx.Bone[indices[i]];
        var verts = Pmx.Vertex.Where(v => (v.Position - selBone.Position).Length() < maxDist);
        var maxT = 0.0;
        foreach (var item in verts)
        {
          var usingBone = FindUsingBone(selBone, item);
          //if (usingBone == -1) continue;
          //var t = (item.Position - selBone.Position).Length() / maxDist;
          var t = DistortedDist(selBone, item) / maxDist;
          if (t>1.0)
          {
            t = 1.0;
          }
          if (t <= 0) continue;
          double x, y;
          form.Spline.Interpolate(t, out x, out y);
          float weight = (float)y / 100.0f;

          Console.Write(t + " ");
          Console.WriteLine(weight);

          if (usingBone == 0)
          {
            item.Weight1 = weight;
          }
          if (usingBone == 1)
          {
            item.Weight2 = weight;
          }
          if (usingBone == 2)
          {
            item.Weight3 = weight;
          }
          if (usingBone == 3)
          {
            item.Weight4 = weight;
          }
          if (usingBone == -1 )//&& !item.SDEF)
          {
            item.SDEF= false ;
            if (item.Bone2 == null)
            {
              item.Weight2 = weight;
              item.Bone2 = selBone;
            }
            else if (item.Bone3 == null)
            {
              item.Weight3 = weight;
              item.Bone3 = selBone;
            }
            else if (item.Bone4 == null)
            {
              item.Weight4 = weight;
              item.Bone4 = selBone;
            }
          }
          if(t > maxT)
          {
            maxT = t;
          }

        }
        DrawSpline.Program.ShowMessage(maxT.ToString());

      }
      /*
       ボーンから指定範囲を選択
縦に伸びた球など歪んだ距離判定空間
ウェイトを数値指定
骨の向きから指向性
距離分布図
  max min から適切な大きさを導く　
歪ませ距離分布図
       */
      UpdateAttEditor();
      Con.Update(Pmx);
      View.UpdateModel();
    }

    private float NormalDist(IPXBone bone,IPXVertex vertex)
    {
      return (vertex.Position - bone.Position).Length();
    }
    
    private float DistortedDist(IPXBone bone,IPXVertex vertex)
    {
      var dist = BoneLocalVerticePosition(bone, vertex);
      dist.X *= space.X;
      dist.Y *= space.Y;
      dist.Z *= space.Z;
      return dist.Length();
    }

    public IEnumerable<VertAttribute> GetVertAttribute(Func<IPXBone,IPXVertex,float> func)
    {
      var indices = View.GetSelectedBoneIndices();
      for (int i = 0; i < indices.Length; i++)
      {
        var selBone = Pmx.Bone[indices[i]];
        for (int vi = 0; vi < Pmx.Vertex.Count; vi++)
        {
          var v = Pmx.Vertex[vi];
          int usingBone = FindUsingBone(selBone, v);
          if (usingBone != -1)
          {
            float weight = BoneWeight(selBone, v);
            yield return new VertAttribute(vi, func(selBone,v), weight);
          }
        }
      }
    }

    void OnClickedVert(IEnumerable< VertAttribute> v)
    {
      var vid = v.Select(i => i.ID).ToArray();
      View.SetSelectedVertexIndices(vid);
      View.UpdateView();
    }

    public void UpdateAttEditor()
    {
      attForm.SetAttr(GetVertAttribute(DistortedDist).ToList());
    }

    public void UpdateGraph()
    {
      Pmx = Con.GetCurrentState();
      IEnumerable<VertAttribute> verts2 = GetVertAttribute(DistortedDist);
      var ps = verts2.Select(v => new System.Drawing.Point((int)(v.Dist * 100.0), (int)(v.Weight * 100.0f))).ToList();
      graph.SetPoints(ps);
    }

    public void OnChangeSpace(float x,float y,float z)
    {
      UpdateGraph();
      UpdateAttEditor();
    }

    public void MainWindow()
    {
      IEnumerable<VertAttribute> verts = GetVertAttribute(DistortedDist);

      form = DrawSpline.Program.SplineStarter(OnUpdate);
      graph = DrawSpline.Program.SplineStarter(UpdateGraph);
      graph.Text = "Distribute";
      space = new SpaceControll(OnChangeSpace,ControllSavehandler,ControllLoadHandler);
      space.Show();
      attForm = Program.RunForm(verts.ToList());
      attForm.OnClickedCol = OnClickedVert;
      UpdateGraph();
    }



    public IEnumerable<IPXBone> SelectedBone()
    {
      var indices = View.GetSelectedBoneIndices();
      for (int i = 0; i < indices.Length; i++)
      {
        var selBone = Pmx.Bone[indices[i]];
        yield return selBone;
      }
    }

    private void ControllLoadHandler(object sender, EventArgs e)
    {
      var path = space.ShowLoad();
      var lines = File.ReadAllLines(path);
      var bone = Pmx.Bone.First(b => b.Name == lines[0]);
      var xyz = lines[1].Split(',').Select(float.Parse).ToArray();
      var v3 = new V3(xyz[0],xyz[1] ,xyz[2]);
      bone.Position = v3;
      var targetXYZ = lines[2].Split(',').Select(float.Parse).ToArray();
      var target = new V3(targetXYZ[0],targetXYZ[1] ,targetXYZ[2]);
      PmxUtil.SetTargetPosition(bone , target);
      space.XYZ = lines[3];
      Con.Update(Pmx);
      View.UpdateModel();

    }

    private void ControllSavehandler(object sender, EventArgs e)
    {
      var bone = SelectedBone().First();
      var pos  = PmxUtil.TargetPosition(bone);
      var distFieldCSV = space.XYZ;
      var list = new string[]{
        bone.Name,
        bone.Position.X + "," +bone.Position.Y + "," +bone.Position.Z ,
        (pos.X + "," + pos.Y + "," + pos.Z) ,
        distFieldCSV,
      };
      var path = space.ShowSave();
      File.WriteAllLines(path, list);
    }

    public override void Run(IPERunArgs args)
    {
      base.Run(args);
      Con = args.Host.Connector.Pmx;
      Pmx = Con.GetCurrentState();
      View = args.Host.Connector.View.PmxView;
      // View.SetSelectedBoneIndices(new int[] { 0});
      // View.UpdateView();


      var selBone = SelectedBone().First();
      //foreach (var item in Pmx.Vertex)
      View.SetSelectedVertexIndices(VertexIndiceInRange(selBone, 0.8f).ToArray());
      //TestMoveVertice(selBone);


      MainWindow();
      Con.Update(Pmx);
      View.UpdateModel();
    }

    private void TestMoveVertice(IPXBone selBone)
    {
      for (int v = 0; v < Pmx.Vertex.Count; v++)
      {
        var item = Pmx.Vertex[v];
        Vector3 vector3 = BoneLocalVerticePosition(selBone, item);
        item.Position = vector3;
      }
    }

    private IEnumerable<int> VertexIndiceInRange(IPXBone selBone,float range)
    {
      for (int v = 0; v < Pmx.Vertex.Count; v++)
      {
        var item = Pmx.Vertex[v];
        Vector3 vector3 = BoneLocalVerticePosition(selBone, item);
        vector3.Z *= 0.2f;
        if (vector3.Length() < range)
        {
          yield return v ;
        }
      }
    }

    private Vector3 BoneLocalVerticePosition(IPXBone selBone,IPXVertex item)
    {
      var pos = PmxUtil.BoneAttitude(selBone);
      pos.Decompose(out Vector3 scale, out Quaternion rot, out Vector3 loc);
      rot = Quaternion.Invert(rot);
      //foreach (var item in Pmx.Vertex)
      {
        // なんかpositionなくなる
        //item.Position -= selBone.Position;
        //Vector4 vector4 = Vector3.Transform(item.Position, Matrix.RotationAxis(rot.Axis, rot.Angle));
        //item.Position = new V3(vector4.X, vector4.Y, vector4.Z);
        var tmp = item.Position - selBone.Position;
        Vector4 vector4 = Vector3.Transform(tmp, Matrix.RotationAxis(rot.Axis, rot.Angle));
        return new V3(vector4.X, vector4.Y, vector4.Z);

      }
    }

    public static void Main(string[] args)
    {
      var pos = new Vector3(1, 0, 0);
      var t = Matrix.RotationAxis(new Vector3(0, 0, 1), 3.141592f / 2.0f );
      var transed = Vector3.Transform(pos, t);
      Matrix right = PmxUtil.LookAt(new Vector3(0,0,0), new Vector3(10,0,0), new Vector3(0,1,0) );
      //Matrix right = Matrix.LookAtLH(new Vector3(0,0,0), new Vector3(10,0,0), new Vector3(0,0,1) );
      right.Decompose(out Vector3 scale, out Quaternion rot, out Vector3 loc);
      Console.WriteLine("");
    }

    private IPXPmx Pmx;
    private IPXPmxConnector Con;
    private PEPlugin.View.IPXPmxViewConnector View;
    private Form1 attForm;

  }
}
