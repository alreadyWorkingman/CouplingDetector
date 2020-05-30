using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Drawing;
using System.Windows.Shapes;

namespace MainForm
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings(ApplicationManager Core)
        {
            InitializeComponent();
            core = Core;

            colors.Add("black", Color.Black);
            colors.Add("green", Color.Green);
            colors.Add("blue", Color.Blue);
            colors.Add("red",Color.Red);
            colors.Add("yellow", Color.Yellow);

            colorBox.ItemsSource = colors;
            colorBox.DisplayMemberPath = nameof(KeyValuePair<string, Color>.Key);
            colorBox.SelectedValuePath = nameof(KeyValuePair<int, Color>.Value);
            colorBox.SelectedIndex = 0;
        }

        public ApplicationManager core;
        Dictionary<string, Color> colors = new Dictionary<string, Color>();
        bool startWithoutVideo;

        private void OpenSaveReadClick(object sender, RoutedEventArgs e)  // создание записываемого файла, передача его пути в ядро
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog1 = new Microsoft.Win32.SaveFileDialog();
            saveFileDialog1.Filter = "(*.txt)|*.txt";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName.Length > 0)
            {
                //core.SetLogPath(saveFileDialog1.FileName);
            }

        }
        private void EnableWithoutVideo(object sender, RoutedEventArgs e) // чекбокс отладки без видео (вкл)
        {
            startWithoutVideo = true;
        }
        private void DisableWithoutVideo(object sender, RoutedEventArgs e)// чекбокс отладки без видео (выкл)
        {
            startWithoutVideo = false;
        }

        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e) //ивент изменения текста, передача числа "толщины" ректангла в ядро
        {
            int thickness;
            if (!Int32.TryParse(textBox1.Text, out thickness))
            {
                thickness = 1;

              //  core.SetThickness(thickness);
            }
        }

        private void ColorBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            core.Color = (Color)colorBox.SelectedValue;
        }
    }
}