using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;



namespace Core
{
   public enum Direction
    {
        Right,
        Left
    }
    internal class Coupling
    {

        internal Coupling(DateTime _detectedTime/*, DateTime _lastTimeDetected*/, Rectangle loc/*, Diretion dir*/)
        {
            detectedTime = _detectedTime;
            //lastTimeDetected = _lastTimeDetected;
            location = loc;
            //direction = dir;
        }
        //internal DateTime firstTimeDetected { get; set; }
        internal DateTime detectedTime { get;private set; }
        internal Rectangle location { get;private set; }


       // Diretion direction { get; set; }
    }
}
