namespace SlrrLib.View
{
    public class MapRpkOrbitinViewPort3D : OrbitingViewport3D
    {
        private string rpkFnam = "";

        public string LastRpkDirectory
        {
            get;
            set;
        } = System.IO.Directory.GetCurrentDirectory();

        public string LastRpkOpened
        {
            get
            {
                return rpkFnam;
            }
        }

        public string GetRpkFileNameFromUser()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (System.IO.Directory.Exists(LastRpkDirectory))
                dlg.InitialDirectory = LastRpkDirectory;
            else
                dlg.InitialDirectory = System.IO.Directory.GetCurrentDirectory();
            dlg.FileName = "";
            dlg.DefaultExt = ".rpk";
            dlg.Filter = "RPKs|*.rpk";
            var result = dlg.ShowDialog();
            rpkFnam = "";
            if (result == true)
            {
                LastRpkDirectory = System.IO.Path.GetDirectoryName(dlg.FileName);
                System.IO.File.WriteAllText("lastDir", LastRpkDirectory);
                return dlg.FileName;
            }
            return null;
        }

        public SlrrLib.Geom.MapRpkModelFactory LoadSceneRpk(bool addMarker = true)
        {
            var rpk = GetRpkFileNameFromUser();
            if (rpk != null)
                rpkFnam = rpk;
            else
                return null;
            var currentFactory = new SlrrLib.Geom.MapRpkModelFactory(rpkFnam, false, true);
            if (currentFactory == null)
                return null;
            RenderScene(currentFactory);
            if (addMarker)
                AddMarker();
            SetPolyTexturesFromRPK();
            return currentFactory;
        }

        public SlrrLib.Geom.MapRpkModelFactory LoadSecondaryRpk()
        {
            var rpk = GetRpkFileNameFromUser();
            if (rpk != null)
                rpkFnam = rpk;
            else
                return null;
            var tempFactory = new SlrrLib.Geom.MapRpkModelFactory(rpkFnam, false, true);
            if (tempFactory == null)
                return null;
            AddToScene(tempFactory);
            return tempFactory;
        }
    }
}