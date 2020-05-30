using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Drawing;
using OpenCvSharp;
using Point = OpenCvSharp.Point;
using Alturos.Yolo;
using Alturos.Yolo.Model;
using Accord.Imaging;
using Accord.Imaging.Filters;
using System.Threading.Tasks;

namespace Core
{
	internal class Detector
	{
		Coupling lastCoupling;
		YoloWrapper yolo;
		Mat frameBuf;
        ContrastStretch contrast = new ContrastStretch(); 
		CLAHE clahe;
        Bitmap tmp;
		bool imEating = false; //@
		private Task task = null;
        
		//Former former; ##пока просто закомменчу, вдруг всё же понадобится
		public Detector()
		{
			clahe.ClipLimit = 20;
			clahe = Cv2.CreateCLAHE();
			//this.former = former;
            task = null;
		}
		private Coupling Filter(IEnumerable<YoloItem> list)
		{
			//Место под тёмную магию фильтрования
			//Пока реализую с надеждой на то, что в коллекции одна единственная верная сцепка 

			YoloItem tmp = list.FirstOrDefault();
			Coupling c = new Coupling(DateTime.Now, new System.Drawing.Rectangle(tmp.X, tmp.Y, tmp.Width, tmp.Height));
			return c;
		}

		internal async Task Analysis(Mat Frame, CancellationToken token)
		{
			if (token.IsCancellationRequested)
			{
				return;
			}

			frameBuf = frameBuf.CvtColor(ColorConversionCodes.BGR2GRAY);

            tmp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frameBuf); //АККУРАТНО, ЕСТЬ ПОДОЗРЕНИЯ ЧТО МОЖЕТ НЕ РАБОТАТЬ !TODO!
            contrast.ApplyInPlace(tmp);
            frameBuf = OpenCvSharp.Extensions.BitmapConverter.ToMat(tmp);//АККУРАТНО, ЕСТЬ ПОДОЗРЕНИЯ ЧТО МОЖЕТ НЕ РАБОТАТЬ !TODO!
            clahe.Apply(frameBuf, frameBuf);

			OpenCvSharp.Size size = new OpenCvSharp.Size(416, 416);
			Cv2.Resize(frameBuf, frameBuf, size);

			var bytes = frameBuf.ToBytes(".bmp");

			if (task == null)
			{
				task = new Task( async() =>
				{
					var items = yolo.Detect(bytes);
					lastCoupling = Filter(items);
				});
				task.Start();
				await task;
			}


		
		}

	}
}