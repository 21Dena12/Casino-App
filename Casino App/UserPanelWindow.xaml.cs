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
    public partial class UserPanelWindow : Window
    {
        public UserPanelWindow()
        {
            InitializeComponent();
        }

        // Открытие игровой сессии
        private void GameSessionsButton_Click(object sender, RoutedEventArgs e)
        {
            GameSessionWindow gameSessionWindow = new GameSessionWindow();
            gameSessionWindow.Show();
        }

        // Открытие финансовых операций
        private void TransactionsButton_Click(object sender, RoutedEventArgs e)
        {
            TransactionWindow transactionWindow = new TransactionWindow();
            transactionWindow.Show();
        }

        // Открытие истории игр
        private void GameHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            GameHistoryWindow gameHistoryWindow = new GameHistoryWindow();
            gameHistoryWindow.Show();
        }

        // Кнопка назад
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
        }
    }

}
