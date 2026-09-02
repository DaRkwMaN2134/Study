using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConfigurationLibrary
{
    public class FileLogger
    {
        static string logfolder = "logs/";
        static string logfile = "log_";
        private static readonly SemaphoreSlim _fileSemaphore = new SemaphoreSlim(1, 1);
        public async Task LogAsync(string message, string level = "INFO:")
        {
            DateTime time = DateTime.Now;
            Directory.CreateDirectory(logfolder);
            string logpath = Path.Combine(logfolder + logfile + time.ToString("dd-MM-yyyy") + ".txt");
            await _fileSemaphore.WaitAsync();
            try
            {

                await File.AppendAllTextAsync(logpath, $"[{time}] {level} {message}\n");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            finally
            {
                _fileSemaphore.Release();
            }

        }
        public async Task LogErrorAsync(string message, Exception ex = null, string level = "ERROR:")
        {
            DateTime time = DateTime.Now;
            string logpath = Path.Combine(logfolder + logfile + time.ToString("dd-MM-yyyy") + ".txt");
            await _fileSemaphore.WaitAsync();
            try
            {
                await File.AppendAllTextAsync(logpath, $"[{time}] {level} {message} - {ex.Message}\n");
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            finally
            {
                _fileSemaphore.Release();
            }
        }
    }
}
