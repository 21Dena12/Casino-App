using LiveCharts;  // Не забудьте подключить нужные пространства имён
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;  // Для INotifyPropertyChanged
using System.Linq;
using System.Windows;
namespace Casino_App
{
    public partial class ReportWindow : Window, INotifyPropertyChanged
    {
        // Реализация интерфейса для уведомления об изменении свойств
        public event PropertyChangedEventHandler PropertyChanged;

        private ChartValues<decimal> _totalValues;
        private string[] _gameDates;

        // Свойства для хранения данных диаграммы
        public ChartValues<decimal> TotalValues
        {
            get => _totalValues;
            set
            {
                _totalValues = value;
                OnPropertyChanged("TotalValues");
            }
        }

        public string[] GameDates
        {
            get => _gameDates;
            set
            {
                _gameDates = value;
                OnPropertyChanged("GameDates");
            }
        }

        public ReportWindow()
        {
            InitializeComponent();
            LoadUsers(); // Загрузка списка пользователей
            DataContext = this; // Установка контекста данных для окна
        }

        // Загрузка пользователей для выбора в отчете
        private void LoadUsers()
        {
            var users = ConnectionClass.db.Пользователи.ToList();
            UserComboBox.ItemsSource = users;
            UserComboBox.DisplayMemberPath = "Login"; // Отображаем логин
            UserComboBox.SelectedValuePath = "Id";    // Храним идентификатор пользователя
        }

        // Генерация отчета по выбранному пользователю
        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (UserComboBox.SelectedValue == null)
            {
                MessageBox.Show("Выберите пользователя для отчета.");
                return;
            }

            var selectedUserId = (int)UserComboBox.SelectedValue;

            // Получаем все записи отчётов выбранного пользователя
            var reportData = ConnectionClass.db.Отчеты
                              .Where(r => r.UserId == selectedUserId)
                              .Select(r => new
                              {
                                  GameDate = r.GeneratedDate,
                                  // Рассчитываем общий итог: выигрыш - проигрыш
                                  Total = (r.Winnings ?? 0) - (r.Losses ?? 0)
                              }).ToList();

            // Заполняем данные для диаграммы
            TotalValues = new ChartValues<decimal>(reportData.Select(r => r.Total).ToArray());
            GameDates = reportData.Select(r => r.GameDate.ToString()).ToArray();  // Форматируем дату

            // Обновляем диаграмму
            DataContext = this;
        }

        // Метод для уведомления об изменении свойства
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Кнопка назад
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

