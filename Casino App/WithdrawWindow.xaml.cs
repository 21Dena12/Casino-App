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
    public partial class WithdrawWindow : Window
    {
        public WithdrawWindow()
        {
            InitializeComponent();
        }

        // Обработка нажатия на кнопку "Вывести"
        private void WithdrawButton_Click(object sender, RoutedEventArgs e)
        {
            decimal amount;
            if (decimal.TryParse(WithdrawAmountTextBox.Text, out amount) && amount > 0)
            {
                // Получаем пользователя и проверяем баланс
                var user = ConnectionClass.db.Пользователи.FirstOrDefault(u => u.Id == CurrentUser.Id);
                if (user != null && user.Balance >= amount)
                {
                    // Добавляем транзакцию в базу данных
                    var transaction = new Транзакции
                    {
                        UserId = CurrentUser.Id,
                        Type = "Withdraw",
                        Amount = amount,
                        TransactionDate = DateTime.Now
                    };

                    ConnectionClass.db.Транзакции.Add(transaction);
                    ConnectionClass.db.SaveChanges();

                    // Уменьшаем баланс пользователя
                    user.Balance -= amount;
                    ConnectionClass.db.SaveChanges();

                    MessageBox.Show("Средства успешно выведены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close(); // Закрываем окно
                }
                else
                {
                    MessageBox.Show("Недостаточно средств на балансе.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Введите корректную сумму.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}
