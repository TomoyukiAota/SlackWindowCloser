using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SlackWindowCloser
{
    internal static class Program
    {
        private static readonly ManualResetEvent _exitEvent = new ManualResetEvent(initialState: false);
        private static readonly Timer _timer = new Timer(interval: 1_000);
        private static readonly Stopwatch _stopwatch = new Stopwatch();
        private static readonly int _maxApplicationRunningTime = 100_000;

        private static void Main(string[] commandLineArguments)
        {
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
            _stopwatch.Start();
            _exitEvent.WaitOne();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ExitApplicaitonIfRunningTimeExceedsMaximum();

            const string processName = "Slack";
            const string partialMainWindowTitle = processName;
            var mainWindowRendererProcess = Process
                    .GetProcessesByName(processName)
                    .SingleOrDefault(
                        process => process.MainWindowTitle.Contains(partialMainWindowTitle)
                    );
            if (mainWindowRendererProcess == null) return;

            Debug.WriteLine("Slack Main Window renderer process is detected.");

            _timer.Stop();
            mainWindowRendererProcess.CloseMainWindow();
            _exitEvent.Set();
        }

        private static void ExitApplicaitonIfRunningTimeExceedsMaximum()
        {
            Debug.WriteLine($"SlackWindowCloser is running for {_stopwatch.Elapsed.TotalSeconds} seconds " +
                            $"without detecting Slack Main Window renderer process.");
            if (_stopwatch.ElapsedMilliseconds >= _maxApplicationRunningTime)
            {
                _exitEvent.Set();
            }
        }
    }
}