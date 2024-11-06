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
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
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
        
         

            private void LoginButton_Click(object sender, RoutedEventArgs e)
            {
                string login = LoginTextBox.Text;
                string password = PasswordBox.Password;

                var user = ConnectionClass.db.Пользователи.FirstOrDefault(u => u.Login == login && u.Password == password);
                if (user != null)
                {
                CurrentUser.Id = user.Id;
                CurrentUser.Login = user.Login;
                CurrentUser.Balance = (double)user.Balance;
                CurrentUser.Role = user.Role;
                MessageBox.Show("Успешный вход!");
                    if (user.Role == "Admin")
                    {
                        AdminPanelWindow adminPanel = new AdminPanelWindow();
                        adminPanel.Show();
                    }
                    else
                    {
                        UserPanelWindow userPanel = new UserPanelWindow();
                        userPanel.Show();
                    }
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
            }
        

    }
}
