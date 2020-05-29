using System;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
  public class YprRotation3D
  {
    private float y;
    private float p;
    private float r;

    public Vector3D yAxis = new Vector3D(1, 0, 0);
    public Vector3D pAxis = new Vector3D(0, 1, 0);
    public Vector3D rAxis = new Vector3D(0, 0, 1);

    public Transform3DGroup TransformValues
    {
      get
      {
        Transform3DGroup ret = new Transform3DGroup();
        var yRot = new AxisAngleRotation3D(yAxis, y);
        var yRotSteal = new RotateTransform3D(yRot);
        ret.Children.Add(yRotSteal);
        pAxis = yRotSteal.Transform(pAxis);
        rAxis = yRotSteal.Transform(rAxis);
        var pRot = new AxisAngleRotation3D(pAxis, p);
        var pRotSteal = new RotateTransform3D(pRot);
        ret.Children.Add(pRotSteal);
        rAxis = pRotSteal.Transform(rAxis);
        var rRot = new AxisAngleRotation3D(rAxis, r);
        var rRotSteal = new RotateTransform3D(rRot);
        ret.Children.Add(rRotSteal);
        return ret;
      }
    }

    public YprRotation3D(Vector3D literalYPR, bool dataIsDegrees = true)
    {
      double toDeg = 180.0 / Math.PI;
      y = (float)literalYPR.X;
      p = (float)literalYPR.Y;
      r = (float)literalYPR.Z;
      if (!dataIsDegrees)
      {
        y = (float)literalYPR.X*(float)toDeg;
        p = (float)literalYPR.Y*(float)toDeg;
        r = (float)literalYPR.Z*(float)toDeg;
      }
    }
    public YprRotation3D(double y, double p, double r)
    {
      this.y = (float)y;
      this.p = (float)p;
      this.r = (float)r;
    }
  }
}
