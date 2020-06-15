using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
    public class OrbitingCamera
    {
        private PerspectiveCamera managedCamera;
        private Vector3D cameraScreenUp;
        private double fov = 90;
        private Vector3D lookingat;

        public PerspectiveCamera Camera
        {
            get
            {
                return managedCamera;
            }
        }

        public Vector3D LookingAt
        {
            get
            {
                return lookingat;
            }
            set
            {
                lookingat = value;
                Vector3D targetTocamera = (Vector3D)managedCamera.Position - lookingat;
                //managedCamera.Position = (Point3D)(-(lookingat - targetTocamera));
                managedCamera.LookDirection = -targetTocamera;
                managedCamera.LookDirection.Normalize();
                managedCamera.FieldOfView = fov;
                managedCamera.UpDirection = new Vector3D(0, 1, 0);
                cameraScreenUp = Vector3D.CrossProduct(managedCamera.LookDirection, new Vector3D(0, 1, 0));//if the target is totally under the camera this breaks
                cameraScreenUp.Normalize();
                cameraScreenUp = Vector3D.CrossProduct(managedCamera.LookDirection, cameraScreenUp);
                cameraScreenUp.Normalize();//should be unnecessary
            }
        }

        public OrbitingCamera(Vector3D lookAt, double fromDistance)
        {
            lookingat = lookAt;
            managedCamera = new PerspectiveCamera();
            Vector3D targetTocamera = new Vector3D(fromDistance, 0, 0);
            managedCamera.Position = (Point3D)(lookAt - targetTocamera);
            managedCamera.LookDirection = targetTocamera;
            managedCamera.LookDirection.Normalize();
            managedCamera.FieldOfView = fov;
            cameraScreenUp = Vector3D.CrossProduct(managedCamera.LookDirection, managedCamera.UpDirection);//if the target is totally under the camera this breaks
            cameraScreenUp.Normalize();
            cameraScreenUp = Vector3D.CrossProduct(managedCamera.LookDirection, cameraScreenUp);
            cameraScreenUp.Normalize();//should be unnecessary
        }

        public void Yaw(double rotateChange)
        {
            Vector3D axis = Vector3D.CrossProduct(cameraScreenUp, managedCamera.LookDirection);
            Vector3D posRotate = (Vector3D)(managedCamera.Position - lookingat);
            RotateTransform3D rot = new RotateTransform3D(new AxisAngleRotation3D(axis, rotateChange));
            posRotate = rot.Transform(posRotate);
            managedCamera.Position = (Point3D)(posRotate + lookingat);
            managedCamera.LookDirection = rot.Transform(managedCamera.LookDirection);
            cameraScreenUp = rot.Transform(cameraScreenUp);
            managedCamera.UpDirection = -cameraScreenUp;
        }

        public void OrbitWithUpVec(double rotateChange)
        {
            Vector3D axis = Vector3D.CrossProduct(cameraScreenUp, managedCamera.LookDirection);
            Vector3D posRotate = (Vector3D)(managedCamera.Position - lookingat);
            RotateTransform3D rot = new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), rotateChange));
            posRotate = rot.Transform(posRotate);
            managedCamera.Position = (Point3D)(posRotate + lookingat);
            managedCamera.LookDirection = rot.Transform(managedCamera.LookDirection);
            cameraScreenUp = rot.Transform(cameraScreenUp);
        }

        public void ZoomCamera(double zoomChange)
        {
            Vector3D curCameraPos = (Vector3D)managedCamera.Position - lookingat;
            double dist = curCameraPos.Length;
            curCameraPos.Normalize();
            dist -= zoomChange;
            if (dist < 5)
                return;
            curCameraPos = curCameraPos * dist;
            managedCamera.Position = (Point3D)(curCameraPos + lookingat);
        }
    }
}