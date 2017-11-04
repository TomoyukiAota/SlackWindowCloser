using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Timers;
using Timer = System.Timers.Timer;

namespace SlackWindowCloser
{
    internal static class Program
    {
        private const string ApplicationName = "SlackWindowCloser";
        private static readonly Configuration Configuration = new Configuration();
        private static readonly ManualResetEvent ExitEvent = new ManualResetEvent(initialState: false);
        private static readonly Timer Timer = new Timer(interval: Configuration.TimerInterval);
        private static readonly Stopwatch Stopwatch = new Stopwatch();
        private static Logger _logger;

        private static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
                return;

            ConfigureByCommandLineOptions(options);
            Timer.Elapsed += Timer_Elapsed;
            Timer.Start();
            Stopwatch.Start();
            ExitEvent.WaitOne();
        }

        private static void ConfigureByCommandLineOptions(CommandLineOptions options)
        {
            Configuration.ConfigureByCommandLineOptions(options);

            if (!Configuration.IsLogFolderPathSpecified) 
                return;

            _logger = new Logger(Configuration.LogFolderPath);
        }

        private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            OutputMessage($"{ApplicationName} is running for {Stopwatch.Elapsed.TotalSeconds} seconds " +
                          $"without detecting Slack Main Window renderer process.");
            ExitApplicaitonIfRunningTimeExceedsMaximum();
            var process = GetSlackMainWindowRendererProcess();
            if (process == null) 
                return;

            OutputMessage("Slack Main Window renderer process is detected.");
            Timer.Stop();
            process.CloseMainWindow();
            OutputMessage($"Slack Main Window is closed. {ApplicationName} will exit.");
            ExitEvent.Set();
        }

        private static void ExitApplicaitonIfRunningTimeExceedsMaximum()
        {
            if (Stopwatch.ElapsedMilliseconds < Configuration.MaxRunningDuration) 
                return;
            
            OutputMessage($"Maximum application running time ({Configuration.MaxRunningDuration/1000} seconds)" +
                          $" has been exceeded. " +
                          $"{ApplicationName} will exit.");
            ExitEvent.Set();
        }

        private static Process GetSlackMainWindowRendererProcess()
        {
            const string processName = "Slack";
            const string partialMainWindowTitle = processName;
            return Process
                .GetProcessesByName(processName)
                .SingleOrDefault(
                    process => process.MainWindowTitle.Contains(partialMainWindowTitle)
                );
        }

        private static void OutputMessage(string message)
        {
            _logger?.Append(message);
            Debug.WriteLine(message);
        }
    }
}