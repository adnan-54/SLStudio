using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
    public class SplGeometry
    {
        private DiffuseMaterial groupMaterial = new DiffuseMaterial();
        private DiffuseMaterial groupBackMaterial = new DiffuseMaterial();

        public SlrrLib.Model.Spl Spl
        {
            get;
            private set;
        }

        public SlrrLib.Model.SplLine Start
        {
            get;
            set;
        } = null;

        public SlrrLib.Model.SplLine Finish
        {
            get;
            set;
        } = null;

        public SplGeometry(SlrrLib.Model.Spl model)
        {
            Spl = model;
        }

        public MeshGeometry3D MeshFromModelData(double thickness = 10.0, bool visualizeNormals = false)
        {
            MeshGeometry3D obj = new MeshGeometry3D();
            for (int i = 0; i < Spl.SplLines.Count - 1; i++)
            {
                if (Spl.SplLines[i] == Start || Spl.SplLines[i] == Finish)
                {
                    addUShape(obj, Spl.SplLines[i].position, 0.9f * Spl.SplLines[i].position + 0.1f * Spl.SplLines[i + 1 % Spl.SplLines.Count].position, 7.0, 7.0, 8.0f);
                    addUShape(obj, Spl.SplLines[i].position, 0.9f * Spl.SplLines[i].position + 0.1f * Spl.SplLines[i + 1 % Spl.SplLines.Count].position, -7.0, -7.0, 8.0f);
                }
                addSplineSegmentToMesh(obj, Spl.SplLines[i].position, Spl.NextLineSkipCircular(i).position,
                                       Spl.SplLines[i].additionalAttributes, Spl.NextLineSkipCircular(i).additionalAttributes,
                                       Spl.PrevLineSkipCircular(i).position, Spl.NextLineSkipCircular(i + 1).position,
                                       Spl.SplLines[i].normal, Spl.NextLineSkipCircular(i).normal);
            }
            return obj;
        }

        public GeometryModel3D ModelFromModelData(double thickness = 10.0, bool visualizeNormals = false)
        {
            DiffuseMaterial material = new DiffuseMaterial();
            material.Brush = Brushes.Red;
            DiffuseMaterial backMaterial = new DiffuseMaterial();
            backMaterial.Brush = Brushes.Red;
            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = MeshFromModelData(thickness, visualizeNormals);
            model.Material = material;
            model.BackMaterial = backMaterial;
            return model;
        }

        public void AddMeshFromModelData(Model3DGroup group, double thickness = 10.0, bool visualizeNormals = false)
        {
            for (int i = 0; i < Spl.SplLines.Count - 1; i++)
            {
                if (Spl.SplLines[i] == Start || Spl.SplLines[i] == Finish)
                {
                    addUShape(group, Spl.SplLines[i].position, 0.9f * Spl.SplLines[i].position + 0.1f * Spl.SplLines[i + 1 % Spl.SplLines.Count].position, 7.0, 7.0, 8.0f);
                    addUShape(group, Spl.SplLines[i].position, 0.9f * Spl.SplLines[i].position + 0.1f * Spl.SplLines[i + 1 % Spl.SplLines.Count].position, -7.0, -7.0, 8.0f);
                }
                addSegmentToMesh(group, Spl.SplLines[i].position, Spl.NextLineSkipCircular(i).position,
                                 Spl.SplLines[i].additionalAttributes, Spl.NextLineSkipCircular(i).additionalAttributes,
                                 Spl.PrevLineSkipCircular(i).position, Spl.NextLineSkipCircular(i + 1).position,
                                 Spl.SplLines[i].normal, Spl.NextLineSkipCircular(i).normal);
            }
        }

        public Model3DGroup ModelGroupFromModelData(double thickness = 10.0, bool visualizeNormals = false)
        {
            groupMaterial.Brush = Brushes.Red;
            groupBackMaterial.Brush = Brushes.Red;
            Model3DGroup ret = new Model3DGroup();
            AddMeshFromModelData(ret, thickness, visualizeNormals);
            return ret;
        }

        public void AddModelGroupFromModelData(Model3DGroup ret, double thickness = 10.0, bool visualizeNormals = false)
        {
            groupMaterial.Brush = Brushes.Red;
            groupBackMaterial.Brush = Brushes.Red;
            AddMeshFromModelData(ret, thickness, visualizeNormals);
        }

        private void addUShape(MeshGeometry3D obj, SlrrLib.Model.SplNode p1, SlrrLib.Model.SplNode p2, double w1, double w2, float height = 3.0f)
        {
            Vector3D p1v = new Vector3D(p1.x, p1.y, p1.z);
            Vector3D p2v = new Vector3D(p2.x, p2.y, p2.z);
            p1v = p2v - p1v;
            p1v.Normalize();
            p1v = Vector3D.CrossProduct(p1v, new Vector3D(0, 1, 0));
            p2v = p1v * (w2 / 2.0);
            p1v = p1v * (w1 / 2.0);
            //left
            obj.Positions.Add(new Point3D(p1.x + p1v.X, p1.y, p1.z + p1v.Z));
            obj.Positions.Add(new Point3D(p1.x + p1v.X, p1.y + height, p1.z + p1v.Z));
            obj.Positions.Add(new Point3D(p2.x + p2v.X, p2.y, p2.z + p2v.Z));
            obj.Positions.Add(new Point3D(p2.x + p2v.X, p2.y + height, p2.z + p2v.Z));
            obj.TextureCoordinates.Add(new Point(0, 0));
            obj.TextureCoordinates.Add(new Point(1, 0));
            obj.TextureCoordinates.Add(new Point(0, 1));
            obj.TextureCoordinates.Add(new Point(1, 1));
            //right
            obj.Positions.Add(new Point3D(p1.x - p1v.X, p1.y, p1.z - p1v.Z));
            obj.Positions.Add(new Point3D(p1.x - p1v.X, p1.y + height, p1.z - p1v.Z));
            obj.Positions.Add(new Point3D(p2.x - p2v.X, p2.y, p2.z - p2v.Z));
            obj.Positions.Add(new Point3D(p2.x - p2v.X, p2.y + height, p2.z - p2v.Z));
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

        private void addSplineSegmentToMesh(MeshGeometry3D obj, SlrrLib.Model.SplNode p1, SlrrLib.Model.SplNode p2,
                                            SlrrLib.Model.SplAttributes atr1, SlrrLib.Model.SplAttributes atr2,
                                            SlrrLib.Model.SplNode p0, SlrrLib.Model.SplNode p3, SlrrLib.Model.SplNode n1, SlrrLib.Model.SplNode n2)
        {
            Vector3D p1v = new Vector3D(p1.x, p1.y, p1.z);
            Vector3D p2v = new Vector3D(p2.x, p2.y, p2.z);
            p1v = p2v - p1v;
            p1v.Normalize();
            p1v = Vector3D.CrossProduct(p1v, new Vector3D(0, 1, 0));
            p2v = p1v * (atr2.splineWidth / 2.0);
            p1v = p1v * (atr1.splineWidth / 2.0);
            double h1 = atr1.speedRatio * 0.03 + 5.0;
            double h2 = atr2.speedRatio * 0.03 + 5.0;
            //center
            obj.Positions.Add(new Point3D(p1.x, p1.y, p1.z));
            obj.Positions.Add(new Point3D(p1.x, p1.y + h1 + 2.0f, p1.z));
            obj.Positions.Add(new Point3D(p2.x, p2.y, p2.z));
            obj.Positions.Add(new Point3D(p2.x, p2.y + h2 + 2.0f, p2.z));
            //center
            obj.TriangleIndices.Add(obj.Positions.Count - 4);
            obj.TriangleIndices.Add(obj.Positions.Count - 3);
            obj.TriangleIndices.Add(obj.Positions.Count - 2);
            obj.TriangleIndices.Add(obj.Positions.Count - 2);
            obj.TriangleIndices.Add(obj.Positions.Count - 3);
            obj.TriangleIndices.Add(obj.Positions.Count - 1);
            obj.TriangleIndices.Add(obj.Positions.Count - 2);
            obj.TriangleIndices.Add(obj.Positions.Count - 3);
            obj.TriangleIndices.Add(obj.Positions.Count - 4);
            obj.TriangleIndices.Add(obj.Positions.Count - 1);
            obj.TriangleIndices.Add(obj.Positions.Count - 3);
            obj.TriangleIndices.Add(obj.Positions.Count - 2);
            obj.Normals.Add(p1v);
            obj.Normals.Add(p1v);
            obj.Normals.Add(p1v);
            obj.Normals.Add(p1v);
            //Interpolate some
            var m1Spl = n1 * (p1 - p2).Length();
            var m2Spl = n2 * (p1 - p2).Length();
            Vector3D m1 = new Vector3D(m1Spl.x, m1Spl.y, m1Spl.z);
            Vector3D m2 = new Vector3D(m2Spl.x, m2Spl.y, m2Spl.z);
            Vector3D pv1 = new Vector3D(p1.x, p1.y, p1.z);
            Vector3D pv2 = new Vector3D(p2.x, p2.y, p2.z);
            int it = 0;
            double w1 = (atr2.splineWidth / 2.0);
            double w2 = (atr1.splineWidth / 2.0);
            Vector3D lastPos = pv1;
            double lastW = (atr1.splineWidth / 2.0);
            for (float t = 0.01f; t <= 1.1f; t += 0.1f)
            {
                float globW = (float)((w1 - w2) * t + w2);
                Vector3D curPos = (2 * t * t * t - 3 * t * t + 1) * pv1 + (t * t * t - 2 * t * t + t) * m1 +
                                  (-2 * t * t * t + 3 * t * t) * pv2 + (t * t * t - t * t) * m2;
                addUShape(obj, new SlrrLib.Model.SplNode((float)lastPos.X, (float)lastPos.Y, (float)lastPos.Z),
                          new SlrrLib.Model.SplNode((float)curPos.X, (float)curPos.Y, (float)curPos.Z),
                          lastW, globW);
                lastPos = curPos;
                lastW = globW;
                it++;
            }
        }

        private void addUShape(Model3DGroup group, SlrrLib.Model.SplNode p1, SlrrLib.Model.SplNode p2, double w1, double w2, float height = 3.0f)
        {
            MeshGeometry3D obj = new MeshGeometry3D();
            Vector3D p1v = new Vector3D(p1.x, p1.y, p1.z);
            Vector3D p2v = new Vector3D(p2.x, p2.y, p2.z);
            p1v = p2v - p1v;
            p1v.Normalize();
            p1v = Vector3D.CrossProduct(p1v, new Vector3D(0, 1, 0));
            p2v = p1v * (w2 / 2.0);
            p1v = p1v * (w1 / 2.0);
            //left
            obj.Positions.Add(new Point3D(p1.x + p1v.X, p1.y, p1.z + p1v.Z));
            obj.Positions.Add(new Point3D(p1.x + p1v.X, p1.y + height, p1.z + p1v.Z));
            obj.Positions.Add(new Point3D(p2.x + p2v.X, p2.y, p2.z + p2v.Z));
            obj.Positions.Add(new Point3D(p2.x + p2v.X, p2.y + height, p2.z + p2v.Z));
            obj.TextureCoordinates.Add(new Point(0, 0));
            obj.TextureCoordinates.Add(new Point(1, 0));
            obj.TextureCoordinates.Add(new Point(0, 1));
            obj.TextureCoordinates.Add(new Point(1, 1));
            //right
            obj.Positions.Add(new Point3D(p1.x - p1v.X, p1.y, p1.z - p1v.Z));
            obj.Positions.Add(new Point3D(p1.x - p1v.X, p1.y + height, p1.z - p1v.Z));
            obj.Positions.Add(new Point3D(p2.x - p2v.X, p2.y, p2.z - p2v.Z));
            obj.Positions.Add(new Point3D(p2.x - p2v.X, p2.y + height, p2.z - p2v.Z));
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
            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = obj;
            model.Material = groupMaterial;
            model.BackMaterial = groupBackMaterial;
            group.Children.Add(model);
        }

        private void addSegmentToMesh(Model3DGroup group, SlrrLib.Model.SplNode p1, SlrrLib.Model.SplNode p2,
                                      SlrrLib.Model.SplAttributes atr1, SlrrLib.Model.SplAttributes atr2,
                                      SlrrLib.Model.SplNode p0, SlrrLib.Model.SplNode p3, SlrrLib.Model.SplNode n1, SlrrLib.Model.SplNode n2)
        {
            MeshGeometry3D obj = new MeshGeometry3D();
            Vector3D p1v = new Vector3D(p1.x, p1.y, p1.z);
            Vector3D p2v = new Vector3D(p2.x, p2.y, p2.z);
            p1v = p2v - p1v;
            p1v.Normalize();
            p1v = Vector3D.CrossProduct(p1v, new Vector3D(0, 1, 0));
            p2v = p1v * (atr2.splineWidth / 2.0);
            p1v = p1v * (atr1.splineWidth / 2.0);
            double h1 = atr1.speedRatio * 0.03 + 5.0;
            double h2 = atr2.speedRatio * 0.03 + 5.0;
            //center
            obj.Positions.Add(new Point3D(p1.x, p1.y, p1.z));
            obj.Positions.Add(new Point3D(p1.x, p1.y + h1 + 2.0f, p1.z));
            obj.Positions.Add(new Point3D(p2.x, p2.y, p2.z));
            obj.Positions.Add(new Point3D(p2.x, p2.y + h2 + 2.0f, p2.z));
            //center
            obj.TriangleIndices.Add(obj.Positions.Count - 4);
            obj.TriangleIndices.Add(obj.Positions.Count - 3);
            obj.TriangleIndices.Add(obj.Positions.Count - 2);
            obj.TriangleIndices.Add(obj.Positions.Count - 2);
            obj.TriangleIndices.Add(obj.Positions.Count - 3);
            obj.TriangleIndices.Add(obj.Positions.Count - 1);
            obj.TriangleIndices.Add(obj.Positions.Count - 2);
            obj.TriangleIndices.Add(obj.Positions.Count - 3);
            obj.TriangleIndices.Add(obj.Positions.Count - 4);
            obj.TriangleIndices.Add(obj.Positions.Count - 1);
            obj.TriangleIndices.Add(obj.Positions.Count - 3);
            obj.TriangleIndices.Add(obj.Positions.Count - 2);
            obj.Normals.Add(p1v);
            obj.Normals.Add(p1v);
            obj.Normals.Add(p1v);
            obj.Normals.Add(p1v);
            //Interpolate some
            var m1Spl = n1 * (p1 - p2).Length();
            var m2Spl = n2 * (p1 - p2).Length();
            Vector3D m1 = new Vector3D(m1Spl.x, m1Spl.y, m1Spl.z);
            Vector3D m2 = new Vector3D(m2Spl.x, m2Spl.y, m2Spl.z);
            Vector3D pv1 = new Vector3D(p1.x, p1.y, p1.z);
            Vector3D pv2 = new Vector3D(p2.x, p2.y, p2.z);
            int it = 0;
            double w1 = (atr2.splineWidth / 2.0);
            double w2 = (atr1.splineWidth / 2.0);
            Vector3D lastPos = pv1;
            double lastW = (atr1.splineWidth / 2.0);
            for (float t = 0.0f; t <= 1.1f; t += 0.1f)
            {
                float globW = (float)((w1 - w2) * t + w2);
                Vector3D curPos = (2 * t * t * t - 3 * t * t + 1) * pv1 + (t * t * t - 2 * t * t + t) * m1 +
                                  (-2 * t * t * t + 3 * t * t) * pv2 + (t * t * t - t * t) * m2;
                addUShape(group, new SlrrLib.Model.SplNode((float)lastPos.X, (float)lastPos.Y, (float)lastPos.Z),
                          new SlrrLib.Model.SplNode((float)curPos.X, (float)curPos.Y, (float)curPos.Z),
                          lastW, globW);
                lastPos = curPos;
                lastW = globW;
                it++;
            }
            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = obj;
            model.Material = groupMaterial;
            model.BackMaterial = groupBackMaterial;
            group.Children.Add(model);
        }
    }
}