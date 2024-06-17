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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace крот
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
        public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Kv.IsEnabled = false;  // Сделать UserControl недоступным при запуске
        }
        
        private void knopka1_Click(object sender, RoutedEventArgs e)
        {
            Kv.IsEnabled = true;
        }

        private void knopka2_Click(object sender, RoutedEventArgs e)
        {
            Kv.IsEnabled = false;
        }



        private List<Point> pathPoints = new List<Point>
{
    new Point(50, 50),    // Начальная точка
    new Point(100, 100),  // Двигаемся вверх и вправо
    new Point(150, 50),   // Двигаемся вниз и вправо
    
    
};


        private int currentPointIndex = 0;
        private void Kv_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (currentPointIndex >= pathPoints.Count) // Если достигли конца списка
            {
                крот.Window1 secondWindow = new крот.Window1();
     
                secondWindow.Show();  // Открыть новое окно
                this.Close();  // Закрыть текущее окно
                

            }
            else
            {
                Point nextPoint = pathPoints[currentPointIndex++];
                double left = nextPoint.X;
                double top = nextPoint.Y;

                // Установка новых координат для Margin картинки
                Kv.Margin = new Thickness(left, top, 0, 0);
            }
        }
        


    }
}