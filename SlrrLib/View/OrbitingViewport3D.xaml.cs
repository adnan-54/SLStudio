using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace SlrrLib.View
{
    public partial class OrbitingViewport3D : UserControl
    {
        private Geom.OrbitingCamera viewPortCamera;
        private ModelVisual3D currentVisuals;
        private Geom.IModelFactory modelFactory = null;
        private Geom.NamedCfgInducedModel marker = null;
        private bool middleMouseDown = false;

        private List<PointLight> lights = new List<PointLight>
    {
      new PointLight { Color = Colors.White, Position = new Point3D(1000, 500, 0), Range = 50000 },
      new PointLight { Color = Colors.DarkGray, Position = new Point3D(-500, -500, 0), Range = 50000 },
      new PointLight { Color = Colors.DarkGray, Position = new Point3D(-500, -500, 500), Range = 50000 }
    };

        private bool animScene = false;
        private Point dragStart;
        private Dictionary<string, ImageSource> imageCahce = new Dictionary<string, ImageSource>();
        private Vector3D objectVisualsLastLoadPosition = new Vector3D(0, 0, 0);
        private List<Geom.NamedModel> currentModels = new List<Geom.NamedModel>();
        private List<Geom.NamedModel> currentVisibleModels = new List<Geom.NamedModel>();

        public IEnumerable<string> ModelGroups
        {
            get;
            private set;
        }

        public IEnumerable<Geom.NamedModel> CurrentModels
        {
            get
            {
                return currentModels.ToList();
            }
            private set
            {
                currentModels = new List<Geom.NamedModel>(value);
            }
        }

        public IEnumerable<Geom.NamedModel> CurrentVisibleModels
        {
            get
            {
                return currentVisibleModels.ToList();
            }
        }

        public Geom.NamedModel SelectedModel
        {
            get;
            private set;
        }

        public Vector3D MarkerPosition
        {
            get
            {
                if (marker == null)
                    return new Vector3D(0, 0, 0);
                return new Vector3D
                {
                    X = marker.BodyLine.LineX,
                    Y = marker.BodyLine.LineY,
                    Z = marker.BodyLine.LineZ
                };
            }
        }

        public Vector3D MarkerPositionInRpkSpatialSpace
        {
            get
            {
                if (marker == null)
                    return new Vector3D(0, 0, 0);
                return new Vector3D
                {
                    X = marker.BodyLine.LineX * 100.0f,
                    Y = marker.BodyLine.LineY * 100.0f,
                    Z = marker.BodyLine.LineZ * 100.0f
                };
            }
        }

        public float ObjectViewDistance
        {
            get;
            set;
        } = float.MaxValue;

        public bool IsViewDistanceManaged
        {
            get;
            set;
        } = false;

        public OrbitingViewport3D()
        {
            InitializeComponent();
        }

        public void AddMarker()
        {
            marker = Geom.NamedCfgInducedModel.ConstructMovableModelFromSCX("Marker_Frame.SCX", "Marker");
            insertModelIntoScene(marker, 0);
        }

        public bool IsMarker(Geom.NamedModel model)
        {
            return model == marker;
        }

        public bool IsMarkerSelected()
        {
            if (SelectedModel == null)
                return false;
            return SelectedModel == marker;
        }

        public void RenderScene(Geom.IModelFactory models)
        {
            modelFactory = models;
            ModelGroups = modelFactory.GetModelGroups();
            if (modelFactory.GetModelGroups() != null && modelFactory.GetModelGroups().Any())
                currentVisuals = generateScene(modelFactory.GetModelGroups().First());
        }

        public void AddToScene(Geom.IModelFactory modelSupply, string modelGroup = null)
        {
            string groupToAdd = modelGroup;
            if (groupToAdd == null)
            {
                if (modelSupply.GetModelGroups() != null && modelSupply.GetModelGroups().Any())
                    groupToAdd = modelSupply.GetModelGroups().First();
                else
                {
                    SlrrLib.Model.MessageLog.AddError("No model groups present in ModelFactory");
                    return;
                }
            }
            var modelsFromFile = modelSupply.GenModels(groupToAdd);
            foreach (var model in modelsFromFile)
            {
                var mesh = (MeshGeometry3D)model.ModelGeom.Geometry;
                Vector3D currAvg = new Vector3D(mesh.Positions.Average(x => x.X), mesh.Positions.Average(x => x.Y), mesh.Positions.Average(x => x.Z));
                model.Translate = currAvg;
                AddModelToScene(model);
            }
        }

        public void ChangeModelGroup(string modelGroup)
        {
            currentVisuals = generateScene(modelGroup);
        }

        public void BringToFrontInDrawOrder(Geom.NamedModel model)
        {
            if (!(model.ModelGeom is GeometryModel3D))
                return;
            if (currentVisuals == null)
                return;
            var model3D = model.ModelGeom as GeometryModel3D;
            var group = currentVisuals.Content as Model3DGroup;
            if (group != null)
            {
                group.Children = new Model3DCollection(group.Children.OrderBy(x => x is GeometryModel3D && (x as GeometryModel3D) == model.ModelGeom ? 10 : 100));
            }
        }

        public void SetAllModelsToTexture(string texturePath, double frontMaterialOpacity, double backMaterialOpacity, Color backMaterialColor)
        {
            if (currentVisuals == null)
                return;
            if (!(currentVisuals.Content is Model3DGroup))
                return;
            ImageBrush imgBrush = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(texturePath, UriKind.Relative)),
                Opacity = frontMaterialOpacity,
                ViewportUnits = BrushMappingMode.Absolute
            };
            Brush b = new SolidColorBrush(backMaterialColor)
            {
                Opacity = backMaterialOpacity
            };
            DiffuseMaterial material = new DiffuseMaterial
            {
                Brush = imgBrush
            };
            DiffuseMaterial backMaterial = new DiffuseMaterial
            {
                Brush = b
            };
            foreach (var model in (currentVisuals.Content as Model3DGroup).Children)
            {
                if (!(model is GeometryModel3D))
                    continue;
                var model3D = model as GeometryModel3D;
                model3D.Material = material;
                model3D.BackMaterial = backMaterial;
            }
        }

        public void SetAllModelsOpacity(double frontMaterialOpacity, double backMaterialOpacity)
        {
            if (currentVisuals == null)
                return;
            if (!(currentVisuals.Content is Model3DGroup))
                return;
            foreach (var model in (currentVisuals.Content as Model3DGroup).Children)
            {
                if (!(model is GeometryModel3D))
                    continue;
                var model3D = model as GeometryModel3D;
                if (model3D.Material is DiffuseMaterial diffFront)
                {
                    var matClone = diffFront.Clone();
                    matClone.Brush.Opacity = frontMaterialOpacity;
                    model3D.Material = matClone;
                }
                if (model3D.BackMaterial is DiffuseMaterial diffBack)
                {
                    var matClone = diffBack.Clone();
                    matClone.Brush.Opacity = frontMaterialOpacity;
                    model3D.BackMaterial = matClone;
                }
            }
        }

        public void SetModelOpacity(Geom.NamedModel pivotModel, double frontMaterialOpacity, double backMaterialOpacity)
        {
            if (currentVisuals == null)
                return;
            if (!(currentVisuals.Content is Model3DGroup))
                return;
            var model3D = pivotModel.ModelGeom;
            if (model3D.Material is DiffuseMaterial diffFront)
            {
                var matClone = diffFront.Clone();
                matClone.Brush.Opacity = frontMaterialOpacity;
                model3D.Material = matClone;
            }
            if (model3D.BackMaterial is DiffuseMaterial diffBack)
            {
                var matClone = diffBack.Clone();
                matClone.Brush.Opacity = frontMaterialOpacity;
                model3D.BackMaterial = matClone;
            }
        }

        public void SetModelToTexture(Geom.NamedModel model, string texturePath, double frontMaterialOpacity, double backMaterialOpacity, Color backMaterialColor)
        {
            ImageBrush imgBrush = new ImageBrush();
            imgBrush.ImageSource = new BitmapImage(new Uri(texturePath, UriKind.Relative));
            imgBrush.Opacity = frontMaterialOpacity;
            imgBrush.ViewportUnits = BrushMappingMode.Absolute;
            Brush b = new SolidColorBrush(backMaterialColor);
            b.Opacity = backMaterialOpacity;
            DiffuseMaterial material = new DiffuseMaterial();
            material.Brush = imgBrush;
            DiffuseMaterial backMaterial = new DiffuseMaterial();
            backMaterial.Brush = b;
            var model3D = model.ModelGeom as GeometryModel3D;
            if (model3D != null)
            {
                model3D.Material = material;
                model3D.BackMaterial = backMaterial;
            }
            BringToFrontInDrawOrder(model);
        }

        public void SetModelToColor(Geom.NamedModel model, Color frontMaterialColor, double frontMaterialOpacity, double backMaterialOpacity, Color backMaterialColor)
        {
            Brush frontBrush = new SolidColorBrush(frontMaterialColor);
            frontBrush.Opacity = frontMaterialOpacity;
            Brush b = new SolidColorBrush(backMaterialColor);
            b.Opacity = backMaterialOpacity;
            DiffuseMaterial material = new DiffuseMaterial();
            material.Brush = frontBrush;
            DiffuseMaterial backMaterial = new DiffuseMaterial();
            backMaterial.Brush = b;
            var model3D = model.ModelGeom as GeometryModel3D;
            if (model3D != null)
            {
                model3D.Material = material;
                model3D.BackMaterial = backMaterial;
            }
            BringToFrontInDrawOrder(model);
        }

        public void ZoomCamera(double zoomChange)
        {
            viewPortCamera.ZoomCamera(zoomChange);
        }

        public void RotateCameraOrbit(double rotateChange)
        {
            viewPortCamera.OrbitWithUpVec(rotateChange);
        }

        public void RotateCameraYaw(double rotateChange)
        {
            viewPortCamera.Yaw(rotateChange);
        }

        public void LookAtPosition(Vector3D pos)
        {
            if (viewPortCamera != null)
            {
                if (marker != null)
                {
                    MoveModelToPosition(marker, pos);
                }
                else
                {
                    viewPortCamera.LookingAt = pos;
                }
            }
        }

        public void SelectMarker()
        {
            SelectModel(marker);
        }

        public void SelectModel(Geom.NamedModel model)
        {
            SelectedModel = model;
            UpdateTranslateOfModelFromPivot(SelectedModel as Geom.NamedCfgInducedModel);
            LookAtPosition(SelectedModel.Translate);
        }

        public void SetTransformOfModel(Geom.NamedModel model, Transform3DGroup transform)
        {
            model.ModelGeom.Transform = transform;
        }

        public void UpdateTranslateOfModelFromPivot(Geom.NamedCfgInducedModel model)
        {
            if (model != null && model.ModelGeom != null && model.BodyLine != null)
            {
                Transform3DGroup transform3DGroup = new Transform3DGroup();
                var newPos = Geom.NamedCfgInducedModel.GetTranslateVec3FromBodyLinePos(model.BodyLine);
                transform3DGroup.Children.Add(new TranslateTransform3D(newPos));
                model.ModelGeom.Transform = transform3DGroup;
                model.Translate = newPos;
            }
        }

        public void RecalculatePivot(Geom.NamedModel model)
        {
            var mesh = (MeshGeometry3D)model.ModelGeom.Geometry;
            model.Translate = new Vector3D(mesh.Positions.Average(x => x.X), mesh.Positions.Average(x => x.Y), mesh.Positions.Average(x => x.Z));
        }

        public void RemoveModelFromScene(Geom.NamedModel model)
        {
            if (currentVisuals == null)
                return;
            if (currentVisuals.Content is Model3DGroup group)
            {
                currentModels.Remove(model);
                group.Children.Remove(model.ModelGeom);
            }
        }

        public void RemoveAllModelsBy3DRepresentation(GeometryModel3D model)
        {
            var filteredModels = currentModels.Where(x => x.ModelGeom == model).ToList();
            foreach (var toRem in filteredModels)
            {
                RemoveModelFromScene(toRem);
            }
        }

        public void ReplaceAll3DRepresentations(GeometryModel3D modelFrom, GeometryModel3D modelTo)
        {
            if (currentVisuals == null)
                return;
            if (currentVisuals.Content is Model3DGroup group)
            {
                var filteredModels = currentModels.Where(x => x.ModelGeom == modelFrom).ToList();
                foreach (var toRem in filteredModels)
                {
                    group.Children.Remove(toRem.ModelGeom);
                    toRem.ModelGeom = modelTo.Clone();
                    group.Children.Add(toRem.ModelGeom);
                }
            }
        }

        public void Add3DRepresentationToScene(GeometryModel3D model, string name = "<unnamed>")
        {
            if (model == null)
                return;
            AddModelToScene(new Geom.NamedModel
            {
                Translate = new Vector3D(model.Bounds.X, model.Bounds.Y, model.Bounds.Z),
                ModelGeom = model,
                Name = name
            });
        }

        public void AddModelToScene(Geom.NamedModel model)
        {
            if (model == null)
                return;
            insertModelIntoScene(model, currentModels.Count);
        }

        public void ExtractUVsForPaintableModels()
        {
            foreach (var model in CurrentModels.OfType<Geom.NamedScxModel>())
            {
                if (model.HasPaintableTexture)
                    refreshUVsFromModel(model, model.PaintableTextureUVIndex + 1);//uv indices start from 1 in abstraction but 0 in binary representation
            }
        }

        public void SetTextureForPaintableModels(string texturePath, double frontMaterialOpacity, double backMaterialOpacity, Color backMaterialColor)
        {
            foreach (var pivotModel in CurrentModels.OfType<Geom.NamedScxModel>())
            {
                if (pivotModel.HasPaintableTexture)
                {
                    SetModelToTexture(pivotModel, texturePath, frontMaterialOpacity, backMaterialOpacity, backMaterialColor);
                }
            }
        }

        public void MoveModelToPosition(Geom.NamedCfgInducedModel model, Vector3D pos)
        {
            moveModelToPositionInternal(model, pos, false);
        }

        public void SetPolyTexturesFromRPK(Geom.MapRpkModelFactory overrideRPK = null)
        {
            var rpkModel = overrideRPK;
            if (rpkModel == null)
            {
                if (!(modelFactory is Geom.MapRpkModelFactory))
                    return;
                rpkModel = modelFactory as Geom.MapRpkModelFactory;
            }
            if (currentVisuals == null)
                return;
            if (!(currentVisuals.Content is Model3DGroup))
                return;

            foreach (var pivot in currentModels.OfType<Geom.NamedPolyModel>())
            {
                if (pivot.PolySource != null)
                {
                    Brush b = new SolidColorBrush(Colors.DarkCyan);
                    b.Opacity = 0.5;
                    DiffuseMaterial backMaterial = new DiffuseMaterial();
                    backMaterial.Brush = b;
                    try
                    {
                        var textInd = pivot.PolySource.Meshes.ElementAt(pivot.MeshIndex).TextureIndex;
                        string textureFnam = System.IO.Path.GetFullPath(rpkModel.ResolveMapTextureIndex(textInd)).ToLower();
                        if (!imageCahce.ContainsKey(textureFnam))
                        {
                            //var bip = FreeImageAPI.FreeImage.LoadEx(textureFnam);
                            //var nonTrans = FreeImageAPI.FreeImage.ConvertTo24Bits(bip);
                            //imageCahce[textureFnam] = imageSourceForBitmap(FreeImageAPI.FreeImage.GetBitmap(nonTrans));
                        }
                        ImageBrush imgBrush = new ImageBrush();
                        imgBrush.ImageSource = imageCahce[textureFnam];
                        imgBrush.Opacity = 1;
                        imgBrush.Stretch = Stretch.Fill;
                        imgBrush.TileMode = TileMode.Tile;
                        imgBrush.ViewboxUnits = BrushMappingMode.RelativeToBoundingBox;
                        imgBrush.Viewbox = new Rect(0, 0, 1, 1);
                        imgBrush.Viewport = new Rect(0, 0, 1, 1);
                        imgBrush.ViewportUnits = BrushMappingMode.Absolute;
                        DiffuseMaterial material = new DiffuseMaterial();
                        material.Brush = imgBrush;//imgBrush;
                        Brush b2 = new SolidColorBrush(Colors.DarkKhaki);
                        b2.Opacity = 0.5;
                        DiffuseMaterial backMaterial2 = new DiffuseMaterial();
                        backMaterial2.Brush = b2;
                        pivot.ModelGeom.Material = material;
                        pivot.ModelGeom.BackMaterial = backMaterial;
                    }
                    catch (Exception)
                    {
                        Brush m = new SolidColorBrush(Colors.Gray);
                        DiffuseMaterial material = new DiffuseMaterial();
                        material.Brush = m;
                        pivot.ModelGeom.Material = material;
                        pivot.ModelGeom.BackMaterial = backMaterial;
                        Model.MessageLog.AddError("Error resolving POLY entry texture: " + pivot.Name);
                    }
                }
            }
        }

        public void UpdateVisibleObjectsViewDistance()
        {
            if (viewPortCamera == null)
                return;
            var curPos = viewPortCamera.LookingAt;
            if ((curPos - objectVisualsLastLoadPosition).Length < ObjectViewDistance / 2.0f)
                return;
            var visibilityColl = (currentVisuals.Content as Model3DGroup);
            var newObjectVisuals = filterObjectsForViewDistance(curPos, ObjectViewDistance);
            foreach (var geom in currentModels.Except(newObjectVisuals))
                visibilityColl.Children.Remove(geom.ModelGeom);
            foreach (var geom in newObjectVisuals.Except(currentModels))
                visibilityColl.Children.Add(geom.ModelGeom);
            objectVisualsLastLoadPosition = curPos;
            currentVisibleModels = newObjectVisuals;
        }

        public event EventHandler SelectedModelMoved;

        protected virtual void OnSelectedModelMoved(EventArgs e)
        {
            var handler = SelectedModelMoved;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler MarkerMoved;

        protected virtual void OnMarkerMoved(EventArgs e)
        {
            var handler = MarkerMoved;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler MarkerMovedByUser;

        protected virtual void OnMarkerMovedByUser(EventArgs e)
        {
            var handler = MarkerMovedByUser;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool deleteObject([In] IntPtr hObject);

        private void moveModelToPositionInternal(Geom.NamedCfgInducedModel model, Vector3D pos, bool userInitiated)
        {
            if (model != null && model.BodyLine != null)
            {
                model.BodyLine.LineX = (float)pos.X / 100.0f;
                model.BodyLine.LineY = (float)pos.Y / 100.0f;
                model.BodyLine.LineZ = (float)pos.Z / 100.0f;
                UpdateTranslateOfModelFromPivot(model);
                if (SelectedModel == model)
                    OnSelectedModelMoved(new EventArgs());
                if (marker == model)
                {
                    viewPortCamera.LookingAt = pos;
                    OnMarkerMoved(new EventArgs());
                    if (userInitiated)
                        OnMarkerMovedByUser(new EventArgs());
                }
            }
        }

        private ImageSource imageSourceForBitmap(System.Drawing.Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                deleteObject(handle);
            }
        }

        private void insertModelIntoScene(Geom.NamedModel model, int index)
        {
            if (!(model.ModelGeom is GeometryModel3D))
                return;
            if (currentVisuals == null)
                return;
            if (currentVisuals.Content is Model3DGroup group)
            {
                currentModels.Insert(Math.Min(index, currentModels.Count), model);
                group.Children.Insert(Math.Min(index, group.Children.Count), model.ModelGeom);
            }
        }

        private ModelVisual3D generateScene(string modelGroup)
        {
            Vector3D avgPos = new Vector3D(0, 0, 0);
            var modelsFromFile = modelFactory.GenModels(modelGroup);
            currentModels.Clear();
            Model3DGroup group = new Model3DGroup();
            foreach (var model in modelsFromFile)
            {
                var mesh = (MeshGeometry3D)model.ModelGeom.Geometry;
                Vector3D currAvg = new Vector3D(mesh.Positions.Average(x => x.X), mesh.Positions.Average(x => x.Y), mesh.Positions.Average(x => x.Z));
                avgPos += currAvg;
                model.Translate = currAvg;
                currentModels.Add(model);
                group.Children.Add(model.ModelGeom);
            }
            avgPos /= (double)modelsFromFile.Count();

            foreach (var light in lights)
                group.Children.Add(light);

            ModelVisual3D visuals = new ModelVisual3D
            {
                Content = group
            };

            viewPortCamera = new Geom.OrbitingCamera(avgPos, 500);
            ctrlViewPort3D.Camera = viewPortCamera.Camera;

            ctrlViewPort3D.Children.Clear();
            ctrlViewPort3D.Children.Add(visuals);

            return visuals;
        }

        private void refreshUVsFromModel(Geom.NamedScxModel model, int uvindex)
        {
            var model3D = model.ModelGeom;
            var meshGeom = model3D.Geometry as MeshGeometry3D;
            if (model.Scxv3Source != null)
            {
                var meshData = model.Scxv3Source.Meshes[model.MeshIndex];
                if (uvindex == 3 && meshData.VertexDatas.All(x => x.IsUVChannel3XDefined && x.IsUVChannel3YDefined))
                    meshGeom.TextureCoordinates = new PointCollection(meshData.VertexDatas.Select(x => new Point(x.UVChannel3X, x.UVChannel3Y)));
                else if (uvindex == 2 && meshData.VertexDatas.All(x => x.IsUVChannel2XDefined && x.IsUVChannel2YDefined))
                    meshGeom.TextureCoordinates = new PointCollection(meshData.VertexDatas.Select(x => new Point(x.UVChannel2X, x.UVChannel2Y)));
                else if (uvindex == 1 && meshData.VertexDatas.All(x => x.IsUVChannel1XDefined && x.IsUVChannel1YDefined))
                    meshGeom.TextureCoordinates = new PointCollection(meshData.VertexDatas.Select(x => new Point(x.UVChannel1X, x.UVChannel1Y)));
                else
                    meshGeom.TextureCoordinates = new PointCollection(meshData.VertexDatas.Select(x => new Point(0, 0)));
            }
            else if (model.Scxv4Source != null)
            {
                var meshData = model.Scxv4Source.Meshes[model.MeshIndex].VertexData;
                if (uvindex == 3 && meshData.IsUV3Defined)
                    meshGeom.TextureCoordinates = new PointCollection(meshData.VertexDataList.Select(x => new Point(x.Uv3.U, x.Uv3.V)));
                else if (uvindex == 2 && meshData.IsUV2Defined)
                    meshGeom.TextureCoordinates = new PointCollection(meshData.VertexDataList.Select(x => new Point(x.Uv2.U, x.Uv2.V)));
                else if (uvindex == 1 && meshData.IsUV1Defined)
                    meshGeom.TextureCoordinates = new PointCollection(meshData.VertexDataList.Select(x => new Point(x.Uv1.U, x.Uv1.V)));
                else
                    meshGeom.TextureCoordinates = new PointCollection(meshData.VertexDataList.Select(x => new Point(0, 0)));
            }
        }

        private Vector3D getVec3(Point3D p)
        {
            return new Vector3D(p.X, p.Y, p.Z);
        }

        private double max3(Size3D s)
        {
            return Math.Max(Math.Max(s.X, s.Y), s.Z);
        }

        private List<Geom.NamedModel> filterObjectsForViewDistance(Vector3D p, float distance)
        {
            List<Geom.NamedModel> ret = new List<Geom.NamedModel>();
            foreach (var entry in currentModels)
            {
                if ((getVec3(entry.ModelGeom.Bounds.Location) - p).Length - max3(entry.ModelGeom.Bounds.Size) < distance)
                    ret.Add(entry);
            }
            return ret;
        }

        private void ctrlOrbitingViewport3D_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (viewPortCamera == null)
                return;
            double zoomChange = e.Delta / 10.0;
            ZoomCamera(zoomChange);
        }

        private void ctrlOrbitingViewport3D_MouseLeave(object sender, MouseEventArgs e)
        {
            animScene = false;
        }

        private void ctrlOrbitingViewport3D_MouseDown(object sender, MouseButtonEventArgs e)
        {
            middleMouseDown = e.ChangedButton == MouseButton.Middle;
            dragStart = e.MouseDevice.GetPosition(ctrlViewPort3D);
            animScene = true;
        }

        private void ctrlOrbitingViewport3D_MouseUp(object sender, MouseButtonEventArgs e)
        {
            animScene = false;
        }

        private void ctrlOrbitingViewport3D_MouseMove(object sender, MouseEventArgs e)
        {
            if (viewPortCamera == null)
                return;
            e.Handled = true;
            if (animScene && !middleMouseDown)
            {
                var pos = e.MouseDevice.GetPosition(ctrlViewPort3D);
                pos.Y -= dragStart.Y;
                pos.X -= dragStart.X;
                RotateCameraOrbit(-pos.X / 5.0);
                RotateCameraYaw(-pos.Y / 5.0);
                dragStart = e.MouseDevice.GetPosition(ctrlViewPort3D);
            }
            else if (animScene)
            {
                var pos = e.MouseDevice.GetPosition(ctrlViewPort3D);
                pos.Y -= dragStart.Y;
                pos.X -= dragStart.X;
                pos.X /= 3.0;
                pos.Y /= 3.0;
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    pos.X /= 7.0;
                    pos.Y /= 7.0;
                }

                var moveVecX = (Vector3D)viewPortCamera.Camera.Position - viewPortCamera.LookingAt;
                moveVecX.Y = 0;
                moveVecX.Normalize();
                var moveVecY = Vector3D.CrossProduct(moveVecX, viewPortCamera.Camera.UpDirection);
                moveVecX *= pos.Y;
                moveVecY *= -pos.X;
                if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    moveVecX = new Vector3D(0, 0, 0);
                    moveVecY = new Vector3D(0, Math.Abs(pos.Y) > Math.Abs(pos.X) ? -pos.Y : -pos.X, 0);
                }
                var moveVec = moveVecX + moveVecY;

                var campos = viewPortCamera.Camera.Position;
                viewPortCamera.Camera.Position = new Point3D(campos.X + moveVec.X, campos.Y + moveVec.Y, campos.Z + moveVec.Z);
                var newLookAtPos = new Vector3D(viewPortCamera.LookingAt.X + moveVec.X, viewPortCamera.LookingAt.Y + moveVec.Y, viewPortCamera.LookingAt.Z + moveVec.Z);
                if (SelectedModel != null && SelectedModel is Geom.NamedCfgInducedModel)
                {
                    MoveModelToPosition(SelectedModel as Geom.NamedCfgInducedModel, newLookAtPos);
                }
                if (marker != null)
                {
                    moveModelToPositionInternal(marker, newLookAtPos, true);
                }
                else
                {
                    viewPortCamera.LookingAt = newLookAtPos;
                }
                if (IsViewDistanceManaged)
                    UpdateVisibleObjectsViewDistance();
                dragStart = e.MouseDevice.GetPosition(ctrlViewPort3D);
            }
        }
    }
}