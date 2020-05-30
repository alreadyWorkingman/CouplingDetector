using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
namespace Core
{
    public class ApplicationManager
    {
        
        VideoParser videoParser;
        Drawing drawing;
        Detector detector;
        Former former;
        Logger logger;
        bool workWithoutVideo;
        private int _thickness;
        private System.Drawing.Color _color;
        private string _videoPath;
        private string _logPath;
        private WriteableBitmap _imgSourse;

        public  void AppStart()
        {
            former = new Former(300);
            detector = new Detector();
            
        }
       public void AppStop() { }

        public System.Drawing.Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                drawing.SetColor(value);
            }
        }
        public int Thickness
        {
            get { return _thickness; }
            set
            {
                _thickness = value;
                drawing.SetThickness(value);
            }
        }
        public string VideoPath
        {
            get { return _videoPath; }
            set {
                _videoPath = value;
                videoParser.SetVideoPath(value);
            }
        }
        public string logPath
        {
            get { return _logPath; }
            set
			{
                this._logPath = value;
                logger.SetLogPath(value);
            }
        }
        public WriteableBitmap imgSourse
        {
            get { return _imgSourse; }
            set { _imgSourse = value;
                videoParser.SetImgSourse(value);
            }
        }
        public void Subscribe(EventHandler<Wagon> eventHandler)
        {
            former.Subscribe(eventHandler);
        }

        private void ThreadDetect()
		{

		}
    }
}
