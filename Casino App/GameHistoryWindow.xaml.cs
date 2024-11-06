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
    public partial class GameHistoryWindow : Window
    {
        public GameHistoryWindow()
        {
            InitializeComponent();
            LoadGameHistory();
        }

        // Загрузка истории игр
        private void LoadGameHistory()
        {
            try
            {
                var currentUser = ConnectionClass.db.Пользователи.FirstOrDefault(p => p.Login == CurrentUser.Login);

                if (currentUser != null)
                {
                    // Получаем историю игр текущего пользователя
                    var gameHistory = (from h in ConnectionClass.db.ИсторияИгр
                                       join g in ConnectionClass.db.Игры on h.GameId equals g.Id
                                       where h.UserId == currentUser.Id
                                       select new
                                       {
                                           PlayDate = h.PlayDate,
                                           Winnings = h.Winnings
                                       }).ToList();

                    // Устанавливаем данные для DataGrid
                    GameHistoryDataGrid.ItemsSource = gameHistory;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки истории игр: {ex.Message}");
            }
        }

        // Кнопка для возврата на предыдущий экран
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
