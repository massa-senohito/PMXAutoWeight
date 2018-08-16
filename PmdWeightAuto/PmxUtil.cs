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
  static class PmxUtil
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

      Matrix right = Matrix.LookAtRH(bone.Position, worldBoneTarget, up);
      right.Decompose(out Vector3 scale, out Quaternion rot, out Vector3 loc);
      return Matrix.Translation(bone.Position) * right;
    }

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
  }
}
