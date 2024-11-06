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
    public partial class ManageUsersWindow : Window
    {
        public ManageUsersWindow()
        {
            InitializeComponent();
            LoadUsers();
        }

        // Загрузка пользователей из базы данных
        private void LoadUsers()
        {
            // Получаем всех пользователей из базы данных
            var users = ConnectionClass.db.Пользователи.ToList();
            UsersDataGrid.ItemsSource = users;
        }

        // Добавление нового пользователя
        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow addUserWindow = new AddUserWindow();
            if (addUserWindow.ShowDialog() == true)
            {
                LoadUsers(); // Обновляем список после добавления
            }
        }

        // Редактирование выбранного пользователя
        private void EditUserButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = (Пользователи)UsersDataGrid.SelectedItem;
            if (selectedUser != null)
            {
                EditUserWindow editUserWindow = new EditUserWindow(selectedUser);
                if (editUserWindow.ShowDialog() == true)
                {
                    LoadUsers(); // Обновляем список после редактирования
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для редактирования.");
            }
        }

        // Удаление выбранного пользователя
        private void DeleteUserButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = (Пользователи)UsersDataGrid.SelectedItem;
            if (selectedUser != null)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    ConnectionClass.db.Пользователи.Remove(selectedUser);
                    ConnectionClass.db.SaveChanges();
                    LoadUsers(); // Обновляем список после удаления
                }
            }
            else
            {
                MessageBox.Show("Выберите пользователя для удаления.");
            }
        }

        // Кнопка назад
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
