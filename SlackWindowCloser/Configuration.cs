namespace SlackWindowCloser
{
    public class Configuration
    {
        public int TimerInterval { get; } = 1_000;
        public int MaxRunningDuration { get; private set; } = 100_000;
        public bool IsLogFolderPathSpecified { get; private set; } = false;
        public string LogFolderPath { get; private set; } = null;

        public void ConfigureByCommandLineOptions(CommandLineOptions options)
        {
            //Multiply by 1000 to convert seconds to milliseconds.
            MaxRunningDuration = options.MaxRunningDuration * 1000;
            
            IsLogFolderPathSpecified = (options.LogFolderPath != null);
            LogFolderPath = options.LogFolderPath;
        }
    }
}