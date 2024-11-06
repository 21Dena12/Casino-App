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
    public partial class AdminPanelWindow : Window
    {
        public AdminPanelWindow()
        {
            InitializeComponent();
        }

        // Управление пользователями
        private void ManageUsersButton_Click(object sender, RoutedEventArgs e)
        {
            ManageUsersWindow manageUsersWindow = new ManageUsersWindow();
            manageUsersWindow.Show();
        }

        // Просмотр отчетов
        private void ViewReportsButton_Click(object sender, RoutedEventArgs e)
        {
            ReportWindow reportsWindow = new ReportWindow();
            reportsWindow.Show();
        }

        // Просмотр транзакций
        private void ViewTransactionsButton_Click(object sender, RoutedEventArgs e)
        {
            TransactionWindow transactionsWindow = new TransactionWindow();
            transactionsWindow.Show();
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
