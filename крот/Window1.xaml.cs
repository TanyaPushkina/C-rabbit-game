using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.Linq;

namespace крот
{

    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            MyImage.Source = new BitmapImage(new Uri("милыйкотик.jpg", UriKind.Relative));
            Imagem.Source = new BitmapImage(new Uri("морковка.jpg", UriKind.Relative));
            InitializeAnimation(MyImage);
        }
        public class CustomImage : Image
        {
            // Добавляем новое свойство для демонстрации
            public string ImageDescription { get; set; }

            public CustomImage(string imagePath)
            {
                Source = new BitmapImage(new Uri(imagePath, UriKind.Relative));
                ImageDescription = "Это специальное изображение";
            }
        }

        private void InitializeAnimation(Image targetImage)
        {
            TranslateTransform transform = new TranslateTransform();
            targetImage.RenderTransform = transform;

            DoubleAnimation animation = new DoubleAnimation()
            {
                From = 0,
                To = -25, // Измените значение для большего или меньшего прыжка
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new Duration(TimeSpan.FromSeconds(0.3)) // Измените продолжительность для быстрого или медленного прыжка
            };

            transform.BeginAnimation(TranslateTransform.YProperty, animation);
        }

        private int clickCount = 0; // Счетчик кликов
        
        private void MyImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            clickCount++;
            System.Diagnostics.Debug.WriteLine("Current click count: " + clickCount);
            if (clickCount == 1)
            {
                System.Diagnostics.Debug.WriteLine("Moving to center");
                MoveImageToCenter(MyImage);
            }
            else if (clickCount == 2)
            {
                System.Diagnostics.Debug.WriteLine("Moving to path and changing background");
                MoveImageToPath();
                this.Background = new SolidColorBrush(Color.FromRgb(175, 250, 175));
                ChangeCarrotsToBackgroundImage(true);
            }
            else if (clickCount == 3)
            {
                System.Diagnostics.Debug.WriteLine("Animating movement and showing message");
                AnimateMovement();
                ChangeCarrotsToBackgroundImage(true);
                statusTextBlock.Text = "Зайчик занимается спортом"; // Устанавливаем текст при начале анимации
                statusTextBlock.Visibility = Visibility.Visible; // Делаем текст видимым
            }
            else if (clickCount == 4)
            {
                System.Diagnostics.Debug.WriteLine("Continuing animation");
                AnimateMovement();
                ChangeCarrotsToBackgroundImage(true);
                statusTextBlock.Text = "Зайчик занимается спортом"; // Устанавливаем текст при начале анимации
                statusTextBlock.Visibility = Visibility.Visible; // Делаем текст видимым
                clickCount = 2; // Сброс счётчика кликов к повторению шагов с 3-го клика
            }
        }
        private int exerciseCount = 0; // Счётчик выполненных упражнений
         

        private bool isAnimating = false; // Флаг состояния анимации
        
        private void AnimateMovement()
        {
            if (!isAnimating)
            {
                isAnimating = true;
                var transform = MyImage.RenderTransform as TranslateTransform ?? new TranslateTransform();
                MyImage.RenderTransform = transform;

                DoubleAnimation moveAnimation = new DoubleAnimation()
                {
                    From = 0,
                    To = 300,
                    Duration = TimeSpan.FromSeconds(4),
                    AutoReverse = true,
                    RepeatBehavior = new RepeatBehavior(1)
                };
               
                moveAnimation.Completed += (s, e) => {
                    isAnimating = false;
                    statusTextBlock.Text = "Зайчик отдыхает";
                    exerciseCount++; // Инкрементируем счётчик упражнений
                    exerciseCountTextBlock.Text = $"Занятие спортом: {exerciseCount}"; // Обновляем текст с количеством упражнений
                    exerciseCountTextBlock.Visibility = Visibility.Visible; // Делаем текст видимым
                    health++; // Увеличиваем здоровье
                    healthTextBlock.Text = $"Здоровье: {health}";
                    healthTextBlock.Visibility = Visibility.Visible;
                };

                DoubleAnimation jumpAnimation = new DoubleAnimation()
                {
                    From = 0,
                    To = -50,
                    Duration = TimeSpan.FromSeconds(0.5),
                    AutoReverse = true,
                    RepeatBehavior = RepeatBehavior.Forever
                };

                transform.BeginAnimation(TranslateTransform.XProperty, moveAnimation);
                transform.BeginAnimation(TranslateTransform.YProperty, jumpAnimation);
            }
        }
        private int carrotsEaten = 0; // Счётчик съеденных морковок
        private int health = 0; // Начальное значение здоровья

        private void Carrot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Image carrotImage && carrotImage.Visibility == Visibility.Visible)
            {
                carrotImage.Visibility = Visibility.Collapsed; // Скрываем морковку
                carrotsEaten++; // Увеличиваем счётчик съеденных морковок
                health++; // Увеличиваем здоровье
                healthTextBlock.Text = $"Здоровье: {health}";
                healthTextBlock.Visibility = Visibility.Visible;
                if (carrotsEaten < 11)
                {
                    carrotsEatenTextBlock.Text = $"Съедено морковки: {carrotsEaten}";
                }
                else
                {
                    carrotsEatenTextBlock.Text = $"Съедено морковки: 11                    Больше нет морковки";
                }

                carrotsEatenTextBlock.Visibility = Visibility.Visible;
            }
        }

        private void StopAnimation()
        {
            if (isAnimating)
            {
                var transform = MyImage.RenderTransform as TranslateTransform;
                if (transform != null)
                {
                    transform.BeginAnimation(TranslateTransform.XProperty, null);
                    transform.BeginAnimation(TranslateTransform.YProperty, null);
                }
                isAnimating = false;
                ///carrotsChanged = false; // Сброс состояния изменения морковок
            }
        }
        

        private void ChangeCarrotsToBackgroundImage(bool toNewImage)
        {
            foreach (UIElement element in MainGrid.Children)
            {
                if (element is Image)
                {
                    var image = (Image)element;
                    if (toNewImage && image.Source.ToString().Contains("морковка.jpg"))
                    {
                        image.Source = new BitmapImage(new Uri("морковкафон1.jpg", UriKind.Relative));
                    }
                    else if (!toNewImage && image.Source.ToString().Contains("морковкафон1.jpg"))
                    {
                        image.Source = new BitmapImage(new Uri("морковка.jpg", UriKind.Relative));
                    }
                }
            }
        }


        private void MoveImageToCenter(Image image)
        {
            TranslateTransform transform = image.RenderTransform as TranslateTransform ?? new TranslateTransform();
            image.RenderTransform = transform;
            double newX = (MainGrid.ActualWidth - image.ActualWidth) / 2;
            double newY = (MainGrid.ActualHeight - image.ActualHeight) / 2;
            transform.X = newX - image.Margin.Left;
            transform.Y = newY - image.Margin.Top;

            MoveCarrot();
        }
        private void MoveCarrot()
        {
            TranslateTransform transform = Imagem.RenderTransform as TranslateTransform ?? new TranslateTransform();
            Imagem.RenderTransform = transform;
            double newX = (MainGrid.ActualWidth - Imagem.ActualWidth) / 2;
            double newY = MainGrid.ActualHeight - Imagem.ActualHeight - 10; // 20 - отступ от низа
            transform.X = newX - Imagem.Margin.Left;
            transform.Y = newY - Imagem.Margin.Top;

            // Исчезновение и появление множества морковок
            Imagem.Visibility = Visibility.Hidden; // Скрываем большую морковку
            CreateMultipleCarrots();
        }


        private void MoveImageToPath()
        {
            Path.Visibility = Visibility.Visible; // Показываем дорожку

            // Создание нового изображения
            CustomImage newImage = new CustomImage("зайчикфон1.jpg")
            {
                Width = MyImage.Width, // Копируем размеры и другие свойства
                Height = MyImage.Height,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            // Располагаем новое изображение на дорожке
            double centerX = (MainGrid.ActualWidth - newImage.Width) / 2;
            double bottomY = 20; // Отступ от нижней части контейнера

            // Удаляем старое изображение
            MainGrid.Children.Remove(MyImage);

            // Добавляем новое изображение
            MainGrid.Children.Add(newImage);
            Canvas.SetLeft(newImage, centerX);
            Canvas.SetBottom(newImage, bottomY);

            // Обновляем ссылку MyImage на новый объект
            MyImage = newImage;

            // Перемещаем изображение на дорожку
            MainGrid.Children.Remove(MyImage);
            MainGrid.Children.Add(MyImage);
            Canvas.SetBottom(MyImage, 20); // Настраиваем высоту над дорожкой
            Canvas.SetLeft(MyImage, (MainGrid.ActualWidth - MyImage.ActualWidth) / 2); // Центрируем на дорожке
            MyImage.VerticalAlignment = VerticalAlignment.Center;

            // Изменение анимации для менее интенсивного прыжка
            ChangeAnimationIntensity(MyImage, -10, 0.5);
            MyImage.MouseDown += MyImage_MouseDown;

        }



        private void ChangeAnimationIntensity(Image targetImage, double newToValue, double newDurationSeconds)
        {
            TranslateTransform transform = targetImage.RenderTransform as TranslateTransform ?? new TranslateTransform();
            targetImage.RenderTransform = transform;

            DoubleAnimation newAnimation = new DoubleAnimation()
            {
                From = 0,
                To = newToValue, // Новое значение для меньшего прыжка
                AutoReverse = true,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new Duration(TimeSpan.FromSeconds(newDurationSeconds)) // Новая длительность для более медленного прыжка
            };

            transform.BeginAnimation(TranslateTransform.YProperty, newAnimation);
        }

         private void CreateMultipleCarrots()
         {
             int numberOfCarrots = 11; // Количество маленьких морковок
             for (int i = 0; i < numberOfCarrots; i++)
             {
                 Image carrotImage = new Image
                 {
                     Source = new BitmapImage(new Uri("морковка.jpg", UriKind.Relative)),
                     Width = 50,
                     Height = 50,
                     HorizontalAlignment = HorizontalAlignment.Left,
                     VerticalAlignment = VerticalAlignment.Bottom
                 };
                 TranslateTransform transform = new TranslateTransform
                 {
                     X = i * 70 // Расстояние между морковками
                 };
                 carrotImage.RenderTransform = transform;
                 MainGrid.Children.Add(carrotImage);
                 carrotImage.MouseDown += Carrot_MouseDown;
            }

             Path.Visibility = Visibility.Visible; // Показываем дорожку
             Path.Width = MainGrid.ActualWidth; // Дорожка на всю ширину Grid
         }

        private void GameButton_Click(object sender, RoutedEventArgs e)
        {
            clickCount = 2;
            exerciseCount = 0;
            carrotsEaten = 0;
            health = 0;

            StopAnimation();

            statusTextBlock.Visibility = Visibility.Collapsed;
            exerciseCountTextBlock.Visibility = Visibility.Collapsed;
            healthTextBlock.Visibility = Visibility.Collapsed;
            carrotsEatenTextBlock.Visibility = Visibility.Collapsed;

            this.Background = new SolidColorBrush(Color.FromRgb(175, 250, 175));

            // Сначала убираем все морковки
            foreach (var child in MainGrid.Children.OfType<Image>().ToList())
            {
                if (child.Source.ToString().Contains("морковка.jpg") || child.Source.ToString().Contains("морковкафон1.jpg"))
                {
                    MainGrid.Children.Remove(child);
                }
            }

            // Создаём морковки заново
            CreateMultipleCarrots();

            // Меняем изображения на морковкафон1.jpg
            ChangeCarrotsToBackgroundImage(true);

            MoveImageToPath();
        }






    }


}