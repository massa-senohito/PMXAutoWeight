using AttEditorWin;
using PEPlugin;
using PEPlugin.Pmx;
using PEPlugin.SDX;
using SlimDX;
using System;
using System.Collections.Generic;
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
          if (usingBone == -1) continue;
          var t = (item.Position - selBone.Position).Length() / maxDist;
          double x, y;
          form.Spline.Interpolate(t, out x, out y);
          float weight = (float)y / 100.0f;
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

    public IEnumerable<VertAttribute> GetVertAttribute()
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
            yield return new VertAttribute(vi, (v.Position - selBone.Position).Length(), weight);
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
      attForm.SetAttr(GetVertAttribute().ToList());
    }
    public void MainWindow()
    {
      IEnumerable<VertAttribute> verts = GetVertAttribute();
      attForm = Program.RunForm(verts.ToList());
      attForm.OnClickedCol = OnClickedVert;
      form = DrawSpline.Program.SplineStarter(OnUpdate);
      var form2 = DrawSpline.Program.SplineStarter(OnUpdate);
      form2.Name = "Distribute";
      var ps = verts.Select(v => new System.Drawing.Point((int)(v.Dist * 200.0), (int)(v.Weight * 200.0f))).ToList();
      form2.SetPoints(ps);
    }

    public override void Run(IPERunArgs args)
    {
      base.Run(args);
      Con = args.Host.Connector.Pmx;
      Pmx = Con.GetCurrentState();
      View = args.Host.Connector.View.PmxView;
      // View.SetSelectedBoneIndices(new int[] { 0});
      // View.UpdateView();
      var indices = View.GetSelectedBoneIndices();
      for (int i = 0; i < indices.Length; i++)
      {
        var selBone = Pmx.Bone[indices[i]];
        //foreach (var item in Pmx.Vertex)
        //View.SetSelectedVertexIndices( VertexIndiceInRange(selBone,0.8f).ToArray() );
      var pos = PmxUtil.BoneAttitude(selBone);
        //pos.Decompose(out Vector3 scale, out Quaternion rot, out Vector3 loc);
        //rot = Quaternion.Invert(rot);
        var dir = (PmxUtil.TargetPosition(selBone) - selBone.Position);
        dir.Normalize();
        Pmx.Primitive.AddBox(0, dir * 0.8f * 5.0f, 0.8f, 0.8f, 0.8f, selBone);
      }
      Con.Update(Pmx);
      View.UpdateModel_Vertex();
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
