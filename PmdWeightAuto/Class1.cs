﻿using AttEditorWin;
using PEPlugin;
using PEPlugin.Pmx;
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

    public override void Run(IPERunArgs args)
    {
      base.Run(args);
      Con = args.Host.Connector.Pmx;
      Pmx = Con.GetCurrentState();
      View = args.Host.Connector.View.PmxView;
     // View.SetSelectedBoneIndices(new int[] { 0});
     // View.UpdateView();

      attForm = Program.RunForm(GetVertAttribute().ToList());
      attForm.OnClickedCol = OnClickedVert;
      form = DrawSpline.Program.SplineStarter(OnUpdate);
    }
    private IPXPmx Pmx;
    private IPXPmxConnector Con;
    private PEPlugin.View.IPXPmxViewConnector View;
    private Form1 attForm;

  }
}