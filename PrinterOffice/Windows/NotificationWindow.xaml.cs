using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace PrinterOffice {
    public partial class NotificationWindow : Window {
        public Brush backgroundBrush { get; }

        public NotificationWindow(string text, Brush backgroundBrush) {
            this.backgroundBrush = backgroundBrush;
            InitializeComponent();

            Dispatcher.BeginInvoke(DispatcherPriority.ApplicationIdle, new Action(() => {
                var desktopWorkingArea = SystemParameters.WorkArea;
                Left = desktopWorkingArea.Right - Width - 10;
                Top = desktopWorkingArea.Bottom - Height - 10;
            }));

            Block.Text = text;
            Border.Background = backgroundBrush;
            startCloseTimer();
        }

        private void startCloseTimer() {
            var timer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(5)};
            timer.Tick += timerTick;
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e) {
            var timer = (DispatcherTimer)sender;
            timer.Stop();
            timer.Tick -= timerTick;
            Close();
        }
    }
}