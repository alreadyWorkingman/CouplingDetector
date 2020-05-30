using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Windows.Media;
using OpenCvSharp;
using System.Drawing;

namespace Core
{
    internal class Drawing
    {
        private int thickness;
        private System.Drawing.Color color;
        private WriteableBitmap imageSource;
        internal void SetColor(System.Drawing.Color color)
        {
            this.color = color;
        }
        internal void SetThickness(int thickness)
        {
            this.thickness = thickness;
        }
        internal void SetImageSource(WriteableBitmap imageSource)
        {
            this.imageSource = imageSource;
        }
        internal void DrawRect(Mat frame, Rectangle loc)
        {
			// Преобразовать полученный параметр в Rect
			Rect location = new Rect(loc.X, loc.Y, loc.Width, loc.Height);
            Cv2.Rectangle(frame, location, new Scalar(color.B,color.G,color.R), thickness);
            imageSource.Lock();
            imageSource.WritePixels(new System.Windows.Int32Rect(0, 0, frame.Width, frame.Height), frame.Data, (int)(frame.DataEnd.ToInt64() - frame.Data.ToInt64()), (int)frame.Step());
            imageSource.Unlock();
        }
    }
}
