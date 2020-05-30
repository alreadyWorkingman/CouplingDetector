using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Core;

namespace MainForm
{

   
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = false;
        }

        public MainWindow(ApplicationManager core)
        {
            InitializeComponent();
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = false;
            ApplicationManager Core = core;
        }
        public ApplicationManager core { get; set; }

        public string videoPath;

        /// <summary>
        /// Количество новых вагонов с относительным отрицательным ID
        /// </summary>
        int countOverWagon = 0;

        struct Note
        {
            //@Предположу, что Направление 0-Вправо, 1 - Влево
            public Note(int id, string tf, string tl, int D)
            {
                ID = id;
                Time_First = tf; Time_Last = tl; dir = D;

            }
            int dir;
            public int ID { get; private set; }
            public string Time_First {  get; private set; }
            public string Time_Last {  get; private set; }//→←
            public string DirStr
            {
                get
                {
                    if (dir == 0)
                        return "→";
                    else if (dir == 1)
                        return "←";
                    else return "→←";
                }
            }

        }

        #region[Работа с Datatable]

        private DataTable datatable;
        public DataTable DataTable
        {
            get
            {
                if (this.datatable == null)
                    this.datatable = this.CreateDataTable();
                return this.datatable;
            }
        }

        private DataTable CreateDataTable()
        {
           var dataTable = new DataTable();
            dataTable.Columns.Add(new System.Data.DataColumn("ID", typeof(int)));
            dataTable.Columns.Add(new System.Data.DataColumn("First Time", typeof(string)));//First Time
            dataTable.Columns.Add(new System.Data.DataColumn("Last Time", typeof(string)));//Last Time
            dataTable.Columns.Add(new System.Data.DataColumn("Dir", typeof(string)));
            return dataTable;
        }
        private void AddDataTable(Note note )
        {
            var row = DataTable.NewRow();
            row["ID"] = note.ID;
            row["First Time"] = note.Time_First;
            row["Last Time"] = note.Time_Last;
            row["Dir"] = note.DirStr;
            DataTable.Rows.Add(row);
        }
        private void IncreaseAllIDTab()
        {
            foreach (DataRow row in DataTable.Rows)
            {
               //row["ID"]
                row["ID"] = (int)row["ID"]+1;
            }
        }
        #endregion

        //Эта функция примет что-то (Данные о вагоне) и запишет в Грид
        public void AddInGrid(object z,Core.Wagon w)
        {
            string first = w.firstTimeDetected.ToShortDateString() + " " + w.firstTimeDetected.ToLongTimeString();
            string last = w.lastTimeDetected.ToShortDateString() + " " + w.lastTimeDetected.ToLongTimeString();

            if (countOverWagon < -w.ID) //Если приходят новые отрицательные
            {
                countOverWagon++;
                IncreaseAllIDTab();
            }
            var note = new Note(w.ID + countOverWagon, first, last, (int)w.direction);
            AddDataTable(note);
        }

        private void myDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            //--------------------------------Тесты-----------------------------
            ////AddDataTable(new Note("999", "2020-20-20 17:20:29", "2020-20-20 17:20:90", "->"));//@ УДАЛИ Примеры
            //AddDataTable(new Note(11, "2020-20-20 17:20:99", "2020-20-20 17:17:30", 0));
            //AddDataTable(new Note(11, "2020-20-20 17:20:34", "2020-20-20 17:17:50", 1));
            //AddDataTable(new Note(2, "2020-20-20 17:20:59", "2020-20-20 17:20:12", 3));
            AddInGrid(this, new Core.Wagon(DateTime.Now, DateTime.Now, 0, Core.Direction.Right));
            AddInGrid(this, new Core.Wagon(DateTime.Now, DateTime.Now, 1, Core.Direction.Right));
            AddInGrid(this, new Core.Wagon(DateTime.Now, DateTime.Now, 2, Core.Direction.Right));
            AddInGrid(this, new Core.Wagon(DateTime.Now, DateTime.Now, 2, Core.Direction.Left));
            AddInGrid(this, new Core.Wagon(DateTime.Now, DateTime.Now, 1, Core.Direction.Left));
            AddInGrid(this, new Core.Wagon(DateTime.Now, DateTime.Now, 0, Core.Direction.Left));
            AddInGrid(this, new Core.Wagon(DateTime.Now, DateTime.Now, -1, Core.Direction.Left));
            AddInGrid(this, new Core.Wagon(DateTime.Now, DateTime.Now, -2, Core.Direction.Left));
            AddInGrid(this, new Core.Wagon(DateTime.Now, DateTime.Now, -3, Core.Direction.Left));
            AddInGrid(this, new Core.Wagon(DateTime.Now, DateTime.Now, -3, Core.Direction.Right));
            AddInGrid(this, new Core.Wagon(DateTime.Now, DateTime.Now, -3, Core.Direction.Left));
            AddInGrid(this, new Core.Wagon(DateTime.Now, DateTime.Now, -4, Core.Direction.Left));
            AddInGrid(this, new Core.Wagon(DateTime.Now, DateTime.Now, -4, Core.Direction.Right));
            AddInGrid(this, new Core.Wagon(DateTime.Now, DateTime.Now, -3, Core.Direction.Right));
            AddInGrid(this, new Core.Wagon(DateTime.Now, DateTime.Now, -2, Core.Direction.Right));
            AddInGrid(this, new Core.Wagon(DateTime.Now, DateTime.Now, -1, Core.Direction.Right));
            //--------------------------------Тесты-----------------------------

            myDataGrid.ItemsSource = DataTable.DefaultView; //@ 
            myDataGrid.Columns[1].Width = DataGridLength.SizeToCells; 
            myDataGrid.Columns[2].Width = DataGridLength.SizeToCells;
            myDataGrid.RowHeight = 25; //@Спорный момент
        }
        
        private void BtnSetVidPath_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog1 = new Microsoft.Win32.OpenFileDialog();
            openFileDialog1.Filter = "Файлы видео (*.mp4, *.cva)|*.mp4;*.cva";
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName.Length > 0)
            {
                btnStart.IsEnabled = true;
                btnStop.IsEnabled = true;
                videoPath = openFileDialog1.FileName;
                //Core.SetVideoPath(videoPath);//@
            }
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap wBitmap = new WriteableBitmap(0,0,96,96,PixelFormats.Bgr24,null);
            myImage.Source = wBitmap;
            //Core.SetImgSource(wBitmap);//@
            EventHandler<Core.Wagon> metod = new EventHandler<Core.Wagon>(AddInGrid);
            // Core.SubscribeWagon(metod);//@

            //Core.AppStart();//@
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            //   Core.AppStop();//@
        }

        private void BtnOption_Click(object sender, RoutedEventArgs e)
        {
            Settings window = new Settings(core);//@
            window.Owner = this;
            window.Show();//@  
        }

    }
}

