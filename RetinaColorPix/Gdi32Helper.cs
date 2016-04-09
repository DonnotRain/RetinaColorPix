using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RetinaColorPix
{
    public class Gdi32Helper
    {
        [DllImport("gdi32")]
        public static extern IntPtr CreateDC(
        string lpszDriver, // 驱动名称
        string lpszDevice, // 设备名称
        string lpszOutput, // 无用，可以设定位"NULL"
        IntPtr lpInitData // 任意的打印机数据
        );
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(
        IntPtr hdcDest, // 目标设备的句柄
        int nXDest, // 目标对象的左上角的X坐标
        int nYDest, // 目标对象的左上角的X坐标
        int nWidth, // 目标对象的矩形的宽度
        int nHeight, // 目标对象的矩形的长度
        IntPtr hdcSrc, // 源设备的句柄
        int nXSrc, // 源对象的左上角的X坐标
        int nYSrc, // 源对象的左上角的X坐标
        int dwRop // 光栅的操作值
        );
        /// <summary>   
        /// 设置鼠标的坐标   
        /// </summary>   
        /// <param name="x">横坐标</param>   
        /// <param name="y">纵坐标</param>          
        [DllImport("User32")]
        public extern static void SetCursorPos(int x, int y);
        public struct POINT
        {
            public int X;
            public int Y;
            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }
        }
        /// <summary>   
        /// 获取鼠标的坐标   
        /// </summary>   
        /// <param name="lpPoint">传址参数，坐标point类型</param>   
        /// <returns>获取成功返回真</returns>   
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);
    }
}
