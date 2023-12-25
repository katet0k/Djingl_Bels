using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Djingl_Bels
{
    public partial class MainWindow : Window
    {
        private bool isDragging = false;
        private Point startPosition;
        private Image draggedImage;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            startPosition = e.GetPosition(null);
            draggedImage = sender as Image;
            draggedImage.CaptureMouse();
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && draggedImage != null)
            {
                Point currentPosition = e.GetPosition(null);
                TranslateTransform transform = draggedImage.RenderTransform as TranslateTransform;

                double deltaX = currentPosition.X - startPosition.X;
                double deltaY = currentPosition.Y - startPosition.Y;

                if (transform != null)
                {
                    transform.X += deltaX;
                    transform.Y += deltaY;
                }

                startPosition = currentPosition;
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            if (draggedImage != null)
            {
                draggedImage.ReleaseMouseCapture();

                // Отримайте координати місця відпускання
                Point dropPosition = e.GetPosition((Grid)draggedImage.Parent);

                // Створіть новий об'єкт Image з вказаними координатами
                Image newImage = CreateNewImage(draggedImage, dropPosition);

                // Встановіть новий об'єкт як перетягуваний для можливого подальшого перетягування
                draggedImage = newImage;
            }
        }

        private Image CreateNewImage(Image originalImage, Point dropPosition)
        {
            Image newImage = new Image
            {
                Source = originalImage.Source,
                Width = originalImage.Width,
                Height = originalImage.Height,
                RenderTransform = new TranslateTransform(),
                Margin = new Thickness(dropPosition.X, dropPosition.Y, 0, 0),
            };

            Grid parentGrid = (Grid)originalImage.Parent;
            parentGrid.Children.Add(newImage);

            newImage.MouseLeftButtonDown += Image_MouseLeftButtonDown;
            newImage.MouseMove += Image_MouseMove;
            newImage.MouseLeftButtonUp += Image_MouseLeftButtonUp;

            return newImage;
        }
    }
}
