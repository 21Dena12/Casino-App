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
using System.Windows.Threading;
using System.Globalization;
namespace Casino_App
{
    public partial class GameSessionWindow : Window
    {
        private double _currentCoefficient = 1.0;
        private double _betAmount = 0;
        private bool _isGameActive = false;
        private bool _betPlaced = false;
        private Random _random = new Random();
        private DispatcherTimer _timer;

        public GameSessionWindow()
        {
            InitializeComponent();
            UpdateBalance();
            StartNewGame();
        }

        // Обновление баланса пользователя
        private void UpdateBalance()
        {
            var currentUser = ConnectionClass.db.Пользователи.FirstOrDefault(p => p.Login == CurrentUser.Login);
            if (currentUser != null)
            {
                BalanceTextBlock.Text = CurrentUser.Balance.ToString("F2", CultureInfo.InvariantCulture);
            }
        }

        // Логика начала новой игры
        private void StartNewGame()
        {
            _isGameActive = false;
            _betPlaced = false;
            _currentCoefficient = 1.0;
            CoefficientTextBlock.Text = _currentCoefficient.ToString("F2");
            GameStatusTextBlock.Text = "Игра начинается через 10 секунд!";
            WinningsTextBlock.Text = "";

            // Запуск таймера для старта игры через 10 секунд
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(10); // Ожидание 10 секунд перед началом
            _timer.Tick += GameStartTimer_Tick;
            _timer.Start();

            // Активируем поле ввода и кнопки
            BetAmountTextBox.IsEnabled = true;
            PlaceBetButton.IsEnabled = true;
            CollectButton.IsEnabled = false; // Кнопка "Забрать" неактивна, пока ставка не сделана
        }

        // Таймер, который запускает игру
        private void GameStartTimer_Tick(object sender, EventArgs e)
        {
            _timer.Stop(); // Останавливаем таймер ожидания
            StartGame();   // Начинаем игру
        }

        // Логика начала игры (увеличение коэффициента)
        private void StartGame()
        {
            GameStatusTextBlock.Text = "Игра идет...";
            _isGameActive = true;
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(0.5); // Интервал обновления коэффициента
            _timer.Tick += CoefficientIncreaseTimer_Tick;
            _timer.Start();
            PlaceBetButton.IsEnabled = false;
        }

        // Логика увеличения коэффициента
        private void CoefficientIncreaseTimer_Tick(object sender, EventArgs e)
        {
            if (_isGameActive)
            {
                _currentCoefficient += _random.NextDouble() * 0.5; // Случайное увеличение коэффициента
                CoefficientTextBlock.Text = _currentCoefficient.ToString("F2");

                // Определяем краш с вероятностью
                if (_random.Next(0, 100) < 5) // 5% шанс на краш
                {
                    GameCrashed();
                }
            }
        }
        private void RecordGameHistory(bool isWin, decimal? winnings)
        {
            var currentUser = ConnectionClass.db.Пользователи.FirstOrDefault(p => p.Login == CurrentUser.Login);
            var lastGame = ConnectionClass.db.Игры.OrderByDescending(g => g.Id).FirstOrDefault();

            if (currentUser != null && lastGame != null)
            {
                // Создаем запись в таблице ИсторияИгры
                var gameHistory = new ИсторияИгр
                {
                    UserId = currentUser.Id,
                   
                    GameId = lastGame.Id,
                    Winnings = winnings, // Если игрок выиграл, передается сумма выигрыша, иначе null или 0
                    PlayDate = DateTime.Now
                };

                // Добавляем запись в БД
                ConnectionClass.db.ИсторияИгр.Add(gameHistory);
                ConnectionClass.db.SaveChanges();
            }
        }
        // Логика ставки
        private void PlaceBetButton_Click(object sender, RoutedEventArgs e)
        {
            if (_betPlaced)
            {
                MessageBox.Show("Ставка уже сделана.");
                return;
            }

            if (double.TryParse(BetAmountTextBox.Text, out _betAmount))
            {
                var currentBalance = ConnectionClass.db.Пользователи.FirstOrDefault(p => p.Login == CurrentUser.Login)?.Balance;

                if ((decimal)_betAmount <= currentBalance && _betAmount > 0)
                {
                    GameStatusTextBlock.Text = "Ставка сделана!";
                    _betPlaced = true;
                    BetAmountTextBox.IsEnabled = false;
                    PlaceBetButton.IsEnabled = false;
                    CollectButton.IsEnabled = true; // Активируем кнопку "Забрать" после ставки
                }
                else
                {
                    MessageBox.Show("Недостаточно средств или неверная сумма.");
                }
            }
            else
            {
                MessageBox.Show("Введите корректную сумму ставки.");
            }
        }

        // Логика краша игры
        private void GameCrashed()
        {
            _isGameActive = false;
            _timer.Stop();
            GameStatusTextBlock.Text = "Игра завершена! Краш!";
            CoefficientTextBlock.Foreground = Brushes.Red;

            // Проверяем, была ли сделана ставка
            if (_betPlaced)
            {
                MessageBox.Show("Вы проиграли ставку!");

                // Записываем проигрыш в историю до сброса ставки
                RecordGameHistory(false, -(decimal)_betAmount);

                // Сбрасываем сумму ставки
                _betAmount = 0;
                ResetGame();
            }
        }


        // Логика завершения игры (если игрок успел снять ставку)
        private void CollectButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isGameActive && _betPlaced)
            {
                EndGameWithWinnings();
                CollectButton.IsEnabled = false; // Отключаем кнопку после выигрыша
            }
            else
            {
                MessageBox.Show("Невозможно забрать выигрыш, если игра завершена или ставка не сделана.");
            }
        }

        private void EndGameWithWinnings()
        {
            var winnings = _betAmount * _currentCoefficient;
            MessageBox.Show($"Вы выиграли: {winnings:F2}");
            var currentUser = ConnectionClass.db.Пользователи.FirstOrDefault(p => p.Login == CurrentUser.Login);
            if (currentUser != null)
            {
                currentUser.Balance += (decimal)winnings;
                ConnectionClass.db.SaveChanges();
                RecordGameHistory(true, (decimal)winnings);
            }
            ResetGame();
            UpdateBalance();

        }

        // Сброс игры и ожидание новой игры
        private void ResetGame()
        {
            BetAmountTextBox.IsEnabled = true;
            PlaceBetButton.IsEnabled = true;
            CollectButton.IsEnabled = false;
            UpdateBalance();
            StartNewGame();
        }

        // Кнопка назад
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }


}



