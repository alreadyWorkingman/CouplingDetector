using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
   public class Wagon
    {

      public Wagon(DateTime _firstTimeDetected, DateTime _lastTimeDetected, int id, Direction dir) 
        {
            firstTimeDetected = _firstTimeDetected;
            lastTimeDetected = _lastTimeDetected;
            ID = id;
            direction = dir;
        }
     public   DateTime firstTimeDetected { get;private set; }
        public DateTime lastTimeDetected { get;private set; }
        public int ID { get;private set; }
      public   Direction direction { get; private set; }
     
    }
 
}
