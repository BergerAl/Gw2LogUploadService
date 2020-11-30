using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Security.Permissions;
using System.Threading.Tasks;

namespace LogUploadService
{
    public class FileWatcher
    {
        private static IConfigurationRoot _config;

        public FileWatcher(IConfigurationRoot config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            Run();
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private static void Run()
        {
            try
            {
                var dirName = _config.GetSection("FileWatcher:directoryName").Value ?? throw new FieldAccessException("FileWatcher:directoryName");
                var fileExtension = _config.GetSection("FileWatcher:fileExtension").Value ?? throw new FieldAccessException("FileWatcher:fileExtension");
                if (!Directory.Exists(dirName))
                {
                    // Display the proper way to call the program.
                    throw new DirectoryNotFoundException(dirName);
                }

                // Create a new FileSystemWatcher and set its properties.
                using (FileSystemWatcher watcher = new FileSystemWatcher())
                {
                    watcher.Path = dirName;
                    watcher.IncludeSubdirectories = true;

                    // Watch for changes in LastAccess and LastWrite times, and
                    // the renaming of files or directories.
                    watcher.NotifyFilter = NotifyFilters.LastAccess
                                         | NotifyFilters.LastWrite
                                         | NotifyFilters.FileName
                                         | NotifyFilters.DirectoryName;

                    // Only watch text files.
                    watcher.Filter = $"*.{fileExtension}";

                    // Add event handlers.
                    watcher.Renamed += OnChanged;

                    // Begin watching.
                    watcher.EnableRaisingEvents = true;

                    // Wait for the user to quit the program.
                    Console.WriteLine("Press 'q' to quit the sample.");
                    while (Console.Read() != 'q') ;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static void OnChanged(object source, FileSystemEventArgs e) {
            // Specify what is done when a file is changed, created, or deleted.
            var logUpdater = new LogFileUpdater(_config);
            var fileHyperlink = Task.Run(() => logUpdater.UploadFile(e.FullPath)).Result;
            var discord = new DiscordWebhook(_config);
            Task.Run(() => discord.SendMessageAsync(content: fileHyperlink));
            Console.WriteLine($"File: {fileHyperlink}");
            }
    }
}
