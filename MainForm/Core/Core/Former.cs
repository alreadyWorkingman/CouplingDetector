using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Core
{
    class Former
    {
        Coupling first =null;  Coupling last = null;
        Train train;

        Direction wagonDirection
        {
            get                 
            {
                //@Растояние до края надо задавать динамически.
                if (last.location.X < 40) //Автоматически левый край изображения в окне. То есть ушёл влево
                    return Direction.Left;
                else return Direction.Right;
            }

        }

        /// <summary>
        /// Мега Костыль. Направление поезда, определяемое по направлению первой сцепки
        /// </summary>
        Direction definingDirect;
        /// <summary>
        /// Мега Костыль № 2.Для детекта направления самого самого первого вагона
        /// </summary>
        bool IsFirst = true;
        int counterId = 0;  //Может статик?
        /// <summary>
        ///Время интервала между поездами в минутах
        /// </summary>
        public int secNewTrain;

        private event EventHandler<Wagon> WagonDetected; 
        public void Subscribe(EventHandler<Wagon> eventHandler)
        {
            WagonDetected += eventHandler;
        }

        public Former(int secTimeLimitTrain = 300)
        {
            secNewTrain = secTimeLimitTrain;
            train = new Train();
        }


        /// <summary>
        /// Опредеяет движения из first в last
        /// </summary>
        /// <param name="last">Координата конца</param>
        /// <param name="first">Координата начала</param>
        /// <returns></returns>
        Direction LastDirection(int first, int last)
        {
            return (last - first > 0) ? Direction.Right : Direction.Left;
        }
        int xLC = 138, xRC = 276;//138 и 276
        bool isHalfW = false;

        /// <summary>
        /// Время первого появления первой сцепки вагона в центральной зоне
        /// </summary>
        DateTime CntrTime;
        /// <summary>
        /// Первая сцепка вагона
        /// </summary>
        Coupling CntrCop = null;
        /// <summary>
        /// Направление первой сцепки вагона
        /// </summary>
        Direction CntrDir;

        int CurIDCntr
        {
            get
            {
                //@ if (counterId == 0) definingDirect = wagonDirection; //Для самого первого (переломного) вагона
                //Минус метода выше - Все положительные =>  + неоднозначность id
                if (IsFirst) definingDirect = CntrDir;
                //Костыльный, но есть отрицательные значения => - неоднозначность id

                if (CntrDir == definingDirect)
                    return counterId++;
                else return --counterId;
            }
        }

        internal void WagonForm(Coupling cop)
        {
            if (cop != null) //Сцепка есть на экране
            {   //Место центральоной (детектируемой) сцепки не занято. Для записи только первого появления в центре
                if (CntrCop == null)
                {
                    if (xLC < cop.location.X && cop.location.X < xRC) //Текщее положение в центральной области
                    {
                        if (isHalfW) //В составе теущего (проверяемого) вагона уже есть сцепка
                        {
                            //Костыл. Для определения направления первой сцепки
                            if (IsFirst) CntrDir = LastDirection(last.location.X, cop.location.X); 

                            // Напаравления только появившейся сцепки совпадает с направлением прошлой сцепки вагона
                            if (LastDirection(last.location.X, cop.location.X) == CntrDir) 
                            {
                                Wagon wagon = new Wagon(CntrTime, cop.detectedTime, CurIDCntr, CntrDir);
                                IsFirst = false;
                                train.AddWagon(wagon);
                                WagonDetected.Invoke(this, wagon);//FFFFF
                            }
                            isHalfW = false; //Вагоно записан, пока сцепка не уйдёт больше не надо.
                        }
                        CntrCop = cop; //Текущая становиться центральной.
                    }
                }
                last = cop; 
            }
            else
            {
                if (CntrCop != null) // спорно. Надо тестировать. Но пока МНогое держится на нем
                {
                    CntrDir = wagonDirection;//Определяет направление по last
                    CntrTime = CntrCop.detectedTime;
                    isHalfW = true;

                    CntrCop = null;
                    last = null;
                }

                if (DateTime.Now.Subtract(CntrTime).TotalSeconds > secNewTrain)
                {
                    //@logger.WriteLog(train); //Не знаю как Поезд будет туда передаваться.
                    train.Dispose();
                    counterId = 0;
                }
            }
        }

        //Метод для остановки детектирования. Нужно приыязать к событию и какие будут параметры?
       public void StopForm()
        {
            //@logger.WriteLog(train); //Не знаю как Поезд будет туда передаваться.
            train.Dispose();
            counterId = 0;
            CntrCop = null;
            last = null;
            IsFirst = true;
            isHalfW = false;
        }
    }
}
