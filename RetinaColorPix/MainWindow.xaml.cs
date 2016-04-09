using System;
using System.Drawing;
using System.Management;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace RetinaColorPix
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {


        }

        private void BtnPick_Click(object sender, RoutedEventArgs e)
        {
            CaptureWin captureWin = new CaptureWin();
            captureWin.ColorGatched += CaptureWin_ColorGatched;
            captureWin.Topmost = true;
            captureWin.Show();
        }

        private void CaptureWin_ColorGatched(System.Drawing.Color currentColor, Gdi32Helper.POINT point)
        {
            TblMouse.Text = point.X.ToString() + "," + point.Y.ToString();
            var selectedColor = System.Windows.Media.Color.FromArgb(currentColor.A, currentColor.R, currentColor.G, currentColor.B);
            bordCurrentColor.Background = new SolidColorBrush();
            TxtARGB.Text = "0x" + currentColor.ToArgb().ToString("x").ToUpper();
            TxtRGB.Text = currentColor.R.ToString().ToLower() + "," + currentColor.G.ToString().ToLower() + "," + currentColor.B.ToString().ToLower();
            TxtHex.Text = "#" + currentColor.R.ToString("x").PadLeft(2, '0') + currentColor.G.ToString("x").PadLeft(2, '0') + currentColor.B.ToString("x").PadLeft(2, '0');

            change.SelectedColor = selectedColor;

        }

        private void change_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<System.Windows.Media.Color> e)
        {
            //if (this.IsLoaded)
            //{
            //    CaptureWin_ColorGatched(e.NewValue)
            //}
        }
    }
}
