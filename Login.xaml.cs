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
using System.Windows.Shapes;

namespace Djingl_Bels
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UserTextBox.Text;
            string password = PasswordBox.Password;

            // Тут ви можете виконати логіку для перевірки логіну та паролю
            // Наприклад, звернутися до бази даних або провести перевірку в пам'яті програми

            if (string.IsNullOrEmpty(username))
            {
                usernameValidationTextBlockL.Text = "Enter again a login!";
            }
            else
            {
                usernameValidationTextBlockL.Text = "";
            }

            if (string.IsNullOrEmpty(password))
            {
                passwordValidationTextBlockP.Text = "Enter again a password!";
            }
            else
            {
                passwordValidationTextBlockP.Text = "";
            }

            // Логіка перевірки логіну та паролю
            if (username == "User" && password == "1234")
            {
                // Відкрити нове вікно StudentsHome.xaml
                MainWindow Home = new MainWindow();
                Home.Show();

                // Закрити поточне вікно Login.xaml
                this.Close();
            }
        }
    }
}