using GW2JadeProtocolTimer.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace GW2JadeProtocolTimer;

public partial class MainWindow : Window
{
    private readonly JadeProtocolTimerState _offensiveTimer = new("Offensive");
    private readonly JadeProtocolTimerState _defensiveTimer = new("Defensive");
    private readonly DispatcherTimer _uiTimer;

    public MainWindow()
    {
        InitializeComponent();

        _uiTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(250) };
        _uiTimer.Tick += (_, _) => RefreshTimers();

        Loaded += MainWindow_Loaded;
        Closed += (_, _) => _uiTimer.Stop();
        StateChanged += MainWindow_StateChanged;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        PlaceLowerRight();
        RefreshTimers();
        _uiTimer.Start();
    }

    private void MainWindow_StateChanged(object? sender, EventArgs e)
    {
        if (WindowState == WindowState.Normal)
        {
            PlaceLowerRight();
            Topmost = false;
            Topmost = true;
        }
    }

    private void OffensiveButton_Click(object sender, RoutedEventArgs e)
    {
        _offensiveTimer.AddTime(DateTime.UtcNow);
        RefreshTimers();
    }

    private void DefensiveButton_Click(object sender, RoutedEventArgs e)
    {
        _defensiveTimer.AddTime(DateTime.UtcNow);
        RefreshTimers();
    }

    private void OffensiveButton_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        _offensiveTimer.ResetToIdle();
        e.Handled = true;
        RefreshTimers();
    }

    private void DefensiveButton_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        _defensiveTimer.ResetToIdle();
        e.Handled = true;
        RefreshTimers();
    }

    private void RefreshTimers()
    {
        DateTime now = DateTime.UtcNow;
        _offensiveTimer.Tick(now);
        _defensiveTimer.Tick(now);

        RefreshTimerRow(_offensiveTimer, OffensiveButton, OffensiveStatusText);
        RefreshTimerRow(_defensiveTimer, DefensiveButton, DefensiveStatusText);
    }

    private void RefreshTimerRow(
        JadeProtocolTimerState timer,
        Button button,
        TextBlock statusText)
    {
        bool isRunning = timer.IsRunning;
        bool isReady = timer.IsReady;

        button.Background = (Brush)FindResource(isRunning ? "Brush.Running" : "Brush.Idle");
        button.BorderBrush = (Brush)FindResource(
            isReady ? "Brush.RowBorderReady"
            : isRunning ? "Brush.RowBorderRunning"
            : "Brush.RowBorderIdle");
        button.BorderThickness = isReady || isRunning ? new Thickness(2) : new Thickness(1);
        button.Opacity = 1;

        statusText.Text = timer.DisplayText();
        statusText.Foreground = (Brush)FindResource(isReady ? "Brush.ReadyAccent" : "Brush.Muted");
        statusText.FontWeight = isReady ? FontWeights.Bold : FontWeights.SemiBold;
    }

    private void PlaceLowerRight()
    {
        const double margin = 28;
        Rect workArea = SystemParameters.WorkArea;
        Left = Math.Max(0, workArea.Right - Width - margin);
        Top = Math.Max(0, workArea.Bottom - Height - margin);
    }

    private void RootBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            DragMove();
    }

    private void HideButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void QuitButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
