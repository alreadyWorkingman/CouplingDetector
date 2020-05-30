using Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace MainForm
{

    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
            public App()
            {
            ApplicationManager Z = new ApplicationManager();
            MainWindow window = new MainWindow(Z);
            window.Show();
            }
    }
}
