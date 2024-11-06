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

namespace Casino_App
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем текущее окно
            this.Close();

            // Возвращаемся на предыдущую страницу, например, MainWindow
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
            private void RegisterButton_Click(object sender, RoutedEventArgs e)
            {
                string login = LoginTextBox.Text;
                string password = PasswordBox.Password;

                if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
                {
                    var user = new Пользователи
                    {
                        Login = login,
                        Password = password,
                        Role = "User",
                        Balance = 0
                    };

                    ConnectionClass.db.Пользователи.Add(user);
                    ConnectionClass.db.SaveChanges();

                    MessageBox.Show("Регистрация успешна!");
                }
                else
                {
                    MessageBox.Show("Введите логин и пароль.");
                }
            }
        

    }
}
