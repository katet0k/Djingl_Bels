using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;
using System.Windows.Media.Imaging;

namespace Djingl_Bels
{
    public partial class MainWindow : Window
    {
        private bool isDragging = false;
        private Point startPosition;
        private Image draggedImage;
        private bool isCloneCreated = false;
        private List<Image> clonedImages = new List<Image>();

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

                // Перевірка, чи є клон для поточного зображення
                if (!isCloneCreated && !clonedImages.Contains(draggedImage))
                {
                    // Створіть клон під час відпускання в точці відпускання
                    Point dropPosition = e.GetPosition((Grid)draggedImage.Parent);
                    Image newImage = CreateNewImage(draggedImage, dropPosition);

                    clonedImages.Add(newImage);

                    // Встановіть позицію оригінального зображення на його початкову позицію
                    SetOriginalImagePosition(draggedImage);

                    draggedImage = newImage;
                    isCloneCreated = true;
                }
                else
                {
                    isCloneCreated = false;
                }
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
                Margin = new Thickness(dropPosition.X - originalImage.ActualWidth / 2, dropPosition.Y - originalImage.ActualHeight / 2, 0, 0),
            };

            Grid parentGrid = (Grid)originalImage.Parent;
            parentGrid.Children.Add(newImage);

            newImage.MouseLeftButtonDown += Image_MouseLeftButtonDown;
            newImage.MouseMove += Image_MouseMove;
            newImage.MouseLeftButtonUp += Image_MouseLeftButtonUp;

            return newImage;
        }

        private void SetOriginalImagePosition(Image image)
        {
            TranslateTransform transform = image.RenderTransform as TranslateTransform;
            if (transform != null)
            {
                transform.X = 0;
                transform.Y = 0;
            }
        }

        // Додано обробник події для TextBox
        private void ColorCodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Перевірка чи введений текст може бути розпізнаний як колір
            if (IsValidColorCode(colorCodeTextBox.Text))
            {
                // Змінення кольору вікна
                Color newColor = (Color)ColorConverter.ConvertFromString(colorCodeTextBox.Text);
                this.Background = new SolidColorBrush(newColor);
            }
        }

        // Додано обробник події для кнопки
        private void ChangeWindowColor_Click(object sender, RoutedEventArgs e)
        {
            // Отримання кольору з TextBox і зміна кольору вікна
            if (IsValidColorCode(colorCodeTextBox.Text))
            {
                Color newColor = (Color)ColorConverter.ConvertFromString(colorCodeTextBox.Text);
                this.Background = new SolidColorBrush(newColor);
            }
        }

        // Валідація коду кольору
        private bool IsValidColorCode(string colorCode)
        {
            try
            {
                ColorConverter.ConvertFromString(colorCode);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        private void CaptureScreenshot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Створюємо бітмап з вмісту екрана
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)this.Width, (int)this.Height, 96, 96, PixelFormats.Pbgra32);
                renderTargetBitmap.Render(this);

                // Створюємо кодек для формату PNG
                PngBitmapEncoder pngImage = new PngBitmapEncoder();
                pngImage.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

                // Зберігаємо знімок екрана в файл
                string screenshotPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "NewYearThree.png");
                using (FileStream fileStream = new FileStream(screenshotPath, FileMode.Create))
                {
                    pngImage.Save(fileStream);
                }

                MessageBox.Show($"Знімок екрана збережено за шляхом: {screenshotPath}", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Помилка при збереженні знімка екрана: {ex.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearField_Click(object sender, RoutedEventArgs e)
        {
            // Створюємо новий екземпляр MainWindow
            MainWindow newMainWindow = new MainWindow();

            // Закриваємо поточний екземпляр MainWindow
            this.Close();

            // Відображаємо новий екземпляр MainWindow
            newMainWindow.Show();
        }


    }
}
