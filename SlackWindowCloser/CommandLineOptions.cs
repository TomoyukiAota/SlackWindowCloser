// Resharper warning for unused members is disabled in this file as below because they are used in Command Line Parser Library.
// ReSharper disable UnusedMember.Global

using CommandLine;
using CommandLine.Text;

namespace SlackWindowCloser
{
    public class CommandLineOptions
    {
        [Option("duration", 
            DefaultValue = 100, 
            HelpText = "The duration (in seconds) which this application runs to search Slack Main Window renderer process.")]
        public int MaxRunningDuration { get; set; }

        [Option("log-folder", 
            HelpText = "The absolute path of the folder where the application log (text file) is saved. " +
                       "When this option is not specified, the log will not be saved.")]
        public string LogFolderPath { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}