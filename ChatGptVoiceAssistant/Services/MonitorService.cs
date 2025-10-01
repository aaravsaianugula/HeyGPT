using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeyGPT.Services
{
    public class MonitorService : IDisposable
    {
        public event EventHandler<Point>? MonitorCaptured;
        public event EventHandler<int>? CountdownTick;

        private CancellationTokenSource? _cancellationTokenSource;

        public Screen[] GetAllScreens()
        {
            return Screen.AllScreens;
        }

        public Screen GetScreenFromPoint(Point point)
        {
            return Screen.FromPoint(point);
        }

        public async Task StartMonitorCapture()
        {
            _cancellationTokenSource = new CancellationTokenSource();

            await Task.Run(async () =>
            {
                for (int i = 10; i > 0; i--)
                {
                    if (_cancellationTokenSource.Token.IsCancellationRequested)
                        return;

                    CountdownTick?.Invoke(this, i);
                    await Task.Delay(1000, _cancellationTokenSource.Token);
                }

                if (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    Point mousePosition = Cursor.Position;
                    MonitorCaptured?.Invoke(this, mousePosition);
                }
            }, _cancellationTokenSource.Token);
        }

        public void CancelMonitorCapture()
        {
            _cancellationTokenSource?.Cancel();
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public Rectangle GetMonitorBounds(Point centerPoint)
        {
            Screen screen = Screen.FromPoint(centerPoint);
            return screen.Bounds;
        }

        public Point GetMonitorCenter(Point pointOnMonitor)
        {
            Rectangle bounds = GetMonitorBounds(pointOnMonitor);
            return new Point(
                bounds.Left + bounds.Width / 2,
                bounds.Top + bounds.Height / 2
            );
        }

        public string GetMonitorInfo(Point point)
        {
            Screen screen = Screen.FromPoint(point);
            int monitorIndex = Array.IndexOf(Screen.AllScreens, screen) + 1;
            return $"Monitor {monitorIndex} ({screen.Bounds.Width}x{screen.Bounds.Height})";
        }
    }
}
