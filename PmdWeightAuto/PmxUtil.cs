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
  public static class PmxUtil
  {

    public static Matrix BoneAttitude(IPXBone bone)
    {
      V3 worldBoneTarget = TargetPosition(bone);
      var toward = worldBoneTarget - bone.Position;
      toward.Normalize();
      var up = Vector3.UnitY;
      if (Vector3.Dot(up, toward) > 0.8)
      {
        up = Vector3.UnitX;
      }

      //Matrix right = Matrix.LookAtRH(bone.Position, worldBoneTarget, up);
      Matrix right = LookAt(bone.Position, worldBoneTarget, up);
      right.Decompose(out Vector3 scale, out Quaternion rot, out Vector3 loc);
      return Matrix.Translation(bone.Position) * right;
    }

    //public static V3 

    public static V3 TargetPosition(IPXBone bone)
    {
      var worldBoneTarget = new V3();
      if (bone.ToBone != null)
      {
        worldBoneTarget = bone.ToBone.Position;
      }
      else
      {
        worldBoneTarget = bone.Position + bone.ToOffset;
      }

      return worldBoneTarget;
    }

    public static void SetTargetPosition(IPXBone bone , V3 pos)
    {
      if (bone.ToBone != null)
      {
        bone.ToBone.Position = pos;
      }
      else
      {
        bone.ToOffset = pos - bone.Position;
      }

    }

    public static Matrix LookAt(Vector3 from , Vector3 to , Vector3 up)
    {
      Vector3 zAxis = to - from;
      zAxis.Normalize();
      Vector3 xAxis = Vector3.Cross(up, zAxis);
      Vector3 yAxis = Vector3.Cross(zAxis, xAxis);
      Matrix m = new Matrix();
      m.set_Rows(0, new Vector4(xAxis, 0));
      m.set_Rows(1, new Vector4(yAxis, 0));
      m.set_Rows(2, new Vector4(zAxis, 0));
      m.set_Rows(3, new Vector4(Vector3.Dot(xAxis, -from),Vector3.Dot(yAxis, -from), Vector3.Dot(zAxis, -from), 1));
      return m;
    }
    static string comma = " , ";
    public static string Csv<T>(this IEnumerable<T> items)
    {
      return items.Aggregate("", (acc, i) => acc + i + comma);
    }

    public static string VertToString(this IPXVertex vertex)
    {
      return "{ " + Math.Round( vertex.Position.X) + comma + Math.Round( vertex.Position.Y) + comma + Math.Round(vertex.Position.Z) + " } ";
    }

    public static string FaceToString(this IPXFace face)
    {
      return " { " +face.Vertex1.VertToString() + face.Vertex2.VertToString() + face.Vertex3.VertToString() + " } ";
    }

    public static bool IsContainVert(this IPXFace f , IPXVertex v)
    {
      return (f.Vertex1 == v || f.Vertex2 == v || f.Vertex3 == v);
    }

    public static Vector3 Cross(this IPXVertex v, IPXVertex vv, IPXVertex vvv)
    {
      var n1 = (v.Position - vv.Position);
      var n2 = (v.Position - vvv.Position);
      n1.Normalize();
      n2.Normalize();
      return Vector3.Cross(n2, n1);
    }

    public static void CopyVert(this IPXVertex v,IPXVertex vv)
    {
      v.Bone1 = vv.Bone1;
      v.Bone2 = vv.Bone2;
      v.Bone3 = vv.Bone3;
      v.Bone4 = vv.Bone4;
      v.EdgeScale = vv.EdgeScale;
      v.Normal = vv.Normal;
      v.Position = vv.Position;
      v.UV = vv.UV;
      v.Weight1 = vv.Weight1;
      v.Weight2 = vv.Weight2;
      v.Weight3 = vv.Weight3;
      v.Weight4 = vv.Weight4;
      v.QDEF = vv.QDEF;
      v.SDEF = vv.SDEF;
      v.SDEF_C = vv.SDEF_C;
      v.SDEF_R0 = vv.SDEF_R0;
      v.SDEF_R1 = vv.SDEF_R1;
      v.UVA1 = vv.UVA1;
      v.UVA2 = vv.UVA2;
      v.UVA3 = vv.UVA3;
      v.UVA4 = vv.UVA4;
    }

    public static IPXVertex LastVert(this IPXFace f, IPXVertex v,IPXVertex vv)
    {
      if(f.Vertex1 == v && f.Vertex2 == vv )
      {
        return f.Vertex3;
      }
      if(f.Vertex2 == v && f.Vertex1 == vv )
      {
        return f.Vertex3;
      }

      if(f.Vertex2 == v && f.Vertex3 == vv )
      {
        return f.Vertex1;
      }
      if (f.Vertex3 == v && f.Vertex2 == vv )
      {
        return f.Vertex1;
      }

      if(f.Vertex1 == v && f.Vertex3 == vv )
      {
        return f.Vertex2;
      }
      if (f.Vertex3 == v && f.Vertex1 == vv )
      {
        return f.Vertex2;
      }
      throw new Exception("頂点が");
    }

    public static void ReAssignFace (this IPXFace f , IPXVertex lastv , IPXVertex v1 , IPXVertex v2)
    {
      if(f.Vertex1 == lastv)
      {
        f.Vertex2 = v1;
        f.Vertex3 = v2;
        return;
      }
      if(f.Vertex2 == lastv)
      {
        f.Vertex1 = v1;
        f.Vertex3 = v2;
        return;
      }
      if(f.Vertex3 == lastv)
      {
        f.Vertex1 = v1;
        f.Vertex2 = v2;
        return;
      }
      throw new Exception("最後の頂点が面にありませんでした");
    }
    public static V3 GetNormalized(this V3 v)
    {
      v.Normalize();
      return v;
    }

    public static V3 Direction(this IPXVertex v,IPXVertex vv)
    {
      return (vv.Position - v.Position).GetNormalized();
    }
  }
}
