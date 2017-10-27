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
        private const int MAX_APPLICATION_RUNNING_TIME = 100_000;

        private static void Main(string[] args)
        {
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
            _stopwatch.Start();
            _exitEvent.WaitOne();
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ExitApplicaitonIfRunningTimeExceedsMaximum();

            var slackProcesses = Process.GetProcessesByName("Slack");
            if (!slackProcesses.Any()) return;

            Debug.WriteLine("Slack processes are detected.");

            _timer.Stop();
            CloseSlackMainWindow(slackProcesses);
            _exitEvent.Set();
        }

        private static void ExitApplicaitonIfRunningTimeExceedsMaximum()
        {
            Debug.WriteLine($"SlackWindowCloser is running for {_stopwatch.Elapsed.Seconds} seconds without detecting Slack processes.");
            if (_stopwatch.ElapsedMilliseconds >= MAX_APPLICATION_RUNNING_TIME)
            {
                _exitEvent.Set();
            }
        }

        private static void CloseSlackMainWindow(IEnumerable<Process> slackProcesses)
        {
            // Wait for 10 seconds so that Slack window shows up.
            // System.Diagnostics.Process does not have events to notify that Main Window is shown up,
            // so waiting is an alternative.
            Thread.Sleep(millisecondsTimeout: 10_000);

            foreach (var process in slackProcesses)
            {
                process.CloseMainWindow();
            }
        }
    }
}