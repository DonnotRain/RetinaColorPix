using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RetinaColorPix
{
    /// <summary>
    /// CaptureWin.xaml 的交互逻辑
    /// </summary>
    public partial class CaptureWin : Window
    {
        public event Action<System.Drawing.Color, Gdi32Helper.POINT> ColorGatched;
        private DispatcherTimer _tm;


        //系统显示缩放值
        private float _scaleX;
        private float _scaleY;

        public CaptureWin()
        {

            InitializeComponent();
            this.Opacity = 0.002;
            this.MouseLeftButtonDown += CaptureWin_MouseLeftButtonDown;

            //获取系统显示dpi
            using (ManagementClass mc = new ManagementClass("Win32_DesktopMonitor"))
            {
                using (ManagementObjectCollection moc = mc.GetInstances())
                {

                    int pixelsPerXLogicalInch = 0; // dpi for x
                    int pixelsPerYLogicalInch = 0; // dpi for y

                    foreach (var o in moc)
                    {
                        var each = (ManagementObject)o;
                        pixelsPerXLogicalInch = int.Parse((each.Properties["PixelsPerXLogicalInch"].Value.ToString()));
                        pixelsPerYLogicalInch = int.Parse((each.Properties["PixelsPerYLogicalInch"].Value.ToString()));
                    }

                    //96为缩放100%时dpi
                    _scaleX = pixelsPerXLogicalInch / 96f;
                    _scaleY = pixelsPerYLogicalInch / 96f;
                }
            }

            this.Loaded += CaptureWin_Loaded;
        }

        private void CaptureWin_Loaded(object sender, RoutedEventArgs e)
        {
            _tm = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(100)};
            _tm.Tick += _tm_Tick;
            _tm.Start();
        }


        private void _tm_Tick(object sender, EventArgs e)
        {
            _tm.Stop();
            IntPtr hdlDisplay = Gdi32Helper.CreateDC("display", null, null, IntPtr.Zero);

            Graphics g = Graphics.FromHdc(hdlDisplay);

            Bitmap bmp = new Bitmap(1, 1, g);

            Graphics gimg = Graphics.FromImage(bmp);

            IntPtr hdlScreen = g.GetHdc();
            IntPtr hdlBmp = gimg.GetHdc();

            Gdi32Helper.POINT p = new Gdi32Helper.POINT();

            var getMousePositionResult = Gdi32Helper.GetCursorPos(out p);

            if (!getMousePositionResult)
            {
                return;
            }

            var xPosition = p.X * (int)_scaleX;
            var yPosition = p.Y * (int)_scaleY;
            Gdi32Helper.BitBlt(hdlBmp, 0, 0, 1, 1, hdlScreen, xPosition, yPosition, 13369376);

            g.ReleaseHdc(hdlScreen);
            gimg.ReleaseHdc(hdlBmp);

            var currentColor = bmp.GetPixel(0, 0);

            if (ColorGatched != null) ColorGatched(currentColor, p);


            g.Dispose();
            gimg.Dispose();

            bmp.Dispose(); // 释放 bmp 所使用的资源

            _tm.Start();
        }

        private void CaptureWin_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _tm.IsEnabled = false;

            _tm.Stop();
            _tm = null;
            this.Close();
        }
    }
}
