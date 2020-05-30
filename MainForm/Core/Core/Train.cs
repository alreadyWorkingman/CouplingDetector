using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
     internal class Train
    {
      internal  List<Wagon> WagonList;
      internal   Train()
        {
            WagonList = new List<Wagon>();
        }
        internal Train(List<Wagon> wL)
        {
            WagonList = wL;
        }
        internal void AddWagon(Wagon wagon)
        { 
            WagonList.Add(wagon);
        }
        internal void Dispose()
        {
            WagonList.Clear();
        }

    }
}
