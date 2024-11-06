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
    public partial class TransactionWindow : Window
    {
        public TransactionWindow()
        {
            InitializeComponent();
            LoadTransactions(); // Загрузка транзакций при открытии окна
        }

        // Загрузка данных транзакций
        private void LoadTransactions()
        {
            var currentUserId = CurrentUser.Id; // Получаем идентификатор текущего пользователя

            var transactions = ConnectionClass.db.Транзакции
                               .Where(t => t.UserId == currentUserId)
                               .Select(t => new
                               {
                                   TransactionDate = t.TransactionDate,
                                   TransactionType = t.Type == "Deposit" ? "Пополнение" : "Вывод",
                                   Amount = t.Amount
                               }).ToList();

            // Привязываем данные к DataGrid
            TransactionDataGrid.ItemsSource = transactions;
        }

        // Обработка нажатия на кнопку "Пополнить"
        private void DepositButton_Click(object sender, RoutedEventArgs e)
        {
            var depositWindow = new DepositWindow(); // Открытие окна пополнения
            depositWindow.ShowDialog();
            LoadTransactions(); // Обновление списка транзакций после операции
        }

        // Обработка нажатия на кнопку "Вывести"
        private void WithdrawButton_Click(object sender, RoutedEventArgs e)
        {
            var withdrawWindow = new WithdrawWindow(); // Открытие окна вывода средств
            withdrawWindow.ShowDialog();
            LoadTransactions(); // Обновление списка транзакций после операции
        }

        // Обработка нажатия на кнопку "Назад"
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Закрываем окно транзакций и возвращаемся на предыдущий экран
        }
    }

}
