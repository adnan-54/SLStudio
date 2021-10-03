using System.Windows;

namespace SlrrLib.View
{
    public partial class BoundBox3DDialog : Window
    {
        public BoundBox3DDialog(float defMaxX = 0, float defMaxY = 0, float defMaxZ = 0,
                                float defMinX = 0, float defMinY = 0, float defMinZ = 0)
        {
            InitializeComponent();
            setMaxX(defMaxX);
            setMaxY(defMaxY);
            setMaxZ(defMaxZ);
            setMinX(defMinX);
            setMinY(defMinY);
            setMinZ(defMinZ);
        }

        public SlrrLib.Geom.BoundBox3D Bounds
        {
            get
            {
                return new SlrrLib.Geom.BoundBox3D(getMaxX(), getMaxY(), getMaxZ(), getMinX(), getMinY(), getMinZ());
            }
        }

        private void setMinX(float val)
        {
            ctrlTextBoxMinX.Text = UIUtil.FloatToString(val);
        }

        private void setMinY(float val)
        {
            ctrlTextBoxMinY.Text = UIUtil.FloatToString(val);
        }

        private void setMinZ(float val)
        {
            ctrlTextBoxMinZ.Text = UIUtil.FloatToString(val);
        }

        private void setMaxX(float val)
        {
            ctrlTextBoxMaxX.Text = UIUtil.FloatToString(val);
        }

        private void setMaxY(float val)
        {
            ctrlTextBoxMaxY.Text = UIUtil.FloatToString(val);
        }

        private void setMaxZ(float val)
        {
            ctrlTextBoxMaxZ.Text = UIUtil.FloatToString(val);
        }

        private float getMinX()
        {
            return UIUtil.ParseOrZero(ctrlTextBoxMinX.Text);
        }

        private float getMinY()
        {
            return UIUtil.ParseOrZero(ctrlTextBoxMinY.Text);
        }

        private float getMinZ()
        {
            return UIUtil.ParseOrZero(ctrlTextBoxMinZ.Text);
        }

        private float getMaxX()
        {
            return UIUtil.ParseOrZero(ctrlTextBoxMaxX.Text);
        }

        private float getMaxY()
        {
            return UIUtil.ParseOrZero(ctrlTextBoxMaxY.Text);
        }

        private float getMaxZ()
        {
            return UIUtil.ParseOrZero(ctrlTextBoxMaxZ.Text);
        }

        private void ctrlButtonOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void ctrlButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}