using System;
using System.IO;
using System.Text;
using ZeroPercentBuilder.Attributes;
using ZeroPercentBuilder.Interfaces;

namespace ZeroPercentBuilder.Loggers
{
    public class FileLogger : IPipelineLogger, IDisposable
    {
        private StreamWriter _writer;

        public void Log(string message) => WriteLine("DEBUG", message);
        public void LogWarning(string message) => WriteLine("WARN", message);
        public void LogError(string message) => WriteLine("ERROR", message);

        public FileLogger(string file)
        {
            string directory = Path.GetDirectoryName(file);

            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            _writer = new StreamWriter
            (
                new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read),
                Encoding.UTF8
                
            );

            _writer.AutoFlush = true;
        }

        public void Dispose()
        {
            _writer.Close();
        }

        private void WriteLine(string level, string message)
        {
            _writer.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}");
        }
    }
}

