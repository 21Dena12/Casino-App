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
    public partial class AddUserWindow : Window
    {
        public AddUserWindow()
        {
            InitializeComponent();
        }

        // Сохранение нового пользователя
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var newUser = new Пользователи()
            {
                Login = LoginTextBox.Text,
                Password = PasswordTextBox.Text,
                Role = ((ComboBoxItem)RoleComboBox.SelectedItem).Content.ToString(),
                Balance = 0 // Начальный баланс
            };

            ConnectionClass.db.Пользователи.Add(newUser);
            ConnectionClass.db.SaveChanges();

            MessageBox.Show("Пользователь добавлен.");
            DialogResult = true; // Закрытие окна и возврат результата
        }
    }

}
