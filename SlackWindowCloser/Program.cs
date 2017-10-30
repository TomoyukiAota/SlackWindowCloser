using System;
using System.Diagnostics;
using System.IO;
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
        private static int _maxRunningDuration = 100_000;
        private static string _logFolderPath = null;
        private static string _logFilePath = null;

        private static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
                return;

            ConfigureByCommandLineOptions(options);
            CreateLogFileIfFolderPathIsSpecified();
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
            _stopwatch.Start();
            _exitEvent.WaitOne();
        }

        private static void ConfigureByCommandLineOptions(CommandLineOptions options)
        {
            _maxRunningDuration = options.MaxRunningDuration * 1000;

            if (options.LogFolderPath == null) return;

            _logFolderPath = options.LogFolderPath;
            var now = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff");
            _logFilePath = $@"{_logFolderPath}/{now}_SlackWindowCloser.txt";
        }

        private static void CreateLogFileIfFolderPathIsSpecified()
        {
            if (_logFolderPath == null) return;

            Directory.CreateDirectory(_logFolderPath);
            var now = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff");
            File.AppendAllText(_logFilePath, $"{now}: Log file is created.");
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Debug.WriteLine($"SlackWindowCloser is running for {_stopwatch.Elapsed.TotalSeconds} seconds " +
                            $"without detecting Slack Main Window renderer process.");

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
            if (_stopwatch.ElapsedMilliseconds >= _maxRunningDuration)
            {
                _exitEvent.Set();
            }
        }
    }
}