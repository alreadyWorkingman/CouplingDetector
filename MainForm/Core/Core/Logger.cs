using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    class Logger
    {
        string filePath =""; //Добавил по умолчанию
        //List<string> logFileLines; //@ Нет нужды. Использую словарь ибо может придти отрицательное значение.
        Dictionary<int, string> wagonDict = new Dictionary<int, string>();

        public Logger(){}
        public Logger(string path) => filePath = path;

        //internal string LogPath { get; set; } Свойством?
        internal void SetLogPath(string path) => filePath = path;

        internal void WriteLog(Train train) //@Сделал VOID
        {
            foreach (Wagon wagon in train.WagonList)
            {
                string note = wagon.firstTimeDetected.ToString("hh:mm:ss.dd.MM.yyyy") + '-'
                    + wagon.lastTimeDetected.ToString("hh:mm:ss.dd.MM.yyyy") + $", {DirectionToStr(wagon.direction)}.";

                if (wagonDict.ContainsKey(wagon.ID))
                    wagonDict[wagon.ID] = wagonDict[wagon.ID].Remove(wagonDict[wagon.ID].Length - 1) + "; " + note;
                else
                    wagonDict.Add(wagon.ID, note);
            }//Ключи не сортирую 
            
            using (var log = new StreamWriter(filePath))
                foreach (var i in wagonDict)
                    log.WriteLine($"{i.Key}, {i.Value}");              
        }

        string DirectionToStr(Direction direction)
        {
            return (direction == Direction.Right) ? "->" : "<-";
        }
    }
}
