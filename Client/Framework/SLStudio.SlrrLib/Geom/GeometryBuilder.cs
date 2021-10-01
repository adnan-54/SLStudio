using System;
using System.Windows;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
    public abstract class GeometryBuilder
    {
        public abstract void GenerateVisuals();

        protected Vector3D getVec3(Point3D p)
        {
            return new Vector3D(p.X, p.Y, p.Z);
        }

        protected double max3(double p1, double p2, double p3)
        {
            return Math.Max(Math.Max(p1, p2), p3);
        }

        protected double max3(Size3D s)
        {
            return Math.Max(Math.Max(s.X, s.Y), s.Z);
        }

        protected void addUShapeAtPos(MeshGeometry3D obj, Vector3D p1v, double w, float height = 3.0f)
        {
            var diff = new Vector3D(w / 2.0, 0, 0);
            addUShape(obj, p1v - diff, p1v + diff, w, w, height);
        }

        protected void addUShape(MeshGeometry3D obj, Vector3D p1v, Vector3D p2v, double w1, double w2, float height = 3.0f)
        {
            var p1 = p1v;
            var p2 = p2v;
            p1v = p2v - p1v;
            p1v.Normalize();
            p1v = Vector3D.CrossProduct(p1v, new Vector3D(0, 1, 0));
            p2v = p1v * (w2 / 2.0);
            p1v = p1v * (w1 / 2.0);
            //left
            obj.Positions.Add(new Point3D(p1.X + p1v.X, p1.Y, p1.Z + p1v.Z));
            obj.Positions.Add(new Point3D(p1.X + p1v.X, p1.Y + height, p1.Z + p1v.Z));
            obj.Positions.Add(new Point3D(p2.X + p2v.X, p2.Y, p2.Z + p2v.Z));
            obj.Positions.Add(new Point3D(p2.X + p2v.X, p2.Y + height, p2.Z + p2v.Z));
            obj.TextureCoordinates.Add(new Point(0, 0));
            obj.TextureCoordinates.Add(new Point(1, 0));
            obj.TextureCoordinates.Add(new Point(0, 1));
            obj.TextureCoordinates.Add(new Point(1, 1));
            //right
            obj.Positions.Add(new Point3D(p1.X - p1v.X, p1.Y, p1.Z - p1v.Z));
            obj.Positions.Add(new Point3D(p1.X - p1v.X, p1.Y + height, p1.Z - p1v.Z));
            obj.Positions.Add(new Point3D(p2.X - p2v.X, p2.Y, p2.Z - p2v.Z));
            obj.Positions.Add(new Point3D(p2.X - p2v.X, p2.Y + height, p2.Z - p2v.Z));
            obj.TextureCoordinates.Add(new Point(0, 0));
            obj.TextureCoordinates.Add(new Point(1, 0));
            obj.TextureCoordinates.Add(new Point(0, 1));
            obj.TextureCoordinates.Add(new Point(1, 1));
            //left
            obj.TriangleIndices.Add(obj.Positions.Count - 1 - 4);
            obj.TriangleIndices.Add(obj.Positions.Count - 3 - 4);
            obj.TriangleIndices.Add(obj.Positions.Count - 2 - 4);
            obj.TriangleIndices.Add(obj.Positions.Count - 2 - 4);
            obj.TriangleIndices.Add(obj.Positions.Count - 3 - 4);
            obj.TriangleIndices.Add(obj.Positions.Count - 4 - 4);
            //right
            obj.TriangleIndices.Add(obj.Positions.Count - 2);
            obj.TriangleIndices.Add(obj.Positions.Count - 3);
            obj.TriangleIndices.Add(obj.Positions.Count - 1);
            obj.TriangleIndices.Add(obj.Positions.Count - 4);
            obj.TriangleIndices.Add(obj.Positions.Count - 3);
            obj.TriangleIndices.Add(obj.Positions.Count - 2);
            //top
            obj.TriangleIndices.Add(obj.Positions.Count - 3);
            obj.TriangleIndices.Add(obj.Positions.Count - 7);
            obj.TriangleIndices.Add(obj.Positions.Count - 5);
            obj.TriangleIndices.Add(obj.Positions.Count - 1);
            obj.TriangleIndices.Add(obj.Positions.Count - 3);
            obj.TriangleIndices.Add(obj.Positions.Count - 5);
            obj.Normals.Add(p1v);
            obj.Normals.Add(p1v);
            obj.Normals.Add(p1v);
            obj.Normals.Add(p1v);
            obj.Normals.Add(p2v);
            obj.Normals.Add(p2v);
            obj.Normals.Add(p2v);
            obj.Normals.Add(p2v);
        }

        protected void addSegmentToMesh(MeshGeometry3D obj, Vector3D p1, Vector3D p2,
                                        float pw1, float ph1, float pw2, float ph2, Vector3D n1, Vector3D n2, bool addCenter = true)
        {
            double h1 = ph2 * 0.03 + 5.0;
            double h2 = ph1 * 0.03 + 5.0;
            Vector3D pv1 = new Vector3D(p1.X, p1.Y, p1.Z);
            Vector3D pv2 = new Vector3D(p2.X, p2.Y, p2.Z);
            int it = 0;
            double w1 = (pw2 / 2.0);
            double w2 = (pw1 / 2.0);
            Vector3D lastPos = pv1;
            double lastW = (pw1 / 2.0);
            if (addCenter)
            {
                addUShape(obj, pv1,
                          pv1 + new Vector3D(2, 0, 0),
                          2, 2, 100);
                addUShape(obj, pv2,
                          pv2 + new Vector3D(2, 0, 0),
                          2, 2, 100);
                addUShape(obj, n1,
                          n1 + new Vector3D(2, 0, 0),
                          2, 2, 100);
                addUShape(obj, n2,
                          n2 + new Vector3D(2, 0, 0),
                          2, 2, 100);
            }
            for (float t = 0.001f; t <= 1.1f; t += 0.1f)
            {
                float h = (float)((h1 - h2) * t + h2);
                float globW = (float)((w1 - w2) * t + w2);

                Vector3D curPos = Math.Pow(1 - t, 3) * pv1 +
                                  3 * Math.Pow(1 - t, 2) * t * n1 +
                                  3 * (1 - t) * t * t * n2 +
                                  t * t * t * pv2;

                addUShape(obj, lastPos,
                          curPos,
                          lastW, globW, h);
                lastPos = curPos;
                lastW = globW;
                it++;
            }
        }

        protected Vector3D scaleVector(Vector3D vector, double length)
        {
            double scale = length / vector.Length;
            return new Vector3D(
                     vector.X * scale,
                     vector.Y * scale,
                     vector.Z * scale);
        }

        protected void addTriangle(MeshGeometry3D mesh, Point3D point1, Point3D point2, Point3D point3)
        {
            int index1 = mesh.Positions.Count;
            mesh.Positions.Add(point1);
            mesh.Positions.Add(point2);
            mesh.Positions.Add(point3);
            mesh.TriangleIndices.Add(index1++);
            mesh.TriangleIndices.Add(index1++);
            mesh.TriangleIndices.Add(index1);
        }

        protected void addSegment(MeshGeometry3D mesh, Point3D point1, Point3D point2, Vector3D up, double thickness = 0.1)
        {
            Vector3D v = point2 - point1;
            Vector3D n1 = scaleVector(up, thickness / 2.0);
            Vector3D n2 = Vector3D.CrossProduct(v, n1);
            n2 = scaleVector(n2, thickness / 2.0);
            Point3D p1pp = point1 + n1 + n2;
            Point3D p1mp = point1 - n1 + n2;
            Point3D p1pm = point1 + n1 - n2;
            Point3D p1mm = point1 - n1 - n2;
            Point3D p2pp = point2 + n1 + n2;
            Point3D p2mp = point2 - n1 + n2;
            Point3D p2pm = point2 + n1 - n2;
            Point3D p2mm = point2 - n1 - n2;
            addTriangle(mesh, p1pp, p1mp, p2mp);
            addTriangle(mesh, p1pp, p2mp, p2pp);
            addTriangle(mesh, p1pp, p2pp, p2pm);
            addTriangle(mesh, p1pp, p2pm, p1pm);
            addTriangle(mesh, p1pm, p2pm, p2mm);
            addTriangle(mesh, p1pm, p2mm, p1mm);
            addTriangle(mesh, p1mm, p2mm, p2mp);
            addTriangle(mesh, p1mm, p2mp, p1mp);
            addTriangle(mesh, p1pp, p1pm, p1mm);
            addTriangle(mesh, p1pp, p1mm, p1mp);
            addTriangle(mesh, p2pp, p2mp, p2mm);
            addTriangle(mesh, p2pp, p2mm, p2pm);
        }

        protected void addSegment(MeshGeometry3D mesh, Vector3D point1, Vector3D point2, Vector3D up, double thickness1, double thickness2)
        {
            addSegment(mesh, new Point3D(point1.X, point1.Y, point1.Z), new Point3D(point2.X, point2.Y, point2.Z), up, thickness1, thickness2);
        }

        protected void addSegment(MeshGeometry3D mesh, Point3D point1, Point3D point2, Vector3D up, double thickness1, double thickness2)
        {
            Vector3D v = point2 - point1;
            Vector3D n1 = scaleVector(up, thickness1 / 2.0);
            Vector3D n2 = Vector3D.CrossProduct(v, n1);
            n2 = scaleVector(n2, thickness1 / 2.0);
            Vector3D n1_2 = scaleVector(up, thickness2 / 2.0);
            Vector3D n2_2 = Vector3D.CrossProduct(v, n1_2);
            n2_2 = scaleVector(n2_2, thickness2 / 2.0);
            Point3D p1pp = point1 + n1 + n2;
            Point3D p1mp = point1 - n1 + n2;
            Point3D p1pm = point1 + n1 - n2;
            Point3D p1mm = point1 - n1 - n2;
            Point3D p2pp = point2 + n1_2 + n2_2;
            Point3D p2mp = point2 - n1_2 + n2_2;
            Point3D p2pm = point2 + n1_2 - n2_2;
            Point3D p2mm = point2 - n1_2 - n2_2;
            addTriangle(mesh, p1pp, p1mp, p2mp);
            addTriangle(mesh, p1pp, p2mp, p2pp);
            addTriangle(mesh, p1pp, p2pp, p2pm);
            addTriangle(mesh, p1pp, p2pm, p1pm);
            addTriangle(mesh, p1pm, p2pm, p2mm);
            addTriangle(mesh, p1pm, p2mm, p1mm);
            addTriangle(mesh, p1mm, p2mm, p2mp);
            addTriangle(mesh, p1mm, p2mp, p1mp);
            addTriangle(mesh, p1pp, p1pm, p1mm);
            addTriangle(mesh, p1pp, p1mm, p1mp);
            addTriangle(mesh, p2pp, p2mp, p2mm);
            addTriangle(mesh, p2pp, p2mm, p2pm);
        }
    }
}