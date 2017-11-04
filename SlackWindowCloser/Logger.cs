using System;
using System.IO;

namespace SlackWindowCloser
{
    public class Logger
    {
        private readonly string _folderPath;
        private readonly string _filePath;

        /// <summary>
        /// Instantiate a logger by the specified folder path.
        /// If the specified folder does not exist, it will be created.
        /// In the specified folder, a text file will be created, 
        /// and calling <see cref="Append"/> will append text to this file.
        /// </summary>
        public Logger(string folderPath)
        {
            _folderPath = folderPath;
            var now = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff");
            _filePath = $@"{_folderPath}/{now}_SlackWindowCloser.txt";
            CreateLogFile();
        }
        
        private void CreateLogFile()
        {
            Directory.CreateDirectory(_folderPath);
            Append("Log file is created.");
        }

        /// <summary>
        /// Append specified text to a text file which is already configured when instantiating this <see cref="Logger"/>.
        /// </summary>
        public void Append(string text)
        {
            var now = DateTime.UtcNow.ToString("yyyyMMdd-HHmmss-fff");
            var line = $"{now}: {text}{Environment.NewLine}";
            File.AppendAllText(_filePath, line);
        }
    }
}