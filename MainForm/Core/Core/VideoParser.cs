using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenCvSharp;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Core
{
    internal class VideoParser
    {
        private WriteableBitmap imageSource;
        private string videoPath;
        private VideoCapture stream;
        private Mat frame;
        private Detector detector;

		public event EventHandler<Mat> FrameParsed;

		internal VideoParser(string path, Detector detector)
        {
            stream = VideoCapture.FromFile(videoPath);
            this.detector = detector;
        }

        public void SetVideoPath(string path)
        {
            videoPath = path;
        }

        //public static BitmapImage ToImage(Mat mat)
        //{

        //    BitmapImage image = new BitmapImage();

        //    using (MemoryStream mem = new MemoryStream(mat.ToBytes()))
        //    {
        //        mat.Dispose();
        //        image.BeginInit();
        //        mem.Position = 0;
        //        image.CacheOption = BitmapCacheOption.OnLoad;
        //        image.StreamSource = mem;
        //        image.EndInit();
        //    }

        //    image.Freeze();
        //    return image;
        //}
        internal void SetImgSourse(WriteableBitmap wBitmap)
        {
            imageSource = wBitmap;   
        }
        internal static void WriteMat(WriteableBitmap img, Mat mat)
        {
            img.Lock();
            img.WritePixels(new Int32Rect(0, 0, mat.Width, mat.Height), mat.Data, (int)(mat.DataEnd.ToInt64() - mat.Data.ToInt64()), (int)mat.Step());
            img.Unlock();
        }

        internal Task ParseVideo(CancellationToken token, bool noDraw = false)
        {
        /*    var image = App.Container.GetInstance<Image>(); *///App - форма, точнее файл с логикой для неё, туда необходимо 
            //кое - что добавить для работы с парсером
            Timer timer = null;
            double fps;
            try { fps = stream.Fps; }
            catch { fps = 25; }

            return Task.Run(() =>
            {
                Mat tmp = new Mat();
                stream.Read(tmp);

                int w = stream.FrameWidth;
                int h = stream.FrameHeight;


                WriteableBitmap img = new WriteableBitmap(w, h, 96, 96, PixelFormats.Bgr24, null);
                //System.Net.Mime.MediaTypeNames.Application.Current?.Dispatcher?.BeginInvoke(new Action(() => //нужна форма
                //{
                //    image.Source = img;
                //}));
                
                timer = new Timer(x =>
                {
                    if (!stream.Read(tmp) || token.IsCancellationRequested || tmp.Empty())
                    {
                        timer?.Dispose();
                        Task.Delay(100);
                        tmp.Dispose();
                        stream.Dispose();
                        return;
                    }

                    Dispatcher.CurrentDispatcher?.BeginInvoke(new Action(() => //та же фигня, что и выше
                    {
                        WriteMat(img, frame);
                    }));

                    stream.Read(tmp);                  
                    
                    frame = tmp;

                    WriteMat(imageSource,frame);
                    detector.Analysis(frame, token);
                    tmp.Dispose();

                }, null, 0, (int)(1000 / fps));
           });
        }


        ~VideoParser()
        {
            frame.Dispose();
            stream.Dispose();
        }

        
    }
}
