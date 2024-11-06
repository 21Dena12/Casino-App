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
    public partial class EditUserWindow : Window
    {
        private Пользователи _user; // Хранит текущего пользователя для редактирования

        public EditUserWindow(Пользователи user)
        {
            InitializeComponent();
            _user = user;

            // Заполняем поля данными выбранного пользователя
            LoginTextBox.Text = _user.Login;
            PasswordTextBox.Text = _user.Password;
            RoleComboBox.SelectedIndex = _user.Role == "Admin" ? 1 : 0; // Выбираем роль
        }

        // Сохранение изменений
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Обновляем данные пользователя
            _user.Login = LoginTextBox.Text;
            _user.Password = PasswordTextBox.Text;
            _user.Role = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString();

            // Сохраняем изменения в базе данных
            ConnectionClass.db.SaveChanges();

            MessageBox.Show("Изменения сохранены.");
            DialogResult = true; // Закрываем окно и возвращаем результат
        }
    }

}
